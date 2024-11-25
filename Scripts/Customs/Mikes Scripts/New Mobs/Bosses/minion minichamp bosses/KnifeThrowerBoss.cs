using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the knife overlord")]
    public class KnifeThrowerBoss : KnifeThrower
    {
        [Constructable]
        public KnifeThrowerBoss() : base()
        {
            Name = "Knife Overlord";
            Title = "the Master of Blades";

            // Enhance stats to match or exceed Barracoon's level
            SetStr(900, 1200);  // Higher strength, making it more resilient
            SetDex(300, 350);   // Higher dexterity for more attacks and speed
            SetInt(200, 300);   // Improved intelligence

            SetHits(12000);     // Increased health to match a boss-tier creature
            SetDamage(25, 40);  // Higher damage range for a more challenging fight

            SetResistance(ResistanceType.Physical, 70, 80);   // Stronger resistance
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Archery, 120.0);  // Increased archery skill for more accurate attacks
            SetSkill(SkillName.Tactics, 100.0);  // Enhanced tactics for better combat strategies
            SetSkill(SkillName.MagicResist, 95.0); // Increased magic resistance to make it harder to defeat

            Fame = 22500;  // Increased fame to match a boss-level creature
            Karma = -22500;

            VirtualArmor = 60;  // Increased virtual armor to make it more difficult to hit

            // Attach a random ability to enhance the boss's unpredictability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if desired
        }

        public KnifeThrowerBoss(Serial serial) : base(serial)
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
