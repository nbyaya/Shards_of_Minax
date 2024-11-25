using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the zen master")]
    public class ZenMonkBoss : ZenMonk
    {
        [Constructable]
        public ZenMonkBoss() : base()
        {
            Name = "Zen Master";
            Title = "the Supreme Monk";

            // Update stats to match or exceed Barracoon and enhance for a boss fight
            SetStr(800); // Match Barracoon's strength upper bound
            SetDex(250); // Match Barracoon's dexterity upper bound
            SetInt(400); // Match the original but increase slightly

            SetHits(12000); // Match Barracoon's health
            SetDamage(20, 40); // Increase damage for a more challenging encounter

            // Increase resistances for the boss version
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Adjusted skills for a harder encounter
            SetSkill(SkillName.MagicResist, 120.0); // Increase magic resistance for the boss version
            SetSkill(SkillName.Tactics, 120.0); // Increase tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increase wrestling

            Fame = 22500; // Increased fame for a boss
            Karma = -22500; // Negative karma for a boss character

            VirtualArmor = 80; // Increased virtual armor

            // Attach the XmlRandomAbility to make the boss more unpredictable
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

            // Extra loot generation could be added here, for example, a special item or additional gold
        }

        public override void OnThink()
        {
            base.OnThink();

            // Boss-specific behavior could go here
            // For example, healing other creatures more efficiently or using special abilities
        }

        public ZenMonkBoss(Serial serial) : base(serial)
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
