using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the battlefield healer")]
    public class BattlefieldHealerBoss : BattlefieldHealer
    {
        [Constructable]
        public BattlefieldHealerBoss() : base()
        {
            Name = "Battlefield Healer";
            Title = "the Supreme Healer";

            // Update stats to match or exceed the required boss level (using Barracoon as the baseline)
            SetStr(700, 900); // Higher strength for a tougher boss
            SetDex(150, 200); // Slightly higher dexterity for better combat capability
            SetInt(400, 600); // Increased intelligence for higher mana pool and spell effectiveness

            SetHits(12000); // Boss-level health
            SetDamage(15, 25); // Increased damage for a higher challenge

            SetResistance(ResistanceType.Physical, 60, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Healing, 120.0, 130.0); // Increase healing skill for stronger effects
            SetSkill(SkillName.Anatomy, 120.0, 130.0);
            SetSkill(SkillName.Magery, 100.0, 120.0); // Higher magery for better spellcasting
            SetSkill(SkillName.MagicResist, 90.0, 110.0); // Stronger magic resistance
            SetSkill(SkillName.Tactics, 80.0, 100.0); // Enhanced tactics for better combat AI

            Fame = 22500; // High fame for boss tier
            Karma = -22500; // Negative karma to match the boss's malevolent nature

            VirtualArmor = 60; // Higher armor for better defense

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

        public BattlefieldHealerBoss(Serial serial) : base(serial)
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
