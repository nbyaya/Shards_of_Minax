using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Marcus")]
    public class EmperorMarcus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorMarcus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Marcus";
            Body = 0x190; // Human male body

            // Stats
            Str = 135;
            Dex = 75;
            Int = 80;
            Hits = 95;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1428 });
            AddItem(new ChainChest() { Hue = 1428 });
            AddItem(new ChainCoif() { Hue = 1428 });
            AddItem(new PlateGloves() { Hue = 1428 });
            AddItem(new Longsword() { Name = "Emperor Marcus' Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Set direction
            this.Direction = Direction.North;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Emperor Marcus glares at you with disdain.");
            }
            else if (speech.Contains("health"))
            {
                Say("Emperor Marcus scoffs, 'Health? What's it to you?'");
            }
            else if (speech.Contains("job"))
            {
                Say("Emperor Marcus sneers, 'My job? I rule this wretched land, if you must know.'");
            }
            else if (speech.Contains("battles"))
            {
                Say("Emperor Marcus grumbles, 'True power lies in manipulation, not valor. But enough about me, what do you want?'");
            }
            else if (speech.Contains("yes"))
            {
                Say("Emperor Marcus raises an eyebrow, 'So, do you have a spine, or are you just another pawn in my grand scheme?'");
            }
            else if (speech.Contains("disdain"))
            {
                Say("Emperor Marcus sighs, 'Disdain? It is merely the weight of ruling such a vast empire. It wears upon the soul.'");
            }
            else if (speech.Contains("scoff"))
            {
                Say("Emperor Marcus admits, 'Though I scoff, the weight of the crown has taken a toll on my well-being. Yet, I have remedies and secret potions to keep me strong.'");
            }
            else if (speech.Contains("wretched"))
            {
                Say("Emperor Marcus explains, 'Wretched because of the rebels and traitors lurking in the shadows, always plotting against my reign. Yet, I have loyal followers that keep me informed.'");
            }
            else if (speech.Contains("empire"))
            {
                Say("Emperor Marcus ponders, 'My empire is vast and full of secrets. There are many hidden treasures within its confines. Help me with a task, and I might just share one with you.'");
            }
            else if (speech.Contains("remedies"))
            {
                Say("Emperor Marcus smirks, 'My remedies are crafted by the best alchemists in the land. If you prove your loyalty, I might consider sharing one.'");
            }
            else if (speech.Contains("rebels"))
            {
                Say("Emperor Marcus scowls, 'These rebels are a thorn in my side. If you help me quash their uprising, you shall be handsomely rewarded.'");
            }
            else if (speech.Contains("task"))
            {
                Say("Emperor Marcus leans closer, 'There's a rogue sorcerer undermining my rule. Find and stop him, and you will earn my favor and an unspecified reward.'");
            }
            else if (speech.Contains("alchemists"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("Emperor Marcus nods, 'I have no reward right now. Please return later.'");
                }
                else
                {
                    Say("Emperor Marcus nods, 'My alchemists are second to none, always experimenting with new brews and potions. They serve me well. Take this example of their work.'");
                    from.AddToBackpack(new ColdHitAreaCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("uprising"))
            {
                Say("Emperor Marcus growls, 'The uprising stems from a village to the west. It seems they don't appreciate the stability I bring. Maybe you could persuade them otherwise?'");
            }

            base.OnSpeech(e);
        }

        public EmperorMarcus(Serial serial) : base(serial) { }

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
