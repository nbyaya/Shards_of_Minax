using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the legendary rap ranger")]
    public class RapRangerBoss : RapRanger
    {
        [Constructable]
        public RapRangerBoss() : base()
        {
            Name = "Rap Ranger";
            Title = "the Legendary Rhyme Master";

            // Update stats to match or exceed the original RapRanger
            SetStr(1200); // Higher strength for a boss-tier NPC
            SetDex(255); // Maximum dexterity for agility
            SetInt(250); // Maximum intelligence for spellcasting

            SetHits(10000); // Boss-tier health
            SetDamage(20, 40); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced resist skills
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000; // Higher fame for a boss
            Karma = -25000; // Negative karma to indicate a villainous boss

            VirtualArmor = 100; // High armor for a tougher boss

            // Attach a random ability for additional dynamic effects
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
            // Additional boss-specific logic or custom speech can be added here
        }

        public RapRangerBoss(Serial serial) : base(serial)
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
