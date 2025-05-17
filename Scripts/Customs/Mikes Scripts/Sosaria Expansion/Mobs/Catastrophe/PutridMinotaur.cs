using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a putrid minotaur corpse")]
    public class PutridMinotaur : BaseCreature
    {
        private DateTime _nextDecayStomp;
        private DateTime _nextBreath;
        private DateTime _nextRegen;

        [Constructable]
        public PutridMinotaur()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "the Putrid Minotaur";
            Body = 262;
            BaseSoundID = 0x597; // Using same base sounds as the Tormented Minotaur
            Hue = 0x83F; // Unique putrid hue – adjust this value as needed

            // Stat ranges are increased beyond the standard version
            SetStr(950, 1050);
            SetDex(420, 435);
            SetInt(150, 170);

            SetHits(5000, 5500);
            SetDamage(20, 35);

            // Damage type: 50% physical, 50% poison
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances (slightly boosted)
            SetResistance(ResistanceType.Physical, 65, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills: advanced melee and defensive skills
            SetSkill(SkillName.Wrestling, 115.0, 117.0);
            SetSkill(SkillName.Tactics, 105.0, 107.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 60;
            
            // Initialize timers for abilities
            _nextDecayStomp = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            _nextBreath = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            _nextRegen = DateTime.UtcNow + TimeSpan.FromSeconds(10);

            SetWeaponAbility(WeaponAbility.Dismount);
        }

        public PutridMinotaur(Serial serial)
            : base(serial)
        {
        }

        // This monster is immune to deadly poison.
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 3; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 10);
            AddLoot(LootPack.Potions);
        }

        // Unique ability: Decay Stomp – an area attack that decays nearby foes.
        public void DecayStomp(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            // Trigger a stomp animation (use your shard’s animation codes as needed)
            Animate(10, 5, 1, true, false, 0);
            DoHarmful(target);

            // Always check that our target is a Mobile before accessing properties:
            if (target is Mobile)
            {
                target.SendAsciiMessage("You are overwhelmed by a putrid stench as your strength begins to decay!");
            }

            // Deal primary damage: mix of physical and poison
            int damage = Utility.RandomMinMax(30, 50);
            AOS.Damage(target, this, damage, 50, 0, 0, 50, 0);

            // Apply Area-of-Effect: damage all mobiles within a 2-tile radius
            foreach (Mobile m in target.GetMobilesInRange(2))
            {
                if (m != target && m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int aoeDamage = Utility.RandomMinMax(15, 30);
                    AOS.Damage(m, this, aoeDamage, 50, 0, 0, 50, 0);
                }
            }
        }

        // Unique ability: Putrid Breath – a ranged toxic gas attack.
        public void PutridBreath(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            // Trigger a breath animation.
            Animate(24, 5, 1, true, false, 0);
            DoHarmful(target);

            // Calculate a base hit chance using a simple skill check.
            double throwingSkill = this.Skills[SkillName.Wrestling].Value; // using Wrestling for a rough attack chance
            double defenderSkill = (target is Mobile) ? target.Skills[SkillName.Wrestling].Value : 50.0;
            double baseHitChance = (throwingSkill + 20.0) / ((defenderSkill * 2) + 20.0);
            baseHitChance = Math.Max(0.05, Math.Min(0.95, baseHitChance));

            // Launch moving particles for a visual effect.
            MovingParticles(target, Utility.RandomList(0x1363, 0x1364, 0x1365, 0x1366, 0x1368), 10, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            // Delay call to simulate the travel time of the toxic cloud.
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                bool hits = Utility.RandomDouble() <= baseHitChance;

                if (hits)
                {
                    target.PlaySound(0x308); // Breath hit sound
                    int rawDamage = Utility.RandomMinMax(25, 40);
                    int physRes = target.PhysicalResistance;
                    int damage = (int)(rawDamage * (1.0 - (physRes / 100.0)));
                    damage = Math.Max(1, damage);
                    
                    // Deal 30% physical and 70% poison damage.
                    AOS.Damage(target, this, damage, 30, 0, 0, 70, 0);

                    // Area damage to mobiles around the target.
                    foreach (Mobile m in target.GetMobilesInRange(2))
                    {
                        if (m != target && m != this && CanBeHarmful(m))
                        {
                            DoHarmful(m);
                            int aoeDamage = Utility.RandomMinMax(15, 25);
                            AOS.Damage(m, this, aoeDamage, 30, 0, 0, 70, 0);
                        }
                    }
                }
                else
                {
                    target.SendAsciiMessage("The noxious fumes miss you!");
                    target.PlaySound(0x238);
                }
            });

            _nextBreath = DateTime.UtcNow + TimeSpan.FromSeconds(15 + (10 * Utility.RandomDouble()));
        }

        // Regeneration: Heal 50 hit points every 10 seconds.
        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= _nextRegen && Hits < HitsMax)
            {
                Hits += 50;
                _nextRegen = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        // Override OnActionCombat to occasionally trigger Decay Stomp or Putrid Breath.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || !InRange(combatant, 15) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= _nextDecayStomp && InRange(combatant, 3))
            {
                DecayStomp(combatant);
                _nextDecayStomp = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
            else if (DateTime.UtcNow >= _nextBreath && !InRange(combatant, 3))
            {
                PutridBreath(combatant);
            }
        }

        // OnGaveMeleeAttack: chance to apply a poisonous lunge that saps stamina.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                // Always check that defender is a Mobile before accessing stamina.
                if (defender is Mobile target)
                {
                    target.SendAsciiMessage("The Putrid Minotaur's filthy blow saturates you with toxic ichor!");
                    // Drain some stamina.
                    target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(10, 20));
                    // Apply additional poison damage (100% poison)
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0);
                }
            }
        }

        public override int GetDeathSound() { return 0x596; }
        public override int GetAttackSound() { return 0x597; }
        public override int GetIdleSound() { return 0x598; }
        public override int GetAngerSound() { return 0x599; }
        public override int GetHurtSound()  { return 0x59A; }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
