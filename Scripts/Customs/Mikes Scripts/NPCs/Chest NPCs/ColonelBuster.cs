using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Colonel Buster")]
    public class ColonelBuster : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ColonelBuster() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Colonel Buster";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 70;
            Int = 80;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x203C; // Beard

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
                Say("Salutations! I am Colonel Buster, a relic of wartime glory.");
            }
            else if (speech.Contains("glory"))
            {
                Say("Glory is earned through valor and perseverance. What is it you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a quest. What do you seek in these turbulent times?");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest often reveals one's true character. Are you prepared to face challenges?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Being prepared is crucial. It means having the resolve to face whatever comes.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the heart of bravery. It sustains you through trials and tribulations.");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is not the absence of fear but the strength to face it. Show me your bravery!");
            }
            else if (speech.Contains("show"))
            {
                Say("To show something is to reveal its essence. Prove your worthiness!");
            }
            else if (speech.Contains("worthiness"))
            {
                Say("Worthiness is earned through actions and deeds. Demonstrate yours to earn the reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("The reward is a symbol of your efforts. If you have shown worthiness, it will be yours.");
            }
            else if (speech.Contains("efforts"))
            {
                Say("Efforts are the steps we take towards a goal. Each effort brings you closer to the reward.");
            }
            else if (speech.Contains("goal"))
            {
                Say("A goal gives us direction and purpose. What is your goal in this endeavor?");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose drives us forward. Without it, we lose our way. Define your purpose clearly.");
            }
            else if (speech.Contains("define"))
            {
                Say("To define is to establish clearly. If you can define your goal, you are one step closer to achieving it.");
            }
            else if (speech.Contains("achieving"))
            {
                Say("Achieving success is a culmination of effort and resolve. Demonstrate both to claim your reward.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is a testament to perseverance. If you seek success, prove your resolve.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is key to overcoming obstacles. Show me your perseverance to earn the reward.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are challenges to overcome. Facing them with perseverance reveals your true strength.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength comes from within. Prove your inner strength, and you will earn your reward.");
            }
            else if (speech.Contains("inner"))
            {
                Say("Inner strength is the foundation of true courage. Show me your inner strength.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is not the absence of fear but the ability to face it. Prove your courage to earn your reward.");
            }
            else if (speech.Contains("face"))
            {
                Say("To face something is to confront it head-on. Show me your ability to face challenges.");
            }
            else if (speech.Contains("confront"))
            {
                Say("To confront is to address directly. If you can confront challenges with courage, you are worthy of the reward.");
            }
            else if (speech.Contains("worthy"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your worthiness has been proven. For your bravery and perseverance, accept this World War II chest as a token of honor.");
                    from.AddToBackpack(new WorldWarIIChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ColonelBuster(Serial serial) : base(serial) { }

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
