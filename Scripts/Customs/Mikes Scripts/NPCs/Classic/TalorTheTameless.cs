using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Talor the Tameless")]
    public class TalorTheTameless : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TalorTheTameless() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Talor the Tameless";
            Body = 0x190; // Human male body

            // Stats
            Str = 96;
            Dex = 90;
            Int = 85;
            Hits = 96;

            // Appearance
            AddItem(new Kilt() { Hue = 64 });
            AddItem(new Doublet() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1174 });
            AddItem(new ShepherdsCrook() { Name = "Talor's Wild Rod" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Talor the Tameless, the master of creatures!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thanks to my animal friends!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to tame and care for the creatures of this land.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Compassion is my guiding virtue, for it takes a kind heart to understand and tame wild creatures. What virtues do you hold dear?");
            }
            else if (speech.Contains("honesty") || speech.Contains("compassion") || speech.Contains("humility"))
            {
                Say("Ah, virtue indeed! Tell me, do you value honesty, compassion, and humility in your adventures?");
            }
            else if (speech.Contains("creatures"))
            {
                Say("Ah, creatures! From the mighty dragon to the meek rabbit, I've befriended them all. One creature, in particular, holds a secret. Care to know more?");
            }
            else if (speech.Contains("animal"))
            {
                Say("Yes, my animal friends are not just companions, but healers too. They have taught me the secrets of herbs and nature's remedies. Have you ever used nature's medicine?");
            }
            else if (speech.Contains("tame"))
            {
                Say("Taming is an art. It's not about subduing, but understanding. Each creature has its own song, and once you learn it, they'll follow you willingly. Ever heard of the Song of the Phoenix?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is a force that can bridge even the wildest of hearts. I once tamed a fierce wyvern using just compassion and understanding. Do you believe that?");
            }
            else if (speech.Contains("secret"))
            {
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    Say("The secret lies with the enigmatic Griffin. A creature of majesty and mystery. If you ever find one, remember to approach with respect. For your efforts in seeking knowledge, accept this gift.");
                    from.AddToBackpack(new EnergyResistAugmentCrystal()); // Reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Please return later for your reward.");
                }
            }
            else if (speech.Contains("medicine"))
            {
                Say("Nature's medicine is powerful. From the bark of the elder tree to the petals of the moonflower, each has its healing property. Seek the Grove of Tranquility for rare herbs.");
            }
            else if (speech.Contains("phoenix"))
            {
                Say("Ah, the Phoenix! A bird of fire and rebirth. It is said its tears can heal any wound. I've been on a quest to find one. Will you aid me?");
            }
            else if (speech.Contains("wyvern"))
            {
                Say("Wyverns are often misunderstood. They are protective, yes, but not evil. I once saved a young wyvern from hunters. Since then, it has been my loyal companion. Have you encountered one?");
            }

            base.OnSpeech(e);
        }

        public TalorTheTameless(Serial serial) : base(serial) { }

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
