using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a bloodthirsty vines corpse")]
    public class BloodthirstyVinesBoss : BloodthirstyVines
    {
        [Constructable]
        public BloodthirstyVinesBoss() : base("Bloodthirsty Vines")
        {
            Name = "Bloodthirsty Vines Overlord";
            Title = "the Supreme Frenzy";

            // Enhance stats to match or exceed Barracoon's stats
            SetStr(1200); // Set a higher strength for a boss-tier challenge
            SetDex(255);  // Maximum dexterity for fast reactions
            SetInt(250);  // High intelligence for better spells and tactics

            SetHits(12000); // Match Barracoon's HP for the boss
            SetDamage(35, 45); // Higher damage for a more challenging encounter

            SetDamageType(ResistanceType.Physical, 60); // More physical damage
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance maxed out
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Improved magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame for a boss-tier NPC
            Karma = -30000; // Still a villain

            VirtualArmor = 100; // Higher armor for added durability

            // Attach a random ability to this boss
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

            // Additional boss logic could be added here, like more aggressive abilities
        }

        public BloodthirstyVinesBoss(Serial serial) : base(serial)
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
