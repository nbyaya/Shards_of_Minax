using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a shadow dragon boss corpse")]
    public class ShadowDragonBoss : ShadowDragon
    {
        [Constructable]
        public ShadowDragonBoss() : base()
        {
            Name = "Shadow Dragon Overlord";
            Title = "the Supreme of Shadows";

            // Update stats to match or exceed Barracoon's or better
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(15000); // Increased health
            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistance
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (negative for boss)

            VirtualArmor = 100; // Increased armor

            Tamable = false; // Boss cannot be tamed
            ControlSlots = 0; // Not controllable

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
            // Boss-specific logic or additional behaviors could be added here
        }

        public ShadowDragonBoss(Serial serial) : base(serial)
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
