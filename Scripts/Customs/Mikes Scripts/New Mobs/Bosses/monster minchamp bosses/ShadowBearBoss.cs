using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow bear corpse")]
    public class ShadowBearBoss : ShadowBear
    {
        [Constructable]
        public ShadowBearBoss()
            : base()
        {
            Name = "Shadow Bear Overlord";
            Title = "the Dark Warden";

            // Enhance stats to match or exceed typical boss stats
            SetStr(1200); // Enhanced strength for the boss
            SetDex(255); // Max dexterity
            SetInt(250); // Upper tier intelligence

            SetHits(12000); // High health to match a boss-tier creature
            SetDamage(35, 50); // Higher damage range

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 120;

            // Attach the XmlRandomAbility for additional randomized powers
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

            // Custom boss logic could go here
            // For now, we'll keep the original abilities intact
        }

        public ShadowBearBoss(Serial serial) : base(serial)
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
