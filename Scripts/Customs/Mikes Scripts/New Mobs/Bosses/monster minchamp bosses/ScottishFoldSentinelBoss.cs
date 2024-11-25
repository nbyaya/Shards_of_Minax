using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Scottish Fold Sentinel corpse")]
    public class ScottishFoldSentinelBoss : ScottishFoldSentinel
    {
        [Constructable]
        public ScottishFoldSentinelBoss() : base()
        {
            Name = "Scottish Fold Sentinel Overlord";
            Title = "the Guardian of Legends";

            // Update stats to match or exceed Barracoon-like values
            SetStr(1200); // Higher Strength than original
            SetDex(255);  // Higher Dexterity than original
            SetInt(250);  // Keeping Intelligence high

            SetHits(12000); // Significant health increase for a boss-tier creature

            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistance values
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Keeping poison resistance high
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // High magic resist for a boss
            SetSkill(SkillName.Tactics, 120.0);    // Increased tactics skill for more effectiveness
            SetSkill(SkillName.Wrestling, 120.0);  // High wrestling skill

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100; // High virtual armor for added tankiness

            Tamable = false;  // Bosses should not be tamable
            ControlSlots = 0;

            // Attach the random ability
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

            // Extra logic for boss behavior, such as enhanced ability use
        }

        public ScottishFoldSentinelBoss(Serial serial) : base(serial)
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
