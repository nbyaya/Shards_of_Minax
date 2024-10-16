using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cypher")]
    public class Cypher : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Cypher() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cypher";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 100;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new LongPants() { Hue = 1175 });
            AddItem(new FancyShirt() { Hue = 1175 });
            AddItem(new Cloak() { Hue = 1175 });
            AddItem(new ThighBoots() { Hue = 1175 });
            AddItem(new LeatherGloves() { Hue = 1175 });
            AddItem(new GnarledStaff() { Name = "Cypher's Hacking Device" });
            AddItem(new SkullCap() { Hue = 1175 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Cypher, the embittered one.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as rotten as this wretched world.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' in this wretched existence? To survive another day.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Do you even understand the pain of existence, or are you another mindless drone?");
            }
            else if (speech.Contains("yes") && SpeechEntryCompleted(30))
            {
                Say("If you can comprehend the agony, then perhaps there's hope for you yet.");
            }
            else if (speech.Contains("rotten"))
            {
                Say("Yes, the very core of this realm seems to decay with each passing day. I once held hope, but that too has faded.");
            }
            else if (speech.Contains("survive"))
            {
                Say("Survival is a relentless game here. Every day brings new challenges, new enemies, and new reasons to question why I persist.");
            }
            else if (speech.Contains("pain"))
            {
                Say("Pain is all I've known for ages. But sometimes, in pain, one can find purpose. Perhaps you seek purpose too?");
            }
            else if (speech.Contains("purpose"))
            {
                Say("Ah, purpose. It's the elusive shadow we all chase. But in this grim world, it seems out of grasp. Yet, you... perhaps you might find it. Seek the Forgotten Shrine. It may hold answers.");
            }
            else if (speech.Contains("shrine"))
            {
                Say("It's a place lost in time, somewhere in the Eastern wilds. I would guide you, but my past encounters with it bring me nothing but torment. If you manage to discover its secrets, come back and tell me. There might be a reward for your efforts.");
            }
            else if (speech.Contains("reward"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, always seeking material gain, aren't we? Very well. If you unveil the secrets of the Forgotten Shrine and share them with me, I might have something special for you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("drone"))
            {
                Say("Many wander these lands without thought, following orders, seeking only survival. But a few, like you, dare to question, dare to understand. That sets you apart.");
            }
            else if (speech.Contains("hope"))
            {
                Say("Hope is a delicate thing, easily shattered. But even in this bleak existence, sparks of hope can ignite a fire. Do you carry such a spark? Here take this.");
                from.AddToBackpack(new MaxxiaScroll()); // Reward for hope
            }

            base.OnSpeech(e);
        }

        private bool SpeechEntryCompleted(int entryNumber)
        {
            // Placeholder for checking if a particular speech entry has been completed
            // Implement logic based on your requirements
            return true;
        }

        public Cypher(Serial serial) : base(serial) { }

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
