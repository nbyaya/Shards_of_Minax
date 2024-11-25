using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss shadow golem corpse")]
    public class BossShadowGolem : ShadowGolem
    {
        [Constructable]
        public BossShadowGolem()
        {
            Name = "Shadow Golem";
            Title = "the Colossus of Shadows";
            Hue = 0x497; // Unique hue for the boss
            Body = 752; // Default Golem body

            SetStr(1200); // Enhanced strength for a boss
            SetDex(255);
            SetInt(250);

            SetHits(12000); // Boss-level health
            SetDamage(35, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // Increased damage distribution
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // Enhanced magery skill

            Fame = 24000; // Increased fame for the boss
            Karma = -24000;

            VirtualArmor = 90; // Boss-level armor

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
            this.Say("The shadows will consume you!");
            PackGold(1500, 2500); // Enhanced gold drop
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities
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
