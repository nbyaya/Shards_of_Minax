using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of The White Witch")]
    public class WhiteWitch : BaseCreature
    {
        private DateTime lastSpecialBrewTime;
        private DateTime lastMysticKeyTime;
        private DateTime lastScrollTime;
        private DateTime lastMoonflowerTime;
        private DateTime lastCrystalsTime;

        [Constructable]
        public WhiteWitch() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "The White Witch";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new WizardsHat() { Hue = 1150 });
            AddItem(new Spellbook() { Name = "Jadis's Tome" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the last reward times to a past time
            lastSpecialBrewTime = DateTime.MinValue;
            lastMysticKeyTime = DateTime.MinValue;
            lastScrollTime = DateTime.MinValue;
            lastMoonflowerTime = DateTime.MinValue;
            lastCrystalsTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am The White Witch.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, as a witch's brew keeps me resilient.");
            }
            else if (speech.Contains("job"))
            {
                Say("My calling is that of a witch. I brew potions, cast spells, and read the ancient tomes.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Do you seek knowledge, traveler? True wisdom can be found in the balance of virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Then ponder the virtues, and their interplay. Each holds a key to a greater understanding.");
            }
            else if (speech.Contains("potions"))
            {
                Say("Ah, potions! My specialty. I can craft many, from healing to the arcane. Some come at a price, while others... require a quest.");
            }
            else if (speech.Contains("spells"))
            {
                Say("Spells are my tools and allies. From the benign to the malevolent, I harness them with precision. If you prove yourself worthy, perhaps I might teach you one.");
            }
            else if (speech.Contains("tomes"))
            {
                Say("The ancient tomes I study contain secrets of the past, both dark and illuminating. Few are privileged to glimpse their pages. Seek the 'Lost Library' if you wish to learn more.");
            }
            else if (speech.Contains("brew"))
            {
                Say("Brewing is an art and science combined. Through my brews, I can heal, harm, or harness the elements. If you're interested, I might have a 'special brew' for the right person.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is the core of all magic and life. Too much of one thing can lead to chaos. To truly master magic, one must find equilibrium. Have you ever felt 'imbalance' in your own life?");
            }
            else if (speech.Contains("key"))
            {
                if (DateTime.UtcNow - lastMysticKeyTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no mystic key for you at the moment. Please return later.");
                }
                else
                {
                    Say("Not just a physical object, but a metaphorical one. In understanding the virtues, one unlocks the keys to deeper wisdom and power. If you assist me in a task, I might bestow upon you a 'mystic key'.");
                    lastMysticKeyTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("library"))
            {
                Say("Ah, the Lost Library. A place of great knowledge, hidden away from the world. I could guide you there, but first, you must retrieve a 'forgotten scroll' for me.");
            }
            else if (speech.Contains("special"))
            {
                if (DateTime.UtcNow - lastSpecialBrewTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no special brew ready at the moment. Please return later.");
                }
                else
                {
                    Say("My special brew is a concoction that grants fleeting insight. However, it requires a rare 'moonflower'. If you bring one to me, I will reward you with this brew.");
                    lastSpecialBrewTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("moonflower"))
            {
                if (DateTime.UtcNow - lastMoonflowerTime < TimeSpan.FromMinutes(10))
                {
                    Say("The moonflower blooms only under the full moon's light in the 'Moonlit Grove'. Should you bring it to me, the special brew will be yours, along with my gratitude.");
                }
                else
                {
                    Say("Bring me the moonflower from the Moonlit Grove, and I will reward you with the special brew.");
                    lastMoonflowerTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("crystals"))
            {
                if (DateTime.UtcNow - lastCrystalsTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no use for more crystals right now. Please return later.");
                }
                else
                {
                    Say("These crystals resonate with ancient magic. They are found deep within the 'Mystic Caves'. Bring me a handful, and the mystic key, along with another reward, will be yours.");
                    lastCrystalsTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("scroll"))
            {
                if (DateTime.UtcNow - lastScrollTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no scroll for you right now. Please return later.");
                }
                else
                {
                    Say("This scroll contains a spell lost to time. It is said to be in the caverns of 'Whispering Shadows'. Retrieve it, and I'll guide you to the Lost Library.");
                    lastScrollTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("moonlit"))
            {
                Say("A serene place, bathed in lunar light. There, the moonflower awaits. Return it to me, and receive your reward. A sample for you.");
                from.AddToBackpack(new ManaLeechAugmentCrystal()); // Reward item
            }
            else if (speech.Contains("caves"))
            {
                Say("The Mystic Caves are deep and treacherous, but they hold many secrets. The enchanted crystals you seek are but one of its many treasures. Return with them, and I shall bestow upon you the mystic key and another gift.");
            }
            else if (speech.Contains("whispering"))
            {
                Say("A dangerous cavern, haunted by echoes of the past. Tread carefully, but know that what you seek, the forgotten scroll, is worth the peril.");
            }
            else if (speech.Contains("temple"))
            {
                Say("The Temple of Scales is a place of reflection, where one confronts their imbalances. Venturing there requires courage. Do so, and the lessons learned will be invaluable.");
            }

            base.OnSpeech(e);
        }

        public WhiteWitch(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastSpecialBrewTime);
            writer.Write(lastMysticKeyTime);
            writer.Write(lastScrollTime);
            writer.Write(lastMoonflowerTime);
            writer.Write(lastCrystalsTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastSpecialBrewTime = reader.ReadDateTime();
            lastMysticKeyTime = reader.ReadDateTime();
            lastScrollTime = reader.ReadDateTime();
            lastMoonflowerTime = reader.ReadDateTime();
            lastCrystalsTime = reader.ReadDateTime();
        }
    }
}
