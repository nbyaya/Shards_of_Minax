using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the temporal overlord")]
    public class RelativistBoss : Relativist
    {
        [Constructable]
        public RelativistBoss() : base()
        {
            Name = "Temporal Overlord";
            Title = "the Master of Time";

            // Update stats to match or exceed Barracoon
            SetStr(700, 900); // Increased strength
            SetDex(200, 250); // Increased dexterity
            SetInt(500, 600); // Increased intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(14, 24); // Increased damage

            // Higher resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Increased skills to make it more challenging
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 25000; // Increased fame for boss-tier
            Karma = -25000; // Boss with negative karma

            VirtualArmor = 80; // More armor to absorb damage

            // Attach the XmlRandomAbility to give it a random power
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
            // Additional boss logic could be added here, like time manipulation effects
        }

        public RelativistBoss(Serial serial) : base(serial)
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
