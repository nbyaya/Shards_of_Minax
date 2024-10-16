using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Ignatius Blackthorn")]
    public class DrIgnatiusBlackthorn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrIgnatiusBlackthorn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Ignatius Blackthorn";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1157 }); // Dark robe to match the necro theme
            AddItem(new BoneHelm() { Hue = 1157 }); // Bone helm to add to the dark look
            AddItem(new BoneGloves() { Hue = 1157 });
            AddItem(new BoneLegs() { Hue = 1157 });
            AddItem(new BoneChest() { Hue = 1157 });
            AddItem(new FireballWand() { Name = "Wand of Alchemical Mysteries", Hue = 1158 });
			
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

            // Initial keyword responses
            if (speech.Contains("name"))
            {
                Say("Ah, you have encountered Dr. Ignatius Blackthorn, master of alchemical necromancy. Seek knowledge to uncover more.");
            }
            else if (speech.Contains("job") && speech.Contains("blackthorn"))
            {
                Say("My job is to unravel the mysteries of the dark arts and alchemical sciences. The pursuit of power is a path of great learning.");
            }
            else if (speech.Contains("health") && speech.Contains("job"))
            {
                Say("My health is but a mere illusion, hidden beneath the veil of necromantic energies. But to truly understand, one must explore further.");
            }
            else if (speech.Contains("mysteries") && speech.Contains("health"))
            {
                Say("Mysteries abound in the dark corners of alchemy and necromancy. To uncover them, one must first grasp the essence of alchemical knowledge.");
            }
            else if (speech.Contains("alchemy") && speech.Contains("mysteries"))
            {
                Say("Alchemy transforms the mundane into the extraordinary. It requires a deep understanding of the fundamental forces.");
            }
            else if (speech.Contains("necro") && speech.Contains("alchemy"))
            {
                Say("The dark arts of necromancy are not for the faint-hearted. They require great courage and a touch of madness. Embrace the darkness to proceed.");
            }
            else if (speech.Contains("power") && speech.Contains("necro"))
            {
                Say("Power lies not only in might but in the mastery of the arcane and the alchemical. Seek knowledge, and you shall wield great power.");
            }
            else if (speech.Contains("knowledge") && speech.Contains("power"))
            {
                Say("Knowledge is the true key to unlocking the secrets of the universe. Embrace it, and the world will unfold before you.");
            }
            else if (speech.Contains("reward") && speech.Contains("knowledge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience is a virtue, young one. Return later for your reward.");
                }
                else
                {
                    Say("Your persistence is commendable. For your efforts, I bestow upon you the Necro-Alchemical Chest, a treasure of great power.");
                    from.AddToBackpack(new NecroAlchemicalChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("persistence") && speech.Contains("reward"))
            {
                Say("Persistence in the face of uncertainty reveals one's true character. Your resolve has earned you this reward.");
            }
            else if (speech.Contains("resolve") && speech.Contains("persistence"))
            {
                Say("Your resolve is admirable. In the realm of alchemical necromancy, it is the key to unlocking the greatest secrets.");
            }

            base.OnSpeech(e);
        }

        public DrIgnatiusBlackthorn(Serial serial) : base(serial) { }

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
