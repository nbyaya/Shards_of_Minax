using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the satyr overlord")]
    public class SatyrPiperBoss : SatyrPiper
    {
        [Constructable]
        public SatyrPiperBoss() : base()
        {
            Name = "Satyr Overlord";
            Title = "the Supreme Piper";

            // Update stats to match or exceed Barracoon and other boss-tier standards
            SetStr(500, 600); // Enhanced strength
            SetDex(200, 250); // Enhanced dexterity
            SetInt(150, 200); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(25, 40); // Increased damage range

            SetResistance(ResistanceType.Physical, 70, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Musicianship, 100.0);
            SetSkill(SkillName.Provocation, 100.0);
            SetSkill(SkillName.MagicResist, 120.0); // Increased Magic Resist for a more challenging fight
            SetSkill(SkillName.Tactics, 100.0); // Increased Tactics
            SetSkill(SkillName.Wrestling, 90.0); // Increased Wrestling

            Fame = 10000; // Increased fame for a boss NPC
            Karma = -10000; // Negative karma for the villainous boss

            VirtualArmor = 50; // Enhanced armor value

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

        public SatyrPiperBoss(Serial serial) : base(serial)
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
