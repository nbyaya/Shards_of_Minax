using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss wood golem corpse")]
    public class BossWoodGolem : WoodGolem
    {
        [Constructable]
        public BossWoodGolem()
            : base()
        {
            Name = "Boss Wood Golem";
            Title = "the Titan of the Forest";
            Hue = 0x497; // Unique hue for a boss golem

            SetStr(1600); // Enhanced strength
            SetDex(255); // High dexterity for faster attacks
            SetInt(350); // Increased intelligence for more powerful abilities

            SetHits(20000); // Increased health for a boss
            SetDamage(40, 55); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // Higher resist skill
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics for better combat efficiency
            SetSkill(SkillName.Wrestling, 120.0); // Stronger wrestling
            SetSkill(SkillName.Magery, 120.0); // Stronger magery for better spellcasting
            SetSkill(SkillName.Meditation, 60.0); // Good meditation skill for mana regeneration

            Fame = 30000; // Boss-level fame
            Karma = -30000;

            VirtualArmor = 120; // Enhanced armor

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
            this.Say("Feel the wrath of the forest!");
            PackGold(1500, 2000); // Enhanced loot
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special abilities like Wooden Barrage and Root Entangle
        }

        public BossWoodGolem(Serial serial)
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
