using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Duke Harmonix")]
    public class DukeHarmonix : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public DukeHarmonix() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Duke Harmonix";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Melodious";

            // Outfit for Duke Harmonix
            AddItem(new FancyShirt(Utility.RandomPinkHue()));
            AddItem(new LongPants(Utility.RandomPinkHue()));
            AddItem(new Shoes(Utility.RandomPinkHue()));
            AddItem(new FeatheredHat(Utility.RandomPinkHue()));
            AddItem(new BaseBook(Utility.RandomPinkHue()) { Name = "Duke's Music Notes" });

            // Set skills related to performance and charisma
            SetSkill(SkillName.Musicianship, 75.0, 100.0);
            SetSkill(SkillName.Provocation, 75.0, 100.0);
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
                        from.SendMessage("Greetings! I am Duke Harmonix, the Melodious.");
                        break;
                    case "job":
                        from.SendMessage("My role is to spread joy through music and harmony.");
                        break;
                    case "health":
                        from.SendMessage("I am as lively as a pop concert! Thanks for asking.");
                        break;
                    case "music":
                        from.SendMessage("Music is the heartbeat of the soul. It connects us all.");
                        break;
                    case "harmony":
                        from.SendMessage("Harmony is about balance and unity. It's the essence of great music.");
                        break;
                    case "essence":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You need to prove your musical spirit before I can reward you.");
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Simple placeholder for reward conditions
            // This could be expanded with more complex conditions
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                BoyBandBox chest = new BoyBandBox();
                from.AddToBackpack(chest);
                from.SendMessage("You've shown great musical spirit! Here is the Boy Band Box as your reward.");
                m_RewardGiven = true;
            }
        }

        public DukeHarmonix(Serial serial) : base(serial)
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
