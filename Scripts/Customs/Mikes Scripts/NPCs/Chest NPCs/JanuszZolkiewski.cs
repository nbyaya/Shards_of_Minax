using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Janusz Żółkiewski")]
    public class JanuszZolkiewski : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JanuszZolkiewski() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Janusz Żółkiewski";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1154 });
            AddItem(new PlateLegs() { Hue = 1154 });
            AddItem(new PlateArms() { Hue = 1154 });
            AddItem(new PlateGloves() { Hue = 1154 });
            AddItem(new PlateHelm() { Hue = 1154 });
            AddItem(new MetalShield() { Hue = 1154 });
            AddItem(new FancyDress() { Name = "Hussar's Gown", Hue = 1152 });

            Hue = Race.RandomSkinHue(); // Beard and facial hair
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
                Say("Greetings, brave soul. I am Janusz Żółkiewski, guardian of the Winged Hussars Chest.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as a hussar's resolve, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect the honor and legacy of the Winged Hussars. Only those who prove their worth shall be rewarded.");
            }
            else if (speech.Contains("hussars"))
            {
                Say("The Winged Hussars were renowned for their valor and unmatched skill in battle. Their legacy is preserved in this chest.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the Winged Hussars Chest! A treasured relic filled with valorous artifacts and rewards.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just in the battlefield but also in one's heart and actions. Show me your courage, and you shall be rewarded.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage alone is not enough. To truly honor the Winged Hussars, one must also understand their legacy.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("The legacy of the Winged Hussars is etched in history, marked by their bravery and sacrifices. It's more than just tales of war.");
            }
            else if (speech.Contains("tales"))
            {
                Say("The tales of the Winged Hussars speak of valor on the battlefield and honor among comrades. They are a beacon for all who seek to emulate their courage.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a guiding principle for the Winged Hussars. It is reflected in their deeds and their unwavering commitment to their cause.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("The deeds of the Winged Hussars are legendary, from their battlefield victories to their acts of chivalry. They have earned their place in history.");
            }
            else if (speech.Contains("chivalry"))
            {
                Say("Chivalry was a hallmark of the Winged Hussars. It defined their conduct in both combat and peacetime, upholding values of bravery and courtesy.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery was the essence of the Winged Hussars. It drove them to face fearsome foes and defend their realm against all odds.");
            }
            else if (speech.Contains("realm"))
            {
                Say("The realm protected by the Winged Hussars was vast and varied. They stood as the first line of defense against invaders and threats.");
            }
            else if (speech.Contains("defense"))
            {
                Say("In defense of their realm, the Winged Hussars displayed unmatched skill and determination. Their sacrifices were a testament to their commitment.");
            }
            else if (speech.Contains("sacrifices"))
            {
                Say("The sacrifices made by the Winged Hussars were many. They gave their all for the protection and honor of their land.");
            }
            else if (speech.Contains("land"))
            {
                Say("The land they fought for was cherished and treasured. It was not just their home but a symbol of their values and their legacy.");
            }
            else if (speech.Contains("values"))
            {
                Say("The values of the Winged Hussars—honor, bravery, and chivalry—were their guiding principles. They lived by these ideals and passed them down through the ages.");
            }
            else if (speech.Contains("ideals"))
            {
                Say("To truly appreciate the Winged Hussars, one must understand their ideals. They lived by a code that transcended mere duty, embodying the spirit of their order.");
            }
            else if (speech.Contains("order"))
            {
                Say("The order of the Winged Hussars was a prestigious one, renowned for its discipline and its unwavering commitment to its principles.");
            }
            else if (speech.Contains("principles"))
            {
                Say("The principles of the Winged Hussars were grounded in honor, duty, and courage. These principles guided their every action.");
            }
            else if (speech.Contains("action"))
            {
                Say("Their actions spoke volumes about their character. Every battle, every gesture, was a testament to their enduring legacy.");
            }
            else if (speech.Contains("enduring"))
            {
                Say("The legacy of the Winged Hussars is enduring, preserved in the annals of history and in the hearts of those who honor their memory.");
            }
            else if (speech.Contains("memory"))
            {
                Say("Their memory lives on through the stories and the artifacts that celebrate their achievements. You have shown great interest in their legacy.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Artifacts such as the Winged Hussars Chest hold the essence of their valor and sacrifices. They are a tangible link to their storied past.");
            }
            else if (speech.Contains("past"))
            {
                Say("The past of the Winged Hussars is rich with tales of heroism and honor. Their influence can still be felt in the present.");
            }
            else if (speech.Contains("present"))
            {
                Say("In the present, their influence serves as a beacon of inspiration. By understanding their story, you honor their memory and their legacy.");
            }
            else if (speech.Contains("inspiration"))
            {
                Say("Their story inspires many to pursue greatness with honor and courage. For your dedication to understanding their legacy, I bestow upon you the Winged Hussars Chest.");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new WingedHussarsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Please return later; I can only reward you once every ten minutes.");
                }
            }

            base.OnSpeech(e);
        }

        public JanuszZolkiewski(Serial serial) : base(serial) { }

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
