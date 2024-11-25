using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a dairy wraith boss corpse")]
    public class DairyWraithBoss : DairyWraith
    {
        [Constructable]
        public DairyWraithBoss()
            : base()
        {
            Name = "Dairy Wraith Overlord";
            Title = "the Supreme Wraith";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Max strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Much higher health
            SetDamage(35, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 60); // Higher physical damage
            SetDamageType(ResistanceType.Fire, 30); // Higher fire damage
            SetDamageType(ResistanceType.Energy, 30); // Higher energy damage

            SetResistance(ResistanceType.Physical, 80, 100); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Still immune to poison
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Stronger skills
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher karma

            VirtualArmor = 120; // More virtual armor

            Tamable = false;
            ControlSlots = 3; // Can be tamed, but likely a tough challenge

            // Attach the XmlRandomAbility to give it a random ability
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

            // Optional additional behavior for the boss can be added here
        }

        public DairyWraithBoss(Serial serial)
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
