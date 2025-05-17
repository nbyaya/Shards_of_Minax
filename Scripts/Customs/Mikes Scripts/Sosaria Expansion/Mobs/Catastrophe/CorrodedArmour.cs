using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corroded armour corpse")]
    public class CorrodedArmour : BaseCreature
    {
        // Timers for abilities
        private DateTime _NextRustSpray;
        private DateTime _NextShieldShatter;

        [Constructable]
        public CorrodedArmour() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "corroded armour";
            this.Body = 637; // Same as SpectralArmour
            this.BaseSoundID = 0x200; // Use the same idle sound for ambience
            this.Hue = 0x959; // A unique corroded hue



            // Equip with themed items
            Buckler buckler = new Buckler();
            ChainCoif coif = new ChainCoif();
            PlateGloves gloves = new PlateGloves();

            buckler.Hue = 0x845; // Slightly different hue for a rusted look
            buckler.Movable = false;
            coif.Hue = 0x845;
            gloves.Hue = 0x845;

            this.AddItem(buckler);
            this.AddItem(coif);
            this.AddItem(gloves);

            // Advanced stats for an epic threat
            this.SetStr(150, 170);
            this.SetDex(120, 140);
            this.SetInt(80, 100);

            this.SetHits(400, 450);
            this.SetStam(250, 300);

            this.SetDamage(15, 30);

            // Mix multiple damage types: physical, cold, and a hint of corrosive energy
            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 20);
            this.SetDamageType(ResistanceType.Energy, 30);

            // Tough resistances
            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 35, 45);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Sharper melee and defensive skills
            this.SetSkill(SkillName.Wrestling, 95.1, 110.0);
            this.SetSkill(SkillName.Tactics, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 95.1, 110.0);

            this.VirtualArmor = 50;
            this.Fame = 10000;
            this.Karma = -10000;
        }

        public CorrodedArmour(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }

        public override int GetIdleSound() { return 0x200; }
        public override int GetAngerSound() { return 0x56; }
        public override int GetAttackSound() { return 0x5A; }
        public override int GetHurtSound() { return 0x1F2; }
        public override int GetDeathSound() { return 0x5B; }

        // Unique Ability #1: Corrosive Touch applied on melee attacks.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 30% chance to apply Corrosive Touch
            if (Utility.RandomDouble() < 0.3)
            {
                if (defender != null && !defender.Deleted && defender is Mobile target)
                {
                    defender.SendAsciiMessage("The corroded armour's touch burns through your defenses!");
                    ApplyCorrosionDebuff(target);
                }
            }

            // Additionally, a chance to trigger a Shield Shatter attack on melee strike
            if (Utility.RandomDouble() < 0.1)
            {
                ShieldShatter();
            }
        }

        // Applies a temporary debuff that reduces the target's armor
        private void ApplyCorrosionDebuff(Mobile target)
        {
            // Inform the target of the effect
            target.SendAsciiMessage("Your armour weakens as rust eats away at it!");
            // Reduce VirtualArmor by 10 points, then restore after 10 seconds
            target.VirtualArmor = Math.Max(0, target.VirtualArmor - 10);
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                if (target != null && !target.Deleted)
                    target.VirtualArmor += 10;
            });
        }

        // Unique Ability #2: Rusting Aura – periodically damage and corrode nearby foes.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map)
                return;

            // Check for nearby enemies within 3 tiles
            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            if (targets.Count > 0 && DateTime.UtcNow >= _NextRustSpray)
            {
                foreach (Mobile target in targets)
                {
                    if (target != null && !target.Deleted && target is Mobile mobileTarget)
                    {
                        mobileTarget.SendAsciiMessage("The rusting aura corrodes your armour!");
                        // Apply immediate corrosive damage
                        AOS.Damage(mobileTarget, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                        // Optionally, simulate a damage-over-time effect 2 seconds later
                        Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                        {
                            if (mobileTarget != null && !mobileTarget.Deleted)
                            {
                                AOS.Damage(mobileTarget, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                            }
                        });
                    }
                }
                _NextRustSpray = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        // Unique Ability #3: Shield Shatter – a localized AoE attack that may break enemy shields.
        public void ShieldShatter()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map)
                return;

            // Ensure the combatant is close (within 5 tiles)
            if (!InRange(combatant, 5))
                return;

            // Begin the attack animation and visual effect
            Effects.SendLocationEffect(this.Location, this.Map, 0x376A, 10, 1);

            // Process nearby enemies in a 3-tile radius around the combatant
            foreach (Mobile m in combatant.GetMobilesInRange(3))
            {
                if (m != null && m != this && CanBeHarmful(m))
                {
                    if (m is Mobile target)
                    {
                        m.SendAsciiMessage("The corroded armour unleashes a shattering shockwave!");
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                        // 20% chance to "break" a shield if the target is carrying one.
                        if (Utility.RandomDouble() < 0.2)
                        {
                            // Attempt to find an equipped shield (this example checks the TwoHanded layer; adjust as needed)
                            Item shield = m.FindItemOnLayer(Layer.TwoHanded);
                            if (shield != null && shield is BaseShield)
                            {
                                shield.Delete();
                                m.SendAsciiMessage("Your shield crumbles into rust!");
                            }
                        }
                    }
                }
            }
        }

        // When the creature dies, drop extra loot and display an effect.
        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
                return false;

            Gold gold = new Gold(Utility.RandomMinMax(300, 500));
            gold.MoveToWorld(this.Location, this.Map);

            Effects.SendLocationEffect(this.Location, this.Map, 0x376A, 10, 1);
            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
