using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sabre overlord")]
    public class SabreFighterBoss : SabreFighter
    {
        [Constructable]
        public SabreFighterBoss() : base()
        {
            Name = "Sabre Overlord";
            Title = "the Supreme Blade";

            // Update stats to match or exceed Barracoon (or better if applicable)
            SetStr(1200); // Increase strength beyond regular SabreFighter stats
            SetDex(300); // Upper dexterity limit
            SetInt(150); // Maintain intelligence as in original SabreFighter

            SetHits(12000); // Increase health to boss level
            SetDamage(30, 40); // Increased damage for a boss

            SetResistance(ResistanceType.Physical, 80); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 90); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Swords, 120.0); // Max out skills for higher difficulty
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 100.0);

            Fame = 25000; // Increased fame for a stronger boss
            Karma = -25000; // Negative karma for a villainous NPC

            VirtualArmor = 75; // Higher armor for better protection

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

        public SabreFighterBoss(Serial serial) : base(serial)
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
