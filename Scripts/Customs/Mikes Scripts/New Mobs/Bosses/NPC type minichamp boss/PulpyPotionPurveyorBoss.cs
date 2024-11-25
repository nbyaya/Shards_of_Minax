using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the potion overlord")]
    public class PulpyPotionPurveyorBoss : PulpyPotionPurveyor
    {
        [Constructable]
        public PulpyPotionPurveyorBoss() : base()
        {
            Name = "Potion Overlord";
            Title = "the Elixir King";

            // Enhanced stats to match or exceed Barracoon
            SetStr(1200); // Higher strength than the original
            SetDex(255);  // Max dexterity
            SetInt(250);  // Higher intelligence than the original

            SetHits(10000); // High health like Barracoon
            SetDamage(20, 40); // Higher damage than the original

            SetResistance(ResistanceType.Physical, 80); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 60); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Max magic resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling

            Fame = 12000; // Higher fame for a boss-tier character
            Karma = -12000; // Negative karma to reflect the boss nature

            VirtualArmor = 80; // Increased virtual armor to make it tougher

            // Attach the XmlRandomAbility for random boss traits
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

            // Additional boss-specific behaviors or speech could go here
            // For example, custom speech for the boss
            if (DateTime.Now >= m_NextSpeechTime)
            {
                int phrase = Utility.Random(4);
                switch (phrase)
                {
                    case 0: this.Say("Behold, the elixir of supreme power!"); break;
                    case 1: this.Say("I have concocted potions that defy the laws of nature!"); break;
                    case 2: this.Say("You dare challenge the Potion Overlord?"); break;
                    case 3: this.Say("The elixirs I brew will change the course of fate itself!"); break;
                }

            }
        }

        public PulpyPotionPurveyorBoss(Serial serial) : base(serial)
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
