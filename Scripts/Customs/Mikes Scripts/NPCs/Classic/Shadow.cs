using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shadow")]
    public class Shadow : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Shadow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shadow";
            Body = 0x190; // Human male body
            
            // Stats
            Str = 120;
            Dex = 120;
            Int = 60;
            Hits = 85;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1100 });
            AddItem(new LeatherCap() { Hue = 1100 });
            AddItem(new LeatherGloves() { Hue = 1100 });
            AddItem(new ChainChest() { Hue = 1100 });
            AddItem(new Kryss() { Name = "Shadow's Blade" });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("Hmph, what do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What's it to you?");
            }
            else if (speech.Contains("job"))
            {
                Say("Job? I'm just a hired blade, nothing more.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life's a battlefield, kid. You fight to survive, or you die trying. Got any regrets?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha, don't we all? Life's a cruel joke, my friend. But if you're gonna survive in this world, you better learn to laugh along with it.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("Shadow is just an alias I use. A name lost in the midst of time, from battles fought and memories buried deep.");
            }
            else if (speech.Contains("battlefield"))
            {
                Say("I've had my fair share of scars and wounds. They tell a story of a life lived on the edge. Ever been to battle?");
            }
            else if (speech.Contains("blade"))
            {
                Say("A blade might be just a tool, but it's a companion in the darkest of times. I've wielded many, but there's one that stands out. Ever heard of the 'Crimson Dagger'?");
            }
            else if (speech.Contains("alias"))
            {
                Say("Names, titles, aliases... they all serve a purpose. Sometimes to hide, other times to be remembered. What's in a name anyway? Would you change yours?");
            }
            else if (speech.Contains("scars"))
            {
                Say("Every scar has a tale. Some of victory, others of loss. But they all shape who we are. Ever received a scar that changed you?");
            }
            else if (speech.Contains("crimson"))
            {
                Say("Ah, the Crimson Dagger, a weapon of legends. I came across it during one of my missions. There's magic in its blade. In the right hands, it can change fortunes. Seek it if you dare, and maybe, just maybe, I might reward you for your efforts.");
            }
            else if (speech.Contains("change"))
            {
                Say("Change is inevitable. Whether it's names or destinies, everything shifts with time. What's something you'd want to change in your life?");
            }
            else if (speech.Contains("received"))
            {
                Say("It's not just the physical scars that matter, but the emotional ones too. They shape our character, our very being. Have you ever faced an event that marked you forever?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Many have sought the Crimson Dagger, few have found it. The journey is treacherous, but the reward... priceless. Return it to me, and I'll ensure you're handsomely compensated.");
            }
            else if (speech.Contains("destinies"))
            {
                Say("Destinies are written in the stars, but our choices shape them. Every decision, every path taken, leads to a different outcome. Have you ever felt you were meant for something greater?");
            }
            else if (speech.Contains("event"))
            {
                Say("Events, big or small, shape our lives. Some leave scars, others memories. It's how we react to them that defines us. Ever made a choice you regret?");
            }
            else if (speech.Contains("meant"))
            {
                Say("Feeling that you're meant for something greater is both a blessing and a curse. It drives you, motivates you, but can also consume you. What drives you forward?");
            }
            else if (speech.Contains("choice"))
            {
                Say("Choices, good or bad, are a part of life. Sometimes they lead us to unexpected places, other times to dead ends. But every choice has a consequence. Have you faced the repercussions of a past decision?");
            }
            else if (speech.Contains("drives"))
            {
                Say("Drive is what pushes us beyond our limits. It's what makes us get up every morning, face challenges, and strive for more. Without it, we're lost. What's your greatest ambition?");
            }
            else if (speech.Contains("repercussions"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Every action has a reaction. Sometimes immediate, other times delayed. But they always come. Have you ever tried to undo a past mistake? Take This.");
                    from.AddToBackpack(new PlateLeggingsOfCommand()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Shadow(Serial serial) : base(serial) { }

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
