using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the water clan overlord")]
    public class WaterClanSamuraiBoss : WaterClanSamurai
    {
        [Constructable]
        public WaterClanSamuraiBoss() : base()
        {
            Name = "Water Clan Overlord";
            Title = "the Supreme Protector";

            // Enhanced stats for the boss-tier version
            SetStr(800); // Enhanced Strength
            SetDex(350); // Enhanced Dexterity
            SetInt(150); // Enhanced Intelligence

            SetHits(12000); // Enhanced Health
            SetDamage(120, 150); // Enhanced Damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 75, 85);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 75, 85);

            SetSkill(SkillName.Bushido, 120.0);
            SetSkill(SkillName.Anatomy, 110.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Macing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 20000; // Higher Fame for the boss
            Karma = -20000; // Negative Karma to fit the villainous boss role

            VirtualArmor = 90; // Increased virtual armor

            // Attach the XmlRandomAbility to provide random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls along with the regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic for the boss can be added here
        }

        public WaterClanSamuraiBoss(Serial serial) : base(serial)
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
