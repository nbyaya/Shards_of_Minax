using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ironclad ogre corpse")]
    public class IroncladOgre : BaseCreature
    {
        private DateTime m_NextEarthquakeStomp;
        private int m_DamageTaken;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IroncladOgre() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ironclad Ogre";
            Body = 1; // Ogre body
            BaseSoundID = 427;
            Hue = 2500; // Unique metallic hue
			BaseSoundID = 427;

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

            PackItem(new Club());

            m_AbilitiesInitialized = false; // Initialize flag
            m_DamageTaken = 0;
        }

        public IroncladOgre(Serial serial) : base(serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 2; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEarthquakeStomp)
                {
                    EarthquakeStomp();
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            m_DamageTaken += amount;

            if (from != null && !from.Deleted && from.Alive)
            {
                int reflectDamage = m_DamageTaken / 10;
                from.Damage(reflectDamage, this);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Reflective Armor strikes back! *");
            }
        }

        private void EarthquakeStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Earthquake Stomp! *");
            PlaySound(0x2F3);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && m.Player)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.Damage(damage, this);
                    m.SendLocalizedMessage(1060019); // The ground beneath you shakes!
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextEarthquakeStomp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for the next Earthquake Stomp
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
            m_NextEarthquakeStomp = DateTime.UtcNow; // Reset cooldown
            m_DamageTaken = 0;
        }
    }
}
