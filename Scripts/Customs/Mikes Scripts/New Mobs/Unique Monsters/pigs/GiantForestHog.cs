using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a giant forest hog corpse")]
    public class GiantForestHog : BaseCreature
    {
        private DateTime m_NextRootedRampage;
        private DateTime m_NextCamouflage;
        private DateTime m_CamouflageEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GiantForestHog()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a giant forest hog";
            Body = 0xCB; // Pig body
            Hue = 2192; // Dark Green hue
            BaseSoundID = 0xC4; // Pig sound

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

        public GiantForestHog(Serial serial)
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

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 8; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRootedRampage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRootedRampage)
                {
                    RootedRampage();
                }

                if (DateTime.UtcNow >= m_NextCamouflage && DateTime.UtcNow >= m_CamouflageEnd)
                {
                    ActivateCamouflage();
                }

                if (DateTime.UtcNow >= m_CamouflageEnd && m_CamouflageEnd != DateTime.MinValue)
                {
                    DeactivateCamouflage();
                }
            }
        }

        private void RootedRampage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground shakes! *");
            PlaySound(0x2F3); // Earthquake sound
            FixedEffect(0x3789, 10, 20); // Earthquake effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Giant Forest Hog's rooted rampage stuns you!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.FixedEffect(0x376A, 9, 32);
                }
            }

            m_NextRootedRampage = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Reset cooldown
        }

        private void ActivateCamouflage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blends into the environment *");
            PlaySound(0x22F); // Stealth sound
            FixedEffect(0x37C4, 10, 36);

            Hidden = true;

            m_CamouflageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Reset cooldown
        }

        private void DeactivateCamouflage()
        {
            Hidden = false;
            m_CamouflageEnd = DateTime.MinValue;
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
