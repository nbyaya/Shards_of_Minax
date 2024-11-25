using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a purse spider overlord corpse")]
    public class PurseSpiderBoss : PurseSpider
    {
        [Constructable]
        public PurseSpiderBoss() : base()
        {
            Name = "Purse Spider Overlord";
            Title = "the Venomous Queen";

            // Increase stats for the boss version
            SetStr(1200); // Boss-tier strength
            SetDex(255); // Boss-tier dexterity
            SetInt(250); // Boss-tier intelligence

            SetHits(12000); // Increased health for the boss
            SetDamage(35, 45); // Increased damage for the boss

            SetResistance(ResistanceType.Physical, 75, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Boss-tier magic resist
            SetSkill(SkillName.Tactics, 120.0); // Boss-tier tactics
            SetSkill(SkillName.Wrestling, 120.0); // Boss-tier wrestling

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Increased negative karma

            VirtualArmor = 100; // Higher virtual armor for the boss

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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss abilities (like BagOfTricks, HiddenWeb, etc.) remain the same
            // The ability timers and behavior would work as in the original PurseSpider
        }

        public PurseSpiderBoss(Serial serial) : base(serial)
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
