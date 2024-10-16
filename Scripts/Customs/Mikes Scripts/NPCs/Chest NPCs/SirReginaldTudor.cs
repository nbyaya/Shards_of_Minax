using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald Tudor")]
    public class SirReginaldTudor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldTudor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Tudor";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            Hue = Race.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2042, 0x203B); // Short hair
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = HairHue;

            SpeechHue = 0; // Default speech hue

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
                Say("Greetings, I am Sir Reginald Tudor, a noble of the Tudor lineage.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the prime of my health, prepared to assist those worthy.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect the honor of the Tudor dynasty and its treasures.");
            }
            else if (speech.Contains("tudor"))
            {
                Say("Ah, the Tudors! A lineage of great power and intrigue. Our history is filled with grand tales and rich legacies.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, the Tudor treasure is legendary. Those who prove themselves worthy may seek out its secrets.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, you must demonstrate your knowledge and resolve. Answer wisely and you may be rewarded.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to many mysteries. Seek wisdom and you shall find the path to your reward.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is essential for great deeds. Only those who remain steadfast in their quest will find success.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Great deeds are often born from great trials. Show me that you are prepared for such trials.");
            }
            else if (speech.Contains("trials"))
            {
                Say("The trials of a noble are many. Each challenge is a step towards greater honor.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a quality that defines our legacy. It is earned through actions and decisions.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Actions speak louder than words. It is through your actions that you will prove your worthiness.");
            }
            else if (speech.Contains("words"))
            {
                Say("Words can inspire, but it is the truth of your actions that will determine your fate.");
            }
            else if (speech.Contains("fate"))
            {
                Say("Fate is shaped by the choices we make. Your path to reward lies in your perseverance.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the key to overcoming obstacles. Stay determined, and you will find the path to your reward.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are but temporary hindrances. Your strength and resolve will guide you through.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not only physical but also mental. Your willpower will see you through the toughest challenges.");
            }
            else if (speech.Contains("willpower"))
            {
                Say("Willpower is the driving force behind success. Harness it, and you will achieve great things.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is a reward for those who are truly deserving. Your journey has been commendable.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its trials, but it is the end that counts. Your perseverance has not gone unnoticed.");
            }
            else if (speech.Contains("end"))
            {
                Say("The end of your journey brings reward. You have shown great dedication and resolve.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return later for another chance.");
                }
                else
                {
                    Say("For your perseverance and dedication, I present you with the Tudor Dynasty chest.");
                    from.AddToBackpack(new TudorDynastyChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Your inquiry is most intriguing. Do continue to seek the wisdom of the Tudor legacy.");
            }

            base.OnSpeech(e);
        }

        public SirReginaldTudor(Serial serial) : base(serial) { }

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
