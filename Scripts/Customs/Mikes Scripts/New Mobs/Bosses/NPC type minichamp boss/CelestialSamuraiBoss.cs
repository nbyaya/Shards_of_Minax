using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the celestial samurai overlord")]
    public class CelestialSamuraiBoss : CelestialSamurai
    {
        [Constructable]
        public CelestialSamuraiBoss() : base()
        {
            // Update name and title for the boss version
            Name = "Celestial Samurai Overlord";
            Title = "the Celestial Warden";

            // Enhance stats to make it a boss-level NPC
            SetStr(850, 1100); // Increase strength
            SetDex(600, 800); // Increase dexterity
            SetInt(600, 800); // Increase intelligence
            SetHits(14000); // Increase health to match or exceed Barracoon

            SetDamage(150, 180); // Increase damage

            // Adjust resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Improve skills to match a top-tier NPC
            SetSkill(SkillName.Bushido, 150.0, 200.0);
            SetSkill(SkillName.Anatomy, 120.0, 150.0);
            SetSkill(SkillName.Fencing, 180.0, 250.0);
            SetSkill(SkillName.Macing, 180.0, 250.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Swords, 180.0, 250.0);
            SetSkill(SkillName.Tactics, 180.0, 250.0);
            SetSkill(SkillName.Wrestling, 180.0, 250.0);

            Fame = 30000; // Higher fame
            Karma = 30000; // Higher karma for a noble celestial being

            VirtualArmor = 100; // Increased virtual armor for the boss

            // Attach a random ability to make it more unpredictable
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

            // Optionally, add some unique celestial loot here
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic for the boss can be added here if needed
        }

        public CelestialSamuraiBoss(Serial serial) : base(serial)
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
