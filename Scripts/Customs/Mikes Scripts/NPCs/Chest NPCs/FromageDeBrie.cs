using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class FromageDeBrie : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public FromageDeBrie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Fromage de Brie";
            Title = "the Cheese Sage";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            AddItem(new FancyShirt(Utility.RandomOrangeHue())); // Cheese-themed clothing
            AddItem(new Bandana(Utility.RandomOrangeHue())); // Cheese-themed hat
            AddItem(new Sandals());

            // Skills
            SetSkill(SkillName.Cooking, 100.0, 120.0);
            SetSkill(SkillName.TasteID, 90.0, 100.0);
            SetSkill(SkillName.Camping, 70.0, 80.0);
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
                switch (e.Speech.ToLower())
                {
                    case "name":
                        from.SendMessage("I am Fromage de Brie, the Cheese Sage.");
                        break;
                    case "job":
                        from.SendMessage("I travel the lands in search of the finest cheeses and to share my knowledge with fellow connoisseurs.");
                        break;
                    case "health":
                        from.SendMessage("My health is as robust as a well-aged Gouda!");
                        break;
                    case "cheese":
                        from.SendMessage("Cheese is the true delight of the senses. Only those who appreciate its depth can understand its true value.");
                        break;
                    case "delight":
                        from.SendMessage("Ah, the delight of cheese! It brings joy and satisfaction to all who partake.");
                        break;
                    case "cache":
                        from.SendMessage("The Cheese Connoisseur's Cache holds many treasures for the true cheese lover. If you prove your worth, it shall be yours.");
                        break;
                    case "prove":
                        from.SendMessage("To prove yourself, simply show me your appreciation for cheese. Share your thoughts on this divine food.");
                        break;
                    case "appreciation":
                        from.SendMessage("True appreciation for cheese involves recognizing its variety, flavor, and history. If you truly understand this, you shall be rewarded.");
                        if (!m_RewardGiven)
                        {
                            CheeseConnoisseursCache chest = new CheeseConnoisseursCache();
                            from.AddToBackpack(chest);
                            from.SendMessage("You have demonstrated your appreciation for cheese. Take this Cheese Connoisseur's Cache as your reward.");
                            m_RewardGiven = true;
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        public FromageDeBrie(Serial serial) : base(serial) { }

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
