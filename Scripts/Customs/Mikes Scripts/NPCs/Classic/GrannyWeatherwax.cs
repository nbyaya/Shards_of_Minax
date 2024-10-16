using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Granny Weatherwax")]
    public class GrannyWeatherwax : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrannyWeatherwax() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Granny Weatherwax";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 60;
            Int = 120;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = 1107 });
            AddItem(new Boots() { Hue = 1107 });
            AddItem(new WizardsHat() { Hue = 1107 });
            AddItem(new Spellbook() { Name = "Weatherwax's Manual" });

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
                Say("Greetings, traveler. I am Granny Weatherwax, a keeper of ancient wisdom.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health, you ask? It's as sturdy as the roots of ancient oaks.");
            }
            else if (speech.Contains("job"))
            {
                Say("My craft, you inquire? I am a master of the arcane and a guardian of secrets.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("But beware, young one, for magic is not to be trifled with. Are you wise enough to respect its power?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Your answer intrigues me. Tell me, do you seek to learn the secrets of the ancient arts?");
            }
            else if (speech.Contains("coven"))
            {
                Say("The wisdom I possess has been handed down through generations. It's a tapestry of tales, spells, and rituals. Have you ever heard of the Coven?");
            }
            else if (speech.Contains("oaks"))
            {
                Say("The oaks you speak of have been here for centuries. They've seen the rise and fall of empires, and they too hold their own stories. If you're keen, I might tell you about the Eldertree.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane arts are vast and deep, spanning across dimensions and realms. One must be careful when dealing with them. For example, there's the forbidden spell of Netherbinding which few dare to invoke.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is an essence, a force that flows through everything. It can be gentle as a stream or as violent as a storm. But there's one place where its essence is the purest - the Astral Plane.");
            }
            else if (speech.Contains("astral"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The Astral Plane is a dimension of pure magic, a realm where thoughts become reality. I've journeyed there many times. If you ever wish to venture there, you'll need an Ethereal Key. For your curiosity, take this as a token of my appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("eldertree"))
            {
                Say("The Eldertree is the oldest of the oaks, a sentinel that has watched over these lands since time immemorial. It's said that its roots touch the very heart of the world. Some believe it guards a Hidden Relic.");
            }
            else if (speech.Contains("netherbinding"))
            {
                Say("Ah, brave soul to even mention it. Netherbinding is a spell that binds one's spirit to the netherworld. It grants immense power, but at a terrible cost. If you're wise, you'd seek the Guardian's Blessing before attempting it.");
            }

            base.OnSpeech(e);
        }

        public GrannyWeatherwax(Serial serial) : base(serial) { }

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
