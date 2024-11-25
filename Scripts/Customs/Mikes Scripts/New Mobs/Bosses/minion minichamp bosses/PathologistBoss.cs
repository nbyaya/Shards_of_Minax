using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the diseased overlord")]
    public class PathologistBoss : Pathologist
    {
        [Constructable]
        public PathologistBoss() : base()
        {
            Name = "Diseased Overlord";
            Title = "the Supreme Pathologist";

            // Update stats to match or exceed Barracoon (or better if necessary)
            SetStr(800); // Strength enhanced to be on par with or better than Barracoon
            SetDex(200); // Dexterity improved for a tougher boss
            SetInt(750); // Intelligence boosted to match the strength of Barracoon

            SetHits(12000); // Health increased to make it more challenging

            SetDamage(29, 38); // Adjusted damage to match Barracoon's boss tier
            SetDamageType(ResistanceType.Physical, 50); // Split damage type (physical + poison)
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 70, 80); // Enhanced resistances for a boss tier
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 85, 100); // Poison resistance boosted
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Improved anatomy skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Increased EvalInt skill
            SetSkill(SkillName.Magery, 110.0, 120.0); // Higher Magery skill
            SetSkill(SkillName.Meditation, 50.0, 75.0); // Meditation enhanced for longer fights
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Maxed out MagicResist skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill for more damage mitigation

            Fame = 22500; // Increased Fame for the boss tier
            Karma = -22500; // Adjusted Karma for the evil nature of the boss

            VirtualArmor = 75; // Enhanced virtual armor for better defense

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

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic or mechanics can be added here if desired
        }

        public PathologistBoss(Serial serial) : base(serial)
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
