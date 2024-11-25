using System;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dark overlord")]
    public class DarkLordBoss : DarkLord
    {
        [Constructable]
        public DarkLordBoss() : base()
        {
            Name = "Dark Overlord";
            Title = "the Supreme Shadow";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Enhanced health

            SetDamage(29, 38); // Enhanced damage

            SetResistance(ResistanceType.Physical, 75); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 80); // Enhanced fire resistance
            SetResistance(ResistanceType.Cold, 80); // Enhanced cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maximum poison resistance
            SetResistance(ResistanceType.Energy, 75); // Enhanced energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling
            SetSkill(SkillName.EvalInt, 100.0); // Enhanced EvalInt
            SetSkill(SkillName.Magery, 100.0); // Enhanced Magery

            Fame = 22500; // Boss fame
            Karma = -22500; // Boss karma

            VirtualArmor = 80; // Enhanced armor

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

        public DarkLordBoss(Serial serial) : base(serial)
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
