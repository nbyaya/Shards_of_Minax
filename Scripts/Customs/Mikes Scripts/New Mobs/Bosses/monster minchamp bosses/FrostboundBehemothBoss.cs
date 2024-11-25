using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frostbound behemoth corpse")]
    public class FrostboundBehemothBoss : FrostboundBehemoth
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextGlacialShield;
        private DateTime m_NextIcyGrasp;

        private bool m_AbilitiesInitialized; // Flag to check if abilities are initialized

        [Constructable]
        public FrostboundBehemothBoss() : base()
        {
            Name = "Frostbound Behemoth";
            Title = "the Frozen Titan";
            Hue = 1469; // Keeping the ice theme hue
            BaseSoundID = 357;

            // Update stats to match or exceed the original Frostbound Behemoth
            SetStr(1200, 1600); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(15000); // Increased health
            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90); // Improved resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 32000; // Increased fame for boss-tier
            Karma = -32000; // Increased karma for boss-tier

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // This is now a boss, no taming
            ControlSlots = 3; // Keep the same slots as the original

            m_AbilitiesInitialized = false; // Initialize flag

            // Attach the XmlRandomAbility to give random abilities to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Enhanced loot drops for the boss
            this.AddLoot(LootPack.FilthyRich, 2); 
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public FrostboundBehemothBoss(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
