using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the stormtrooper commander")]
    public class StormtrooperBoss : Stormtrooper2
    {
        [Constructable]
        public StormtrooperBoss() : base()
        {
            Name = "Stormtrooper Commander";
            Title = "the Supreme Officer";

            // Enhanced Stats (matching or exceeding Stormtrooper2)
            SetStr(1200);  // Enhanced strength for the boss
            SetDex(255);   // Maximum dexterity
            SetInt(250);   // Enhanced intelligence

            SetHits(12000);  // Boss-level health

            SetDamage(50, 75);  // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 75.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Archery, 100.0);
            SetSkill(SkillName.Fencing, 100.0);
            SetSkill(SkillName.Swords, 100.0);
            SetSkill(SkillName.Parry, 100.0);
            SetSkill(SkillName.Ninjitsu, 100.0);
            SetSkill(SkillName.Bushido, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 70;  // Boss-level armor

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

            // Extra boss phrases
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "For the Empire... and for Lord Vader!"); break;
                case 1: this.Say(true, "The galaxy will bow before us!"); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public override void OnThink()
        {
            base.OnThink();
            // You can add additional boss behavior here if needed
        }

        public StormtrooperBoss(Serial serial) : base(serial)
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
