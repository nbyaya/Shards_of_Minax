using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grillmaster Gary")]
    public class GrillmasterGary : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrillmasterGary() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grillmaster Gary";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 55;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 58 }); // Clothing item with hue 58
            AddItem(new Tunic() { Hue = 295 });    // Clothing item with hue 295
            AddItem(new Boots() { Hue = 2426 });   // Boots with hue 2426
            AddItem(new LeatherGloves() { Name = "Gary's Grilling Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Welcome to my grill! I am Grillmaster Gary, the master of the flames.");
            }
            else if (speech.Contains("health"))
            {
                Say("As for my health, I feel as fiery as my grill!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm the maestro of the grill, turning raw ingredients into culinary delights!");
            }
            else if (speech.Contains("passion"))
            {
                Say("True flavor is not just in the ingredients but in the passion of the cook! Do you appreciate the art of cooking?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you are a kindred spirit! Cooking is not just about feeding the body but also the soul.");
            }
            else if (speech.Contains("flames"))
            {
                Say("Ever since I was a child, I was fascinated by flames. There's a certain dance they perform that is truly mesmerizing.");
            }
            else if (speech.Contains("fiery"))
            {
                Say("The fire within me is more than just for cooking, it's a spirit that keeps me going. Every day, I make sure to tend it, to keep it alive.");
            }
            else if (speech.Contains("culinary"))
            {
                Say("Culinary arts require a balance of skill, artistry, and knowledge. Sometimes, I experiment with rare ingredients to create new dishes.");
            }
            else if (speech.Contains("art"))
            {
                Say("Art in cooking is not just about taste, but also presentation. It's a whole experience. Have you ever tried creating a dish from scratch?");
            }
            else if (speech.Contains("presentation"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("When you plate your food in an appealing way, it speaks volumes about your dedication. Tell you what, for your keen interest, take this!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("child"))
            {
                Say("My parents used to take me to the town festivals where they had these grand bonfires. I think that's where my love for flames began.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of cooking is intertwined with the memories and emotions we attach to food. Every dish tells a story.");
            }
            else if (speech.Contains("ingredients"))
            {
                Say("Over the years, I've gathered rare ingredients from distant lands. Some have unique flavors, others have magical properties.");
            }

            base.OnSpeech(e);
        }

        public GrillmasterGary(Serial serial) : base(serial) { }

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
