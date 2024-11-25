using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the storm overlord")]
    public class BattleStormCallerBoss : BattleStormCaller
    {
        [Constructable]
        public BattleStormCallerBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Tempest Bringer";

            // Enhanced stats to match or exceed the original BattleStormCaller
            SetStr(800, 1000); // Stronger strength range
            SetDex(200, 250); // Higher dexterity
            SetInt(500, 600); // Higher intelligence range

            SetHits(12000); // Matching boss-tier health
            SetDamage(30, 40); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 80, 100);

            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Higher EvalInt skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Higher Magery skill
            SetSkill(SkillName.Meditation, 80.0, 100.0); // Better Meditation skill
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Enhanced MagicResist skill
            SetSkill(SkillName.Tactics, 80.0, 100.0); // Enhanced Tactics skill
            SetSkill(SkillName.Wrestling, 70.0, 90.0); // Better Wrestling skill

            Fame = 22500; // High fame, fitting for a boss
            Karma = -22500; // Negative karma, matching its evil nature

            VirtualArmor = 70; // Enhanced Virtual Armor

            // Attach random ability for additional challenge
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

            // Additional logic for the boss can be added here if necessary
        }

        public BattleStormCallerBoss(Serial serial) : base(serial)
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
