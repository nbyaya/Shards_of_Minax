using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a molten golem overlord corpse")]
    public class MoltenGolemBoss : MoltenGolem
    {
        [Constructable]
        public MoltenGolemBoss() : base()
        {
            Name = "Molten Golem Overlord";
            Title = "the Infernal Fury";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching Barracoon's upper strength or higher
            SetDex(255); // Maximum dexterity
            SetInt(250); // Upper intelligence for a more boss-like feel

            SetHits(12000); // Boosted health for a more challenging boss
            SetDamage(35, 45); // Increased damage for stronger attacks

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30); // Increased fire damage
            SetDamageType(ResistanceType.Energy, 20); // Keeping energy damage

            SetResistance(ResistanceType.Physical, 80, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Slightly increased cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 60, 75); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 110.0); // Slightly boosted tactics for a boss feel
            SetSkill(SkillName.Wrestling, 110.0); // Increased wrestling skill

            Fame = 30000; // Increased fame for a higher-tier boss
            Karma = -30000; // High negative karma for a boss enemy

            VirtualArmor = 100; // Increased virtual armor for better defense

            // Attach the random ability for extra challenge
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
            // Additional boss logic could be added here for more dynamic behavior
        }

        public MoltenGolemBoss(Serial serial) : base(serial)
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
