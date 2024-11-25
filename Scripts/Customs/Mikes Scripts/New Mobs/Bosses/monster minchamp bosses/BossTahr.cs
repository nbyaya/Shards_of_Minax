using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss tahr corpse")]
    public class BossTahr : Tahr
    {
        [Constructable]
        public BossTahr() : base()
        {
            Name = "Boss Tahr";
            Title = "the Savage";
            Hue = 0x497; // Unique hue for boss
            Body = 0xD1; // Keep the same body as the original Tahr, can be customized if needed
            BaseSoundID = 0x99; // Retain original sound

            SetStr(1200, 1500); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Keep intelligence at a high value

            SetHits(12000); // High boss health

            SetDamage(35, 45); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0); // High magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // High fame for boss
            Karma = -30000;

            VirtualArmor = 90; // High armor for boss

            Tamable = false; // Bosses are not tamable
            ControlSlots = 0;

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("I will crush you, puny mortal!");
            PackGold(1500, 2000); // Increased gold drop for a boss
            AddLoot(LootPack.FilthyRich, 3); // Enhanced loot pack
            AddLoot(LootPack.Gems, 12); // More gems for the boss

            // Additional special loot
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities and actions
        }

        public BossTahr(Serial serial) : base(serial)
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
