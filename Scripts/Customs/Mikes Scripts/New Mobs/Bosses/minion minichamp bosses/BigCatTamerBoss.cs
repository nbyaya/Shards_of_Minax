using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the big cat overlord")]
    public class BigCatTamerBoss : BigCatTamer
    {
        [Constructable]
        public BigCatTamerBoss() : base()
        {
            Name = "Big Cat Overlord";
            Title = "the Supreme Tamer";

            // Update stats to match or exceed Barracoon
            SetStr(1000); // Set higher strength for boss
            SetDex(200); // Set higher dexterity for boss
            SetInt(200); // Set higher intelligence for boss

            SetHits(12000); // Match Barracoon's health
            SetDamage(25, 40); // Increase damage for boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the random ability
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
            // Additional boss logic could be added here, e.g., unique behaviors or commands
        }

        public BigCatTamerBoss(Serial serial) : base(serial)
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
