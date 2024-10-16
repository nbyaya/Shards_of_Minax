using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a leprechaun corpse")]
    public class Leprechaun : BaseCreature
    {
        private DateTime m_NextGoldRain;
        private DateTime m_NextInvisibility;
        private DateTime m_InvisibilityEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Leprechaun()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a leprechaun";
            Body = 723; // GreenGoblin body
            BaseSoundID = 0x45A; // Pixie sound
            Hue = 1588; // Green hue

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

            VirtualArmor = 38;

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public Leprechaun(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextGoldRain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 121)); // Random time between 60 and 120 seconds
                    m_NextInvisibility = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 121)); // Random time between 60 and 120 seconds
                    m_InvisibilityEnd = DateTime.UtcNow; // Start with invisibility off
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGoldRain)
                {
                    DoGoldRain();
                }

                if (DateTime.UtcNow >= m_NextInvisibility && DateTime.UtcNow >= m_InvisibilityEnd)
                {
                    DoInvisibility();
                }
            }

            if (DateTime.UtcNow >= m_InvisibilityEnd && Hidden)
            {
                Hidden = false;
            }
        }

        private void DoGoldRain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Rains gold coins *");
            PlaySound(0x2E6);

            for (int i = 0; i < 5; i++)
            {
                Gold gold = new Gold(Utility.RandomMinMax(50, 100));
                gold.MoveToWorld(GetSpawnPosition(3), Map);

                if (Utility.RandomDouble() < 0.2)
                {
                    gold.Name = "Leprechaun's Trick Gold";
                    gold.Hue = 0x8A5;
                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate { if (!gold.Deleted) gold.Delete(); }));
                }
            }

            m_NextGoldRain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)); // Update to a random time between 60 and 120 seconds
        }

        private void DoInvisibility()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Vanishes *");
            PlaySound(0x22F);
            Hidden = true;

            m_InvisibilityEnd = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)); // Random time between 10 and 20 seconds
            m_NextInvisibility = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 120)); // Update to a random time between 60 and 120 seconds
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Location;
        }

        public override bool OnBeforeDeath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Grants a fortune *");
            PlaySound(0x202);

            Mobile killer = LastKiller;

            if (killer != null && killer.Player)
            {
                List<Item> items = new List<Item>();

                for (int i = 0; i < Utility.RandomMinMax(1, 3); i++)
                {
                    switch (Utility.Random(5))
                    {
                        case 0: items.Add(new Gold(1000)); break;
                        case 1: items.Add(new GoldRing()); break;
                        case 2: items.Add(new GoldBracelet()); break;
                        case 3: items.Add(new Diamond()); break;
                        case 4: items.Add(new Emerald()); break;
                    }
                }

                foreach (Item item in items)
                {
                    killer.AddToBackpack(item);
                    killer.SendLocalizedMessage(1072223); // An item has been placed in your backpack.
                }
            }

            return base.OnBeforeDeath();
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
            m_NextGoldRain = DateTime.UtcNow; // Reset timing to ensure it reinitializes
            m_NextInvisibility = DateTime.UtcNow;
            m_InvisibilityEnd = DateTime.MinValue; // Ensure invisibility end is reset
        }
    }
}
