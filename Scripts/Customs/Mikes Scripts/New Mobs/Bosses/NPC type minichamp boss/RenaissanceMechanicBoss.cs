using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the mechanized overlord")]
    public class RenaissanceMechanicBoss : RenaissanceMechanic
    {
        [Constructable]
        public RenaissanceMechanicBoss() : base()
        {
            Name = "Mechanized Overlord";
            Title = "the Supreme Mechanic";

            // Update stats to match or exceed Barracoon (adjusted for this type of NPC)
            SetStr(1200); // Upper end of Barracoon's strength, making it more powerful
            SetDex(255); // Upper dexterity, to make it faster
            SetInt(250); // Higher intelligence to match the boss tier

            SetHits(12000); // Higher health to make it a boss
            SetStam(300); // Stamina boosted for longer fights
            SetMana(750); // Mana boosted for more magic usage

            SetDamage(30, 50); // Higher damage range for more challenge

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Boosted magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics for smarter combat
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling for better defense

            Fame = 25000; // Increased fame for a boss
            Karma = -25000; // Negative karma since it is an enemy boss

            VirtualArmor = 80; // Increased armor for durability

            // Attach the XmlRandomAbility attachment
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

            // Custom loot for the boss
            PackItem(new Item(0x1BEF) { Name = "mechanical gears", Amount = Utility.Random(5, 10) }); // Additional mechanical items for flavor
        }

        public RenaissanceMechanicBoss(Serial serial) : base(serial)
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
