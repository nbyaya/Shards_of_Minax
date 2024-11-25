using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Felix Tradewell")]
    public class FelixTradewell : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokeToAboutTrade;
        private bool spokeToAboutCommerce;
        private bool spokeToAboutChest;
        private bool spokeToAboutReward;

        [Constructable]
        public FelixTradewell() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Felix Tradewell";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new BodySash() { Hue = Utility.RandomMetalHue() });
            AddItem(new Cap() { Hue = Utility.RandomMetalHue() });

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize state
            lastRewardTime = DateTime.MinValue;
            spokeToAboutTrade = false;
            spokeToAboutCommerce = false;
            spokeToAboutChest = false;
            spokeToAboutReward = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, greetings! I am Felix Tradewell, at your service.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as good as a well-kept ledger! How can I assist you today?");
            }
            else if (speech.Contains("job"))
            {
                Say("I manage the finest trade goods in these lands. My job is to ensure everyone gets their fair share.");
            }
            else if (speech.Contains("trade"))
            {
                if (spokeToAboutTrade)
                {
                    Say("We've talked about trade before. Is there something more you wish to discuss?");
                }
                else
                {
                    Say("Trade is the lifeblood of commerce. Tell me, what brings you to my humble trade post?");
                    spokeToAboutTrade = true;
                }
            }
            else if (speech.Contains("commerce"))
            {
                if (spokeToAboutTrade)
                {
                    Say("Commerce thrives on trust and good relations. Your honesty is most appreciated. Do you seek more about trade?");
                    spokeToAboutCommerce = true;
                }
                else
                {
                    Say("Commerce is crucial to the prosperity of any region. You must understand trade to appreciate it.");
                }
            }
            else if (speech.Contains("chest"))
            {
                if (spokeToAboutCommerce)
                {
                    Say("Ah, the Merchant’s Chest! It holds many secrets and treasures. But before I give it to you, I must be sure you understand commerce well.");
                    spokeToAboutChest = true;
                }
                else
                {
                    Say("The chest you seek is filled with trade goods. You must first prove your knowledge of commerce.");
                }
            }
            else if (speech.Contains("prosperity"))
            {
                if (spokeToAboutChest)
                {
                    Say("For your keen interest in trade and commerce, I present you with this Merchant’s Chest. May it serve you well!");
                    if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                    {
                        from.AddToBackpack(new MerchantChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        spokeToAboutReward = true;
                    }
                    else
                    {
                        Say("I have no reward for you at this moment. Please return later.");
                    }
                }
                else
                {
                    Say("You must first prove your understanding of trade and commerce before receiving a reward.");
                }
            }
            else if (speech.Contains("appreciate"))
            {
                if (spokeToAboutCommerce)
                {
                    Say("Appreciation is key to good relations. If you truly appreciate trade, you will find rewards in understanding it.");
                }
                else
                {
                    Say("To appreciate is to understand. Have you learned about trade yet?");
                }
            }
            else if (speech.Contains("secrets"))
            {
                if (spokeToAboutChest)
                {
                    Say("The chest contains many secrets of the trade. But remember, the greatest secrets are those you uncover yourself.");
                }
                else
                {
                    Say("Secrets of trade are not given lightly. You must understand commerce first.");
                }
            }
            else if (speech.Contains("understand"))
            {
                if (spokeToAboutCommerce)
                {
                    Say("To understand commerce is to see the intricate dance of trade. Have you learned the values I spoke of?");
                }
                else
                {
                    Say("Understanding comes with time and experience. Have you spoken about trade and commerce yet?");
                }
            }
            else if (speech.Contains("learn"))
            {
                if (spokeToAboutCommerce)
                {
                    Say("Learning about trade and commerce opens doors to many opportunities. Keep asking questions and you'll unlock more.");
                }
                else
                {
                    Say("Learning is a journey. Start by discussing trade and commerce with me.");
                }
            }
            else if (speech.Contains("values"))
            {
                if (spokeToAboutCommerce)
                {
                    Say("The values of trade and commerce include honesty, fairness, and the pursuit of mutual benefit. Do you understand these values?");
                }
                else
                {
                    Say("Values are important. Have you discussed trade and commerce with me to learn more?");
                }
            }

            base.OnSpeech(e);
        }

        public FelixTradewell(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokeToAboutTrade);
            writer.Write(spokeToAboutCommerce);
            writer.Write(spokeToAboutChest);
            writer.Write(spokeToAboutReward);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokeToAboutTrade = reader.ReadBool();
            spokeToAboutCommerce = reader.ReadBool();
            spokeToAboutChest = reader.ReadBool();
            spokeToAboutReward = reader.ReadBool();
        }
    }
}
