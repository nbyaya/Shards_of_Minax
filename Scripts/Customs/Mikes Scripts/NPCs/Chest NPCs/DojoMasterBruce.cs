using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dojo Master Bruce")]
    public class DojoMasterBruce : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DojoMasterBruce() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dojo Master Bruce";
            Body = 0x190; // Male body

            // Stats
            Str = 100;
            Dex = 90;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 1152 });
            AddItem(new PlateArms() { Hue = 1152 });
            AddItem(new PlateLegs() { Hue = 1152 });
            AddItem(new PlateHelm() { Hue = 1152 });
            AddItem(new PlateGloves() { Hue = 1152 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new WoodenShield() { Hue = 1152 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Utility.RandomHairHue();

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
                Say("Greetings, traveler. I am Dojo Master Bruce.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to train warriors in the ancient arts of the dojo.");
            }
            else if (speech.Contains("dojo"))
            {
                Say("The dojo is a place of discipline, honor, and strength. It is where true warriors are forged.");
            }
            else if (speech.Contains("training"))
            {
                Say("Training is the path to mastery. Only through dedication and hard work can one achieve greatness.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical. A true warrior has strength of mind and spirit as well.");
            }
            else if (speech.Contains("physical"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your dedication to learning the ways of the dojo has earned you a reward. Please accept this Dojo Legacy Chest.");
                    from.AddToBackpack(new DojoLegacyChest());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public DojoMasterBruce(Serial serial) : base(serial) { }

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
