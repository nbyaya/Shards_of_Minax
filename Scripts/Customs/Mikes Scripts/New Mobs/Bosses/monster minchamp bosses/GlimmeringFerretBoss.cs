using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a glimmering ferret boss corpse")]
    public class GlimmeringFerretBoss : GlimmeringFerret
    {
        [Constructable]
        public GlimmeringFerretBoss() : base()
        {
            // Update name and title for the boss
            Name = "Glimmering Ferret Boss";
            Title = "the Supreme Glimmer";

            // Enhance stats to match or exceed Barracoon's level
            SetStr(1200); // Higher strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // High intelligence

            SetHits(12000); // Higher health
            SetDamage(35, 45); // Higher damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 70.0); // Increased anatomy skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Strong magic eval skill
            SetSkill(SkillName.Magery, 120.0, 150.0); // Strong magery
            SetSkill(SkillName.Meditation, 50.0, 70.0); // Higher meditation skill
            SetSkill(SkillName.MagicResist, 150.0); // Maxed out magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Higher wrestling skill

            Fame = 30000;  // Higher fame for boss
            Karma = -30000; // Higher karma for boss

            VirtualArmor = 120; // Higher armor value

            Tamable = false;  // Cannot be tamed
            ControlSlots = 0; // Not tamable, no control slots needed
            MinTameSkill = 0; // Doesn't need a minimum skill for taming

            // Attach a random ability for added challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            // Generate the base loot
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
            // Additional logic or abilities could be added for the boss here
        }

        public GlimmeringFerretBoss(Serial serial) : base(serial)
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
