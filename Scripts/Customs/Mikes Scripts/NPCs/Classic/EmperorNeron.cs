using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Nero")]
    public class EmperorNeron : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorNeron() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Neron";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 65;
            Int = 85;
            Hits = 85;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new GoldRing() { Name = "Emperor Nero's Ring" });
            AddItem(new Boots() { Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Emperor Nero, ruler of this wretched land.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as frail as the empire I command.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job,' as you call it, is to preside over this decaying realm.");
            }
            else if (speech.Contains("power"))
            {
                Say("True power lies not in thrones and scepters, but in the hearts of the people. Do you comprehend?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then tell me, do you believe in the strength of the people?");
            }
            else if (speech.Contains("wretched"))
            {
                Say("The wretchedness you sense in this land stems from the countless betrayals and conspiracies against me.");
            }
            else if (speech.Contains("frail"))
            {
                Say("My frailty has been exacerbated by the treacherous advisors who surround me, claiming to be loyal.");
            }
            else if (speech.Contains("decaying"))
            {
                Say("This realm decays not from age, but from the corruption and greed of those I once trusted.");
            }
            else if (speech.Contains("hearts"))
            {
                Say("Hearts can be swayed and manipulated, just as easily as they can be won. Trust in the hearts of the people is a double-edged sword.");
            }
            else if (speech.Contains("betrayals"))
            {
                Say("The betrayals have left deep scars, not just on my heart but on this empire. Yet, I remain steadfast.");
            }
            else if (speech.Contains("advisors"))
            {
                Say("I have had advisors who've plotted against me, yet there are a few who have shown true loyalty. To those who prove their loyalty, I reward generously.");
            }
            else if (speech.Contains("corruption"))
            {
                Say("The corruption runs deep, like a pestilence through the land. If only I had a loyal subject to help me root it out.");
            }
            else if (speech.Contains("trust"))
            {
                Say("It's difficult to trust when one has been betrayed so many times. But you, traveler, have a trustworthy aura. Prove your loyalty, and you shall receive an unspecified reward.");
            }
            else if (speech.Contains("scars"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("These scars serve as a reminder of the past. They tell tales of both pain and perseverance. Take this as a reminder.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public EmperorNeron(Serial serial) : base(serial) { }

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
