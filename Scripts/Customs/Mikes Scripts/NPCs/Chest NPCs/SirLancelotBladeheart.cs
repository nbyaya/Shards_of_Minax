using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class SirLancelotBladeheart : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public SirLancelotBladeheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Lancelot Bladeheart";
            Title = "the Fencing Master";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            
            // Add fencing-themed equipment
            AddItem(new LeatherChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Dagger() { Hue = Utility.RandomMetalHue(), Name = "Bladeheart's Dagger" });

            // Set skills related to fencing
            SetSkill(SkillName.Fencing, 100.0);
            SetSkill(SkillName.Parry, 100.0);
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
                    from.SendMessage("Greetings, I am Sir Lancelot Bladeheart, the master of the blade.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("My duty is to train aspiring fencers and guard the secrets of the blade.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("I am in excellent health, ready for any duel.");
                }
                else if (speech.Contains("fencing"))
                {
                    from.SendMessage("Fencing is an art of precision and agility. Only the most skilled can master it.");
                }
                else if (speech.Contains("bladeheart"))
                {
                    from.SendMessage("The name Bladeheart speaks of a legendary swordsman, skilled and fearless.");
                }
                else if (speech.Contains("master"))
                {
                    from.SendMessage("To be a master, one must practice relentlessly and never cease to improve.");
                }
                else if (speech.Contains("skill"))
                {
                    from.SendMessage("Skill in fencing requires dedication and the will to push one's limits.");
                }
                else if (speech.Contains("dedication"))
                {
                    from.SendMessage("Dedication is the key to mastering any craft. Without it, even the most talented will falter.");
                }
                else if (speech.Contains("craft"))
                {
                    from.SendMessage("Crafting a perfect technique requires understanding both the art and the science behind it.");
                }
                else if (speech.Contains("art"))
                {
                    from.SendMessage("The art of fencing is as much about strategy as it is about physical prowess.");
                }
                else if (speech.Contains("strategy"))
                {
                    from.SendMessage("A well-devised strategy can turn the tide of any duel. It requires foresight and adaptability.");
                }
                else if (speech.Contains("foresight"))
                {
                    from.SendMessage("Foresight allows one to anticipate an opponent's moves and react accordingly.");
                }
                else if (speech.Contains("opponent"))
                {
                    from.SendMessage("An opponent is not merely a rival, but a partner in the dance of combat.");
                }
                else if (speech.Contains("rival"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must first prove your worth to receive my reward.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true; // Assuming the player has met the conditions
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                WearableFencingChest chest = new WearableFencingChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Fencing Champion's Chest as your reward.");
                m_RewardGiven = true;
            }
        }

        public SirLancelotBladeheart(Serial serial) : base(serial)
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
