using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a mud golem boss corpse")]
    public class MudGolemBoss : MudGolem
    {
        [Constructable]
        public MudGolemBoss() : base()
        {
            Name = "Mud Golem Overlord";
            Title = "the Supreme Golem";

            // Update stats to match or exceed those of a powerful boss
            SetStr(1200); // Increased strength for a boss
            SetDex(255); // Maximize dexterity
            SetInt(250); // Maximize intelligence

            SetHits(12000); // Significantly higher health
            SetDamage(35, 45); // Increased damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased Magic Resist for a boss
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000; // High fame for a boss-level creature
            Karma = -24000; // Negative karma for a villainous creature

            VirtualArmor = 90; // Strong virtual armor

            Tamable = false; // Make it untamable (bosses typically aren't tamable)
            ControlSlots = 3;
            MinTameSkill = 0;

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
            // Additional boss logic could be added here (e.g., unique abilities)
        }

        public MudGolemBoss(Serial serial) : base(serial)
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
