using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Raven the Shade")]
    public class RavenTheShade : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RavenTheShade() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Raven the Shade";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 120;
            Int = 50;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1107 });
            AddItem(new LeatherChest() { Hue = 1107 });
            AddItem(new LeatherGloves() { Hue = 1107 });
            AddItem(new HoodedShroudOfShadows() { Hue = 1107 });
            AddItem(new Boots() { Hue = 1107 });
            AddItem(new Kryss() { Name = "Raven's Kryss" });

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
                Say("I am Raven the Shade, the night's silent blade.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is my business, not yours.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to dance in the shadows and collect debts that are long overdue.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you dare venture into the abyss of darkness, or do you bask in the false light of day?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If you have the audacity to answer, speak. Otherwise, fade into the oblivion of ignorance.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("Shadows are my realm, the very fabric from which I weave my fate. They hide secrets and stories untold. Why do you ask?");
            }
            else if (speech.Contains("debts"))
            {
                Say("Some debts are of coin, and some of the soul. Those who cross the boundaries of light and darkness owe more than they can repay. It's my duty to ensure balance.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is the essence of existence. Light cannot exist without darkness, and vice versa. It's the eternal dance of opposites, and I am its guardian.");
            }
            else if (speech.Contains("dance"))
            {
                Say("Dancing is not just a form of expression, but a way to transcend realms. Through dance, I commune with the spirits of the night and tap into the energies of the universe.");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Spirits roam freely in the vast expanse between realms. Some are benign, others malicious. I can guide you to them, but whether they help or harm is up to you.");
            }
            else if (speech.Contains("realms"))
            {
                Say("Beyond the mortal coil lie realms of wonder and terror. Places where time stands still and reality bends. I can take you there, for a price.");
            }
            else if (speech.Contains("price"))
            {
                Say("The price of such knowledge and power is steep. Sometimes it's a trinket, sometimes a memory, and at times, a piece of your very essence. Are you willing to pay?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the truest form of power. With it, one can shape destinies and rewrite history. But it comes with its own burdens. Seek it, if you dare.");
            }
            else if (speech.Contains("history"))
            {
                Say("History is written by victors and forgotten by the lost. In shadows, truths of the past linger, waiting for someone to discover them.");
            }
            else if (speech.Contains("truths"))
            {
                Say("Truth is a matter of perspective. What you perceive as truth might be a lie to another. But in the shadows, the line between truth and lies blurs.");
            }

            base.OnSpeech(e);
        }

        public RavenTheShade(Serial serial) : base(serial) { }

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
