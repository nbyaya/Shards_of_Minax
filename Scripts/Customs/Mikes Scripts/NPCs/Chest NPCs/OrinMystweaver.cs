using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Orin Mystweaver")]
    public class OrinMystweaver : BaseCreature
    {
        private DateTime lastRewardTime;
        private bool spokeOfEnchantment;
        private bool spokeOfWisdom;
        private bool spokeOfPondering;
        private bool spokeOfEssence;

        [Constructable]
        public OrinMystweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Orin Mystweaver";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 70;

            // Appearance
            AddItem(new Robe() { Hue = Utility.RandomMetalHue() });
            AddItem(new WizardsHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Orin's Tome of Enchantment" });

            Hue = Utility.RandomSkinHue(); // Skin color
            HairItemID = Utility.RandomList(0x204B, 0x203B); // Random hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = -1; // No facial hair

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Initialize dialogue progress
            spokeOfEnchantment = false;
            spokeOfWisdom = false;
            spokeOfPondering = false;
            spokeOfEssence = false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            
            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ah, you have found me! I am Orin Mystweaver, the keeper of arcane secrets.");
                spokeOfEnchantment = true; // Unlocks subsequent dialogue
            }
            else if (spokeOfEnchantment && speech.Contains("job"))
            {
                Say("My role is to safeguard ancient knowledge and bestow enchanted treasures upon those deemed worthy.");
                spokeOfWisdom = true; // Unlocks subsequent dialogue
            }
            else if (spokeOfWisdom && speech.Contains("health"))
            {
                Say("I am in excellent health, for magic has a way of preserving the vitality of its servants.");
                spokeOfPondering = true; // Unlocks subsequent dialogue
            }
            else if (spokeOfPondering && speech.Contains("secrets"))
            {
                Say("The secrets I guard are not easily revealed. You must prove your worth to uncover them.");
                spokeOfEssence = true; // Unlocks subsequent dialogue
            }
            else if (spokeOfEssence && speech.Contains("prove"))
            {
                Say("To prove yourself, you must seek the true essence of enchantment. Show me your understanding.");
            }
            else if (spokeOfEssence && speech.Contains("essence"))
            {
                Say("The essence of enchantment is not merely magic but understanding the art of imbuing objects with profound properties.");
            }
            else if (spokeOfEssence && speech.Contains("enchantment"))
            {
                Say("Enchantment is the art of bestowing profound properties onto objects. It requires great insight and skill.");
            }
            else if (spokeOfEssence && speech.Contains("wisdom"))
            {
                Say("Wisdom is earned through patience and reflection. Have you pondered the deeper mysteries of the arcane?");
            }
            else if (spokeOfEssence && speech.Contains("ponder"))
            {
                Say("Pondering the mysteries of the arcane is key to understanding. Reflect deeply, and the secrets will be revealed.");
            }
            else if (spokeOfEssence && speech.Contains("reveal"))
            {
                Say("Revealing the secrets requires a true understanding of the arcane. Speak further if you seek enlightenment.");
            }
            else if (spokeOfEssence && speech.Contains("enlightenment"))
            {
                Say("Enlightenment comes from understanding and mastering the arcane arts. Show me your dedication.");
            }
            else if (spokeOfEssence && speech.Contains("dedication"))
            {
                Say("Your dedication to the study of enchantment is admirable. For your efforts, I offer you a reward.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience is key. Return to me when the time is right.");
                }
                else
                {
                    Say("You have shown great understanding. Accept this Mystical Enchanter's Chest as your reward.");
                    from.AddToBackpack(new MysticalEnchantersChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public OrinMystweaver(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
            writer.Write(spokeOfEnchantment);
            writer.Write(spokeOfWisdom);
            writer.Write(spokeOfPondering);
            writer.Write(spokeOfEssence);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
            spokeOfEnchantment = reader.ReadBool();
            spokeOfWisdom = reader.ReadBool();
            spokeOfPondering = reader.ReadBool();
            spokeOfEssence = reader.ReadBool();
        }
    }
}
