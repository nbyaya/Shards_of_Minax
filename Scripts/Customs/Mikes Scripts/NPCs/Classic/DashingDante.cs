using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dashing Dante")]
    public class DashingDante : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DashingDante() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dashing Dante";
            Body = 0x190; // Human male body

            // Stats
            Str = 95;
            Dex = 75;
            Int = 45;
            Hits = 70;

            // Appearance
            AddItem(new LongPants(38)); // Pants with hue 38
            AddItem(new FancyShirt(1904)); // Shirt with hue 1904
            AddItem(new Boots(64)); // Boots with hue 64
            AddItem(new GoldRing { Name = "Dante's Signet Ring" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Dashing Dante, a courtesan of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("My well-being is as fine as a silk gown on a summer breeze.");
            }
            else if (speech.Contains("job"))
            {
                Say("My profession, dear interlocutor, is the art of companionship and intrigue.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("humility") || speech.Contains("ponder"))
                {
                    Say("True virtue lies not only in deeds but in the warmth of one's heart. What virtues do you hold dear, my friend?");
                }
                else
                {
                    Say("Your answer reveals much about your character. Honesty, Compassion, Valor, Justice... which virtues guide your path?");
                }
            }
            else if (speech.Contains("dashing"))
            {
                Say("Ah, my moniker is indeed unusual. My parents hoped I'd grow into an adventurous and charming soul, and so, Dashing Dante I became!");
            }
            else if (speech.Contains("silk"))
            {
                Say("The silk gowns of which I speak come from the rare silkworms of the eastern lands. They're soft as a whisper and shine like the moon. Would you like to see one?");
            }
            else if (speech.Contains("companionship"))
            {
                Say("In this lonely world, companionship is a solace to many. I provide a listening ear, comfort, and sometimes a song or tale to brighten one's day. It's an art, really.");
            }
            else if (speech.Contains("compassion"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Compassion is a rare gem. Once, I met a weary traveler on the road who was starving. I shared my food, and in gratitude, he gifted me a mysterious token. Here, I think you should have it.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("parents"))
            {
                Say("My parents were simple folks, living off the land. They believed that a name can shape one's destiny. They're no longer with us, but their teachings remain in my heart.");
            }
            else if (speech.Contains("silkworms"))
            {
                Say("The silkworms are fascinating creatures. Their cocoons are transformed into the most delicate fabrics. A merchant in town deals with them. If you're interested, you should pay a visit.");
            }
            else if (speech.Contains("song"))
            {
                Say("Music has a way of connecting souls. If you have some time, I could sing you a ballad of old, a tale of love and loss that has touched many hearts.");
            }
            else if (speech.Contains("token"))
            {
                Say("This token has been with me for years. I'm not sure of its origin, but it brings good luck. Please, take it as a token of our newfound friendship.");
            }

            base.OnSpeech(e);
        }

        public DashingDante(Serial serial) : base(serial) { }

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
