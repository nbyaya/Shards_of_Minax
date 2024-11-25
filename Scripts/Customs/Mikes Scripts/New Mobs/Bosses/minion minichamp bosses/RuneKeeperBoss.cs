using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the rune lord")]
    public class RuneKeeperBoss : RuneKeeper
    {
        [Constructable]
        public RuneKeeperBoss() : base()
        {
            Name = "Rune Lord";
            Title = "the Supreme Rune Keeper";

            // Enhanced stats to match or exceed Barracoon
            SetStr(800); // Upper bound of Barracoon's strength or better
            SetDex(200); // Upper bound of Barracoon's dexterity or better
            SetInt(700); // Upper bound of Barracoon's intelligence or better

            SetHits(12000); // Boss-level health
            SetDamage(15, 30); // Higher damage output

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Cold, 30);

            // Enhanced resistances to match a stronger boss
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 75, 85);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 75, 85);

            // Improved skills for a boss-tier NPC
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 80.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Negative karma as a villain

            VirtualArmor = 70; // Higher virtual armor

            // Attach random ability
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

        public RuneKeeperBoss(Serial serial) : base(serial)
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
