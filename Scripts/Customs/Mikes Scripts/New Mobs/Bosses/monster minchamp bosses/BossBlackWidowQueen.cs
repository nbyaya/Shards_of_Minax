using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss black widow queen corpse")]
    public class BossBlackWidowQueen : BlackWidowQueen
    {
        [Constructable]
        public BossBlackWidowQueen() : base()
        {
            Name = "Black Widow Queen";
            Title = "the Arachnid Overlord";
            Hue = 1798; // Unique hue for the boss version
            Body = 28; // Maintain original body type

            // Enhanced stats
            SetStr(1200, 1500);
            SetDex(255, 300);
            SetInt(250, 350);

            SetHits(12000); // Boss-level health
            SetDamage(50, 75); // Increased damage for the boss

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Boss-level magic resist
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Meditation, 75.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Higher virtual armor

            Tamable = false; // This boss is not tamable
            ControlSlots = 0; // Boss-level control slot count

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat (5 MaxxiaScrolls)
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You dare challenge the Queen of Spiders?");
            PackGold(1500, 2000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special abilities like Deadly Kiss, Summon Spiders, etc.
        }

        public BossBlackWidowQueen(Serial serial) : base(serial)
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
