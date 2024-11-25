using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the protester overlord")]
    public class ProtesterBoss : Protester
    {
        [Constructable]
        public ProtesterBoss() : base()
        {
            Name = "Protester Overlord";
            Title = "the Supreme Dissenter";

            // Enhanced stats to match boss-tier
            SetStr(425); // Stronger than the original protester
            SetDex(150); // Faster and more agile
            SetInt(750); // High intelligence for more challenging abilities

            SetHits(12000); // Matching boss-tier health
            SetDamage(25, 40); // Significantly more damage

            SetResistance(ResistanceType.Physical, 70, 80); // Stronger resistances
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Better wrestling skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Enhanced tactics skill

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Negative karma

            VirtualArmor = 70; // Increased armor for more durability

            // Attach the XmlRandomAbility for additional effects
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
            // Additional boss-specific logic can be added here
        }

        public ProtesterBoss(Serial serial) : base(serial)
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
