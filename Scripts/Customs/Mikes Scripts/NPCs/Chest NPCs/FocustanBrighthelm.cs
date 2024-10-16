using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Focustan Brighthelm")]
    public class FocustanBrighthelm : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FocustanBrighthelm() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Focustan Brighthelm";
            Body = 0x190; // Human male body
            Hue = Utility.RandomNeutralHue(); // Neutral hue for a mystical look
            Title = "the Focused Mage";

            // Equipment
            AddItem(new Robe(Utility.RandomBlueHue())); // Magical looking robe
            AddItem(new WizardsHat(Utility.RandomBlueHue())); // Wizards hat to match the robe
            AddItem(new Sandals());
            AddItem(new GoldRing());

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
                Say("I am Focustan Brighthelm, a mage dedicated to the art of concentration and focus.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thanks to my meditative practices.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to help those who seek to enhance their magical focus and abilities.");
            }
            else if (speech.Contains("focus"))
            {
                Say("Focus is the key to mastering any magical art. It enhances your concentration and power.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic flows through us all. It is only with focus that one can truly harness its power.");
            }
            else if (speech.Contains("art"))
            {
                Say("The art of magic is complex, requiring both skill and unwavering focus.");
            }
            else if (speech.Contains("skill"))
            {
                Say("To master a skill, one must devote themselves fully and practice diligently.");
            }
            else if (speech.Contains("devote"))
            {
                Say("Devotion is the cornerstone of mastery. Without it, even the greatest talents are wasted.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("Mastery is achieved through a deep understanding and perfect execution of one's craft.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding comes from experience and a willingness to learn from every challenge.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience is a teacher like no other. It reveals the true nature of one's abilities.");
            }
            else if (speech.Contains("abilities"))
            {
                Say("Your abilities are reflections of your inner strength and focus.");
            }
            else if (speech.Contains("strength"))
            {
                Say("True strength lies in the ability to concentrate and direct one's willpower effectively.");
            }
            else if (speech.Contains("willpower"))
            {
                Say("Willpower is the force that drives your focus and determination.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination fuels your quest for improvement and mastery in any field.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Every quest is a journey towards greater knowledge and personal growth.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the treasure of the wise. It guides and enriches our lives.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Indeed, treasure can be both material and knowledge. Which kind do you seek?");
            }
            else if (speech.Contains("seek"))
            {
                Say("To seek is to embark on a journey with purpose and intent.");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Purpose drives our actions and gives meaning to our pursuits.");
            }
            else if (speech.Contains("pursuits"))
            {
                Say("Our pursuits are reflections of our deepest desires and aspirations.");
            }
            else if (speech.Contains("desires"))
            {
                Say("Desires are the sparks that ignite our passions and ambitions.");
            }
            else if (speech.Contains("ambitions"))
            {
                Say("Ambitions shape our path and determine our goals.");
            }
            else if (speech.Contains("goals"))
            {
                Say("Achieving goals requires focus, dedication, and a clear vision.");
            }
            else if (speech.Contains("vision"))
            {
                Say("A clear vision guides us through challenges and helps us stay on course.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Challenges test our resolve and help us grow stronger.");
            }
            else if (speech.Contains("grow"))
            {
                Say("Growth is a continuous process of learning and adaptation.");
            }
            else if (speech.Contains("learning"))
            {
                Say("Learning is the key to personal and intellectual development.");
            }
            else if (speech.Contains("intellectual"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must return later for another reward.");
                }
                else
                {
                    Say("You have shown a keen interest in mastering focus. Accept this chest, which contains items to aid you in your quest.");
                    from.AddToBackpack(new FocusBonusChest()); // Reward the player
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public FocustanBrighthelm(Serial serial) : base(serial) { }

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
