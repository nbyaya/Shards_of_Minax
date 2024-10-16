using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Saito Kurin")]
    public class SaitoKurin : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SaitoKurin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Saito Kurin";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 70;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new LeatherChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherArms() { Hue = Utility.RandomNeutralHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });
            AddItem(new BlackStaff() { Name = "Kurin's Staff", Hue = Utility.RandomMetalHue() });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Speech Hue
            SpeechHue = 0; // Default speech hue

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
                Say("I am Saito Kurin, a master of the hidden arts. To know more, speak of virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Virtues guide our actions and our hearts. Speak of courage to proceed further.");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is the strength to face adversity. Speak of cunning to continue your path.");
            }
            else if (speech.Contains("cunning"))
            {
                Say("Cunning is the art of deception and strategy. To understand further, discuss wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the insight gained through experience. Speak of strategy to unlock the next step.");
            }
            else if (speech.Contains("strategy"))
            {
                Say("Strategy involves planning and foresight. To complete the puzzle, speak of resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the determination to see one's goals through. To earn the reward, speak of honor.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is a code of conduct and respect. Speak of deception to understand its role.");
            }
            else if (speech.Contains("deception"))
            {
                Say("Deception can be a tool when used wisely. To claim your prize, speak of virtue.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Virtue encompasses all that is good and just. Your persistence has been noted. To receive your reward, speak of secrets.");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return when the time is right.");
                }
                else
                {
                    Say("Your journey through the dialogue has proven your worth. Accept this chest of shinobi secrets as your reward.");
                    from.AddToBackpack(new ShinobiSecretsChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Speak of virtues, and I shall guide you through the path to understanding.");
            }

            base.OnSpeech(e);
        }

        public SaitoKurin(Serial serial) : base(serial) { }

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
