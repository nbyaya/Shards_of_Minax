using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Poke")]
    public class ProfessorPoke : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorPoke() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Poke";
            Body = 0x191; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = 1151 }); // Pokéball themed robe
            AddItem(new FeatheredHat() { Hue = 1151 });
            AddItem(new Sandals() { Hue = 1151 });

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
                Say("Hello there! I am Professor Poke, a scholar of rare treasures and mythical creatures.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to explore and catalog the wonders of the world, from ancient relics to legendary Pokémon.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! I have a special Pokéball Treasure Chest that holds many rare and mystical items.");
            }
            else if (speech.Contains("pokeball"))
            {
                Say("The Pokéball Treasure Chest is a marvel of craftsmanship and magic. To obtain it, you'll need to prove your knowledge.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to unlocking the greatest treasures. Answer my questions correctly, and you shall be rewarded.");
            }
            else if (speech.Contains("questions"))
            {
                Say("Let's begin! What is the name of the legendary Pokémon known for its time-altering abilities?");
            }
            else if (speech.Contains("dialga"))
            {
                Say("Correct! Dialga is indeed the legendary Pokémon of time. Next question: What is the name of the legendary Pokémon associated with space?");
            }
            else if (speech.Contains("palkia"))
            {
                Say("Excellent! Palkia is the legendary Pokémon of space. Now, tell me: What is the name of the Pokémon that controls the balance between time and space?");
            }
            else if (speech.Contains("arceus"))
            {
                Say("Indeed! Arceus is said to create the universe's balance. Now, can you tell me which Pokémon is known as the 'Eternal Pokémon'?");
            }
            else if (speech.Contains("giratina"))
            {
                Say("You’re on a roll! Giratina is known as the 'Eternal Pokémon.' Next, which Pokémon is known for its mysterious power over shadows?");
            }
            else if (speech.Contains("darkrai"))
            {
                Say("Correct! Darkrai is the Pokémon with shadowy powers. Finally, can you name the Pokémon known for its healing abilities and ability to create life?");
            }
            else if (speech.Contains("celesteela"))
            {
                Say("Not quite. Try again. The Pokémon known for creating life is actually the 'Creation Pokémon' often seen as a guardian of the universe.");
            }
            else if (speech.Contains("shaymin"))
            {
                Say("No, Shaymin is known for its healing powers but not as a creator of life. Try again. Think of the Pokémon revered as the guardian of the universe.");
            }
            else if (speech.Contains("mew"))
            {
                Say("Mew is indeed an ancient Pokémon, but the one I’m referring to is known as the 'Creation Pokémon.' Try one more time.");
            }
            else if (speech.Contains("arceus"))
            {
                Say("Correct! Arceus is known for creating the universe. For your wisdom and patience, I present to you a Pokéball Treasure Chest.");
                if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new PokeballTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("Please return later for your reward.");
                }
            }
            else
            {
                Say("I’m afraid I don’t know about that. Ask me about my name, job, or treasure to proceed.");
            }

            base.OnSpeech(e);
        }

        public ProfessorPoke(Serial serial) : base(serial) { }

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
