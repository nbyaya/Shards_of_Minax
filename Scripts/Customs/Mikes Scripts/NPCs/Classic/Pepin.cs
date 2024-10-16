using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pepin")]
    public class Pepin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Pepin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pepin";
            Body = 0x190; // Human male body

            // Stats
            Str = 60;
            Dex = 50;
            Int = 120;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = 1155 });
            AddItem(new Sandals() { Hue = 0 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler! I am Pepin, the healer of Tristram.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, thanks to my knowledge of the healing arts.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job here in Tristram is to mend the wounded and cure ailments.");
            }
            else if (speech.Contains("virtue compassion"))
            {
                Say("I often ponder the virtue of compassion. To heal the wounded is to show true compassion.");
            }
            else if (speech.Contains("compassion yes") || speech.Contains("compassion no"))
            {
                Say("Do you hold compassion in your heart, traveler?");
            }
            else if (speech.Contains("tristram"))
            {
                Say("Aye, the once joyous and thriving village now hides under the shadow of the cathedral. Many brave souls have ventured there, only to never return.");
            }
            else if (speech.Contains("cathedral"))
            {
                Say("The cathedral was once a place of worship, but it's now believed to be cursed. Whispers speak of an ancient artifact hidden deep within.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("It's a relic from times long past. Some say it possesses the power to heal... or to destroy. If you ever find it, bring it to me. I'll make sure it's used for good.");
            }
            else if (speech.Contains("relic"))
            {
                Say("The relic is said to be a gem, glowing with a mystic light. Legend has it that it was the heart of a fallen star. Whoever holds it is granted untold power.");
            }
            else if (speech.Contains("gem"))
            {
                Say("If you indeed find the gem and bring it to me, in recognition of your compassion, I shall reward you generously.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("It's not gold nor silver I offer, but something more valuable. For your brave heart, I'll share a secret healing potion recipe, unknown to most.");
                    from.AddToBackpack(new AlchemyAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("potion"))
            {
                Say("My secret potion is crafted from moonlit herbs and crystal clear water. But its most vital ingredient is the essence of compassion, which I sense in you.");
            }

            base.OnSpeech(e);
        }

        public Pepin(Serial serial) : base(serial) { }

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
