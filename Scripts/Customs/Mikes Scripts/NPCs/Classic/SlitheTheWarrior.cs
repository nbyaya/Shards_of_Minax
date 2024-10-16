using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Slithe the Warrior")]
    public class SlitheTheWarrior : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SlitheTheWarrior() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Slithe the Warrior";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 100;
            Int = 75;
            Hits = 100;

            // Appearance
            AddItem(new PlateChest() { Hue = 1316 });
            AddItem(new PlateArms() { Hue = 1316 });
            AddItem(new Cloak() { Hue = 1316 });
            AddItem(new PlateLegs() { Hue = 1316 });
            AddItem(new Bascinet() { Hue = 1316 });
            AddItem(new PlateGorget() { Hue = 1316 });
            AddItem(new PlateGloves() { Hue = 1316 });
            AddItem(new Broadsword() { Name = "Slithe's Blade" });

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
                Say("I am Slithe the Warrior!");
            }
            else if (speech.Contains("health"))
            {
                Say("Only minor wounds!");
            }
            else if (speech.Contains("job"))
            {
                Say("I fight battles!");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is seen not in the force of arms, but in the force of will! Art thou valiant?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then never flee unless the need is dire!");
            }
            else if (speech.Contains("silthe"))
            {
                Say("Ah, so you've heard of me! Long have I been in battles, defending the honor of my land.");
            }
            else if (speech.Contains("wounds"))
            {
                Say("These are but scratches, earned in fierce combat. Have you seen such battle scars?");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Ah, the clashes of swords and the roar of combat. There is a special mantra I chant to keep my valor strong. Want to know of it?");
            }
            else if (speech.Contains("mantra") && speech.Contains("yes"))
            {
                Say("The mantra of Valor is a sacred chant. While I cannot reveal it all, the third syllable is RAH. Use it wisely!");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is what drives me. It's not about the glory or the loot, but the principle. Do you value honor as I do?");
            }
            else if (speech.Contains("scars"))
            {
                Say("Every scar tells a story of valor, of challenges faced and overcome. Your journey must've given you tales to tell too.");
            }
            else if (speech.Contains("tales"))
            {
                Say("Stories of valor, bravery, and sacrifice are what shape our world. Every hero has tales worth sharing. Will you share yours?");
            }
            else if (speech.Contains("hero"))
            {
                Say("Being a hero is not about fame or fortune. It's about doing what's right, even when it's hard. Are you ready to make the hard choices?");
            }
            else if (speech.Contains("choices"))
            {
                Say("In every battle, we make choices that define us. Some are easy, some are tough. But in the end, it's the choices we make that craft our legacy. What legacy do you seek?");
            }
            else if (speech.Contains("legacy"))
            {
                Say("Every warrior wishes to leave a legacy behind, a tale of valor and honor. My legacy is my battles and the lessons they taught me. What lessons have your battles taught you?");
            }

            base.OnSpeech(e);
        }

        public SlitheTheWarrior(Serial serial) : base(serial) { }

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
