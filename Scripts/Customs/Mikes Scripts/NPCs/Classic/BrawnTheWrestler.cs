using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Brawn the Wrestler")]
    public class BrawnTheWrestler : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrawnTheWrestler() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brawn the Wrestler";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 70;
            Hits = 130;

            // Appearance
            AddItem(new ShortPants(2126));
            AddItem(new FancyShirt(2126));
            AddItem(new ThighBoots(1904));
            AddItem(new BodySash { Name = "Brawn's Championship Belt" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Brawn the Wrestler!");
            }
            else if (speech.Contains("health"))
            {
                Say("Fit as a fiddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I wrestle and train the youth!");
            }
            else if (speech.Contains("wrestling"))
            {
                Say("Wrestling is not just about might. It's about wit, determination, and heart. Do you have heart?");
            }
            else if (speech.Contains("yes"))
            {
                Say("That's the spirit! Always fight with passion and never give up!");
            }
            else if (speech.Contains("no"))
            {
                Say("One must find their heart before they can truly wrestle.");
            }
            else if (speech.Contains("brawn"))
            {
                Say("Ah, you've heard of me? They say I've the strength of ten men and the speed of a panther!");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("The fiddle is an interesting instrument, isn't it? I once met a bard who played a tune so enchanting it made me question the nature of honesty.");
            }
            else if (speech.Contains("youth"))
            {
                Say("The youth of today are our future. I teach them not only to be strong in body, but also in mind and spirit.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination is key in wrestling, but also in understanding the deeper truths of life. Speaking of truths, have you ever pondered the mantra of Honesty? The first syllable is ZIM.");
            }
            else if (speech.Contains("strength"))
            {
                Say("True strength isn't about how hard you can hit, but how hard you can get hit and keep moving forward.");
            }
            else if (speech.Contains("bard"))
            {
                Say("Ah, that bard I met had a lute made from a rare wood. He said it was from the lost forest of Elara.");
            }
            else if (speech.Contains("teach"))
            {
                Say("I teach the youngsters more than just moves; I teach them values, discipline, and the importance of hard work.");
            }

            base.OnSpeech(e);
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
            {
                from.AddToBackpack(new BeltSlotChangeDeed()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                Say("For your thoughtful inquiry, please accept this reward.");
            }
            else
            {
                Say("I have no reward right now. Please return later.");
            }
        }

        public BrawnTheWrestler(Serial serial) : base(serial) { }

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
