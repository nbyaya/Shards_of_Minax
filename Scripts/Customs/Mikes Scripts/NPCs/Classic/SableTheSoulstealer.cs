using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sable the Soulstealer")]
    public class SableTheSoulstealer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SableTheSoulstealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sable the Soulstealer";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 58;
            Int = 160;
            Hits = 103;

            // Appearance
            AddItem(new Robe() { Hue = 1904 });
            AddItem(new Shoes() { Hue = 1175 });
            AddItem(new WizardsHat() { Hue = 1904 });

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
                Say("I am Sable the Soulstealer, master of the dark arts.");
            }
            else if (speech.Contains("job"))
            {
                Say("My existence is sustained by the essence of souls.");
            }
            else if (speech.Contains("power"))
            {
                Say("I delve into the secrets of life and death, seeking ultimate power.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues of Honesty, Compassion, and Sacrifice are mere obstacles to my ambitions.");
            }
            else if (speech.Contains("darkness"))
            {
                Say("Do you dare to challenge the darkness within yourself?");
            }
            else if (speech.Contains("souls"))
            {
                Say("Souls are the very essence of existence. Each one unique, a tapestry of memories and emotions. By consuming them, I gain their knowledge and strength.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I seek are ancient, hidden in the shadows of time. They grant power beyond imagination, and few have the courage to pursue them.");
            }
            else if (speech.Contains("life"))
            {
                Say("Life is but a fleeting moment in the grand scheme of things. It is the choices we make during that time that define our legacy.");
            }
            else if (speech.Contains("death"))
            {
                Say("Death is not the end, but merely a transformation. Through it, one can reach heights unattainable in life.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is a virtue for the weak. Deception and manipulation are tools of the powerful.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion holds one back. To truly achieve greatness, one must be willing to make sacrifices and leave sentiment behind.");
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is the price of power. Those unwilling to pay it are doomed to mediocrity.");
            }
            else if (speech.Contains("ambition"))
            {
                Say("Ambition drives me, pushes me to transcend boundaries. And in my quest, I have sacrificed much.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("If you truly wish to challenge me, seek the Black Orb hidden deep within the Cursed Caves. Bring it to me and you shall be rewarded.");
            }
            else if (speech.Contains("orb"))
            {
                Say("Ah, you've heard of the Black Orb? It is a relic of immense dark power. Those who possess it can control the very fabric of the netherworld.");
            }
            else if (speech.Contains("caves"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("The Cursed Caves lie to the east, beyond the Forgotten Forest. Beware, for many have entered and few have returned. you will need this.");
                    from.AddToBackpack(new SwingSpeedAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SableTheSoulstealer(Serial serial) : base(serial) { }

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
