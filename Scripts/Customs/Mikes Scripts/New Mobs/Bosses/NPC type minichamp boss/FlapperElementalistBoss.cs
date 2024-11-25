using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the jazzy overlord")]
    public class FlapperElementalistBoss : FlapperElementalist
    {
        [Constructable]
        public FlapperElementalistBoss() : base()
        {
            Name = "Jazzy Overlord";
            Title = "the Supreme Elementalist";

            // Update stats to make it boss-tier
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255);  // Matching Barracoon's upper dexterity
            SetInt(250);  // Higher intelligence for a more powerful mage

            SetHits(12000); // Increased health
            SetDamage(29, 38); // Stronger damage to match Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance fully maxed out
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.EvalInt, 120.0); // Improved evaluation for more spell damage
            SetSkill(SkillName.Magery, 120.0);  // Stronger magery skill for more powerful spells
            SetSkill(SkillName.Tactics, 120.0);  // Stronger tactics for better combat decisions
            SetSkill(SkillName.Wrestling, 120.0); // Improved wrestling for better melee resistance

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Higher armor for better defense

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

            // Additional boss logic could be added here, such as using more advanced spells, etc.
        }

        public FlapperElementalistBoss(Serial serial) : base(serial)
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
