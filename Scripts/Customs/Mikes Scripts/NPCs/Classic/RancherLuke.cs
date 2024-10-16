using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rancher Luke")]
    public class RancherLuke : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RancherLuke() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rancher Luke";
            Body = 0x190; // Human male body

            // Stats
            Str = 95;
            Dex = 85;
            Int = 50;
            Hits = 85;

            // Appearance
            AddItem(new LongPants() { Hue = 1180 });
            AddItem(new FancyShirt() { Hue = 1180 });
            AddItem(new StrawHat() { Hue = 1170 });
            AddItem(new Boots() { Hue = 1157 });

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
                Say("Howdy, partner! I'm Rancher Luke, keeper of these here cattle.");
            }
            else if (speech.Contains("health"))
            {
                Say("Them cows are healthy as can be!");
            }
            else if (speech.Contains("job"))
            {
                Say("Ranchin' cattle's my trade. Ain't nothin' like it!");
            }
            else if (speech.Contains("stampede"))
            {
                Say("True humility is knowin' that sometimes, nature has its own plans. Ever faced the fury of a stampede?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, faced 'em many a time. Nature humbles a man, makes him realize he's just a speck in the grand scheme. How 'bout you, partner?");
            }
            else if (speech.Contains("cattle"))
            {
                Say("My cattle are like family to me. It's a tough job, but the connection with nature is worth it. Did you know cows have their own way of communicatin'?");
            }
            else if (speech.Contains("communicatin"))
            {
                Say("Yep! Each moo and gesture tells a story. If you listen closely, sometimes they even seem to chant a rhythmic sound... almost like a mantra.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Ah, speaking of mantras, I once heard an old tale from a monk. He said the mantra of Compassion starts with 'NIX'. Keep it in mind if you're ever on a spiritual journey.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility's a big part of the rancher's life. Whether it's the elements or the cattle themselves, somethin's always remindin' us of our place. You ever tried ranchin' yourself?");
            }
            else if (speech.Contains("no"))
            {
                Say("Well, it ain't for everyone. But if you ever feel the need to connect with the land and its creatures, you know where to find me. It's a life full of lessons and tales.");
            }
            else if (speech.Contains("ranch"))
            {
                Say("This here ranch has been in my family for generations. My grandpappy built it with his bare hands. Did you know there's a secret spot on this ranch?");
            }
            else if (speech.Contains("secret"))
            {
                Say("Aye, there's a small grove 'round the back, where the sun meets the horizon just right. Folk say it's enchanted. You might find somethin' interestin' there if you look closely.");
            }
            else if (speech.Contains("enchanted"))
            {
                // No response needed as per XML definition
            }

            base.OnSpeech(e);
        }

        public RancherLuke(Serial serial) : base(serial) { }

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
