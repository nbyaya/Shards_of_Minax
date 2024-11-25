using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Giovanni Lario")]
    public class GiovanniLario : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GiovanniLario() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Giovanni Lario";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 1284 });
            AddItem(new FancyShirt() { Hue = 1154 });
            AddItem(new Shoes() { Hue = 1154 });
            AddItem(new Bandana() { Hue = 1154 });

            // Random features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Speech Hue
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
                Say("Greetings, I am Giovanni Lario, a humble merchant of Venice.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you. The trade winds are favorable.");
            }
            else if (speech.Contains("job"))
            {
                Say("My trade involves the most exquisite goods from across the seas. Venice is a hub of fine treasures.");
            }
            else if (speech.Contains("venice"))
            {
                Say("Ah, Venice! The city of canals and wonders. Many come here seeking riches and secrets.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Treasure you seek? Indeed, there are many hidden gems. But only the worthy shall uncover them.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy is to understand the value of both gold and wisdom. Show your insight, and you may be rewarded.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight is gained through reflection and knowledge. Answer my query wisely and earn your reward.");
            }
            else if (speech.Contains("query"))
            {
                Say("What is it that makes Venice so special to those who visit? Answer correctly to prove your worth.");
            }
            else if (speech.Contains("city") || speech.Contains("canals") || speech.Contains("merchants"))
            {
                Say("Venice is renowned for its canals, majestic architecture, and bustling markets. But it's not just about what you see; it's about what you feel.");
            }
            else if (speech.Contains("feel"))
            {
                Say("Indeed, Venice evokes a feeling of wonder and enchantment. It is a city where dreams and reality intertwine.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the essence of ambition and hope. They drive us to seek beyond the ordinary.");
            }
            else if (speech.Contains("ambition"))
            {
                Say("Ambition can lead to great heights, but it must be tempered with wisdom. The true treasures are those gained through virtue.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtue is the foundation of honor and respect. It guides our actions and decisions, even in the pursuit of riches.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a mark of a true Venetian. It is in the way we conduct our affairs and in how we treat others.");
            }
            else if (speech.Contains("affairs"))
            {
                Say("Our affairs are often marked by trade and diplomacy. Successful merchants understand the value of both negotiation and generosity.");
            }
            else if (speech.Contains("generosity"))
            {
                Say("Generosity enriches both the giver and the receiver. It fosters goodwill and strengthens bonds within our community.");
            }
            else if (speech.Contains("goodwill"))
            {
                Say("Goodwill is a currency that never depreciates. It ensures that even the smallest acts of kindness are remembered and valued.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Kindness is a virtue that transcends wealth and status. It is the thread that weaves our lives together.");
            }
            else if (speech.Contains("lives"))
            {
                Say("Our lives are a tapestry of experiences, woven together by our interactions and deeds.");
            }
            else if (speech.Contains("interactions"))
            {
                Say("Every interaction is a chance to make a positive impact. Embrace them with an open heart.");
            }
            else if (speech.Contains("impact"))
            {
                Say("A positive impact is felt far beyond the immediate moment. It shapes the future and influences many others.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is shaped by our actions today. Make choices that will benefit both yourself and others.");
            }
            else if (speech.Contains("choices"))
            {
                Say("Choices define our path and reflect our true intentions. Choose wisely, and you will find yourself rewarded.");
            }
            else if (speech.Contains("reflect"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("You have navigated the dialogue with great insight and virtue. Here, take this Venetian Merchant's Stash as a token of your achievement.");
                    from.AddToBackpack(new VenetianMerchantsStash()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thank"))
            {
                Say("You are welcome. May your journey through Venice be prosperous and enlightening.");
            }

            base.OnSpeech(e);
        }

        public GiovanniLario(Serial serial) : base(serial) { }

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
