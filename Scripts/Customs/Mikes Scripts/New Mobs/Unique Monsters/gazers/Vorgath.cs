using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a voidcaller corpse")]
    public class Vorgath : ElderGazer
    {
        private DateTime m_NextVoidRift;
        private DateTime m_NextDarkPulse;
        private DateTime m_NextVoidShift;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Vorgath()
            : base()
        {
            Name = "Vorgath the Voidcaller";
            Body = 22; // ElderGazer body
            Hue = 1763; // Dark purple hue for a void-like appearance
			BaseSoundID = 377;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public Vorgath(Serial serial)
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
                    Random rand = new Random();
                    m_NextVoidRift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarkPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVoidShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVoidRift)
                {
                    CreateVoidRift();
                }

                if (DateTime.UtcNow >= m_NextDarkPulse)
                {
                    DarkPulse();
                }

                if (DateTime.UtcNow >= m_NextVoidShift)
                {
                    VoidShift();
                }
            }
        }

        private void CreateVoidRift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A rift in the void opens up! *");
            Point3D location = new Point3D(X, Y, Z);
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You are being hurt by the void rift!");
                        m.Damage(5, this);
                    }
                }
            });
            m_NextVoidRift = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after use
        }

        private void DarkPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A dark pulse radiates outward! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);
                    m.Mana -= damage;
                    if (m.Mana < 0) m.Mana = 0;
                }
            }
            m_NextDarkPulse = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown after use
        }

        private void VoidShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vorgath shifts into the void! *");
            int originalVirtualArmor = VirtualArmor;
            VirtualArmor += 50; // Increase virtual armor to reduce damage

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                VirtualArmor = originalVirtualArmor; // Restore original virtual armor
            });

            m_NextVoidShift = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set cooldown after use
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
