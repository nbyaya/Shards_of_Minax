using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the field medic boss")]
    public class FieldMedicBoss : FieldMedic
    {
        [Constructable]
        public FieldMedicBoss() : base()
        {
            Name = "Field Medic Overlord";
            Title = "the Supreme Healer";

            // Update stats to match or exceed Barracoon
            SetStr(425); // Increased strength for boss-tier difficulty
            SetDex(250); // Increased dexterity for agility
            SetInt(400); // Increased intelligence for stronger healing and magic

            SetHits(4500); // Significantly increased health to make it a boss
            SetDamage(25, 40); // Increased damage range for tougher combat

            SetResistance(ResistanceType.Physical, 70, 80); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 50, 70); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 50, 70); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 70); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 50, 70); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 100.0); // Improved anatomy skill
            SetSkill(SkillName.Healing, 120.0); // Increased healing skill for more effective heals
            SetSkill(SkillName.Magery, 100.0); // Improved magery skill for better magic
            SetSkill(SkillName.MagicResist, 90.0); // Increased magic resistance
            SetSkill(SkillName.Tactics, 100.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 80.0); // Increased wrestling skill

            Fame = 15000; // Higher fame to reflect boss status
            Karma = 15000; // Higher karma to balance with the boss nature

            VirtualArmor = 60; // Increased armor to make it more difficult to defeat

            // Attach the XmlRandomAbility for dynamic abilities
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
            
            // Additional logic or abilities could be added here for this boss-tier NPC
        }

        public FieldMedicBoss(Serial serial) : base(serial)
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
