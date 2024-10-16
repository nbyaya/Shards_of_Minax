using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lord Richard Vaultington")]
    public class LordRichardVaultington : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LordRichardVaultington() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Richard Vaultington";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });
            
            Hue = Race.RandomSkinHue(); // Beard and facial hair
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

            // Basic Information
            if (speech.Contains("name"))
            {
                Say("Greetings, I am Lord Richard Vaultington, keeper of treasures and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, thank you for your concern.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard the royal treasures and ensure the secrets of the vault remain hidden.");
            }
            else if (speech.Contains("vault"))
            {
                Say("Ah, the vault. It holds many wonders. Only the worthy may access its contents.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The vault contains secrets of ancient power and hidden knowledge. If you seek them, you must prove your worth.");
            }
            
            // Unlocking the dialogue based on previous responses
            else if (speech.Contains("worth"))
            {
                Say("Proving one's worth involves demonstrating knowledge and understanding. What do you wish to know?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to many things. Tell me, what have you learned on your journey?");
            }
            else if (speech.Contains("journey"))
            {
                Say("Your journey is a testament to your quest for understanding. What specifically have you discovered?");
            }
            else if (speech.Contains("discovered"))
            {
                Say("Discovery is a significant step. Have you encountered any mysteries or challenges?");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("Mysteries are often the path to deeper understanding. Have you solved any puzzles or riddles?");
            }
            else if (speech.Contains("puzzles"))
            {
                Say("Puzzles can reveal much about one's character. Have you encountered any notable ones recently?");
            }
            else if (speech.Contains("notable"))
            {
                Say("Notable puzzles are those that test your wit and perseverance. Share an example, and we may proceed further.");
            }
            else if (speech.Contains("example"))
            {
                Say("An example of a notable puzzle is the riddle of the ancient tome. Have you encountered such a riddle?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Riddles are a classic challenge. Solving them often requires insight. What was the riddle you faced?");
            }
            else if (speech.Contains("faced"))
            {
                Say("Facing challenges is part of the journey. What did you learn from these experiences?");
            }
            else if (speech.Contains("learned"))
            {
                Say("Learning from experience is valuable. Apply that knowledge, and you will be closer to your goal.");
            }

            // Final Reward
            else if (speech.Contains("goal"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your dedication and perseverance in solving the puzzles and understanding the mysteries, accept this token of our appreciation.");
                    from.AddToBackpack(new KingdomsVaultChest()); // Give the reward chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LordRichardVaultington(Serial serial) : base(serial) { }

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
