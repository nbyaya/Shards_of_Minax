using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lucky McTavish")]
    public class LuckyMcTavish : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokenAboutName = false;
        private bool spokenAboutHealth = false;
        private bool spokenAboutJob = false;
        private bool spokenAboutTreasure = false;
        private bool spokenAboutHero = false;
        private bool spokenAboutWisdom = false;
        private bool spokenAboutCourage = false;

        [Constructable]
        public LuckyMcTavish() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lucky McTavish";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new GreenGourd() { Hue = 0x4E4 }); // Green themed item
            AddItem(new FancyShirt() { Hue = 0x3F3 }); // Green shirt
            AddItem(new Kilt() { Hue = 0x1F1 }); // Traditional kilt
            AddItem(new Sandals() { Hue = 0x1F1 }); // Traditional footwear
            AddItem(new Cap() { Hue = 0x1F1 }); // Green hat

            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = 0x203B; // Standard hair
            HairHue = 0x3F3; // Green hair hue

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            if (!from.InRange(this, 3)) return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, ye be speakin' to Lucky McTavish, the luckiest leprechaun ye'll ever meet! Have ye heard of me before?");
                spokenAboutName = true;
            }
            else if (spokenAboutName && speech.Contains("health"))
            {
                Say("I’m as healthy as a clover in the springtime, thank ye kindly! Health is a true treasure, don't ye agree?");
                spokenAboutHealth = true;
            }
            else if (spokenAboutHealth && speech.Contains("job"))
            {
                Say("Me job is to guard me treasure and make sure the luck of the Irish stays strong. Ever heard of me treasure?");
                spokenAboutJob = true;
            }
            else if (spokenAboutJob && speech.Contains("treasure"))
            {
                Say("Ah, treasure ye say? It’s a fine trove of loot. But first, ye need to show yer worth. Are ye a hero?");
                spokenAboutTreasure = true;
            }
            else if (spokenAboutTreasure && speech.Contains("hero"))
            {
                Say("A true hero is one who helps others and seeks no reward. Show me yer heroism and the treasure will be yours. Do ye have the courage?");
                spokenAboutHero = true;
            }
            else if (spokenAboutHero && speech.Contains("courage"))
            {
                Say("Courage is more than just bravery. It’s about facing challenges with a steady heart. Have ye wisdom as well?");
                spokenAboutCourage = true;
            }
            else if (spokenAboutCourage && speech.Contains("wisdom"))
            {
                Say("Wisdom comes from knowledge and experience. Use both wisely to gain me favor. Are ye ready for the reward?");
                spokenAboutWisdom = true;
            }
            else if (spokenAboutWisdom && speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Ye’ve already claimed me treasure for now. Return later for another chance.");
                }
                else
                {
                    Say("Ye’ve shown great courage and wisdom. For yer efforts, take this Leprechaun’s Trove, filled with all sorts of goodies!");
                    from.AddToBackpack(new LeprechaunsTrove()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("thanks"))
            {
                Say("Ye’re welcome! May the luck of the Irish be with ye always!");
            }
            else if (speech.Contains("courage") && !spokenAboutCourage)
            {
                Say("To show courage, ye must first understand what it means. Have ye spoken about me job or treasure?");
            }
            else if (speech.Contains("wisdom") && !spokenAboutWisdom)
            {
                Say("Wisdom is earned through experience. If ye haven't spoken about courage, then ye might need to start there.");
            }
            else if (speech.Contains("hero") && !spokenAboutHero)
            {
                Say("Heroism requires deeds of valor. Make sure ye've talked about me treasure and job to get the full story.");
            }
            else if (speech.Contains("treasure") && !spokenAboutTreasure)
            {
                Say("Treasure awaits those who prove their worth. Have ye learned about me job and health?");
            }
            else if (speech.Contains("job") && !spokenAboutJob)
            {
                Say("Me job is to guard me treasure. To unlock more, ye need to know about me health and name.");
            }
            else if (speech.Contains("health") && !spokenAboutHealth)
            {
                Say("Health is a treasure in itself. Before ye learn more, speak to me about me name.");
            }
            else if (speech.Contains("name") && !spokenAboutName)
            {
                Say("I’m Lucky McTavish, a name ye should remember. Start with this and we’ll see what more ye can uncover.");
            }
            
            base.OnSpeech(e);
        }

        public LuckyMcTavish(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokenAboutName);
            writer.Write(spokenAboutHealth);
            writer.Write(spokenAboutJob);
            writer.Write(spokenAboutTreasure);
            writer.Write(spokenAboutHero);
            writer.Write(spokenAboutWisdom);
            writer.Write(spokenAboutCourage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokenAboutName = reader.ReadBool();
            spokenAboutHealth = reader.ReadBool();
            spokenAboutJob = reader.ReadBool();
            spokenAboutTreasure = reader.ReadBool();
            spokenAboutHero = reader.ReadBool();
            spokenAboutWisdom = reader.ReadBool();
            spokenAboutCourage = reader.ReadBool();
        }
    }
}
