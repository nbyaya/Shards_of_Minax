using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shield overlord")]
    public class ShieldBearerBoss : ShieldBearer
    {
        [Constructable]
        public ShieldBearerBoss() : base()
        {
            Name = "Shield Overlord";
            Title = "the Unyielding Defender";

            // Update stats to match or exceed Barracoon's (or better)
            SetStr(1200); // Higher strength
            SetDex(150); // Higher dexterity
            SetInt(80);  // Higher intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(15, 25); // Higher damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 90, 100);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            // Update skills
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.Parry, 100.0, 120.0);

            Fame = 22500; // Enhanced fame
            Karma = -22500; // Enhanced karma

            VirtualArmor = 80; // Better armor

            // Attach random ability
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
            // Additional boss logic could be added here if needed
        }

        public ShieldBearerBoss(Serial serial) : base(serial)
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
