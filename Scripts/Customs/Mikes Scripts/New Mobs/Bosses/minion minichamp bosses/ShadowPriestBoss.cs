using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shadow priest overlord")]
    public class ShadowPriestBoss : ShadowPriest
    {
        [Constructable]
        public ShadowPriestBoss() : base()
        {
            Name = "Shadow Priest Overlord";
            Title = "the Supreme Shadow Priest";

            // Update stats to match or exceed the original
            SetStr(200); // Slightly higher than the original strength
            SetDex(150); // Keeping the same dexterity as the original's max
            SetInt(300); // Same as original

            SetHits(12000); // Significantly higher health for the boss-tier version

            SetDamage(15, 25); // Higher damage range compared to the original

            SetResistance(ResistanceType.Physical, 60, 80); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 50, 70); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 70, 90); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 60, 80); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 70, 90); // Increased energy resistance

            SetSkill(SkillName.EvalInt, 120.0); // Higher skill for boss
            SetSkill(SkillName.Magery, 120.0); // Higher skill for boss
            SetSkill(SkillName.Meditation, 120.0); // Higher skill for boss
            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 100.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 90.0); // Higher wrestling skill

            Fame = 25000; // Increased fame for the boss
            Karma = -25000; // Increased karma for the boss

            VirtualArmor = 60; // Increased virtual armor

            // Attach the random ability for additional challenges or buffs
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
            // Additional boss logic can be added here
        }

        public ShadowPriestBoss(Serial serial) : base(serial)
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
