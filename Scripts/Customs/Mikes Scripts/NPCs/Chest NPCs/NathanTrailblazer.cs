using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class NathanTrailblazer : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public NathanTrailblazer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nathan Trailblazer";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Frontier Explorer";

            AddItem(new LeatherChest() { Name = "Explorer's Vest", Hue = Utility.RandomNondyedHue() });
            AddItem(new LeatherLegs() { Name = "Explorer's Pants", Hue = Utility.RandomNondyedHue() });
            AddItem(new Bandana() { Hue = Utility.RandomNondyedHue() });
            AddItem(new Boots() { Hue = Utility.RandomNondyedHue() });
            AddItem(new Spear() { Name = "Pioneer's Spear", Hue = Utility.RandomNondyedHue() });

            // Stats
            SetSkill(SkillName.Swords, 80.0, 100.0);
            SetSkill(SkillName.Parry, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string speech = e.Speech.ToLower();

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                if (speech.Contains("name"))
                {
                    Say("Ahoy there! I’m Nathan Trailblazer, at your service.");
                }
                else if (speech.Contains("job"))
                {
                    Say("My job is to explore the wild frontiers and uncover hidden treasures. It's quite an adventurous life!");
                }
                else if (speech.Contains("health"))
                {
                    Say("I’m in good health, though the adventures can be quite taxing. The frontier is harsh but rewarding.");
                }
                else if (speech.Contains("frontier"))
                {
                    Say("The frontier holds many secrets. Only the brave and persistent will uncover them. Speaking of which...");
                }
                else if (speech.Contains("secrets"))
                {
                    Say("Secrets lie in every corner of these lands. Some are hidden in plain sight, waiting to be discovered.");
                }
                else if (speech.Contains("treasure"))
                {
                    Say("Treasure is not just gold and jewels but the experiences and stories we gather along the way. Each treasure hunt has its own tale.");
                }
                else if (speech.Contains("experience"))
                {
                    Say("Experience is gained through trials and triumphs. It is the true reward of any journey. However, the real treasure is often the friends we make along the way.");
                }
                else if (speech.Contains("friends"))
                {
                    Say("Friends are invaluable. They accompany you through thick and thin. But enough about friends; let's talk about the essence of exploration.");
                }
                else if (speech.Contains("exploration"))
                {
                    Say("Exploration is about discovery and adventure. It's a quest for knowledge and understanding, often leading to unexpected rewards.");
                }
                else if (speech.Contains("rewards"))
                {
                    Say("Rewards come in many forms. The greatest reward of exploration is the journey itself, but occasionally, there’s a tangible prize. Do you seek such a reward?");
                }
                else if (speech.Contains("seek"))
                {
                    Say("To seek is to pursue something with purpose. In my case, I seek the hidden wonders of the frontier. If you're keen on seeking, you must first prove your worth.");
                }
                else if (speech.Contains("prove"))
                {
                    Say("Proving oneself is a matter of persistence and curiosity. Show me you understand the essence of adventure, and you may earn a reward.");
                }
                else if (speech.Contains("adventure"))
                {
                    Say("Adventure is at the heart of exploration. It’s a daring journey into the unknown. If you’re truly adventurous, you might be rewarded with something special.");
                }
                else if (speech.Contains("special"))
                {
                    Say("Special things are reserved for those who truly understand the spirit of exploration. If you’ve been attentive, you might just earn it.");
                }
                else if (speech.Contains("reserved"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You must prove your worth through your journey before claiming any reward.");
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
            // Placeholder for reward conditions; this can be expanded with specific checks
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                FrontierExplorersStash chest = new FrontierExplorersStash();
                from.AddToBackpack(chest);
                from.SendMessage("Congratulations! You have proven yourself worthy. Here is the Frontier Explorer's Stash as your reward.");
                m_RewardGiven = true;
            }
        }

        public NathanTrailblazer(Serial serial) : base(serial) { }

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
