using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a tempest spirit corpse")]
    public class TempestSpiritBoss : TempestSpirit
    {
        [Constructable]
        public TempestSpiritBoss()
            : base()
        {
            Name = "Tempest Overlord";
            Title = "the Supreme Tempest Spirit";

            // Update stats to match or exceed Barracoon's stats or improve as necessary
            SetStr(1200); // Higher strength than the base Tempest Spirit
            SetDex(255); // Upper dexterity
            SetInt(250); // Upper intelligence

            SetHits(12000); // High health for boss tier
            SetDamage(35, 50); // Higher damage range for boss tier

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 70.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // More negative karma for boss tier

            VirtualArmor = 120; // Higher virtual armor for a tougher boss

            Tamable = false; // Bosses are not tamable
            ControlSlots = 0; // Not tamable, can't be controlled

            // Attach the XmlRandomAbility to give the boss a random ability
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
            // Additional logic for the boss could be added here
        }

        public TempestSpiritBoss(Serial serial)
            : base(serial)
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
