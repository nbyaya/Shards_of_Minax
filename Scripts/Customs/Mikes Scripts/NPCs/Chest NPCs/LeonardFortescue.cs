using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class LeonardFortescue : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public LeonardFortescue() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Leonard Fortescue";
            Title = "the Historian";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Equip items
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGorget() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Spellbook() { Name = "Fortescue's Tome of Lore" });

            HairItemID = Utility.RandomList(0x203B, 0x2046); // Short hair styles
            HairHue = Utility.RandomHairHue();

            // Set skills to reflect knowledge
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Inscribe, 80.0, 100.0);
            SetSkill(SkillName.ItemID, 80.0, 100.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Greetings, I am Leonard Fortescue, the Historian.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard ancient treasures and share knowledge of their history.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you for your concern.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure of the Confederation! A remarkable relic indeed.");
            }
            else if (speech.Contains("cache"))
            {
                Say("The Confederation Cache holds many valuable items. Only those who prove their worth may access it.");
            }
            else if (speech.Contains("confederation"))
            {
                Say("The Confederation was a grand alliance, forged in unity and strength. It is honored here.");
            }
            else if (speech.Contains("reward"))
            {
                if (CheckRewardConditions(from))
                {
                    GiveReward(from);
                }
                else
                {
                    Say("You must prove yourself worthy before I can grant you the treasure.");
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return !m_RewardGiven; // Reward only once
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                ConfederationCache cache = new ConfederationCache(); // Create the custom chest
                from.AddToBackpack(cache);
                Say("You have demonstrated your knowledge and worth. Accept this Confederation Cache as your reward.");
                m_RewardGiven = true; // Mark reward as given
            }
        }

        public LeonardFortescue(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
