using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the blues singing gorgon overlord")]
    public class BluesSingingGorgonBoss : BluesSingingGorgon
    {
        [Constructable]
        public BluesSingingGorgonBoss() : base()
        {
            Name = "Blues Singing Gorgon Overlord";
            Title = "the Supreme Blues Singer";
            Hue = Utility.RandomRedHue(); // Giving it a more boss-like hue

            // Update stats to match or exceed Barracoon for a powerful boss
            SetStr(1200); // Max strength for a boss
            SetDex(255); // Max dexterity for a boss
            SetInt(250); // Upper-end intelligence

            SetHits(12000); // Matching Barracoon's health for a boss-tier encounter
            SetDamage(30, 50); // Increased damage for a stronger boss

            // Enhance resistance to be much stronger
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Increase skills to be more challenging
            SetSkill(SkillName.Magery, 120.0, 150.0); 
            SetSkill(SkillName.MagicResist, 120.0, 150.0); 
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Boss-level karma

            VirtualArmor = 80; // Increased virtual armor for extra durability

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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic could be added here
        }

        public BluesSingingGorgonBoss(Serial serial) : base(serial)
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
