using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Augustus")]
    public class EmperorAugustus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorAugustus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Augustus";
            Body = 0x190; // Human male body
            
            // Stats
            Str = 130;
            Dex = 70;
            Int = 90;
            Hits = 90;
            
            // Appearance
            AddItem(new Robe(1303)); // Robe with hue 1303
            AddItem(new GoldRing()); // Custom item, you need to create or add it
            AddItem(new Boots(1175)); // Boots with hue 1175

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
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
                Say("Greetings, traveler. I am Emperor Augustus, ruler of this land.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, thanks for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job as Emperor is to ensure the prosperity and safety of my people.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Power is not measured by the crown one wears, but by the wisdom one possesses. Are you wise?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom is a rare and precious gift. Use it to make the world a better place.");
            }
            else if (speech.Contains("prosperity"))
            {
                Say("The prosperity of this land is my utmost priority. Gold flows, markets bustle, but true prosperity lies in the happiness of its people. Would you agree?");
            }
            else if (speech.Contains("safety"))
            {
                Say("Safety is paramount. An emperor must protect his land from both external and internal threats. Sometimes, the gravest dangers come from within. Can you identify such dangers?");
            }
            else if (speech.Contains("crown"))
            {
                Say("The crown is but a symbol. What truly matters is the burden it represents: the hopes and dreams of an entire nation resting on one's shoulders. Have you ever borne such a weight?");
            }
            else if (speech.Contains("power"))
            {
                Say("Power is a double-edged sword. It can bring peace or destruction, depending on the hands that wield it. Tell me, what would you do with great power?");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you seek rewards. A wise leader knows when to reward loyalty and service. As you've shown interest in the matters of this realm, I shall grant you something.");
                    from.AddToBackpack(new MirrorOfKalandra()); // Reward item, you need to create or add it
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("threats"))
            {
                Say("Threats to this empire come in many forms: invaders from foreign lands, treachery from within, and even the looming shadow of famine. But with a vigilant eye, we shall overcome. How do you face your threats?");
            }
            else if (speech.Contains("no"))
            {
                Say("Honesty is another form of wisdom. It's better to admit our limitations than to pretend we have none. There is always room for growth.");
            }

            base.OnSpeech(e);
        }

        public EmperorAugustus(Serial serial) : base(serial) { }

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
