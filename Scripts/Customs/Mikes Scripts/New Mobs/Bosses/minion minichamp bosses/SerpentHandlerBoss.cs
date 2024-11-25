using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of the serpent overlord")]
    public class SerpentHandlerBoss : SerpentHandler
    {
        [Constructable]
        public SerpentHandlerBoss() : base()
        {
            Name = "Serpent Overlord";
            Title = "the Master Handler";

            // Update stats to match or exceed Barracoon's level
            SetStr(800); // Enhanced Strength
            SetDex(200); // Enhanced Dexterity
            SetInt(300); // Enhanced Intelligence

            SetHits(12000); // Increased hit points for a boss-tier creature

            SetDamage(25, 35); // Enhanced damage for a more challenging fight

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 80, 90); // Stronger poison resistance
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Magery, 90.0);
            SetSkill(SkillName.EvalInt, 90.0);
            SetSkill(SkillName.Poisoning, 100.0);

            Fame = 22500;  // Increased Fame to reflect its boss status
            Karma = -22500; // Negative Karma for the evil boss

            VirtualArmor = 70; // Higher armor for greater resilience

            // Attach a random ability
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
            // Additional boss logic can be placed here if needed
        }

        public SerpentHandlerBoss(Serial serial) : base(serial)
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
