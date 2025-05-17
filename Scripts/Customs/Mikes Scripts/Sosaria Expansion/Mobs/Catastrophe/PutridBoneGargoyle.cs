using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;


namespace Server.Mobiles
{
    [CorpseName("a putrid bone gargoyle corpse")]
    public class PutridBoneGargoyle : BaseCreature
    {
        // Cooldown timers for the unique abilities
        private DateTime _nextBoneShatter, _nextPutridRoar, _nextDecayCloud;

        [Constructable]
        public PutridBoneGargoyle()
            : base(AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a putrid bone gargoyle";
            this.Body = 722;
            this.BaseSoundID = 372;
            this.Hue = 1800; // Unique hue to differentiate from the base Putrid Undead Gargoyle

            // Primary stats
            this.SetStr(600);
            this.SetDex(130, 140);
            this.SetInt(1200);

            this.SetHits(800, 850);

            this.SetDamage(25, 35);

            // Damage types: a mix of physical, cold, and energy damage
            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Cold, 30);
            this.SetDamageType(ResistanceType.Energy, 50);

            // Resistances
            this.SetResistance(ResistanceType.Physical, 60, 70);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 55, 65);
            this.SetResistance(ResistanceType.Poison, 60, 70);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            // Skills
            this.SetSkill(SkillName.EvalInt, 130.0);
            this.SetSkill(SkillName.Magery, 135.0);
            this.SetSkill(SkillName.Mysticism, 110.0);
            this.SetSkill(SkillName.Meditation, 115.0, 120.0);
            this.SetSkill(SkillName.MagicResist, 190.0, 200.0);
            this.SetSkill(SkillName.Tactics, 110.0);
            this.SetSkill(SkillName.Wrestling, 105.0, 110.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.VirtualArmor = 40;

            // Chance-based loot drops
            if (0.05 > Utility.RandomDouble())
                PackItem(new TatteredAncientScroll());

            if (0.12 > Utility.RandomDouble())
                PackItem(new InfusedGlassStave());

            if (0.18 > Utility.RandomDouble())
                PackItem(new AncientPotteryFragments());
        }

        public PutridBoneGargoyle(Serial serial)
            : base(serial)
        {
        }

        public override bool Unprovokable { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }
        public override int Meat { get { return 1; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 6);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(2, 5));

            // Rare drop chance (more powerful than the base drop)
            if (Utility.RandomDouble() < 0.002)
            {
                PackItem(new GargoyleMastersRobes());
            }
        }

        // Main combat loop: checks ability cooldowns and uses unique abilities in combat
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 15) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            DateTime now = DateTime.UtcNow;

            // Bone Shatter: a ranged attack that hurls a bone shard at a target causing AoE damage
            if (now >= _nextBoneShatter)
            {
                BoneShatter(combatant);
                _nextBoneShatter = now + TimeSpan.FromSeconds(10.0);
                return;
            }

            // Putrid Roar: emits a toxic, stunning roar affecting nearby foes
            if (now >= _nextPutridRoar)
            {
                PutridRoar();
                _nextPutridRoar = now + TimeSpan.FromSeconds(15.0);
                return;
            }

            // Decay Cloud: summons a poisonous cloud that damages enemies over time
            if (now >= _nextDecayCloud)
            {
                DecayCloud();
                _nextDecayCloud = now + TimeSpan.FromSeconds(20.0);
                return;
            }
        }

        // Bone Shatter: launches a bone shard toward the target, damaging the target and nearby foes within 2 tiles
        private void BoneShatter(Mobile target)
        {
            if (target == null || target.Deleted || !InRange(target, 15))
                return;

            DoHarmful(target);
            Animate(AnimationType.Attack, 0);
            MovingParticles(target, Utility.RandomList(0x1F1, 0x1F2, 0x1F3), 10, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                // Verify target is still valid before dealing damage
                if (target is Mobile t && !t.Deleted)
                {
                    t.PlaySound(0x307); // Bone cracking sound effect
                    int damage = Utility.RandomMinMax(30, 40);
                    AOS.Damage(t, this, damage, 100, 0, 0, 0, 0);

                    // Deal area damage to targets within a 2-tile radius of the primary target
                    foreach (Mobile m in t.GetMobilesInRange(2))
                    {
                        if (m != t && m != this && CanBeHarmful(m))
                        {
                            DoHarmful(m);
                            int aoeDamage = Utility.RandomMinMax(15, 25);
                            AOS.Damage(m, this, aoeDamage, 100, 0, 0, 0, 0);
                        }
                    }
                }
            });
        }

        // Putrid Roar: releases a deafening roar, applying deadly poison and reducing stamina of all nearby enemies
        private void PutridRoar()
        {
            PlaySound(0x38D); // Roaring sound
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "RRROOOAAAARRR!");

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && CanBeHarmful(m) && m is Mobile)
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                if (m is Mobile target)
                {
                    ApplyPoison(target, Poison.Deadly);
                    // Reduce 20% of the target's current stamina
                    target.Stam -= (int)(target.Stam * 0.2);
                }
            }
        }

        // Decay Cloud: creates a lingering poison cloud that delivers initial and delayed damage ticks to foes in an 8-tile radius
        private void DecayCloud()
        {
            PlaySound(0x1E8); // Eerie decay sound
            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && CanBeHarmful(m) && m is Mobile target)
                {
                    DoHarmful(target);
                    // Immediate damage tick
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0);
                    // Schedule a delayed damage tick after 2 seconds
                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                    {
                        if (target != null && !target.Deleted)
                            AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0);
                    });
                }
            }
        }

        // OnGaveMeleeAttack: occasionally applies a Decaying Touch to drain life from the defender and heal the gargoyle
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3 && defender is Mobile target)
            {
                int extraDamage = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, extraDamage, 100, 0, 0, 0, 0);
                int healAmount = extraDamage / 2;
                this.Hits += healAmount;
                target.SendAsciiMessage("You feel your life force draining away...");
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
