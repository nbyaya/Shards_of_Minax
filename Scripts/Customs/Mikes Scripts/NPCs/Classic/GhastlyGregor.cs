using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ghastly Gregor")]
    public class GhastlyGregor : BaseCreature
    {
        [Constructable]
        public GhastlyGregor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ghastly Gregor";
            Body = 0x190; // Human male body

            // Stats
            Str = 140;
            Dex = 60;
            Int = 100;
            Hits = 110;

            // Appearance
            AddItem(new Robe() { Hue = 1154 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new BoneGloves() { Name = "Gregor's Grasping Gloves" });

            // Customizing appearance
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("I am Ghastly Gregor, the necromancer. What pitiful questions do you have for me?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is sustained by dark magic. I am beyond the concept of health.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' is to embrace the shadows and command the undead. I revel in the art of death.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Knowledge is power, and power is all that matters. Are you clever enough to handle my truths?");
            }
            else if (speech.Contains("yes"))
            {
                Say("You seem audacious. Prove your worth. What do you know of the dark arts?");
            }
            else if (speech.Contains("ghastly"))
            {
                Say("Long ago, I was merely Gregor. But through my studies and mastery over the forbidden arts, I became Ghastly Gregor. My name itself carries the weight of my power.");
            }
            else if (speech.Contains("magic"))
            {
                Say("The dark magic that sustains me was acquired from ancient texts and forbidden rituals. I traded my mortality for knowledge, and with it, I've found eternal life, albeit not as the living know it.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are not merely the absence of light. They are alive, sentient, and ever-present. By embracing the shadows, one learns to manipulate the very fabric of reality.");
            }
            else if (speech.Contains("studies"))
            {
                Say("I have spent countless hours in the catacombs, deciphering old tomes and consulting spirits. It's there that I learned the true nature of life and death.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("These texts, shrouded in mystery, were not easy to obtain. Many were guarded by the restless dead. But for one who is worthy, they might share a fragment of their knowledge as a reward.");
            }
            else if (speech.Contains("manipulate"))
            {
                Say("To manipulate is to control. And to control the undead, one must first understand them. Only then can their true power be harnessed.");
            }
            else if (speech.Contains("worthy"))
            {
                Say("I see potential in you. Answer me this riddle, and I may deem you worthy of a reward. 'I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.' What am I?");
            }
            else if (speech.Contains("riddle"))
            {
                // Reward logic
                Say("Ah, you've deciphered it! An echo is the answer. Very well, take this token of my appreciation. May it serve you in the dark times ahead.");
                from.AddToBackpack(new BushidoAugmentCrystal()); // Assuming this is the item to reward
            }

            base.OnSpeech(e);
        }

        public GhastlyGregor(Serial serial) : base(serial) { }

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
