using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Brawny Bart")]
    public class BrawnyBart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrawnyBart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brawny Bart";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 40;
            Int = 20;
            Hits = 90;

            // Appearance
            AddItem(new ShortPants(2126)); // ShortPants with hue 2126
            AddItem(new Doublet(2126)); // Doublet with hue 2126
            AddItem(new Sandals(2126)); // Sandals with hue 2126
            AddItem(new Bandana(2126)); // Bandana with hue 2126
            AddItem(new Cutlass() { Name = "Bart's Cutlass" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            SpeechHue = 0; // Default speech hue

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
                Say("Arr, I be Brawny Bart, the fiercest pirate on these waters!");
            }
            else if (speech.Contains("health"))
            {
                Say("I've weathered many storms, and me health be as strong as a kraken!");
            }
            else if (speech.Contains("job"))
            {
                Say("A pirate's life be me only job, plunderin' and seekin' hidden treasures!");
            }
            else if (speech.Contains("battles"))
            {
                Say("The virtue of valor be a sailor's creed. Be ye valiant, matey?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Aye, a true pirate never flees unless Davy Jones himself be on his tail!");
            }
            else if (speech.Contains("waters"))
            {
                Say("Ah, these waters be teemin' with tales of lost ships and ghostly apparitions. 'Tis not for the faint of heart, matey.");
            }
            else if (speech.Contains("kraken"))
            {
                Say("Aye, once or twice, them beasts be no match for Brawny Bart! Though they left scars that'll never heal, both on me ship and in me heart.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, many have asked, few have returned. But if ye prove yerself to me, maybe I'll share a clue or two.");
            }
            else if (speech.Contains("clue"))
            {
                Say("Fetch me the golden compass from the isle of shadows. It's been a dream of mine, but I've never dared to venture there.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("To the east, where the sun meets the sea. But be warned, it's cursed! Only the bravest souls dare to tread there. If ye bring back the compass, I'll give ye a reward fit for a king!");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, now that's a surprise! But let's just say, it's something every adventurer dreams of. Here is a taste.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("davy"))
            {
                Say("The phantom of the deep, the keeper of souls lost at sea. Even pirates like meself fear his locker.");
            }

            base.OnSpeech(e);
        }

        public BrawnyBart(Serial serial) : base(serial) { }

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
