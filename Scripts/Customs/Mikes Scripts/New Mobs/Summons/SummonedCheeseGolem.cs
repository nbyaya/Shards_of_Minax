using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a cheese golem corpse")]
    public class SummonedCheeseGolem : BaseCreature
    {
        private DateTime m_NextClayQuagmire;
        private DateTime m_NextMuddyForm;
        private DateTime m_NextFistOfClay;
        private DateTime m_NextMudslide;
        private DateTime m_NextEarthenShield;
        private bool m_HasShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedCheeseGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a Cheese Golem";
            Body = 752;
            Hue = 1955; // Unique clay hue
			BaseSoundID = 357;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;
            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public SummonedCheeseGolem(Serial serial) : base(serial)
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
                    m_NextClayQuagmire = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMuddyForm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextFistOfClay = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMudslide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextEarthenShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextClayQuagmire)
                {
                    ClayQuagmire();
                }

                if (DateTime.UtcNow >= m_NextMuddyForm)
                {
                    MuddyForm();
                }

                if (DateTime.UtcNow >= m_NextFistOfClay)
                {
                    FistOfClay();
                }

                if (DateTime.UtcNow >= m_NextMudslide)
                {
                    Mudslide();
                }

                if (DateTime.UtcNow >= m_NextEarthenShield)
                {
                    EarthenShield();
                }
            }
        }

        private void ClayQuagmire()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground becomes treacherous with clay! *");
            // Implement Clay Quagmire logic here

            m_NextClayQuagmire = DateTime.UtcNow + TimeSpan.FromSeconds(20 + new Random().Next(0, 10)); // Random cooldown between 20s and 30s
        }

        private void MuddyForm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The golem blends into the mud! *");
            m_HasShield = false; // Disable the shield during Muddy Form

            // Implement Muddy Form logic here

            m_NextMuddyForm = DateTime.UtcNow + TimeSpan.FromSeconds(25 + new Random().Next(0, 15)); // Random cooldown between 25s and 40s
        }

        private void FistOfClay()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Throws a clump of clay! *");
            // Implement Fist of Clay logic here

            m_NextFistOfClay = DateTime.UtcNow + TimeSpan.FromSeconds(30 + new Random().Next(0, 20)); // Random cooldown between 30s and 50s
        }

        private void Mudslide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A surge of mud crashes across the battlefield! *");
            // Implement Mudslide logic here

            m_NextMudslide = DateTime.UtcNow + TimeSpan.FromSeconds(40 + new Random().Next(0, 30)); // Random cooldown between 40s and 70s
        }

        private void EarthenShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The golem forms an earthen shield! *");
            m_HasShield = true;

            // Implement Earthen Shield logic here

            m_NextEarthenShield = DateTime.UtcNow + TimeSpan.FromSeconds(35 + new Random().Next(0, 25)); // Random cooldown between 35s and 60s
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (0.05 > Utility.RandomDouble())
            {
                // Drop special items or loot
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
