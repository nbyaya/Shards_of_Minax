using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a wolf spider overlord corpse")]
    public class GiantWolfSpiderBoss : GiantWolfSpider
    {
        [Constructable]
        public GiantWolfSpiderBoss() : base()
        {
            Name = "Wolf Spider Overlord";
            Title = "the Giant Terror";

            // Update stats to match or exceed Barracoon's stats for a boss
            SetStr(1200); // Enhanced strength for a boss-tier fight
            SetDex(255); // Max dexterity for high speed and evasion
            SetInt(250); // Increased intelligence for higher magical effectiveness

            SetHits(12000); // Much higher health
            SetDamage(35, 50); // Higher damage to pose a serious threat

            // Enhance resistances for the boss version
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000; // Keep the same fame as the regular version
            Karma = -24000; // Same karma as the regular version

            VirtualArmor = 90;

            Tamable = false; // Make sure it's untamable for a boss
            ControlSlots = 3;

            // Attach the XmlRandomAbility to provide unique random abilities to this boss
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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optionally, boss-specific AI or behaviors can be added here
        }

        public GiantWolfSpiderBoss(Serial serial) : base(serial)
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
