using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the forest overlord")]
    public class ForestScoutBoss : ForestScout
    {
        [Constructable]
        public ForestScoutBoss() : base()
        {
            Name = "Forest Overlord";
            Title = "the Supreme Scout";

            // Enhance stats to match or exceed Barracoon's level
            SetStr(800); // Higher strength for a boss-tier character
            SetDex(500); // High dexterity to be formidable in combat
            SetInt(200); // Intelligent enough to be a tactical threat

            SetHits(12000); // High health to match a boss-tier NPC
            SetDamage(25, 35); // Higher damage for more challenging encounters

            SetResistance(ResistanceType.Physical, 75); // More resilient
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Archery, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);

            Fame = 22500; // High fame
            Karma = -22500; // Negative karma for a hostile boss

            VirtualArmor = 60; // Better virtual armor for durability

            m_NextAmbushTime = DateTime.Now + TimeSpan.FromSeconds(10); // Faster ambush cooldown for difficulty

            // Attach random ability to enhance the NPC
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

            PackGold(200, 400); // Richer gold drops
            AddLoot(LootPack.Rich); // Bosses should drop rich loot

            this.Say(true, "The forest's vengeance is upon you...");
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add boss-level behavior here, such as increased ambush frequency or special attacks
        }

        public ForestScoutBoss(Serial serial) : base(serial)
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
