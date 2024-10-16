using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nox Nora")]
    public class NoxNora : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NoxNora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nox Nora";
            Body = 0x190; // Human male body
            
            // Stats
            Str = 125;
            Dex = 82;
            Int = 75; // Default Int, you can adjust as needed
            Hits = 82;

            // Appearance
            AddItem(new LeatherChest() { Hue = 1260 });
            AddItem(new LeatherGorget() { Hue = 1260 });
            AddItem(new Kryss()); // Ensure you have this item type or adjust accordingly
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Nox Nora the Rogue!");
            }
            else if (speech.Contains("health"))
            {
                Say("My wounds are but scratches!");
            }
            else if (speech.Contains("job"))
            {
                Say("I walk the path of shadows and secrets.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("True virtue lies in understanding the thin line between right and wrong. Do you comprehend?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then you have glimpsed the path I tread. Keep your secrets well.");
            }
            else if (speech.Contains("rogue"))
            {
                Say("You seem curious about rogues. We are masters of stealth and deception, skilled in the arts of theft and subterfuge.");
            }
            else if (speech.Contains("scratches"))
            {
                Say("These scratches were from my last encounter with a rival rogue. It's a dangerous life we lead.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are a rogue's best ally. They hide us, protect us, and sometimes hold ancient secrets.");
            }
            else if (speech.Contains("stealth"))
            {
                Say("Stealth is not just about moving quietly. It's about understanding your environment and using it to your advantage. Do you seek to learn?");
            }
            else if (speech.Contains("rival"))
            {
                Say("My rival, Lysa, is a formidable rogue. She believes in a different path, one that I cannot agree with.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are the currency of rogues. Share a secret of yours, and perhaps I'll share one in return.");
            }
            else if (speech.Contains("learn"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If you truly wish to learn, you must first prove yourself. Bring me the feather of a raven as a token of your commitment.");
                }
            }
            else if (speech.Contains("raven"))
            {
                Say("Ravens are symbols of mystery and change. Their feathers hold power and significance. Return with one, and we shall proceed.");
            }
            else if (speech.Contains("share"))
            {
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    Say("Ah, a secret shared is a bond formed. As a token of trust, take this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Ensure you have this item or adjust accordingly
                    lastRewardTime = DateTime.UtcNow;
                }
                else
                {
                    Say("I have no reward right now. Please return later.");
                }
            }

            base.OnSpeech(e);
        }

        public NoxNora(Serial serial) : base(serial) { }

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
