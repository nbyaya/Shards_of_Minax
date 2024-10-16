using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grip Gareth")]
    public class GripGareth : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GripGareth() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grip Gareth";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 105;
            Int = 60;
            Hits = 150;

            // Appearance
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new BodySash() { Hue = 36 });
            AddItem(new ThighBoots() { Hue = 38 });
            AddItem(new PlateGloves() { Name = "Gareth's Gripping Gloves" });
            
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
                Say("I am Grip Gareth, the wrestler!");
            }
            else if (speech.Contains("health"))
            {
                Say("Feeling strong today!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to wrestle and entertain the crowd!");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Strength and humility go hand in hand. Do you agree?");
            }
            else if (speech.Contains("yes") && (speech.Contains("strength") || speech.Contains("humility")))
            {
                Say("Indeed, true strength is not in overpowering others but in mastering oneself.");
            }
            else if (speech.Contains("wrestler"))
            {
                Say("Wrestling runs deep in my veins. It's been a family tradition for generations. My grandfather was the legendary Grasp Graham.");
            }
            else if (speech.Contains("strong"))
            {
                Say("Strength is not just physical. It's mental too. The toughest matches I've had were the ones I battled in my mind.");
            }
            else if (speech.Contains("entertain"))
            {
                Say("It's not just about the cheers and jeers. It's the look in a kid's eyes when they see their hero stand tall. That's the true reward.");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility has saved me more times in the ring than any fancy move. Remembering where I came from, and who I fight for, keeps me grounded.");
            }
            else if (speech.Contains("grandfather"))
            {
                Say("Grasp Graham was a titan in his time. He once told me a secret technique that I've never shared with anyone. Prove your worth, and maybe I'll let you in on it.");
            }
            else if (speech.Contains("mental"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Overcoming one's own doubts is the hardest challenge. But it's also the most rewarding. In fact, for showing interest, here's a small token of my appreciation.");
                    from.AddToBackpack(new FireballAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("hero"))
            {
                Say("Being a hero is not about the strength of your punch, but the size of your heart. I've seen heroes in all shapes and sizes.");
            }
            else if (speech.Contains("grounded"))
            {
                Say("When the world cheers for you, it's easy to forget the roots. But, always remember, it's the roots that keep the tree standing.");
            }

            base.OnSpeech(e);
        }

        public GripGareth(Serial serial) : base(serial) { }

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
