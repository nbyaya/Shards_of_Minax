using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss mountain gorilla corpse")]
    public class BossMountainGorilla : MountainGorilla
    {
        [Constructable]
        public BossMountainGorilla()
        {
            Name = "Boss Mountain Gorilla";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for a boss
            Body = 0x1D; // Gorilla body

            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(15000); // Increased health
            SetDamage(40, 50); // Enhanced damage

            SetResistance(ResistanceType.Physical, 90, 110);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Meditation, 75.0); // Slight increase to Meditation for extra mana regen

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Negative karma

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
            this.Say("Feel the might of the Mountain Gorilla!");
            PackGold(1500, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special abilities
        }

        public BossMountainGorilla(Serial serial) : base(serial)
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
