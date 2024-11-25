using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a urukoth corpse")]
    public class UruKothBoss : UruKoth
    {
        [Constructable]
        public UruKothBoss() : base()
        {
            Name = "Uru'Koth the Devouring Overlord";
            Title = "the Supreme Insatiable";
            Hue = 1764; // Custom hue for the boss

            // Enhance stats to match or exceed Barracoon's
            SetStr(1200); // Higher strength than the original UruKoth
            SetDex(255); // Maximum dexterity for enhanced evasion and speed
            SetInt(250); // Higher intelligence for better spellcasting

            SetHits(12000); // Increased health to make the boss more challenging
            SetDamage(35, 45); // Increased damage range for more potent attacks

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Enhanced fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Enhanced poison resistance
            SetResistance(ResistanceType.Energy, 60, 75); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Stronger magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics for better combat skills
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 24000; // Keeping fame value from original UruKoth
            Karma = -24000; // Negative karma for a villainous boss

            VirtualArmor = 100; // Increased armor to withstand more damage

            PackItem(new BossTreasureBox());
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
            // Additional boss-specific behavior could be added here
        }

        public UruKothBoss(Serial serial) : base(serial)
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
