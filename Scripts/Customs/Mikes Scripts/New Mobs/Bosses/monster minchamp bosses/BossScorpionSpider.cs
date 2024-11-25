using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss scorpion spider corpse")]
    public class BossScorpionSpider : ScorpionSpider
    {
        [Constructable]
        public BossScorpionSpider()
        {
            Name = "Scorpion Spider";
            Title = "the Colossus";
            Hue = 1786; // Unique hue for the boss
            Body = 28;
            BaseSoundID = 0x388;

            // Enhanced stats
            SetStr(1200); // Increased strength
            SetDex(255);
            SetInt(250);
            
            SetHits(12000); // Boss-level health

            SetDamage(45, 60); // Increased damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Skills enhancements
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Enhanced armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("My venom will crush you!");
            PackGold(1000, 1500); // Enhanced loot
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossScorpionSpider(Serial serial)
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
