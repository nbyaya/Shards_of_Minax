using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frosty ferret corpse")]
    public class FrostyFerret : BaseCreature
    {
        private DateTime m_NextIceBreath;
        private DateTime m_NextFrostyAura;
        private DateTime m_NextFrostyTrail;
        private DateTime m_NextFrostNova;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostyFerret()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a frosty ferret";
            Body = 0x117; // Ferret body
            Hue = 1574; // Light blue hue for a frosty look
			BaseSoundID = 0xCF;

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

        public FrostyFerret(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextIceBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFrostyAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrostyTrail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIceBreath)
                {
                    IceBreath();
                }

                if (DateTime.UtcNow >= m_NextFrostyAura)
                {
                    FrostyAura();
                }

                if (DateTime.UtcNow >= m_NextFrostyTrail)
                {
                    FrostyTrail();
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                {
                    FrostNova();
                }
            }
        }

        private void IceBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Breathes a chilling mist *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Frosty Ferret's ice breath chills you and slows your movements!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.Damage(10, this);
                }
            }

            m_NextIceBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for IceBreath
        }

        private void FrostyAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a frosty aura *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Frosty Ferret's frosty aura slows your attacks and defenses!");
                    m.Damage(5, this); // Optional additional damage
                }
            }

            m_NextFrostyAura = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for FrostyAura
        }

        private void FrostyTrail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Leaves a frosty trail *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You feel a chill as you walk through the Frosty Ferret's trail!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextFrostyTrail = DateTime.UtcNow + TimeSpan.FromSeconds(35); // Cooldown for FrostyTrail
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes a frost nova *");
            FixedEffect(0x376A, 10, 30);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A wave of frost radiates from the Frosty Ferret, chilling you to the bone!");
                    m.Freeze(TimeSpan.FromSeconds(4));
                    m.Damage(15, this);
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for FrostNova
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m.InRange(this, 1))
            {
                // Create a light snowfall effect around the Frosty Ferret
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1)), 0x376A, 9, 32, 1154, 0, 9502, 0);
            }
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
    }
}
