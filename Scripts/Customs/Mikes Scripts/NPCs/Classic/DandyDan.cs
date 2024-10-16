using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dandy Dan")]
    public class DandyDan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DandyDan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dandy Dan";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 90;
            Int = 80;
            Hits = 85;

            // Appearance
            AddItem(new FancyShirt(1153)); // Fancy shirt with hue 1153
            AddItem(new LongPants(1153)); // Long pants with hue 1153
            AddItem(new Boots(1153)); // Boots with hue 1153
            AddItem(new FeatheredHat(1153)); // Feathered hat with hue 1153
            AddItem(new GoldRing { Name = "Dan's Dazzling Ring" });
            AddItem(new Longsword { Name = "Dan's Dashing Rapier" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Dandy Dan, the rogue.");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm quite nimble and cunning, always dodging danger.");
            }
            else if (speech.Contains("valor"))
            {
                Say("But remember, my friend, true valor lies in the heart, not in thievery.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you seek to walk the path of valor, my friend?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If so, remember that valor is not about winning battles but about doing what is right.");
            }
            else if (speech.Contains("health"))
            {
                Say("Ah, I've seen better days, but my spirit remains unbroken. It's all thanks to an old healer named Mira. Have you met her?");
            }
            else if (speech.Contains("mira"))
            {
                Say("Mira is a skilled healer who once saved me from a deadly poison. She taught me the value of compassion and whispered a secret - the third syllable of the mantra of Compassion is MUH. It might aid you on your journey.");
            }
            else if (speech.Contains("thievery"))
            {
                Say("Thievery is a path I once walked, but no longer. There's a certain thrill to it, but the cost is one's own integrity. Tell me, have you heard of the Guild of Shadows?");
            }
            else if (speech.Contains("guild"))
            {
                Say("A secretive group of thieves, rumored to operate from the shadows of every city. They have codes and secrets, but their path is fraught with peril. Beware if you ever cross them.");
            }
            else if (speech.Contains("danger"))
            {
                Say("Oh, I've faced my share of danger. From dragons to trolls, every challenge has its lessons. Among the most treacherous are the merfolk of the deep. Ever heard tales of them?");
            }
            else if (speech.Contains("merfolk"))
            {
                Say("Enchanting beings of the underwater realm. They possess secrets of the ocean and are guardians of its mysteries. I once bartered with a mermaid for a trinket, and in return, she shared tales of sunken treasures.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Many a sailor and adventurer dreams of finding sunken treasures. But remember, gold and gems are fleeting. True treasures are found in friendships and memories.");
            }
            else if (speech.Contains("right"))
            {
                Say("Doing what's right is not always easy. It requires listening to one's heart and standing firm against injustice. In this land, the Temple of Virtue is a beacon for those seeking the path of righteousness. Have you visited it?");
            }
            else if (speech.Contains("temple"))
            {
                Say("It's a sacred place where many come to meditate on the virtues. Within its walls, wisdom is shared, and souls are rejuvenated. I once met a monk there who spoke of inner peace.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Inner peace is a state of balance and harmony within oneself. It's achieved by understanding oneself, accepting the past, and having hope for the future. It's a journey, not a destination.");
            }

            base.OnSpeech(e);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
        }

        public DandyDan(Serial serial) : base(serial) { }

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
