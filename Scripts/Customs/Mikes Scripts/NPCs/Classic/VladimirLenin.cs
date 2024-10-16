using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vladimir Lenin")]
    public class VladimirLenin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VladimirLenin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Vladimir Lenin";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 90;
            Hits = 75;

            // Appearance
            AddItem(new LongPants() { Hue = 1910 });
            AddItem(new Tunic() { Hue = 1102 });
            AddItem(new Boots() { Hue = 1102 });
            AddItem(new WarAxe() { Name = "Bolshevik Blade" });

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
                Say("I am Vladimir Lenin, the revolutionary!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is irrelevant, for the revolution lives on!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to overthrow the oppressive bourgeoisie!");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The true path to progress is through the collective will of the proletariat! Are you with us?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then rise, comrade, and join the ranks of the revolutionaries!");
            }
            else if (speech.Contains("revolutionary"))
            {
                Say("The winds of change are ever blowing, and it's up to brave souls like us to guide its course.");
            }
            else if (speech.Contains("change"))
            {
                Say("Like the seasons, societies too undergo cycles. But for meaningful change, action is required.");
            }
            else if (speech.Contains("bourgeoisie"))
            {
                Say("They hoard the wealth while the workers suffer. It's time to redistribute the means of production!");
            }
            else if (speech.Contains("proletariat"))
            {
                Say("It's the workers, the common folk, who are the backbone of any society. Their unity can topple even the most oppressive regimes.");
            }
            else if (speech.Contains("hope"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Hope is the beacon that guides us through the darkest nights. For your dedication to the cause, take this as a token of my appreciation.");
                    from.AddToBackpack(new ChivalryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("wealth"))
            {
                Say("True wealth isn't gold or jewels, but the bonds we share and the just world we strive to create.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Together, united under a single cause, we can overcome any obstacle.");
            }
            else if (speech.Contains("action"))
            {
                Say("Words alone cannot change the world. It's our deeds that define our legacy.");
            }

            base.OnSpeech(e);
        }

        public VladimirLenin(Serial serial) : base(serial) { }

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
