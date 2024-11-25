using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Roderick Tricky")]
    public class RoderickTricky : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RoderickTricky() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Roderick Tricky";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 85;
            Hits = 60;

            // Appearance
            AddItem(new LeatherChest() { Hue = 1175 });
            AddItem(new LeatherLegs() { Hue = 1175 });
            AddItem(new LeatherGloves() { Hue = 1175 });
            AddItem(new LeatherCap() { Hue = 1175 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new Dagger() { Name = "Rogue's Delight", Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
            // Speech Hue
            SpeechHue = 1155; // Slightly different color for a mischievous tone

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
                Say("Ah, a curious soul! I am Roderick Tricky, master of stealth and deception.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Why, it's to keep my secrets well-guarded and to aid those who prove worthy.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are best kept in the dark. But I might share a morsel with someone who can handle it.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("To be worthy, one must demonstrate their cleverness and perseverance.");
            }
            else if (speech.Contains("cleverness"))
            {
                Say("Ah, cleverness is key to understanding the true nature of things. Show me your wit, and I might reward you.");
            }
            else if (speech.Contains("wit"))
            {
                Say("To prove your wit, you must solve a riddle. Are you ready?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Here is a riddle for you: I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?");
            }
            else if (speech.Contains("echo"))
            {
                Say("Correct! An echo it is. Now, let us continue. To proceed, you must speak of the shadows.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are the refuge of the clever. Tell me, what is it that hides in the shadows but reveals truth?");
            }
            else if (speech.Contains("truth"))
            {
                Say("Indeed, truth is what the shadows sometimes reveal. But let us move on. Speak to me of the elusive nature of thieves.");
            }
            else if (speech.Contains("thieves"))
            {
                Say("Thieves are crafty and elusive. They know the value of discretion. To earn my favor, you must understand this: what is the greatest treasure a thief seeks?");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The greatest treasure a thief seeks is often not gold but a secret well-guarded. Show me that you understand the value of secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, you do understand. For your perseverance and wit, you are ready for the final step. Speak to me now of the reward you seek.");
            }
            else if (speech.Contains("step"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience is a virtue. Return when the time is right.");
                }
                else
                {
                    Say("Your cleverness and patience have earned you a reward. Take this chest of secrets as a token of my appreciation.");
                    from.AddToBackpack(new RoguesHiddenChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("chest"))
            {
                Say("The chest holds many secrets. Use it wisely and may it aid you on your journey.");
            }

            base.OnSpeech(e);
        }

        public RoderickTricky(Serial serial) : base(serial) { }

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
