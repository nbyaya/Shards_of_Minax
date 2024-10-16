using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Joan of Arc")]
    public class JoanOfArc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JoanOfArc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Joan of Arc";
            Body = 0x191; // Human female body

            // Stats
            Str = 140;
            Dex = 40;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 38 });
            AddItem(new PlateChest() { Hue = 38 });
            AddItem(new PlateHelm() { Hue = 38 });
            AddItem(new PlateGloves() { Hue = 38 });
            AddItem(new PlateArms() { Hue = 38 });
            AddItem(new PlateGorget() { Hue = 38 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new Halberd() { Name = "Lady Joan of Arc's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Joan of Arc, the Maid of Orléans!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern, for I am guided by a higher purpose.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am on a divine mission to free France from its oppressors!");
            }
            else if (speech.Contains("divine destiny"))
            {
                Say("The voices of angels guide my path. Do you believe in divine destiny?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your answer reveals much about your character. Proceed with courage, my friend.");
            }
            else if (speech.Contains("orleans"))
            {
                Say("Orléans was a significant victory for France. It showed that through unity and faith, we could triumph over adversity.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("This purpose is driven by my faith and the visions I receive from above. The heavens have chosen a path for me, and I cannot stray from it.");
            }
            else if (speech.Contains("oppressors"))
            {
                Say("Our land has suffered much, and our people have endured great hardships. But with courage and determination, we can reclaim what was taken.");
            }
            else if (speech.Contains("voices"))
            {
                Say("These voices, sent by God, guide my every move. They've shown me a path, one that not many would dare tread. But with their guidance, I remain unyielding.");
            }
            else if (speech.Contains("victory"))
            {
                Say("That victory was not just for me or the soldiers, but for all of France. It was a beacon of hope in our darkest times.");
            }
            else if (speech.Contains("visions"))
            {
                Say("My visions often show me battles and decisions I must make. Sometimes they're clear, other times shrouded in mystery. But I trust in their wisdom.");
            }
            else if (speech.Contains("reclaim"))
            {
                Say("There's an ancient relic, lost to the sands of time, that could aid our cause. If you could find it, I'd be in your debt.");
            }
            else if (speech.Contains("relic"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You have my gratitude for assisting in this noble quest. As a token of my appreciation, accept this reward. May it serve you well.");
                    from.AddToBackpack(new TacticsAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public JoanOfArc(Serial serial) : base(serial) { }

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
