using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Treasure-seeking Tina")]
    public class TreasureSeekingTina : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TreasureSeekingTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Treasure-seeking Tina";
            Body = 0x191; // Human female body

            // Stats
            Str = 125;
            Dex = 60;
            Int = 30;
            Hits = 88;

            // Appearance
            AddItem(new ShortPants() { Hue = 1157 });
            AddItem(new FancyShirt() { Hue = 1156 });
            AddItem(new Boots() { Hue = 1171 });
            AddItem(new TricorneHat() { Hue = 1155 });
            AddItem(new ThinLongsword() { Name = "Tina's Treasure Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Arr, I be Treasure-seeking Tina, scourin' the seas for hidden riches!");
            }
            else if (speech.Contains("health"))
            {
                Say("I be hearty and hale, matey!");
            }
            else if (speech.Contains("job"))
            {
                Say("Me job be huntin' for buried treasures, and defendin' me ship from scallywags!");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Ye be seekin' true adventure on the high seas, aye?");
            }
            else if (speech.Contains("unknown"))
            {
                Say("If ye be brave enough to face the unknown, ye may find the greatest treasures of all!");
            }
            else if (speech.Contains("tina"))
            {
                Say("Aye, I earned that name from the countless treasures I've found! But me most prized possession remains a mystery.");
            }
            else if (speech.Contains("hearty"))
            {
                Say("It be the sea air and the thrill of the hunt that keeps me spirits high and me bones sturdy.");
            }
            else if (speech.Contains("buried"))
            {
                Say("I've got tales of treasures I've discovered and some that eluded me. The most elusive be the Heart of the Ocean.");
            }
            else if (speech.Contains("seas"))
            {
                Say("The high seas be a place of wonder and danger. I've faced sea monsters, storms, and the ghost ship of Captain Blackbeard.");
            }
            else if (speech.Contains("brave"))
            {
                Say("If ye prove yerself brave and help me find the Heart of the Ocean, I'll reward ye handsomely.");
            }
            else if (speech.Contains("possession"))
            {
                Say("Me most prized possession be a map, said to lead to the Heart of the Ocean. But a crucial piece be missing.");
            }
            else if (speech.Contains("sea"))
            {
                Say("The sea air, with its salty tang and the call of the gulls, be a constant reminder of the freedom and danger that awaits.");
            }
            else if (speech.Contains("heart"))
            {
                Say("Legend says it be a gem so pure and blue, it contains the very soul of the sea. Many have sought it, few have returned.");
            }
            else if (speech.Contains("blackbeard"))
            {
                Say("Captain Blackbeard be a dreaded pirate, and his ghost ship still haunts these waters. Some say he knows the way to the Heart.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If ye help me find the missing piece of me map and locate the Heart, I promise ye a share of the treasure and a surprise gift from me own collection. A sample for ye.");
                    from.AddToBackpack(new RingSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public TreasureSeekingTina(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
