using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the diplomat lord")]
    public class DiplomatBoss : Diplomat
    {
        [Constructable]
        public DiplomatBoss() : base()
        {
            Name = "Diplomat Lord";
            Title = "the Supreme Negotiator";

            // Update stats to match or exceed the original Diplomat
            SetStr(500, 750);  // Enhanced Strength
            SetDex(150, 200);  // Enhanced Dexterity
            SetInt(500, 600);  // Enhanced Intelligence

            SetHits(1500, 2000);  // Boss-level Health

            SetDamage(15, 25);  // Enhanced Damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 110.1, 120.0); // Enhanced Magic Skills
            SetSkill(SkillName.Magery, 115.5, 120.0);
            SetSkill(SkillName.Meditation, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.5, 120.0);

            Fame = 10000;  // Increased Fame for a boss
            Karma = -10000;  // Negative Karma for a boss

            VirtualArmor = 70;  // Enhanced Armor

            // Attach the random ability XML attachment
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new ThundergodsVigor());
			PackItem(new MiniYew());
            // Drop 5 MaxxiaScrolls in addition to normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Add custom messages
            if (Utility.Random(2) == 0)
            {
                this.Say(true, "Let diplomacy be your downfall...");
            }
            else
            {
                this.Say(true, "Peace is but a fleeting illusion...");
            }
        }

        public DiplomatBoss(Serial serial) : base(serial)
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
