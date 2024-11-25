using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the lightning overlord")]
    public class LightningBearerBoss : LightningBearer
    {
        [Constructable]
        public LightningBearerBoss() : base()
        {
            Name = "Lightning Overlord";
            Title = "the Stormbringer";

            // Update stats to match or exceed Barracoon
            SetStr(1100); // Matching upper strength
            SetDex(250); // Matching upper dexterity
            SetInt(200); // Higher intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Higher damage range, same as Barracoon

            SetResistance(ResistanceType.Physical, 75); // Increase resistances
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 85);

            SetSkill(SkillName.MagicResist, 100.0); // Match or exceed Barracoon's skill
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Higher fame for a boss-tier creature
            Karma = -22500; // Negative karma for an evil boss

            VirtualArmor = 70; // Higher virtual armor

            // Attach the XmlRandomAbility for random boss abilities
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
            // Additional boss logic or abilities could be added here
        }

        public LightningBearerBoss(Serial serial) : base(serial)
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
