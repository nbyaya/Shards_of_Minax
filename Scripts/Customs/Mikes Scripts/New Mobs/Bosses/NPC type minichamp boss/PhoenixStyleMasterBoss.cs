using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the phoenix overlord")]
    public class PhoenixStyleMasterBoss : PhoenixStyleMaster
    {
        [Constructable]
        public PhoenixStyleMasterBoss() : base()
        {
            Name = "Phoenix Overlord";
            Title = "the Supreme Monk";

            // Enhance stats to match or exceed those of Barracoon
            SetStr(1200); // Increased strength for boss-tier
            SetDex(255); // Increased dexterity for boss-tier
            SetInt(250); // Increased intelligence for boss-tier

            SetHits(12000); // Higher health for a boss
            SetDamage(30, 45); // Increased damage for a boss

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Remain maxed for poison resistance
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            // Add random abilities via XmlRandomAbility
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Higher virtual armor for a boss-tier NPC

            // Additional speech delays for extra challenge and flavor
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

        public PhoenixStyleMasterBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
