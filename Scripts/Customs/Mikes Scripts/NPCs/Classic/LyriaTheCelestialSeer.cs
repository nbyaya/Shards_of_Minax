using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Celestial Seer")]
    public class LyriaTheCelestialSeer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaTheCelestialSeer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Celestial Seer";
            Body = 0x191; // Human female body

            // Stats
            Str = 65;
            Dex = 70;
            Int = 145;
            Hits = 85;

            // Appearance
            AddItem(new Robe() { Hue = 1177 }); // Robe with hue 1177
            AddItem(new Boots() { Hue = 1176 }); // Boots with hue 1176
            AddItem(new Cloak() { Name = "Lyria's Divination" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Lyria the Celestial Seer, a being far removed from your mortal realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? In this form, I know not of such mortal concerns.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role, if you can call it that, is to observe your world from the celestial realm and occasionally offer cryptic guidance.");
            }
            else if (speech.Contains("struggles"))
            {
                Say("Do you mortals ever tire of your endless struggles and trivial pursuits?");
            }
            else if (speech.Contains("wisdom") || speech.Contains("distractions"))
            {
                Say("Ah, I see your curiosity remains undiminished. Tell me, do you seek wisdom or merely distractions?");
            }
            else if (speech.Contains("distractions"))
            {
                Say("Distractions, you say? Very well, mortal. Seek your amusements and let the heavens weep at your folly.");
            }
            else if (speech.Contains("celestial"))
            {
                Say("The celestial realm is an expanse beyond your understanding, filled with energies and beings you could scarcely imagine.");
            }
            else if (speech.Contains("mortal"))
            {
                Say("Mortality is both a blessing and a curse. It grants you purpose, but it also blinds you to the vastness of the cosmos.");
            }
            else if (speech.Contains("guidance"))
            {
                Say("At times, I may offer a rare artifact or insight to those who truly seek knowledge. Show me you are deserving, and you might receive a reward.");
            }
            else if (speech.Contains("amusements"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In seeking amusements, you might miss the greater mysteries of life. But here, take this as a token of the unknown.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("pursuits"))
            {
                Say("From up here, your pursuits seem so tiny, yet they define your entire existence. It's a unique duality only mortals can experience.");
            }
            else if (speech.Contains("cosmos"))
            {
                Say("The cosmos is vast and timeless. Your world is but a speck in its grand design. Yet every speck has its purpose.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Artifacts from the celestial realm are infused with energies unknown to your kind. They can be a boon or a bane, depending on the wielder.");
            }

            base.OnSpeech(e);
        }

        public LyriaTheCelestialSeer(Serial serial) : base(serial) { }

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
