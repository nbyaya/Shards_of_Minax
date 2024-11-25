using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the astral overlord")]
    public class AstralTravelerBoss : AstralTraveler
    {
        [Constructable]
        public AstralTravelerBoss() : base()
        {
            Name = "Astral Overlord";
            Title = "the Supreme Traveler";

            // Update stats to match or exceed Barracoon and enhance the original stats
            SetStr(1200); // Enhanced Strength
            SetDex(255); // Maximum Dexterity
            SetInt(750); // Maximum Intelligence

            SetHits(12000); // Enhanced Health

            SetDamage(20, 40); // Enhanced Damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced Resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500; // Enhanced Fame
            Karma = -22500; // Enhanced Karma

            VirtualArmor = 75; // Enhanced Virtual Armor

            // Attach the XmlRandomAbility for dynamic gameplay features
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
            // Additional boss logic could be added here for special behaviors
        }

        public AstralTravelerBoss(Serial serial) : base(serial)
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
