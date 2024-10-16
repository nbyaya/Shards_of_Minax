using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Percival Cacheford")]
    public class PercivalCacheford : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public PercivalCacheford() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Percival Cacheford";
            Title = "the Historian";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();

            // Appearance
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomNeutralHue()));

            // Skills
            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    Say("I am Percival Cacheford, a historian dedicated to uncovering lost treasures.");
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to study and preserve ancient artifacts, especially those of great historical value.");
                }
                else if (speech.Contains("health"))
                {
                    Say("I am in good health, thank you for your concern.");
                }
                else if (speech.Contains("treasure"))
                {
                    Say("Ah, treasures! They hold stories of past ages. The Confederation Cache, for example, is one such treasure.");
                }
                else if (speech.Contains("cache"))
                {
                    Say("The Confederation Cache is a trove of valuable items from an era long past. To earn it, you must show your knowledge of history.");
                }
                else if (speech.Contains("history"))
                {
                    Say("History is a tapestry woven with the deeds and lives of those who came before us. It is both our heritage and our guide.");
                }
                else if (speech.Contains("reward"))
                {
                    if (!m_RewardGiven)
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You have already received your reward. Come back another time if you wish.");
                    }
                }
                else
                {
                    Say("I am not sure what you mean. Can you elaborate?");
                }
            }

            base.OnSpeech(e);
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                ConfederationCache cache = new ConfederationCache();
                from.AddToBackpack(cache);
                Say("You have shown a keen interest in history. Here is the Confederation Cache as a token of appreciation.");
                m_RewardGiven = true;
            }
        }

        public PercivalCacheford(Serial serial) : base(serial)
        {
        }

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
