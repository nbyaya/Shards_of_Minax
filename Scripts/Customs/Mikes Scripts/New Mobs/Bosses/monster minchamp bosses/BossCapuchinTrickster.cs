using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss capuchin trickster corpse")]
    public class BossCapuchinTrickster : CapuchinTrickster
    {
        [Constructable]
        public BossCapuchinTrickster()
        {
            Name = "Capuchin Trickster";
            Title = "the Deceiver";
            Hue = 1969; // Unique hue for the boss
            Body = 0x1D; // Gorilla body, can be customized if necessary

            // Enhanced stats to make the boss stronger
            SetStr(1200, 1600);
            SetDex(255);
            SetInt(250);

            SetHits(15000); // Increased hit points for the boss

            SetDamage(40, 50); // Higher damage for a boss

            // Enhanced resistances for the boss
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Enhanced skills for the boss
            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Negative karma for this evil boss

            VirtualArmor = 120; // Higher armor to make the boss tougher

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add 5 MaxxiaScroll drops
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("Your tricks will never outwit me!");
            PackGold(1500, 2000); // Enhanced gold drops for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // Extra ingots for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities like decoy and trap
        }

        public BossCapuchinTrickster(Serial serial) : base(serial)
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
