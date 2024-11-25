using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a storm overlord corpse")]
    public class StormHeraldBoss : StormHerald
    {
        [Constructable]
        public StormHeraldBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Raging Tempest";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Stronger strength for a boss-tier version
            SetDex(255); // Max dexterity for a faster and more dangerous boss
            SetInt(250); // Max intelligence for more powerful magic

            SetHits(12000); // Stronger health for the boss
            SetDamage(35, 50); // Increased damage output for boss-tier

            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Higher skill for combat
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 115.0, 125.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 35000;
            Karma = -35000;

            VirtualArmor = 90;

            // Attach a random ability to the boss
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
            // Additional boss logic could be added here
        }

        public StormHeraldBoss(Serial serial) : base(serial)
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
