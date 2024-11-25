using System;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of The Avatar of Elements")]
    public class AvatarOfElementsBoss : AvatarOfElements
    {
        [Constructable]
        public AvatarOfElementsBoss() : base()
        {
            Name = "Avatar of the Elements";
            Title = "the Supreme Elemental";
            Hue = 0x4001; // Unique hue for the boss appearance

            // Update stats to match or exceed boss standards
            SetStr(1200); // Higher strength to reflect boss-level power
            SetDex(255); // Maxed dexterity for high attack speed and agility
            SetInt(250); // High intelligence for enhanced magical capabilities

            SetHits(12000); // Matching the HP of a boss-tier NPC
            SetDamage(40, 50); // Increased damage output

            // Resistance values enhanced for boss difficulty
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 100); // Full poison resistance for the boss-tier challenge
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 150.0);
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 100.0, 150.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 150.0);
            SetSkill(SkillName.Wrestling, 100.0, 150.0);

            // Random skills for diversity
            SetSkill(SkillName.Chivalry, Utility.RandomMinMax(80, 120));
            SetSkill(SkillName.Fencing, Utility.RandomMinMax(80, 120));
            SetSkill(SkillName.Parry, Utility.RandomMinMax(80, 120));

            Fame = 25000; // Increased fame for a boss-tier NPC
            Karma = -25000; // Negative karma for the boss-level antagonist

            VirtualArmor = 85; // Higher armor for increased damage resistance

            // Attach random ability via XML
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls in addition to regular loot
            this.AddLoot(LootPack.UltraRich, 3);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public AvatarOfElementsBoss(Serial serial) : base(serial)
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
