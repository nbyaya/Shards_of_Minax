using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class BakerPercivalCrumb : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public BakerPercivalCrumb() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baker Percival Crumb";
            Title = "the Master Baker";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Outfit
            AddItem(new FullApron() { Hue = Utility.RandomNeutralHue() });
            AddItem(new StrawHat() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            
            // Skills
            SetSkill(SkillName.Cooking, 80.0, 100.0);
            SetSkill(SkillName.TasteID, 75.0, 100.0);
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

                switch (speech)
                {
                    case "name":
                        from.SendMessage("Ah, you have met Baker Percival Crumb, at your service!");
                        break;
                    case "job":
                        from.SendMessage("My job is to bake the finest pastries and bread this side of the realm.");
                        break;
                    case "health":
                        from.SendMessage("I am in tip-top shape, thank you! Baking keeps me spry.");
                        break;
                    case "baking":
                        from.SendMessage("Baking is both an art and a science. The key is in the ingredients and the love you put into them.");
                        break;
                    case "pastries":
                        from.SendMessage("My pastries are renowned for their delightful taste. Would you like to learn more?");
                        break;
                    case "recipe":
                        from.SendMessage("Ah, you must prove your worth before I share my secret recipe. Are you up for a challenge?");
                        break;
                    case "challenge":
                        from.SendMessage("Very well! If you can answer this: What is the secret ingredient for a truly delightful pastry?");
                        break;
                    case "secret ingredient":
                        from.SendMessage("Excellent! You have the spirit of a true baker. For your keen sense, here is the Baker’s Delight chest as a reward.");
                        if (!m_RewardGiven)
                        {
                            from.AddToBackpack(new BakersDelightChest()); // Reward the special chest
                            m_RewardGiven = true;
                        }
                        break;
                    default:
                        from.SendMessage("I’m afraid I don't understand that.");
                        break;
                }
            }
        }

        public BakerPercivalCrumb(Serial serial) : base(serial) { }

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
