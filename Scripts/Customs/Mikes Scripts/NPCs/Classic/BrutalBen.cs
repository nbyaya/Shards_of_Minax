using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Brutal Ben")]
    public class BrutalBen : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrutalBen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brutal Ben";
            Body = 0x190; // Human male body

            // Stats
            Str = 170;
            Dex = 55;
            Int = 18;
            Hits = 120;

            // Appearance
            AddItem(new ChainChest() { Hue = 1102 });
            AddItem(new ChainLegs() { Hue = 1102 });
            AddItem(new PlateHelm() { Hue = 1102 });
            AddItem(new LeatherGloves() { Name = "Ben's Bashing Gloves" });

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
                Say("Brutal Ben, they call me. What's it to you?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ha! I've seen worse. But that's none of your business.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I'm in the business of death, friend. Not that you'd understand.");
            }
            else if (speech.Contains("murderer"))
            {
                Say("Think you're tough, eh? Let me ask you this: Do you know the taste of blood?");
            }
            else if (speech.Contains("blood"))
            {
                Say("Ha! You talk big, but words won't save you in the shadows of the night.");
            }
            else if (speech.Contains("brutal"))
            {
                Say("Brutal? I got that name from countless fights in the taverns. Few dare to cross me after hearing the tales.");
            }
            else if (speech.Contains("worse"))
            {
                Say("I've been beaten, broken, and left for dead more times than you can count. Yet here I stand. Do you know what keeps me going?");
            }
            else if (speech.Contains("death"))
            {
                Say("Death isn't just my business, it's an art. And the blade? It's my brush. Ever heard of the Silent Dagger?");
            }
            else if (speech.Contains("tough"))
            {
                Say("Being tough is more than muscle and bravado. It's about survival. You ever been to the Dead Man's Alley?");
            }
            else if (speech.Contains("taverns"))
            {
                Say("Ah, the taverns. Where I've made enemies and, surprisingly, a few friends. Ever met Old Barley the bartender?");
            }
            else if (speech.Contains("broken"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Even when I was down, I never lost hope. The amulet my mother gave me has always brought me luck. Say, you seem like someone who appreciates trinkets. Here, have this.");
                    from.AddToBackpack(new MaxxiaScroll()); // Assuming AdventurerKey is a valid item
                    lastRewardTime = DateTime.UtcNow;
                }
            }
            else if (speech.Contains("silent"))
            {
                Say("Ah, the Silent Dagger, the guild of assassins. Some say they're just a myth, but those in the know, fear them. I might or might not be connected, but ask too much and you might find out the hard way.");
            }
            else if (speech.Contains("alley"))
            {
                Say("It's a dark place, where many have met their end. It's not for the faint of heart. You looking to prove something?");
            }
            else if (speech.Contains("barley"))
            {
                Say("Old Barley? He's seen it all. From the rowdiest brawls to the most touching reunions. If walls could talk, they'd probably ask him for the stories.");
            }
            else if (speech.Contains("guild"))
            {
                Say("Guilds are more than just groups. They're families, bound by purpose and sometimes, dark secrets. Be careful who you trust.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Many have tried to prove themselves, to earn respect or fear. Most fail. But a rare few... they become legends. What will your story be?");
            }
            else if (speech.Contains("brawls"))
            {
                Say("I've been in many brawls, and each scar tells a story. Some of pride, some of regret. But all of survival. Care to challenge me?");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is a rare commodity these days. Even in the darkest corners, you find betrayal. Remember, keep your friends close, but your enemies closer.");
            }
            else if (speech.Contains("legends"))
            {
                Say("Legends are born from deeds, from choices made in crucial moments. Will you be remembered as a hero, or fade into obscurity?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("A challenge? From you? Haha! Come back when you've seen more of the world and its horrors. Then we'll talk.");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("Betrayal stings more than the sharpest blade. I've been betrayed, but I've also done my share. It's a circle that never ends.");
            }
            else if (speech.Contains("hero"))
            {
                Say("Heroes are a dime a dozen, but true heroes? They're the ones who stand when all else have fallen. Who are you in the grand tapestry of fate?");
            }
            else if (speech.Contains("horrors"))
            {
                Say("Horrors are everywhere. From the deepest dungeons to the highest towers. But the greatest horrors? They lie within us.");
            }
            else if (speech.Contains("blade"))
            {
                Say("The blade is an extension of one's self. It tells a story with every swing, every cut. But a blade is only as good as its wielder.");
            }
            else if (speech.Contains("fate"))
            {
                Say("Fate is a fickle mistress. She weaves our destinies with threads of gold and shadow. What path have you chosen?");
            }

            base.OnSpeech(e);
        }

        public BrutalBen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
