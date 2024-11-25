using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Theophilus Mirage")]
    public class TheophilusMirage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TheophilusMirage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Theophilus Mirage";
            Title = "the Illusionist";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Equip the NPC with themed items
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new WizardsHat(Utility.RandomBlueHue()));

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string speech = e.Speech.ToLower();

            if (!from.InRange(this, 3))
                return;

            if (speech.Contains("name"))
            {
                Say("Greetings, traveler. I am Theophilus Mirage, the Illusionist.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My craft involves weaving illusions and dreams. I delight in creating fantastical visions.");
            }
            else if (speech.Contains("illusion"))
            {
                Say("Illusions are but reflections of one's desires and fears. They can reveal truths if one is perceptive.");
            }
            else if (speech.Contains("reflections"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience is a virtue. Please return later for a special reward.");
                }
                else
                {
                    Say("You have demonstrated curiosity and perseverance. For your efforts, I present to you the Ethereal Plane Chest.");
                    from.AddToBackpack(new EtherealPlaneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public TheophilusMirage(Serial serial) : base(serial) { }

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
