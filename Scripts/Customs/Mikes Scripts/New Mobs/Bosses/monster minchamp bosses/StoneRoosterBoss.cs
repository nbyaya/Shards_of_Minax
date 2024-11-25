using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a stone rooster corpse")]
    public class StoneRoosterBoss : StoneRooster
    {
        private DateTime m_NextStonePeck;
        private DateTime m_NextQuakeEgg;

        [Constructable]
        public StoneRoosterBoss()
            : base()
        {
            Name = "Stone Rooster Overlord";
            Title = "the Quaking Terror";

            // Enhanced Stats to match or exceed Barracoon-level power
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 50); // Stronger damage range

            SetResistance(ResistanceType.Physical, 75, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher karma penalty

            VirtualArmor = 100; // Higher virtual armor

            Tamable = false; // This version is untamable
            ControlSlots = 0;

            m_NextStonePeck = DateTime.UtcNow;
            m_NextQuakeEgg = DateTime.UtcNow;

            // Attach a random ability
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

            // Add more boss-level loot
            this.AddLoot(LootPack.FilthyRich, 3);
            this.AddLoot(LootPack.Rich, 2);
            this.AddLoot(LootPack.Gems, 10);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add additional boss behavior here if necessary
        }

        public StoneRoosterBoss(Serial serial)
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
