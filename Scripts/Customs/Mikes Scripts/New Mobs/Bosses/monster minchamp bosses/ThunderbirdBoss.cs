using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a thunderbird corpse")]
    public class ThunderbirdBoss : Thunderbird
    {
        [Constructable]
        public ThunderbirdBoss()
            : base()
        {
            Name = "Thunderbird Overlord";
            Title = "the Stormbringer";
            Hue = 1360; // Electric blue hue

            // Update stats to match or exceed the original Thunderbird (using Barracoon's upper values)
            SetStr(1200); // Upper strength
            SetDex(255); // Upper dexterity
            SetInt(250); // Upper intelligence

            SetHits(12000); // Enhanced health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 120; // Increased armor

            Tamable = false; // Bosses are not tamable
            ControlSlots = 3;
            MinTameSkill = 93.9;

            PackItem(new BossTreasureBox());
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

        public ThunderbirdBoss(Serial serial)
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
