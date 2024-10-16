using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marcel Dupont")]
    public class MarcelDupont : BaseCreature
    {
        private DateTime lastRewardTime;

        private bool hasAskedName;
        private bool hasAskedJob;
        private bool hasAskedHealth;
        private bool hasAskedSilent;
        private bool hasAskedMystery;
        private bool hasAskedFuture;

        [Constructable]
        public MarcelDupont() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marcel Dupont";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 95;
            Hits = 70;

            // Appearance
            AddItem(new Cap() { Hue = 1155 });
            AddItem(new JesterHat() { Hue = 1155 });
            AddItem(new FancyShirt() { Hue = 1155 });
            AddItem(new LongPants() { Hue = 1155 });
            AddItem(new Shoes() { Hue = 1155 });
            
            // Facial features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = -1; // No facial hair for this character

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
                if (!hasAskedName)
                {
                    Say("Ah, you are curious! I am Marcel Dupont, the master of silent mysteries.");
                    hasAskedName = true;
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to entertain with my silent performances and keep my secrets well guarded.");
                }
            }
            else if (hasAskedName && speech.Contains("job"))
            {
                if (!hasAskedJob)
                {
                    Say("A mime's job is to embody silence and mystery, performing without a single word.");
                    hasAskedJob = true;
                }
                else if (speech.Contains("health"))
                {
                    Say("I am as agile and silent as ever, thank you for asking!");
                }
            }
            else if (hasAskedJob && speech.Contains("health"))
            {
                if (!hasAskedHealth)
                {
                    Say("My health is as impeccable as my performance. Silence keeps me fit.");
                    hasAskedHealth = true;
                }
                else if (speech.Contains("silent"))
                {
                    Say("Silence is not empty; it's full of answers, if only one listens carefully.");
                }
            }
            else if (hasAskedHealth && speech.Contains("silent"))
            {
                if (!hasAskedSilent)
                {
                    Say("Silence, my friend, is a language of its own. It can speak volumes without uttering a word.");
                    hasAskedSilent = true;
                }
                else if (speech.Contains("mystery"))
                {
                    Say("Mysteries are the heart of my craft. Can you solve them to earn a special reward?");
                }
            }
            else if (hasAskedSilent && speech.Contains("mystery"))
            {
                if (!hasAskedMystery)
                {
                    Say("Mysteries unravel like layers. Each clue is a step towards the truth.");
                    hasAskedMystery = true;
                }
                else if (speech.Contains("future"))
                {
                    Say("The future holds many secrets, but only if you can decipher the present mysteries.");
                }
            }
            else if (hasAskedMystery && speech.Contains("future"))
            {
                if (!hasAskedFuture)
                {
                    Say("The future is a riddle wrapped in silence. Speak the answer to claim your prize.");
                    hasAskedFuture = true;
                }
                else
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("The future holds no rewards right now. Please return later.");
                    }
                    else
                    {
                        Say("You have answered correctly! For your cleverness, take this Mime's Silent Chest.");
                        from.AddToBackpack(new MimeSilentChest()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                }
            }
            else
            {
                Say("Try asking about my name, job, health, or mysteries to unlock more secrets.");
            }

            base.OnSpeech(e);
        }

        public MarcelDupont(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(hasAskedName);
            writer.Write(hasAskedJob);
            writer.Write(hasAskedHealth);
            writer.Write(hasAskedSilent);
            writer.Write(hasAskedMystery);
            writer.Write(hasAskedFuture);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            hasAskedName = reader.ReadBool();
            hasAskedJob = reader.ReadBool();
            hasAskedHealth = reader.ReadBool();
            hasAskedSilent = reader.ReadBool();
            hasAskedMystery = reader.ReadBool();
            hasAskedFuture = reader.ReadBool();
        }
    }
}
