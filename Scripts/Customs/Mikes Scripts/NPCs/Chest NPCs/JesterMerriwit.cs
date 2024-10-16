using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Merriwit")]
    public class JesterMerriwit : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public JesterMerriwit() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Merriwit";
            Body = 0x190; // Human male body
            Hue = Utility.RandomBrightHue();
            Title = "the Jester";

            AddItem(new JesterHat() { Hue = Utility.RandomBlueHue() });
            AddItem(new FancyShirt(Utility.RandomBrightHue()));
            AddItem(new LongPants(Utility.RandomBrightHue()));
            AddItem(new Shoes(Utility.RandomBrightHue()));
            AddItem(new Bandana() { Hue = Utility.RandomRedHue() });

            SetSkill(SkillName.Magery, 50.0, 100.0);
            SetSkill(SkillName.Parry, 50.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, greetings! I am Jester Merriwit, the keeper of mirth and merriment.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to spread joy and laughter. I perform tricks and tell tales to entertain all who seek amusement.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in the pink of health, full of joy and ready for more jests!");
            }
            else if (speech.Contains("tricks"))
            {
                Say("I have many tricks up my sleeve. If you can entertain me with a riddle or a jest, you might earn a special reward.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("What has keys but can't open locks, and can play the most delightful tunes? Think carefully, traveler.");
            }
            else if (speech.Contains("piano"))
            {
                Say("Well done! You've guessed it. Pianos are full of keys but can't unlock anything. Now, here's a little something special.");
                GiveReward(from);
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                ClownsWhimsicalChest chest = new ClownsWhimsicalChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy with your cleverness. Take this Clown's Whimsical Chest as your reward!");
                m_RewardGiven = true;
            }
            else
            {
                from.SendMessage("You've already received your reward. Try again later for more fun!");
            }
        }

        public JesterMerriwit(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
