using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Reginald of Chesterton")]
    public class SirReginaldOfChesterton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirReginaldOfChesterton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald of Chesterton";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            Hue = Race.RandomSkinHue(); // Random skin hue
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

            if (speech.Contains("name"))
            {
                Say("Hail, traveler! I am Sir Reginald of Chesterton, protector of this land.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as steadfast as my honor, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard this realm and ensure that only the most worthy find the treasures hidden within.");
            }
            else if (speech.Contains("chivalry"))
            {
                Say("Chivalry is the code by which we live, a blend of honor, courage, and gallantry. Do you wish to know more?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is facing fear with resolve. It is a trait valued above many. How do you display your courage?");
            }
            else if (speech.Contains("display"))
            {
                Say("To display courage is to act bravely in the face of adversity. Have you proven your bravery?");
            }
            else if (speech.Contains("bravery"))
            {
                Say("Bravery is shown through deeds and actions. Show me your bravery, and you might earn a reward.");
            }
            else if (speech.Contains("deeds"))
            {
                Say("Deeds of valor are remembered throughout history. Have you performed any notable deeds recently?");
            }
            else if (speech.Contains("notable"))
            {
                Say("Notable deeds are those that stand out and inspire others. Tell me of your notable deeds.");
            }
            else if (speech.Contains("inspire"))
            {
                Say("To inspire is to motivate others through your actions. A true knight inspires others to be better.");
            }
            else if (speech.Contains("knight"))
            {
                Say("A knight is bound by honor and duty. Do you seek to be a knight yourself?");
            }
            else if (speech.Contains("duty"))
            {
                Say("Duty is a commitment to serve and protect. It is a noble path. What duty do you follow?");
            }
            else if (speech.Contains("serve"))
            {
                Say("Serving others is a path of humility and respect. How do you serve those around you?");
            }
            else if (speech.Contains("humility"))
            {
                Say("Humility is recognizing one's place in the grand scheme of things. It is crucial for a noble heart.");
            }
            else if (speech.Contains("noble"))
            {
                Say("Nobility is not just a title but a way of living with integrity and honor. Do you aspire to nobility?");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the foundation of trust and honor. A person of integrity is steadfast and true.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is earned through consistent actions and truthfulness. It is the bedrock of strong relationships.");
            }
            else if (speech.Contains("relationships"))
            {
                Say("Strong relationships are built on mutual respect and understanding. How do you cultivate your relationships?");
            }
            else if (speech.Contains("respect"))
            {
                Say("Respect is earned by treating others with kindness and fairness. It is the cornerstone of all meaningful connections.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Kindness is a gentle strength that can heal and bring joy. Show me your kindness.");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is a bright light in the darkness. It is often found in the simplest of moments.");
            }
            else if (speech.Contains("moments"))
            {
                Say("Moments of true connection and happiness are the most cherished. Have you found such moments?");
            }
            else if (speech.Contains("connection"))
            {
                Say("True connection is when hearts and minds meet. It brings a sense of unity and purpose.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is strength. When people come together with a common purpose, great things can be achieved.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical, but also moral and emotional. It guides us through trials and challenges.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Trials test our resolve and character. Overcoming them shows true strength and determination.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination is the will to persevere despite obstacles. It leads to success and achievement.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the culmination of hard work and perseverance. It is often the reward of those who strive.");
            }
            else if (speech.Contains("strive"))
            {
                Say("A reward is given for your efforts and achievements. For your bravery and perseverance, I present to you the chest of Medieval England. May it serve you well!");
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    from.AddToBackpack(new MedievalEnglandChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                else
                {
                    Say("You have not yet proven your valor. Return when you have shown more of your qualities.");
                }
            }

            base.OnSpeech(e);
        }

        public SirReginaldOfChesterton(Serial serial) : base(serial) { }

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
