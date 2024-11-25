using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss taurus harpy corpse")]
    public class BossTaurusHarpy : TaurusHarpy
    {
        [Constructable]
        public BossTaurusHarpy()
            : base()
        {
            Name = "Boss Taurus Harpy";
            Title = "the Earthshaker";
            Hue = 2068; // Boss-specific earthy brown hue (you can change it if desired)
            Body = 30; // Harpy body
            BaseSoundID = 402; // Harpy sound

            // Enhanced Stats
            SetStr(1200, 1500);
            SetDex(250, 300);
            SetInt(250, 350);

            SetHits(15000); // Boss-level health
            SetDamage(45, 55); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 24000; // Boss-level fame
            Karma = -24000;

            VirtualArmor = 120; // Boss-level armor

            // Attach random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add the special loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include the base loot
            this.Say("You dare challenge the Earthshaker?");
            PackGold(1500, 2000); // Increased gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossTaurusHarpy(Serial serial)
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
