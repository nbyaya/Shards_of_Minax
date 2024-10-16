using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tiberius Alaric")]
    public class TiberiusAlaric : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TiberiusAlaric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tiberius Alaric";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Necklace() { Hue = Utility.RandomMetalHue() });
            AddItem(new Shoes() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Tiberius' Tome of Secrets" });
            
            // Additional Appearance
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2045);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203C, 0x2046);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, seeker of knowledge. I am Tiberius Alaric, the Enigmatic Sage.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, sustained by the mysteries of the arcane.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to guard the secrets and treasures of the mystic realm.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are the essence of the unknown. Seek and you shall uncover mysteries.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries are deep and wondrous. Only the true seeker will unravel them.");
            }
            else if (speech.Contains("arcane"))
            {
                Say("The arcane forces shape the world and conceal hidden truths. Respect them, and they will aid you.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! Only those who prove their worth shall receive the chest of the mystic's enigma.");
            }
            else if (speech.Contains("prove"))
            {
                Say("To prove your worth, demonstrate your knowledge of the arcane and the mysteries of this world.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the key to unlocking many doors. Seek wisdom in the arcane and beyond.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom comes with understanding and experience. It guides us through the labyrinth of life.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is the foundation of wisdom. To truly comprehend, one must be patient and observant.");
            }
            else if (speech.Contains("comprehend"))
            {
                Say("To comprehend is to grasp the essence of knowledge and mysteries. It requires dedication and insight.");
            }
            else if (speech.Contains("insight"))
            {
                Say("Insight allows us to see beyond the surface. It is a crucial element in solving the mysteries of the arcane.");
            }
            else if (speech.Contains("arcane") && speech.Contains("insight"))
            {
                Say("The arcane holds many secrets that can be unveiled through deep insight. Only then will the true nature of the mysteries be revealed.");
            }
            else if (speech.Contains("mystic"))
            {
                Say("Mystic forces are intertwined with the very fabric of reality. To understand them is to grasp the essence of magic itself.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the art of manipulating the arcane. It can reveal hidden truths and grant great power to those who master it.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power comes from knowledge and understanding. Use it wisely, for it can be both a boon and a burden.");
            }
            else if (speech.Contains("boon"))
            {
                Say("A boon is a blessing, a gift that aids us in our quest. It is often given to those who prove their worth.");
            }
            else if (speech.Contains("quest"))
            {
                Say("A quest is a journey of discovery and challenge. It tests the mettle of those who seek it.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are trials that test our resolve. Overcoming them often leads to great rewards.");
            }
            else if (speech.Contains("reward"))
            {
                Say("Ah, rewards are the culmination of our efforts. They are bestowed upon those who have proven their worth.");
            }
            else if (speech.Contains("worth"))
            {
                Say("Proving one's worth is essential for receiving rewards. It demonstrates dedication and perseverance.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance is the ability to persist in the face of obstacles. It is a key trait for those who seek to unravel mysteries.");
            }
            else if (speech.Contains("obstacles"))
            {
                Say("Obstacles are challenges that can either hinder or motivate us. Overcoming them often leads to greater understanding.");
            }
            else if (speech.Contains("understanding") && speech.Contains("obstacles"))
            {
                Say("Understanding how to overcome obstacles is crucial. It involves adapting and learning from each challenge faced.");
            }
            else if (speech.Contains("challenge") && speech.Contains("reward"))
            {
                Say("The ultimate reward for overcoming challenges is often something of great value. It reflects the effort and skill invested.");
            }
            else if (speech.Contains("skill"))
            {
                Say("Skill is honed through practice and experience. It is necessary for overcoming complex challenges and achieving rewards.");
            }
            else if (speech.Contains("experience"))
            {
                Say("Experience provides valuable lessons and insights. It shapes our ability to handle challenges and grasp deeper understanding.");
            }
            else if (speech.Contains("grasp"))
            {
                Say("To grasp something is to fully understand and hold it. This is essential for mastering the arcane and solving mysteries.");
            }
            else if (speech.Contains("mastering"))
            {
                Say("Mastering the arcane requires dedication and deep understanding. It is the final step in achieving true knowledge and power.");
            }
            else if (speech.Contains("true knowledge"))
            {
                Say("True knowledge is the ultimate goal. It is the result of a journey through learning, challenges, and self-discovery.");
            }
            else if (speech.Contains("self-discovery"))
            {
                Say("Self-discovery is a journey inward. It reveals our true potential and understanding of the world around us.");
            }
            else if (speech.Contains("potential"))
            {
                Say("Potential is the inherent ability to grow and achieve. Recognizing and developing it is key to success and mastery.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the achievement of one's goals. It often comes from persistence, knowledge, and overcoming challenges.");
            }
            else if (speech.Contains("achievement"))
            {
                Say("An achievement reflects the culmination of effort and skill. It is a milestone in the journey of mastery.");
            }
            else if (speech.Contains("milestone"))
            {
                Say("A milestone is a significant point in one's journey. It represents progress and accomplishment.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is the movement forward in one's journey. It is marked by achievements and milestones.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey of discovery is filled with challenges and rewards. Embrace each step, and you will find the ultimate treasure.");
            }
            else if (speech.Contains("treasure") && speech.Contains("journey"))
            {
                Say("The treasure you seek is the culmination of your journey. For your perseverance and insight, I present you with the chest of the mystic's enigma.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The chest has already been given. Return later to claim your reward.");
                }
                else
                {
                    from.AddToBackpack(new MysticEnigmaChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Continue your quest for understanding, and the answers will become clear.");
            }

            base.OnSpeech(e);
        }

        public TiberiusAlaric(Serial serial) : base(serial) { }

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
