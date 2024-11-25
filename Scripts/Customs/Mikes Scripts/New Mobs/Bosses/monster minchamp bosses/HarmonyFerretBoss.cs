using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a supreme harmony ferret corpse")]
    public class HarmonyFerretBoss : HarmonyFerret
    {
        [Constructable]
        public HarmonyFerretBoss()
            : base()
        {
            Name = "Supreme Harmony Ferret";
            Title = "the Master of Melody";

            // Update stats to match or exceed the original Harmony Ferret
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health
            SetDamage(40, 50); // Higher damage range

            // Resistance improvements
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist skill
            SetSkill(SkillName.Tactics, 110.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 110.0); // Increased wrestling

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Increased virtual armor

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

        public HarmonyFerretBoss(Serial serial)
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
