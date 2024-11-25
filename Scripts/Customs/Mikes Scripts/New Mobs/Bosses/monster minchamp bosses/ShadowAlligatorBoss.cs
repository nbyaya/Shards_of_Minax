using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow alligator corpse")]
    public class ShadowAlligatorBoss : ShadowAlligator
    {
        [Constructable]
        public ShadowAlligatorBoss()
            : base()
        {
            Name = "Shadow Alligator Lord";
            Title = "the Shadow Overlord";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Much higher health
            SetDamage(40, 50); // Enhanced damage

            SetResistance(ResistanceType.Physical, 80, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 80.0, 100.0); // Enhanced skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhanced skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Enhanced skill
            SetSkill(SkillName.Meditation, 60.0, 80.0); // Enhanced skill
            SetSkill(SkillName.MagicResist, 150.0); // Enhanced resist skill
            SetSkill(SkillName.Tactics, 110.0, 120.0); // Enhanced skill
            SetSkill(SkillName.Wrestling, 110.0, 120.0); // Enhanced skill

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Enhanced armor

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

        public ShadowAlligatorBoss(Serial serial) : base(serial)
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
