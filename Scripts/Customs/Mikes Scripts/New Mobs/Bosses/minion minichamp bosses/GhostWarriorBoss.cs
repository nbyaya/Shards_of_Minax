using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ghost lord")]
    public class GhostWarriorBoss : GhostWarrior
    {
        [Constructable]
        public GhostWarriorBoss() : base()
        {
            Name = "Ghost Lord";
            Title = "the Undying Phantom";

            // Enhance stats to match or exceed the previous boss (Barracoon-style enhancements)
            SetStr(800); // Increased strength for the boss-tier version
            SetDex(300); // Increased dexterity
            SetInt(150); // Keep intelligence at the upper range

            SetHits(12000); // Boss-tier health
            SetDamage(25, 35); // Slightly higher damage for the boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 90, 100); // The ghost's cold resistance will be maxed
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased tactics skill
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Increased magic resist skill
            SetSkill(SkillName.Anatomy, 70.0, 90.0); // Slight increase in anatomy skill

            Fame = 15000; // Increased fame for the boss
            Karma = -15000; // Increased karma penalty for a powerful boss

            VirtualArmor = 80; // Increased virtual armor to make it more resilient

            // Attach the XmlRandomAbility to provide extra randomized abilities
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
            
            // Additional boss logic or behavior could be added here for more challenge
        }

        public GhostWarriorBoss(Serial serial) : base(serial)
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
