using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Flippy Fiona")]
    public class FlippyFiona : BaseCreature
    {
        [Constructable]
        public FlippyFiona() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Flippy Fiona";
            Body = 0x190; // Human female body

            // Stats
            Str = 90;
            Dex = 75;
            Int = 50;
            Hits = 90;

            // Appearance
            AddItem(new Skirt() { Hue = 53 });
            AddItem(new FancyShirt() { Hue = 53 });
            AddItem(new Sandals() { Hue = 1194 });
            AddItem(new LeatherGloves() { Name = "Fiona's Flipping Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Set direction
            Direction = Direction.North;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Flippy Fiona, the wrestler!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in the peak of health, ready for any match!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to wrestle and entertain the crowd!");
            }
            else if (speech.Contains("heart"))
            {
                Say("True strength lies not just in muscles, but in the heart! Do you value compassion?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Compassion is the true source of strength! It's the foundation of my wrestling style.");
            }
            else if (speech.Contains("wrestler"))
            {
                Say("I've wrestled in various arenas across the land. My most memorable match was against Brutal Benny.");
            }
            else if (speech.Contains("peak"))
            {
                Say("Yes, to stay at my peak, I train daily and maintain a balanced diet. Do you also train?");
            }
            else if (speech.Contains("entertain"))
            {
                Say("The crowd's energy fuels me! I remember a time when I wrestled for a cause, raising funds for an orphanage.");
            }
            else if (speech.Contains("arenas"))
            {
                Say("From bustling city arenas to quiet town squares, each place has its own unique charm. But nothing beats wrestling under the moonlit sky!");
            }
            else if (speech.Contains("benny"))
            {
                Say("Benny and I were rivals, but deep down, we respected each other. He even gifted me a special amulet after our match.");
            }
            else if (speech.Contains("train"))
            {
                Say("Training is a relentless cycle of sweat, pain, and reward. If you're interested, I have an old training manual I could lend you.");
            }
            else if (speech.Contains("diet"))
            {
                Say("It's a mix of high protein, essential fats, and complex carbs. And every now and then, a sweet treat! It's all about balance.");
            }
            else if (speech.Contains("orphanage"))
            {
                Say("That orphanage holds a special place in my heart. For every win, I donate some of my earnings. Helping those kids gives me purpose.");
            }
            else if (speech.Contains("manual"))
            {
                Say("Ah, here it is! This training manual served me well. I hope it aids you in your journey. Take it as a token of appreciation!");
                // Assuming NoRegRobe is an item in your game
                if (from.Backpack != null)
                    from.AddToBackpack(new MaxxiaScroll()); // Add this item to the player's backpack
            }
            else if (speech.Contains("amulet"))
            {
                Say("The amulet is said to bring luck to its wearer. I've felt its power during crucial moments in the ring.");
            }

            base.OnSpeech(e);
        }

        public FlippyFiona(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
