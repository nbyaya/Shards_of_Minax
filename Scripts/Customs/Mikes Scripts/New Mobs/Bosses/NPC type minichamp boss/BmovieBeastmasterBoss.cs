using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the B-movie Beastmaster Supreme")]
    public class BmovieBeastmasterBoss : BmovieBeastmaster
    {
        [Constructable]
        public BmovieBeastmasterBoss() : base()
        {
            Name = "B-movie Beastmaster Supreme";
            Title = "the Ultimate Beastmaster";

            // Update stats to match or exceed Barracoon or a boss-tier level
            SetStr(1200); // Upper end of BmovieBeastmaster's strength
            SetDex(255); // Upper end of BmovieBeastmaster's dexterity
            SetInt(250); // Upper end of BmovieBeastmaster's intelligence

            SetHits(12000); // Boss-level health
            SetDamage(25, 35); // Slightly higher damage range than original

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Higher resistances
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased skill range
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0); // Boosted magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500; // Boosted fame to match a boss
            Karma = -22500;

            VirtualArmor = 75; // Increased virtual armor

            // Attach the XmlRandomAbility for additional random enhancements
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

            // Add some special phrases
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The silver screen shall avenge me!"); break;
                case 1: this.Say(true, "Cut! That's a wrap..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic for the boss could go here if needed
        }

        public BmovieBeastmasterBoss(Serial serial) : base(serial)
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
