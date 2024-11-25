using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Sagittarius Harpy corpse")]
    public class BossSagittariusHarpy : SagittariusHarpy
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BossSagittariusHarpy() : base()
        {
            Name = "Sagittarius Harpy";
            Title = "the Colossus";
            Hue = 2071; // Unique hue resembling arrows for a boss
            Body = 30; // Harpy body
            BaseSoundID = 402; // Harpy sound

            SetStr(1200, 1500); // Enhanced strength
            SetDex(250, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(15000); // Increased health for the boss
            SetDamage(35, 50); // Higher damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 60.0, 85.0); // Improved skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // Boss-level virtual armor

            Tamable = false; // Make it untamable for the boss
            ControlSlots = 3; // No change here, it remains controlled like the original


            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Add 5 MaxxiaScrolls on death
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("My arrows will pierce your soul!");
            PackGold(1500, 2000); // Increased gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for a boss
        }

        public BossSagittariusHarpy(Serial serial) : base(serial)
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
