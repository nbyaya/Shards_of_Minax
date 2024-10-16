using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of King Cedric")]
    public class KingCedric : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KingCedric() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "King Cedric";
            Body = 0x190; // Human male body

            // Stats
            Str = 115;
            Dex = 90;
            Int = 85;
            Hits = 85;

            // Appearance
            AddItem(new Robe() { Hue = 1302 });
            AddItem(new Boots() { Hue = 1302 });
            AddItem(new VikingSword() { Name = "King Cedric's Sword" });

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
                Say("I am King Cedric, ruler of this wretched kingdom!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ha! What does it matter in this miserable realm?");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' as king? To preside over this wretched, doomed land.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valiant? Ha! In this cursed realm, valor is but a fleeting shadow.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you dare to defy me, insignificant one?");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("This kingdom was once a beacon of hope, but now it's overshadowed by darkness and treachery.");
            }
            else if (speech.Contains("realm"))
            {
                Say("This realm has been plagued by an ancient curse. Many have tried to lift it, all have failed.");
            }
            else if (speech.Contains("doomed"))
            {
                Say("We were betrayed by one of our own, the former Court Mage. His treachery brought this doom upon us.");
            }
            else if (speech.Contains("darkness"))
            {
                Say("The darkness is not just a metaphor. It's a living entity, seeking to consume everything. If only we had the lost artifact, we might stand a chance.");
            }
            else if (speech.Contains("curse"))
            {
                Say("The curse manifests as a never-ending night, and with it, creatures of the abyss have risen. Only a hero can turn the tide now.");
            }
            else if (speech.Contains("mage"))
            {
                Say("The former Court Mage, Elarion, sought power above all. In his ambition, he unleashed forces he couldn't control. Now, he resides in the Tower of Desolation, protected by his dark creations.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("The lost artifact, the Orb of Luminance, was stolen years ago. Its light has the power to pierce the darkness. Seek it, and perhaps you shall be rewarded.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Indeed, for those who aid this kingdom in its darkest hour, I promise treasures beyond imagination. But beware, the path is fraught with danger. Here take this.");
                    from.AddToBackpack(new BagOfHealth()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KingCedric(Serial serial) : base(serial) { }

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
