using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss markhor corpse")]
    public class BossMarkhor : Markhor
    {
        [Constructable]
        public BossMarkhor()
        {
            Name = "Markhor";
            Title = "the Unstoppable";
            Hue = 0x497; // Unique hue for a boss
            Body = 0xD1; // Goat body (same as the original Markhor)

            SetStr(1200); // Enhanced strength, higher than original Markhor
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence for the boss

            SetHits(15000); // Boss-level health

            SetDamage(35, 45); // Higher damage than the original Markhor

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Boss-level virtual armor

            Tamable = false;
            ControlSlots = 0; // Not tamable


            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot: Add MaxxiaScrolls to the boss drop
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You will not survive my charge!");
            PackGold(1500, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(75, 150))); // Increased iron ingot drops
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for horn twisting, shaggy shield, and raging charge
        }

        public BossMarkhor(Serial serial)
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
