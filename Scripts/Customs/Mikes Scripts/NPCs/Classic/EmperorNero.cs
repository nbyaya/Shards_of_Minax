using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Nero")]
    public class EmperorNero : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorNero() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Nero";
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
            AddItem(new Dagger { Name = "Emperor's Dagger" });

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
                Say("I am Emperor Nero, ruler of these lands!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the peak of health, as befitting an emperor.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to govern and lead my people wisely.");
            }
            else if (speech.Contains("justice"))
            {
                Say("The virtue of justice guides my reign. Do you uphold justice in your actions?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Justice is the cornerstone of a just society. Continue to uphold it, and you shall find favor in my realm.");
            }
            else if (speech.Contains("lands"))
            {
                Say("These lands have been passed down for generations, and I aim to honor the legacy of my ancestors.");
            }
            else if (speech.Contains("peak"))
            {
                Say("The peak of my health is thanks to the royal physicians and the rare herbs they procure for me.");
            }
            else if (speech.Contains("govern"))
            {
                Say("Governing requires listening to the people, making decisions that benefit the majority, and ensuring that law and order is maintained.");
            }
            else if (speech.Contains("ancestors"))
            {
                Say("My ancestors were wise rulers, establishing traditions and principles that I uphold today. They believed in rewarding those who showed loyalty and dedication to the empire.");
            }
            else if (speech.Contains("herbs"))
            {
                Say("The herbs not only help in maintaining my health but also have mysterious properties that even our alchemists are trying to uncover.");
            }
            else if (speech.Contains("law"))
            {
                Say("Law is essential in maintaining peace and order. I believe in fair trials and ensuring that the guilty face their actions. Do you believe in the importance of the law?");
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
                    Say("For those who prove their loyalty and dedication to the empire, I personally ensure they are rewarded generously. You seem to be on a noble quest. Aid me in a matter, and I might bestow upon you a gift from the royal treasury.");
                    from.AddToBackpack(new HarmAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("alchemists"))
            {
                Say("The alchemists of my kingdom are constantly exploring new elixirs and potions. Their knowledge and skills are unparalleled in the realm.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials in our kingdom are overseen by wise judges who ensure fairness and justice. It's important that everyone, regardless of their standing, gets a fair chance.");
            }
            else if (speech.Contains("treasury"))
            {
                Say("Our royal treasury is filled with artifacts from different ages, gold, and gems of immense value. However, the most valuable of all are the relics of ancient power. Check out this one!");
                from.AddToBackpack(new HarmAugmentCrystal()); // The action defined in the XML
            }

            base.OnSpeech(e);
        }

        public EmperorNero(Serial serial) : base(serial) { }

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
