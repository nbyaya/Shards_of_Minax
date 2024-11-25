using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the knight of valor lord")]
    public class KnightOfValorBoss : KnightOfValor
    {
        [Constructable]
        public KnightOfValorBoss() : base()
        {
            Name = "Knight of Valor Lord";
            Title = "the Supreme Guardian";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1500); // Higher strength than the original
            SetDex(250); // Higher dexterity than the original
            SetInt(200); // Higher intelligence than the original

            SetHits(1500); // Increased health range
            SetDamage(30, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 85, 95); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increased skills
            SetSkill(SkillName.Swords, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Parry, 120.0, 140.0);

            Fame = 15000; // Increased fame
            Karma = 15000; // Increased karma

            VirtualArmor = 80; // Higher virtual armor

            // Attach random ability via XmlRandomAbility
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
            // Additional boss-specific logic could be added here if necessary
        }

        public KnightOfValorBoss(Serial serial) : base(serial)
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
