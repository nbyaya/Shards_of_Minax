using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the forensic overlord")]
    public class ForensicAnalystBoss : ForensicAnalyst
    {
        [Constructable]
        public ForensicAnalystBoss() : base()
        {
            Name = "Forensic Overlord";
            Title = "the Supreme Analyst";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(150); // Matching Barracoon's upper dexterity
            SetInt(750); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Increased stamina for a boss fight
            SetMana(750); // Increased mana for a boss fight

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

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
            // Additional boss logic could be added here
        }

        public ForensicAnalystBoss(Serial serial) : base(serial)
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