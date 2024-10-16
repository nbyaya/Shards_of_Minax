using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a red squirrel corpse")]
    public class RedSquirrel : BaseCreature
    {
        private DateTime m_NextSearingBite;
        private DateTime m_NextTreeClimb;
        private DateTime m_NextFlamingAcorn;
        private DateTime m_NextDisorientingSqueal;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RedSquirrel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a red squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2431; // Bright red hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public RedSquirrel(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSearingBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextTreeClimb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFlamingAcorn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextDisorientingSqueal = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSearingBite)
                {
                    SearingBite();
                }

                if (DateTime.UtcNow >= m_NextTreeClimb)
                {
                    TreeClimb();
                }

                if (DateTime.UtcNow >= m_NextFlamingAcorn)
                {
                    FlamingAcorn();
                }

                if (DateTime.UtcNow >= m_NextDisorientingSqueal)
                {
                    DisorientingSqueal();
                }
            }
        }

        private void SearingBite()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The red squirrel bites with searing heat! *");
            PlaySound(0x208); // Bite sound

            if (Combatant != null && Combatant.Alive)
            {
                int damage = Utility.RandomMinMax(8, 15);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0); // Fire damage
                ((Mobile)Combatant).SendMessage("You are burned by the searing bite!");

                // Apply burning effect
                Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyBurningEffect((Mobile)Combatant));

                // Apply debuff
                ((Mobile)Combatant).SendMessage("Your resistance to fire is reduced!");

            }

            m_NextSearingBite = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for SearingBite
        }

        private void ApplyBurningEffect(Mobile target)
        {
            if (target != null && target.Alive)
            {
                int burnDamage = Utility.RandomMinMax(3, 6);
                AOS.Damage(target, this, burnDamage, 0, 100, 0, 0, 0); // Fire damage over time
                target.SendMessage("You continue to burn from the squirrel's bite!");
                Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyBurningEffect(target));
            }
        }

        private void TreeClimb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The red squirrel quickly climbs up a nearby tree! *");
            PlaySound(0x2D6); // Climbing sound effect

            Point3D newLocation = new Point3D(Location.X, Location.Y, Location.Z + 10); // Climb up by 10 units
            if (Map.CanFit(newLocation, 16, false, false))
            {
                Location = newLocation;
                Effects.SendLocationEffect(Location, Map, 0x3728, 10, 10); // Visual effect of climbing

                // Bonus damage and evasion
                this.SetDamage(this.DamageMin + 2, this.DamageMax + 4); // Increase damage
                this.VirtualArmor += 10; // Increase evasion
            }

            m_NextTreeClimb = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for TreeClimb
        }

        private void FlamingAcorn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The red squirrel throws a flaming acorn! *");
            PlaySound(0x208); // Throwing sound effect

            Point3D loc = Location;
            FlamingAcornItem acorn = new FlamingAcornItem();
            acorn.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeAcorn(acorn));

            m_NextFlamingAcorn = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for FlamingAcorn
        }

        private void ExplodeAcorn(FlamingAcornItem acorn)
        {
            if (acorn.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The flaming acorn explodes in a burst of flames! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(acorn.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are hit by the explosive flaming acorn!");
                    }
                    m.PlaySound(0x1DD); // Explosion sound
                }
            }

            acorn.Delete();
        }

        private void DisorientingSqueal()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The red squirrel emits a disorienting squeal! *");
            PlaySound(0x2D7); // Squeal sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("You feel disoriented by the squirrel's squeal!");
                    m.SendLocalizedMessage(1060032); // Disorienting message
                    // Apply disorientation effect
                    m.Paralyze(TimeSpan.FromSeconds(2)); // Paralyze effect for 2 seconds
                }
            }

            m_NextDisorientingSqueal = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for DisorientingSqueal
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }

        private class FlamingAcornItem : Item
        {
            public FlamingAcornItem()
                : base(0xF7E) // Acorn item ID
            {
                Name = "a flaming acorn";
                Movable = false;
            }

            public FlamingAcornItem(Serial serial)
                : base(serial)
            {
            }

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
}
