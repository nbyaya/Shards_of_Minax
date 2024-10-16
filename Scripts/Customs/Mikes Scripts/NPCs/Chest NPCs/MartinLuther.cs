using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Martin Luther")]
    public class MartinLuther : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MartinLuther() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Martin Luther";
            Title = "the Historian";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            
            // Equip the NPC
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new SkullCap(Utility.RandomBlueHue()));
            
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
                Say("Greetings, I am Martin Luther, a historian of great tales.");
            }
            else if (speech.Contains("job"))
            {
                Say("My work is to recount the history of the civil rights movements and share the stories of freedom and struggle.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you for asking.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the essence of our history. It is a journey of courage and perseverance.");
            }
            else if (speech.Contains("struggle"))
            {
                Say("The struggle for equality has shaped our world. It is a battle worth every effort.");
            }
            else if (speech.Contains("civil rights"))
            {
                Say("The civil rights movement is a powerful chapter in our history. The story of those who fought for equality deserves to be remembered.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must wait a while before I can offer another reward.");
                }
                else
                {
                    Say("You have shown great interest in our history. As a token of appreciation, I present to you the Civil Rights Strongbox.");
                    from.AddToBackpack(new CivilRightsStrongbox()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I am here to share the stories of our past. Ask me about our history, and I shall tell you.");
            }

            base.OnSpeech(e);
        }

        public MartinLuther(Serial serial) : base(serial) { }

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
