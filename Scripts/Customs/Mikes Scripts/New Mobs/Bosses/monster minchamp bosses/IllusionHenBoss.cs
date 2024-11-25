using System;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an illusion hen corpse")]
    public class IllusionHenBoss : IllusionHen
    {
        [Constructable]
        public IllusionHenBoss()
            : base()
        {
            Name = "Illusion Hen Overlord";
            Title = "the Supreme Illusionist";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Upper dexterity from original
            SetInt(250); // Upper intelligence from original

            SetHits(12000); // Much higher health for the boss tier
            SetDamage(35, 45); // Increased damage for the boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false; // Boss should not be tamable
            ControlSlots = 0;

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
            // Additional boss logic could be added here, like special moves or effects
        }

        public IllusionHenBoss(Serial serial)
            : base(serial)
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
