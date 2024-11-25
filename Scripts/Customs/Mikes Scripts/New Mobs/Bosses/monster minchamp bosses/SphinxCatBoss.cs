using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sphinx cat boss corpse")]
    public class SphinxCatBoss : SphinxCat
    {
        [Constructable]
        public SphinxCatBoss()
            : base()
        {
            Name = "Sphinx Cat Boss";
            Title = "the Mystic Overlord";
            Hue = 1295; // Unique boss hue, a slight variation for distinction

            // Enhance stats to match or exceed those of Barracoon's boss-tier levels
            SetStr(1200); // Much higher strength than original
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Boss-level health
            SetDamage(45, 55); // Increased damage range to make the boss more dangerous

            // Resisting values are improved for more challenging encounters
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist for more challenge
            SetSkill(SkillName.Tactics, 120.0); // Boosted tactics for combat efficiency
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling for better defense

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased negative karma

            VirtualArmor = 100; // Increased virtual armor for better defense

            Tamable = false; // Boss creatures cannot be tamed
            ControlSlots = 0; // No control slots needed as it's a boss

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

            // Optional: Additional boss-specific AI or abilities could go here
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
