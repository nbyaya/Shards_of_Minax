using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Qi Gong Overlord")]
    public class QiGongHealerBoss : QiGongHealer
    {
        [Constructable]
        public QiGongHealerBoss() : base()
        {
            Name = "Qi Gong Overlord";
            Title = "the Supreme Healer";

            // Update stats to match or exceed boss-level requirements
            SetStr(600); // Increase strength for boss tier
            SetDex(300); // Increase dexterity for boss tier
            SetInt(400); // Increase intelligence for boss tier

            SetHits(6000); // Increase health significantly for boss tier

            SetDamage(20, 40); // Increase damage to be more challenging

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Increase resistances to higher boss-level values
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Healing, 110.0, 120.0); // Boost healing skill
            SetSkill(SkillName.Anatomy, 110.0, 120.0); // Boost anatomy skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Boost magery skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Boost eval int skill
            SetSkill(SkillName.Meditation, 100.0, 120.0); // Boost meditation skill
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Boost magic resist skill

            Fame = 15000; // Increased fame for boss-level NPC
            Karma = 15000; // Increased karma for boss-level NPC

            VirtualArmor = 60; // Increased virtual armor for boss-level defense

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
            // Additional boss logic could be added here, such as special combat moves or effects
        }

        public QiGongHealerBoss(Serial serial) : base(serial)
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
