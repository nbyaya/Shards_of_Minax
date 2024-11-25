using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the scout commander")]
    public class ScoutLeaderBoss : ScoutLeader
    {
        [Constructable]
        public ScoutLeaderBoss() : base()
        {
            Name = "Scout Commander";
            Title = "the Supreme Leader";

            // Update stats to be boss-level
            SetStr(600, 800); // Enhanced strength
            SetDex(400, 500); // Enhanced dexterity
            SetInt(300, 450); // Enhanced intelligence

            SetHits(12000); // Increased health to match boss-level
            SetDamage(30, 45); // Enhanced damage

            // Resistance enhancements
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Enhanced skill levels
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.Archery, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma

            VirtualArmor = 75; // Enhanced virtual armor

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (speech, animations, etc.) can be added here
        }

        public ScoutLeaderBoss(Serial serial) : base(serial)
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
