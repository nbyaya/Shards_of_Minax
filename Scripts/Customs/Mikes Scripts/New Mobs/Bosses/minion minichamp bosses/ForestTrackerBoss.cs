using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the forest overlord")]
    public class ForestTrackerBoss : ForestTracker
    {
        [Constructable]
        public ForestTrackerBoss() : base()
        {
            Name = "Forest Overlord";
            Title = "the Ultimate Tracker";

            // Enhance stats to match or exceed Barracoon (or better if possible)
            SetStr(800); // Enhanced strength for the boss
            SetDex(300); // Enhanced dexterity for the boss
            SetInt(150); // Maximum intelligence (matching original)

            SetHits(12000); // Enhanced hit points for the boss

            SetDamage(25, 40); // Enhanced damage range for the boss

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 90.1, 120.0); // Boosted skill levels for a more challenging fight
            SetSkill(SkillName.Archery, 110.1, 120.0);
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Hiding, 90.0, 120.0);
            SetSkill(SkillName.Stealth, 90.0, 120.0);
            SetSkill(SkillName.Tracking, 110.1, 120.0);

            Fame = 25000; // Increased fame to match a boss-level entity
            Karma = -25000; // Increased karma loss for a more dangerous foe

            VirtualArmor = 60; // Enhanced armor rating for a more durable boss

            // Attach the XmlRandomAbility for dynamic abilities
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
            // Additional boss logic can be inserted here if necessary
        }

        public ForestTrackerBoss(Serial serial) : base(serial)
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
