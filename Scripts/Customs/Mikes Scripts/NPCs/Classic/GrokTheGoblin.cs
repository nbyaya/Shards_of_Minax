using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grok the Goblin")]
    public class GrokTheGoblin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrokTheGoblin() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grok the Goblin";
            Body = 0x14; // Goblin body

            // Stats
            Str = 120;
            Dex = 70;
            Int = 50;
            Hits = 80;

            // Appearance
            Hue = 1160;
            AddItem(new ShortPants() { Hue = 1161 });
            AddItem(new Shirt() { Hue = 1161 });
            AddItem(new LeatherCap() { Hue = 1162 });
            AddItem(new Dagger() { Name = "Grok's Dagger" });

            HairItemID = 0x204F; // Hair ID for Goblin
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
                Say("Me Grok, mighty Goblin!");
            }
            else if (speech.Contains("health"))
            {
                Say("Grok in good health!");
            }
            else if (speech.Contains("job"))
            {
                Say("Grok guards the Goblin cave!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor for Goblins is in strength and cunning! Are you strong?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then prove it! Tell Grok what you've done!");
            }
            else if (speech.Contains("mighty"))
            {
                Say("Me Grok, mightiest among Goblins! They call me chief!");
            }
            else if (speech.Contains("good"))
            {
                Say("Me feel good because Grok eat good food, like squishy human snacks!");
            }
            else if (speech.Contains("cave"))
            {
                Say("Inside cave, many treasures hidden! But not for weaklings! Maybe Grok share secret if you prove worthy.");
            }
            else if (speech.Contains("cunning"))
            {
                Say("Cunning like sneaking into human camps, stealing shinies! You sneak before?");
            }
            else if (speech.Contains("camps"))
            {
                Say("Yes! Human camps full of things Goblins like! You bring Grok something shiny from camp, Grok give you reward!");
            }
            else if (speech.Contains("prove"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To prove worthy, you bring Grok a rare gem from deep inside dungeon. Do this, and Grok might share cave secrets and give you reward!");
                    from.AddToBackpack(new MaxxiaScroll()); // Example reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("chief"))
            {
                Say("They call Grok chief because Grok smartest and strongest! Grok protect tribe! You meet other Goblins in tribe?");
            }
            else if (speech.Contains("snacks"))
            {
                Say("Snacks like juicy rats and spiders! But Grok favorite is roasted frog! You try?");
            }

            base.OnSpeech(e);
        }

        public GrokTheGoblin(Serial serial) : base(serial) { }

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
