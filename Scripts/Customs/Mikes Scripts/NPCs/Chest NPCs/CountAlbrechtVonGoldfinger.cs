using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Count Albrecht von Goldfinger")]
    public class CountAlbrechtVonGoldfinger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CountAlbrechtVonGoldfinger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Count Albrecht von Goldfinger";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 95;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new GoldBracelet() { Hue = Utility.RandomMetalHue() });
            AddItem(new FancyShirt() { Hue = Utility.RandomNondyedHue() });

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
                Say("Greetings, I am Count Albrecht von Goldfinger, guardian of rare treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as the finest steel. Thank you for your concern.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to safeguard the treasures of my house and share them with worthy adventurers.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, treasures! The heart of my duties. You must prove yourself to gain access to them.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Show me your worth, and the treasures of my chest shall be yours.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy is to understand the value of history and valor. Let us see if you possess such qualities.");
            }
            else if (speech.Contains("value"))
            {
                Say("Value is derived from understanding and respect. What do you know of valor?");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is more than bravery; it is the courage to act rightly despite fear. What is your role in this quest?");
            }
            else if (speech.Contains("role"))
            {
                Say("My role is to oversee the treasures and ensure they are bestowed upon the deserving. Do you seek the chest?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a journey of discovery. Have you proven your resolve?");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is tested through challenges. Have you encountered the chest of Teutonic treasures before?");
            }
            else if (speech.Contains("chest"))
            {
                Say("The chest is a repository of history and wealth. Are you familiar with the legends of Teutonic knights?");
            }
            else if (speech.Contains("legends"))
            {
                Say("Legends speak of valor and treasures beyond imagination. What can you tell me about these knights?");
            }
            else if (speech.Contains("knights"))
            {
                Say("Knights of old were bound by honor and duty. What qualities of honor do you possess?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor guides one's actions and decisions. Have you demonstrated such qualities in your endeavors?");
            }
            else if (speech.Contains("demonstrated"))
            {
                Say("Demonstrating worth involves actions and choices. What do you know about the Germanic history related to this chest?");
            }
            else if (speech.Contains("germanic"))
            {
                Say("Germanic history is rich with tales of bravery and lore. Can you recount a tale of bravery you know?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is the heart of a hero's spirit. Share with me a story of your own brave deeds.");
            }
            else if (speech.Contains("story"))
            {
                Say("Every tale of valor contributes to our legacy. Do you wish to claim the chest now?");
            }
            else if (speech.Contains("claim"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Please return later.");
                }
                else
                {
                    Say("You have shown great insight and valor. For your dedication, accept this chest filled with Teutonic treasures.");
                    from.AddToBackpack(new TeutonicTreasuresChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thank"))
            {
                Say("You are most welcome. May the treasures serve you well.");
            }

            base.OnSpeech(e);
        }

        public CountAlbrechtVonGoldfinger(Serial serial) : base(serial) { }

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
