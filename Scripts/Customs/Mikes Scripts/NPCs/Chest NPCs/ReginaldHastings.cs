using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Reginald Hastings")]
    public class ReginaldHastings : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ReginaldHastings() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Reginald Hastings";
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
            
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Medium hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = 0x2045; // Beard

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
                Say("Greetings, I am Reginald Hastings, curator of ancient relics. Tell me, do you seek knowledge of the Victorian era?");
            }
            else if (speech.Contains("victorian"))
            {
                Say("Indeed, the Victorian era was a time of great progress and elegance. What more would you like to learn about this fascinating period?");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress during the Victorian era was marked by industrial advancements and cultural shifts. Do you find such innovations intriguing?");
            }
            else if (speech.Contains("innovations"))
            {
                Say("The innovations of the era include steam engines, telegraphs, and more. Have you explored any artifacts related to these advancements?");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Artifacts are crucial to understanding history. Some are hidden, waiting to be discovered. Do you enjoy the thrill of uncovering such items?");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("Discoveries often lead to further exploration. If you seek hidden treasures, the quest may take you through intricate puzzles and hidden clues.");
            }
            else if (speech.Contains("puzzles"))
            {
                Say("Puzzles are a way to challenge and reward the curious. Solving them can reveal great secrets. Have you encountered any intriguing puzzles recently?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are often embedded within the fabric of history. Revealing them requires perseverance. Are you prepared for such a journey?");
            }
            else if (speech.Contains("prepared"))
            {
                Say("Preparation is key to success. If you are ready, I have a reward for those who prove their dedication to uncovering the past.");
            }
            else if (speech.Contains("collection"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("The rewards are not yet available. Return after a while.");
                }
                else
                {
                    Say("Your dedication and curiosity have been commendable. As a token of appreciation, here is a chest from the Victorian era.");
                    from.AddToBackpack(new VictorianEraChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("chest"))
            {
                Say("This chest holds treasures from the Victorian era. Each item within it tells a story of its own. Enjoy the discovery!");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Treasures from the Victorian age are not only valuable but imbued with the spirit of an age long past.");
            }
            else if (speech.Contains("story"))
            {
                Say("Stories of the Victorian era are rich with intrigue and drama. Each artifact has a tale to tell. Are you a collector of such stories?");
            }
            else if (speech.Contains("collector"))
            {
                Say("Collectors often seek rare and unique items. Your interest in history aligns well with this pursuit. Have you collected any noteworthy items yourself?");
            }
            else if (speech.Contains("items"))
            {
                Say("Indeed, the items of the Victorian era can be both beautiful and meaningful. Perhaps you will find something to add to your collection from this chest.");
            }

            base.OnSpeech(e);
        }

        public ReginaldHastings(Serial serial) : base(serial) { }

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
