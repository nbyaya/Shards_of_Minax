using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a trapdoor spider boss corpse")]
    public class GiantTrapdoorSpiderBoss : GiantTrapdoorSpider
    {
        [Constructable]
        public GiantTrapdoorSpiderBoss() : base()
        {
            // Update name and title for the boss
            Name = "Giant Trapdoor Spider";
            Title = "the Overlord of the Burrow";

            // Enhanced stats based on the original and Barracoon stats
            SetStr(1200); // Stronger strength for boss
            SetDex(255); // Max dexterity for better speed
            SetInt(250); // Increased intelligence for stronger spellcasting

            SetHits(12000); // Boss-tier health
            SetDamage(35, 50); // Higher damage for a stronger attack

            SetResistance(ResistanceType.Physical, 85, 90); // Stronger physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 85, 90); // Stronger poison resistance
            SetResistance(ResistanceType.Energy, 60, 70); // Stronger energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance for tougher spells
            SetSkill(SkillName.Tactics, 100.0); // Higher tactics for smarter combat
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling for close combat strength

            Fame = 35000; // Increased fame
            Karma = -35000; // Increased karma loss

            VirtualArmor = 100; // Higher armor value to make it harder to hit

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override the GenerateLoot to include the 5 MaxxiaScroll drops
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public GiantTrapdoorSpiderBoss(Serial serial) : base(serial)
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
