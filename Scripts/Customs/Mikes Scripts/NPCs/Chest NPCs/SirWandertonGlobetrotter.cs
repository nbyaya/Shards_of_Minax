using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Wanderton Globetrotter")]
    public class SirWandertonGlobetrotter : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirWandertonGlobetrotter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Wanderton Globetrotter";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new WideBrimHat() { Name = "Explorer's Hat", Hue = 0xA2 });
            AddItem(new LeatherChest() { Name = "Adventurer's Vest", Hue = 0x8A });
            AddItem(new LeatherLegs() { Name = "Explorer's Pants", Hue = 0x8A });
            AddItem(new Boots() { Name = "Traveling Boots", Hue = 0x8A });
            AddItem(new Backpack());
            
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x204B);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203C, 0x204C);

            SpeechHue = 0;

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
                Say("Ah, greetings! I am Sir Wanderton Globetrotter, at your service. Have you ever heard of my travels?");
            }
            else if (speech.Contains("travels"))
            {
                Say("My travels have taken me to many strange and wondrous places. Do you seek any particular adventure?");
            }
            else if (speech.Contains("adventure"))
            {
                Say("Every adventure starts with a journey. Have you ever wondered about the mysteries of old maps?");
            }
            else if (speech.Contains("maps"))
            {
                Say("Maps are like gateways to new adventures. I carry many maps in my backpack. What kind of treasure are you seeking?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasures! They are hidden in many forms. To find the right treasure, one must understand the art of exploration. What have you learned about exploration?");
            }
            else if (speech.Contains("exploration"))
            {
                Say("Exploration is not just about finding new lands, but also about discovering the stories they hold. Have you heard any tales of old explorers?");
            }
            else if (speech.Contains("tales"))
            {
                Say("Indeed, tales of old explorers are filled with wisdom and secrets. Speaking of secrets, do you know the importance of perseverance in a quest?");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is key to unlocking great rewards. If you have the perseverance of a true adventurer, you might be worthy of a special prize. What do you think about ancient artifacts?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Ancient artifacts hold the history and magic of ages past. To truly appreciate them, one must be willing to explore the unknown. Are you ready to prove your worth?");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must demonstrate your knowledge and spirit. Tell me, what do you value most in a quest?");
            }
            else if (speech.Contains("value"))
            {
                Say("Value lies in the journey and the lessons learned along the way. For your dedication, you might be rewarded with something special. Are you interested in exploring hidden places?");
            }
            else if (speech.Contains("hidden"))
            {
                Say("Hidden places often hold the greatest treasures. For those who seek them with true intent, rewards await. But first, tell me, have you ever encountered any legendary explorers?");
            }
            else if (speech.Contains("legendary"))
            {
                Say("Legendary explorers are often the stuff of myth and legend. They are known for their courage and insight. Do you believe you have what it takes to follow in their footsteps?");
            }
            else if (speech.Contains("footsteps"))
            {
                Say("Walking in the footsteps of legends requires both bravery and wisdom. If you can show me you have both, I might have a special reward for you. Are you ready for a challenge?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are what make an adventure worthwhile. To meet this challenge, you must first demonstrate your curiosity and courage. What kind of quest do you find most intriguing?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is a journey with purpose. If you are intrigued by such things, then you might be ready for a unique reward. Tell me, how do you prepare for an adventure?");
            }
            else if (speech.Contains("prepare"))
            {
                Say("Preparation is crucial for any successful adventure. If you are well-prepared, then you may be deserving of a special prize. Are you ready to receive it?");
            }
            else if (speech.Contains("receive"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at the moment. Please return later.");
                }
                else
                {
                    Say("You have shown great curiosity and spirit. For your dedication, I present to you the Traveler's Chest. May it aid you in your future journeys!");
                    from.AddToBackpack(new TravelerChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirWandertonGlobetrotter(Serial serial) : base(serial) { }

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
