using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a nature dragon corpse")]
    public class NatureDragonBoss : NatureDragon
    {
        [Constructable]
        public NatureDragonBoss() : base()
        {
            Name = "Nature Dragon Overlord";
            Title = "the Ancient Guardian";

            // Update stats to match or exceed the original NatureDragon (or better)
            SetStr(1200); // Improved strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Improved damage range

            SetResistance(ResistanceType.Physical, 80, 95); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Increased armor

            Tamable = false;
            ControlSlots = 0; // Ensuring it's not tamable

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
            // Additional boss logic could be added here if needed
        }

        public NatureDragonBoss(Serial serial) : base(serial)
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
