using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the glam rock boss")]
    public class GlamRockMageBoss : GlamRockMage
    {
        [Constructable]
        public GlamRockMageBoss() : base()
        {
            Name = "Glam Rock Overlord";
            Title = "the Legendary Rock Star";

            // Update stats to match or exceed the boss tier (comparable to Barracoon)
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255);  // Matching Barracoon's upper dexterity
            SetInt(250);  // Maximum intelligence

            SetHits(12000); // Significantly higher health than the original
            SetDamage(20, 35); // Slightly higher damage than the original

            SetResistance(ResistanceType.Physical, 75, 85); // Boosted resistances
            SetResistance(ResistanceType.Fire, 75, 85); 
            SetResistance(ResistanceType.Cold, 65, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;

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
            // Add any special boss behavior if needed
        }

        public GlamRockMageBoss(Serial serial) : base(serial)
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
