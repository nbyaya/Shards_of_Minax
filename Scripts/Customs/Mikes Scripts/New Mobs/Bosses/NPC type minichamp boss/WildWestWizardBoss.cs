using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the wild west wizard overlord")]
    public class WildWestWizardBoss : WildWestWizard
    {
        [Constructable]
        public WildWestWizardBoss() : base()
        {
            Name = "Wild West Wizard Overlord";
            Title = "the Supreme Magician";

            // Update stats to match or exceed the original WildWestWizard
            SetStr(1200); // Matching the higher strength
            SetDex(255); // Maxing out dexterity
            SetInt(250); // Maxing out intelligence

            SetHits(10000); // Boss-tier health
            SetDamage(20, 30); // Increased damage range

            // Set higher resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 12000;  // Increased fame for boss
            Karma = -12000; // Negative karma for evil boss

            VirtualArmor = 80;  // Enhanced virtual armor

            // Attach a random ability to the boss
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
            // Additional boss-specific behavior could be added here
        }

        public WildWestWizardBoss(Serial serial) : base(serial)
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
