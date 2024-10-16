using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Chester McRad")]
    public class ChesterMcRad : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ChesterMcRad() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Chester McRad";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = RandomHue() });
            AddItem(new ShortPants() { Hue = RandomHue() });
            AddItem(new Sandals() { Hue = RandomHue() });
            AddItem(new Cap() { Hue = RandomHue() });

            // Random hair and beard
            HairItemID = Utility.RandomList(0x2044, 0x203C, 0x2041); // Various hair styles
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0; // No beard

            // Speech Hue
            SpeechHue = Utility.RandomNeutralHue(); 

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
                Say("Yo, dude! I'm Chester McRad, the master of '90s nostalgia!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm totally rad and in tip-top shape, just like my favorite '90s jams!");
            }
            else if (speech.Contains("job"))
            {
                Say("I dig up the raddest '90s relics and keep the retro vibes alive!");
            }
            else if (speech.Contains("relics"))
            {
                Say("Radical '90s relics are my specialty! Ever seen a Tamagotchi or Beanie Babies?");
            }
            else if (speech.Contains("tamagotchi"))
            {
                Say("Oh man, Tamagotchis were so cool! They were like pets you could keep in your pocket.");
            }
            else if (speech.Contains("beanie babies"))
            {
                Say("Beanie Babies were all the rage! Got a whole bag of them in my chest.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, my chest! It's packed with all the '90s treasures you'd ever need!");
            }
            else if (speech.Contains("treasures"))
            {
                Say("You've mentioned treasures! Let me tell you about some epic '90s toys.");
            }
            else if (speech.Contains("toys"))
            {
                Say("Classic '90s toys include Tamagotchis, Beanie Babies, and more. Did you know?");
            }
            else if (speech.Contains("know"))
            {
                Say("Did you know Beanie Babies were worth a lot in their time? Some rare ones are even more valuable now.");
            }
            else if (speech.Contains("valuable"))
            {
                Say("Indeed! Some '90s items are super collectible. But let's talk about something even cooler.");
            }
            else if (speech.Contains("cooler"))
            {
                Say("Cooler than Beanie Babies? That's tough! But how about a chest full of rad '90s stuff?");
            }
            else if (speech.Contains("stuff"))
            {
                Say("Rad '90s stuff like the items in my chest. It's a blast from the past!");
            }
            else if (speech.Contains("past"))
            {
                Say("Ah, the past! A time of neon colors and epic toys. Speaking of which...");
            }
            else if (speech.Contains("epic"))
            {
                Say("You got it! My chest is filled with epic '90s treasures. Prove your '90s knowledge and get a reward!");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Chill out, dude! Come back later for some more rad treasures.");
                }
                else
                {
                    Say("You've proven yourself to be a true '90s enthusiast! Here, take this Radical '90s Relics chest as a reward!");
                    from.AddToBackpack(new Radical90sRelicsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("true"))
            {
                Say("Absolutely true! The '90s were an awesome decade, and this chest is proof of that.");
            }
            else if (speech.Contains("decade"))
            {
                Say("Yes, the '90s were a fantastic decade. A time of incredible fashion and toys.");
            }
            else if (speech.Contains("fashion"))
            {
                Say("Fashion was wild in the '90s! Bright colors, baggy clothes, and so much more.");
            }
            else if (speech.Contains("colors"))
            {
                Say("Bright colors were everywhere! Neon greens, pinks, and blues were all the rage.");
            }
            else if (speech.Contains("neon"))
            {
                Say("Neon colors, yes! Like the glow of a Tamagotchi or the sparkle of a Beanie Baby.");
            }
            else if (speech.Contains("glow"))
            {
                Say("The glow of a Tamagotchi was so mesmerizing. It was like a tiny, digital pet.");
            }
            else if (speech.Contains("digital"))
            {
                Say("Digital pets like Tamagotchis were all the rage. It's amazing how technology evolved.");
            }

            base.OnSpeech(e);
        }

        private int RandomHue()
        {
            return Utility.RandomList(Utility.RandomPinkHue(), Utility.RandomBlueHue(), Utility.RandomGreenHue(), Utility.RandomOrangeHue(), Utility.RandomRedHue());
        }

        public ChesterMcRad(Serial serial) : base(serial) { }

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
