using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a thul'gor the forsaken corpse")]
    public class ThulGorTheForsakenBoss : ThulGorTheForsaken
    {
        [Constructable]
        public ThulGorTheForsakenBoss() : base()
        {
            Name = "Thul'Gor the Forsaken";
            Title = "the Unyielding";

            // Update stats to match or exceed Barracoon's or better
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80); // Max physical resistance
            SetResistance(ResistanceType.Fire, 80); // Max fire resistance
            SetResistance(ResistanceType.Cold, 70); // Max cold resistance
            SetResistance(ResistanceType.Poison, 80); // Max poison resistance
            SetResistance(ResistanceType.Energy, 70); // Max energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // Max virtual armor

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

        public ThulGorTheForsakenBoss(Serial serial) : base(serial)
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
