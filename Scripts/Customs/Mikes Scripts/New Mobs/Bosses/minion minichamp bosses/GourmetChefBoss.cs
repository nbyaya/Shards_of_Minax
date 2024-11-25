using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the gourmet overlord")]
    public class GourmetChefBoss : GourmetChef
    {
        [Constructable]
        public GourmetChefBoss() : base()
        {
            Name = "Gourmet Overlord";
            Title = "the Master Chef";

            // Enhanced stats for boss version
            SetStr(600, 900); // Increased strength compared to original
            SetDex(200, 250); // Increased dexterity compared to original
            SetInt(300, 500); // Increased intelligence compared to original

            SetHits(12000); // Increased health to boss level
            SetDamage(15, 25); // Increased damage range

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 30);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Cooking, 100.0, 120.0); // Enhanced cooking skill

            Fame = 22500; // Increased fame for boss tier
            Karma = -22500; // Negative karma to reflect the "boss" nature

            VirtualArmor = 60; // Higher virtual armor


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

            // Extra loot and phrases for the boss
            PackGold(500, 600); // Increased gold drop for boss tier
            AddLoot(LootPack.Average); // Adding better loot pack for boss

            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "I am the king of the kitchen..."); break;
                case 1: this.Say(true, "You are about to be cooked alive!"); break;
            }

            PackItem(new RawRibs(Utility.RandomMinMax(20, 30))); // Larger amount of raw ribs
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional behavior could be added here, such as casting spells or summoning minions
        }

        public GourmetChefBoss(Serial serial) : base(serial)
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
