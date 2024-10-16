using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class EldricCuvier : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public EldricCuvier() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Eldric Cuvier";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Alchemist";

            AddItem(new Robe(Utility.RandomBrightHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomBrightHue()));

            SetSkill(SkillName.Alchemy, 100.0, 100.0);
            SetSkill(SkillName.Inscribe, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 75.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Eldric Cuvier, master alchemist.");
            }
            else if (speech.Contains("job"))
            {
                Say("My craft involves the creation of potent elixirs and rare concoctions. The art of alchemy is complex and requires deep knowledge.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as robust as my most resilient potion. Thank you for your concern. Good health is essential for any successful alchemist.");
            }
            else if (speech.Contains("alchemy"))
            {
                Say("Alchemy is the art of transforming base materials into extraordinary substances. Seek the secrets of the forbidden arts if you dare.");
            }
            else if (speech.Contains("potions"))
            {
                Say("My potions can heal, harm, or even transform. Beware of those who misuse such power. Potions require careful crafting and knowledge.");
            }
            else if (speech.Contains("forbidden"))
            {
                Say("The forbidden arts hold both great risk and great reward. Only the bold dare to uncover their secrets. The path is fraught with danger.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of alchemy are hidden within ancient texts and cryptic formulas. To uncover them, one must delve deeply into the forbidden knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to mastering the art of alchemy. It is said that true mastery comes from understanding both the light and the dark aspects.");
            }
            else if (speech.Contains("mastery"))
            {
                Say("True mastery in alchemy requires not just skill, but wisdom and patience. One must balance the elements and understand the deeper mysteries.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is gained through experience and study. It is the application of knowledge that reveals the true power of alchemy.");
            }
            else if (speech.Contains("power"))
            {
                Say("The power of alchemy can reshape reality itself. With great power comes great responsibility. Use it wisely, or face the consequences.");
            }
            else if (speech.Contains("responsibility"))
            {
                Say("Responsibility is a cornerstone of ethical alchemy. One must always consider the impact of their actions on the world around them.");
            }
            else if (speech.Contains("impact"))
            {
                Say("Every action has an impact, whether small or large. In alchemy, the effects of a potion or formula can ripple through time and space.");
            }
            else if (speech.Contains("time"))
            {
                Say("Time is both a constraint and a tool in alchemy. The right timing can make the difference between success and failure in crafting potions.");
            }
            else if (speech.Contains("failure"))
            {
                Say("Failure is a part of the learning process. Every failed experiment brings one closer to success. Embrace your mistakes and learn from them.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success in alchemy is achieved through perseverance and experimentation. Each success opens new doors to greater mysteries.");
            }
            else if (speech.Contains("doors"))
            {
                Say("The doors to greater mysteries are opened by those who seek and explore. Each discovery leads to more questions and deeper understanding.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Questions drive the pursuit of knowledge. Do not fear them; instead, let them guide you to greater revelations.");
            }
            else if (speech.Contains("revelations"))
            {
                Say("Revelations often come when least expected. Stay curious and open to new ideas, and the secrets of alchemy will reveal themselves.");
            }
            else if (speech.Contains("reveal"))
            {
                if (!m_RewardGiven)
                {
                    Say("You have shown a keen interest in the art of alchemy. As a token of your curiosity and dedication, I present you with the Forbidden Alchemist's Cache.");
                    from.AddToBackpack(new ForbiddenAlchemistsCache()); // Give the reward
                    m_RewardGiven = true;
                }
                else
                {
                    Say("I have already given you a reward. Seek out other mysteries of the alchemical world.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public EldricCuvier(Serial serial) : base(serial) { }

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
