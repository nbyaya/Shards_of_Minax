using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme logician")]
    public class LogicianBoss : Logician
    {
        [Constructable]
        public LogicianBoss() : base()
        {
            Name = "Supreme Logician";
            Title = "the Paradoxical Overlord";

            // Update stats to match or exceed Barracoon (or improve them)
            SetStr(800); // Higher strength
            SetDex(200); // Higher dexterity
            SetInt(400); // Higher intelligence

            SetHits(12000); // Higher health
            SetDamage(29, 38); // Higher damage range

            SetResistance(ResistanceType.Physical, 75, 80); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.MagicResist, 150.0); // Increased MagicResist
            SetSkill(SkillName.EvalInt, 120.0); // Stronger EvalInt
            SetSkill(SkillName.Magery, 120.0); // Stronger Magery
            SetSkill(SkillName.Tactics, 120.0); // Stronger Tactics
            SetSkill(SkillName.Wrestling, 120.0); // Stronger Wrestling

            Fame = 22500; // Increased Fame
            Karma = -22500; // Increased Karma (negative)

            VirtualArmor = 70; // Increased Virtual Armor

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

            // Add extra logic for boss behavior if needed
        }

        public LogicianBoss(Serial serial) : base(serial)
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
