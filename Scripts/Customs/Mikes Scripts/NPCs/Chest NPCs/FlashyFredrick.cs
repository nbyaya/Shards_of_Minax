using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Flashy Fredrick")]
    public class FlashyFredrick : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public FlashyFredrick() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Flashy Fredrick";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Fashion Guru";

            // Dress the NPC in funky fashion
            AddItem(new FancyShirt(Utility.RandomBrightHue()));
            AddItem(new Kilt(Utility.RandomBrightHue()));
            AddItem(new ThighBoots(Utility.RandomBrightHue()));
            AddItem(new TricorneHat(Utility.RandomBrightHue()));

            SetSkill(SkillName.ItemID, 75.0, 100.0);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    from.SendMessage("I am Flashy Fredrick, the ultimate fashionista! Do you know the meaning of 'fashion'?");
                }
                else if (speech.Contains("fashion"))
                {
                    from.SendMessage("Fashion is an expression of who we are. It's about making a statement and being bold! And what about 'style'?");
                }
                else if (speech.Contains("style"))
                {
                    from.SendMessage("Style is not just what you wear, but how you wear it. Confidence is key! Do you understand the concept of 'confidence'?");
                }
                else if (speech.Contains("confidence"))
                {
                    from.SendMessage("Confidence is the aura you project through your attire and actions. It's essential in fashion! But have you ever heard of 'boldness'?");
                }
                else if (speech.Contains("boldness"))
                {
                    from.SendMessage("Boldness is about standing out and taking risks with your fashion choices. It's a sign of true style! How do you feel about 'risks'?");
                }
                else if (speech.Contains("risks"))
                {
                    from.SendMessage("Taking risks is part of life and fashion. It can lead to great rewards! Speaking of rewards, would you like to know about 'rewards'?");
                }
                else if (speech.Contains("rewards"))
                {
                    from.SendMessage("A reward is a token of appreciation for your efforts. If you’ve proven your fashion sense, you may earn one. But do you know what 'effort' means?");
                }
                else if (speech.Contains("effort"))
                {
                    from.SendMessage("Effort is the energy you put into your work and style. It shows in your appearance! If you understand 'appearance', you're close to earning a reward.");
                }
                else if (speech.Contains("appearance"))
                {
                    from.SendMessage("Appearance is how you present yourself to the world. It’s the culmination of style, confidence, and effort. If you can prove your understanding, you may earn a special reward!");
                    GiveReward(from);
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                FunkyFashionChest chest = new FunkyFashionChest();
                from.AddToBackpack(chest);
                from.SendMessage("You've proven yourself a true fashionista! Here, take this Funky Fashion Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public FlashyFredrick(Serial serial) : base(serial) { }

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
