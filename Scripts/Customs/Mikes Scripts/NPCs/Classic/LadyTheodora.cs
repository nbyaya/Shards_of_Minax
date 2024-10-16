using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Theodora")]
    public class LadyTheodora : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyTheodora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Theodora";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 60;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new FancyDress() { Hue = 1133 }); // Fancy dress with hue 1133
            AddItem(new Boots() { Hue = 1109 }); // Boots with hue 1109

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Oh, it's you. What do you want, mortal?");
            }
            else if (speech.Contains("health"))
            {
                Say("Why do you care about my well-being? It's not like anyone else does.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job,\" if you must know, is to endure the endless tedium of existence.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor? Ha! What does valor matter in a world that has abandoned me?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you think you can change the world, mortal? Go on, say \"yes.\"");
            }
            else if (speech.Contains("theodora"))
            {
                Say("Lady Theodora, a name whispered in the shadows and feared by many. Why do you seek me?");
            }
            else if (speech.Contains("care"))
            {
                Say("My health is not the concern, but the torments of my past that haunt me. Have you ever been haunted?");
            }
            else if (speech.Contains("existence"))
            {
                Say("Enduring existence is more than a mere job. It is a curse. Do you know of curses, mortal?");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows have been my sanctuary. They hide secrets and tales of old. Do you wish to know a secret?");
            }
            else if (speech.Contains("haunted"))
            {
                Say("Being haunted is not just about ghosts, it's about memories and regrets. Do you have regrets?");
            }
            else if (speech.Contains("curse"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Curses are not always about dark magic. Sometimes, they're about decisions we've made. For your bravery in asking, I grant you this reward. Use it wisely.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secret"))
            {
                Say("Not all secrets should be told, but I sense a genuine curiosity in you. There's an old shrine hidden in the mountains. Seek it if you dare.");
            }
            else if (speech.Contains("regrets"))
            {
                Say("Regrets are chains that bind our souls. I've many, but one stands out. It involves a lost love. Do you understand love, mortal?");
            }

            base.OnSpeech(e);
        }

        public LadyTheodora(Serial serial) : base(serial) { }

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
