using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the urban tracker boss")]
    public class UrbanTrackerBoss : UrbanTracker
    {
        [Constructable]
        public UrbanTrackerBoss() : base()
        {
            Name = "Urban Tracker Boss";
            Title = "the Shadow's Terror";

            // Update stats to match or exceed the desired boss values
            SetStr(800, 1200); // Strength comparable to a boss-tier NPC
            SetDex(450, 600); // Dexterity pushed to the higher end
            SetInt(250, 350); // Intelligence increased for difficulty

            SetHits(7000, 10000); // Increased health for boss-tier challenge
            SetDamage(25, 35); // Higher damage output for more challenge

            SetResistance(ResistanceType.Physical, 70, 90); // Higher resistance
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 90, 100); // Strong poison resistance
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced skill levels for higher difficulty
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Fencing, 100.0, 120.0);

            Fame = 10000; // Increased fame for boss-tier recognition
            Karma = -10000; // Negative karma for a villainous boss

            VirtualArmor = 75; // Increased armor for higher survivability

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

        public UrbanTrackerBoss(Serial serial) : base(serial)
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
