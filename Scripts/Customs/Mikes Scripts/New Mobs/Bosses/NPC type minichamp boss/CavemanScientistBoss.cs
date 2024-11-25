using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the caveman overlord")]
    public class CavemanScientistBoss : CavemanScientist
    {
        [Constructable]
        public CavemanScientistBoss() : base()
        {
            Name = "Caveman Overlord";
            Title = "the Supreme Scientist";

            // Enhanced stats (based on Barracoon's stats as a reference)
            SetStr(1200); // Increased strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Increased intelligence

            SetHits(10000); // Increased health

            SetDamage(29, 38); // Higher damage

            SetResistance(ResistanceType.Physical, 75); // Higher resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced skill levels
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Boss-level fame
            Karma = -22500; // Boss-level karma

            VirtualArmor = 70; // Improved virtual armor

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

        public CavemanScientistBoss(Serial serial) : base(serial)
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
