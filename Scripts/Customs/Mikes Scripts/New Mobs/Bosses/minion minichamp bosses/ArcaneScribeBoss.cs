using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the arcane overlord")]
    public class ArcaneScribeBoss : ArcaneScribe
    {
        [Constructable]
        public ArcaneScribeBoss() : base()
        {
            Name = "Arcane Overlord";
            Title = "the Supreme Scribe";

            // Update stats to match or exceed Barracoon for a boss-tier NPC
            SetStr(500); // Upper bound strength, slightly higher than original
            SetDex(150); // Upper bound dexterity, slightly higher
            SetInt(800); // Upper bound intelligence, matching or better than original

            SetHits(12000); // Boss-tier health
            SetDamage(25, 40); // Higher damage range for a boss

            SetDamageType(ResistanceType.Physical, 60); // Adjusted damage type
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);

            SetResistance(ResistanceType.Physical, 65, 75); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 60, 70); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 50, 60); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Higher energy resistance

            SetSkill(SkillName.EvalInt, 120.0); // Improved skill level
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 110.0);
            SetSkill(SkillName.MagicResist, 120.0); // Stronger resistance
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 22500; // Higher fame for boss-tier
            Karma = -22500; // Negative karma for evil alignment

            VirtualArmor = 60; // Slightly higher armor

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
            // Additional boss logic could be added here
        }

        public ArcaneScribeBoss(Serial serial) : base(serial)
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
