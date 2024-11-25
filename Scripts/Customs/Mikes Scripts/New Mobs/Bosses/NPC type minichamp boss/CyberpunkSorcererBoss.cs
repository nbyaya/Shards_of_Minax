using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of the cyberpunk overlord")]
    public class CyberpunkSorcererBoss : CyberpunkSorcerer
    {
        [Constructable]
        public CyberpunkSorcererBoss() : base()
        {
            Name = "Cyberpunk Overlord";
            Title = "the Augmented Sorcerer";

            // Update stats to match or exceed the original Cyberpunk Sorcerer
            SetStr(1200); // Increased strength for the boss
            SetDex(255); // Increased dexterity for the boss
            SetInt(250); // Increased intelligence for the boss

            SetHits(12000); // Increased hit points to match a boss tier
            SetDamage(29, 45); // Increased damage range for the boss

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhanced magical skills
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Improved magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased combat skills
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 10000; // Increased fame to match a boss
            Karma = -10000; // Negative karma for a villain

            VirtualArmor = 70; // Higher armor value

            // Attach the XmlRandomAbility for additional dynamic abilities
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
            // Additional boss behavior could be added here
        }

        public CyberpunkSorcererBoss(Serial serial) : base(serial)
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
