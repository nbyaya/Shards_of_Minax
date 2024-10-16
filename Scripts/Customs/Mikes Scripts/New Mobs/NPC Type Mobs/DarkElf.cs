using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a dark elf")]
    public class DarkElf : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between dark elf speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public DarkElf() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Set the skin hue to a dark color, typical of dark elves
            Hue = 0x840; // You may need to adjust this depending on your shard's hue for dark elf skin

            // Adjust the body value if your server has specific ones for elves
            if (Female = Utility.RandomBool())
            {
                Body = 0x191; // This might be different in your shard for a female elf
                Name = NameList.RandomName("female");
                Title = "the Malevolent"; // Dark Elves would likely have intimidating titles
            }
            else
            {
                Body = 0x190; // This might be different in your shard for a male elf
                Name = NameList.RandomName("male");
                Title = "the Malevolent"; // Using a fixed title for simplicity
            }

            // Dark Elves are often associated with spider-like or drow-themed equipment
            AddItem(new Robe(0x455)); // Dark hue

            // Create dark elf hair and potentially facial hair, adjust the hue to something dark or white
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2048)); // Elf hair styles
            hair.Hue = 0x47E; // White hair color, which is common for dark elves
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!Female)
            {
                Item beard = new Item(Utility.RandomList(0x2040, 0x2041, 0x204B, 0x204D)); // Select a beard style
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            // Define the stats and skills typical for a dark elf NPC
            SetStr(350, 500);
            SetDex(350, 500);
            SetInt(350, 500);
            SetHits(500, 900);

            SetDamage(100, 200);

            SetSkill(SkillName.Magery, 120.0, 200.0);
            SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);

            // Adjust the karma for a dark elf to be negative as they are typically evil
            Fame = 18000;
            Karma = -18000;

            // Dark elf speech patterns can be ominous and threatening
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        // Implementing the AI for speech
        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Your soul will be consumed by darkness!"); break;
                        case 1: this.Say(true, "Feel the wrath of the shadow!"); break;
                        case 2: this.Say(true, "You cannot escape the night!"); break;
                        case 3: this.Say(true, "I'll feast on your heart!"); break;
                    }

                    this.Hidden = true; // Hide when a player is detected

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }

                base.OnThink();
            }            
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The shadows will not protect you!"); break;
                        case 1: this.Say(true, "You dare strike me?"); break;
                        case 2: this.Say(true, "Darkness, arise and aid me!"); break;
                        case 3: this.Say(true, "You will pay for your insolence!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation
        public override void GenerateLoot()
        {
            PackGold(500, 750);
            AddLoot(LootPack.UltraRich);  // Even richer loot than before
        }

        public DarkElf(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
