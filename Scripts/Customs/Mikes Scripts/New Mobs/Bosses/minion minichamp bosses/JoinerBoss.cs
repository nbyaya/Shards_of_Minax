using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master joiner")]
    public class JoinerBoss : Joiner
    {
        [Constructable]
        public JoinerBoss() : base()
        {
            Name = "Master Joiner";
            Title = "the Wall Builder";

            // Update stats to be more powerful than the original Joiner
            SetStr(800); // Increased strength
            SetDex(200); // Increased dexterity
            SetInt(100); // Increased intelligence

            SetHits(12000); // Boss-level health
            SetDamage(25, 40); // Increased damage range

            SetResistance(ResistanceType.Physical, 70, 80); // Improved resistance
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Carpentry, 120.0); // Enhanced carpentry skill
            SetSkill(SkillName.Tactics, 100.0); // Increased tactics
            SetSkill(SkillName.MagicResist, 90.0); // Higher magic resistance
            SetSkill(SkillName.Wrestling, 100.0); // Improved wrestling skill

            Fame = 22500; // High fame for boss
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 70; // Increased armor

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

            // Boss-specific logic can go here (e.g., special abilities)
        }

        public JoinerBoss(Serial serial) : base(serial)
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
