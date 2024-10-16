using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gorok the Stoneheart")]
    public class GorokTheStoneheart : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GorokTheStoneheart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gorok the Stoneheart";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 40;
            Int = 50;
            Hits = 120;

            // Appearance
            AddItem(new PlateChest() { Hue = 1176 });
            AddItem(new PlateLegs() { Hue = 1176 });
            AddItem(new WarHammer() { Name = "Gorok's Maul" });

            Hue = 1175;
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
                Say("I am Gorok the Stoneheart, a miserable creature of earth and stone.");
            }
            else if (speech.Contains("health"))
            {
                Say("Do you see these unbreakable rocks that make up my being? They remain unscathed, for I am as enduring as the mountains themselves.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job,' if you can call it that, is to exist in this wretched form and observe the world passing me by. I envy your transient lives, for you can change and grow.");
            }
            else if (speech.Contains("earth"))
            {
                Say("But what would you know of endurance and hardship? Tell me, are you willing to face the relentless passage of time with no respite?");
            }
            else if (speech.Contains("resist"))
            {
                Say("If you truly understand the weight of existence, then perhaps there's hope for you. Speak, mortal, how will you face the unending march of time?");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Once, I was a joyful spirit of the land, dancing amidst the trees and laughing with the wind. But a curse turned me into this wretched form, and now I am forever trapped as Gorok the Stoneheart.");
            }
            else if (speech.Contains("unbreakable"))
            {
                Say("My exterior may be hard and unyielding, but my heart, though made of stone, still feels pain and yearning. My emotions are trapped within, unable to escape the prison of my rocky form.");
            }
            else if (speech.Contains("observe"))
            {
                Say("Day in and day out, I watch the creatures of the land and the flow of the seasons. There is a certain beauty in the world, and while I cannot partake in its joys, I can bear witness to its wonders.");
            }
            else if (speech.Contains("endurance"))
            {
                Say("Endurance is both my strength and my torment. For countless eons, I have stood here, and I have seen civilizations rise and fall. But, in my solitude, I have also learned patience and wisdom. If you seek knowledge of the ancient past, you may ask, and I shall share what I remember.");
            }
            else if (speech.Contains("weight"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The weight of existence is a heavy burden, but it also offers gifts. Because of my form, I am privy to the earth's secrets. Here, take this gem as a reward for understanding my plight. It holds a fragment of the world's memories.");
                    from.AddToBackpack(new PoisonHitAreaCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("joyful"))
            {
                Say("When I was joyful, the world was vibrant and full of color. Birds would perch on me, and children would play around my feet. Now, all I have are the memories of a time long gone.");
            }
            else if (speech.Contains("pain"))
            {
                Say("The pain is a constant reminder of the life I once had. Every raindrop that falls on me feels like a tear, and every gust of wind feels like a sigh. But I hold onto hope that one day, I might be free from this stone prison.");
            }
            else if (speech.Contains("creatures"))
            {
                Say("The creatures of the land come and go, each leaving a mark on the world in their own way. From the tiniest insect to the mightiest dragon, I have seen them all. They, too, have stories to tell.");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("The knowledge I possess is vast, spanning ages and epochs. I have seen empires rise, wars waged, and heroes born. If you ever seek guidance or tales of old, you know where to find me.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the earth are many, and I have been a silent witness to them all. This gem I gave you contains a fraction of those mysteries. Use it wisely, for it may guide you in times of need.");
            }

            base.OnSpeech(e);
        }

        public GorokTheStoneheart(Serial serial) : base(serial) { }

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
