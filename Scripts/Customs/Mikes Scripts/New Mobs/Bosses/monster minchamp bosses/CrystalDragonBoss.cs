using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a crystal dragon overlord corpse")]
    public class CrystalDragonBoss : CrystalDragon
    {
        [Constructable]
        public CrystalDragonBoss() : base()
        {
            // Boss Version: Enhance stats and title
            Name = "Crystal Dragon Overlord";
            Title = "the Supreme of Crystals";

            // Enhance stats to match or exceed Barracoon's stats
            SetStr(1200); // Matching or exceeding Barracoon's upper strength
            SetDex(255); // Matching or exceeding Barracoon's upper dexterity
            SetInt(250); // Already close to Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(29, 38); // Matching Barracoon's damage range

            // Resistances are adjusted to match Barracoon's
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 80);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Evil reputation

            VirtualArmor = 100; // Stronger armor

            // Attach a random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Set abilities initialization for the boss
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to other loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // You can add more custom logic for the boss if needed
        }

        public CrystalDragonBoss(Serial serial) : base(serial)
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

            // Reset abilities and flags after deserialization
        }
    }
}
