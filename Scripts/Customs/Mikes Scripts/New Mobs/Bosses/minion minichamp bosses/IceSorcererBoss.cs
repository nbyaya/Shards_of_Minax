using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ice overlord")]
    public class IceSorcererBoss : IceSorcerer
    {
        [Constructable]
        public IceSorcererBoss() : base()
        {
            Name = "Ice Overlord";
            Title = "the Supreme Sorcerer";

            // Update stats to match or exceed Barracoon (similar to IceSorcerer)
            SetStr(200, 300); // Higher strength for the boss
            SetDex(75, 125);  // Increased dexterity
            SetInt(400, 600); // Increased intelligence for a tougher mage

            SetHits(10000, 12000); // Boss-level health

            SetDamage(30, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 50, 60); // Tougher resistances
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 80, 90); // Even more cold resistance
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            // Attach the XmlRandomAbility attachment
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

            // Additional custom loot or sayings
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The cold will be your end!"); break;
                case 1: this.Say(true, "I shall freeze you in time!"); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(20, 30)));
        }

        public IceSorcererBoss(Serial serial) : base(serial)
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
