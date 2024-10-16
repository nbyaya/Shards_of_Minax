using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Revolvus Marx")]
    public class RevolvusMarx : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RevolvusMarx() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Revolvus Marx";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Tunic() { Hue = Utility.RandomMetalHue() });
            AddItem(new Kilt() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomMetalHue() });

            // Hair
            HairItemID = Utility.RandomList(0x203B, 0x2043); // Hair styles
            HairHue = Utility.RandomHairHue();

            // Speech Hue
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
                Say("Greetings, I am Revolvus Marx, a voice for the people’s cause.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as vigorous as the fires of revolution!");
            }
            else if (speech.Contains("job"))
            {
                Say("I strive to stir the hearts and minds towards a new dawn of equality and justice.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("Revolution is not merely a change, but a fundamental shift towards a just society.");
            }
            else if (speech.Contains("equality"))
            {
                Say("Equality is the cornerstone of true freedom. Without it, we remain in chains.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice for all is a noble ideal, but one we must work towards tirelessly.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, the Workers’ Revolution Chest. It symbolizes our struggle and hope. Solve the riddle of the revolution, and you shall earn it.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("The true revolution lies within understanding and courage. Show me your resolve, and I shall reward you.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("To truly resolve is to commit to change. It requires unwavering dedication.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication fuels the revolution. It drives us towards our ideals and dreams.");
            }
            else if (speech.Contains("ideals"))
            {
                Say("Our ideals are our guiding stars. They inspire us to fight for a better world.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the visions of a brighter future. They keep our spirits high in the face of adversity.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our will. Overcoming it is a mark of true strength and conviction.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical, but also moral. It is the fortitude to stand by our beliefs.");
            }
            else if (speech.Contains("beliefs"))
            {
                Say("Our beliefs shape our actions. They are the foundation upon which we build our revolution.");
            }
            else if (speech.Contains("foundation"))
            {
                Say("The foundation of any great movement is its principles. They provide structure and purpose.");
            }
            else if (speech.Contains("principles"))
            {
                Say("Principles guide us in our quest for change. They help us stay true to our cause.");
            }
            else if (speech.Contains("cause"))
            {
                Say("Our cause is the heart of our revolution. It represents the changes we seek to achieve.");
            }
            else if (speech.Contains("changes"))
            {
                Say("Change is the essence of revolution. It is through change that we progress and grow.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is the forward movement towards our goals. It is the path we walk to achieve our dreams.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the visions of a brighter future. They keep our spirits high in the face of adversity.");
            }
            else if (speech.Contains("future"))
            {
                Say("The future is where we place our hopes. It is the destination of our revolutionary journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Our journey is marked by trials and triumphs. It is the path we take to reach our goals.");
            }
            else if (speech.Contains("goals"))
            {
                Say("Goals give us direction. They are the targets we aim for as we push forward in our quest for change.");
            }
            else if (speech.Contains("change"))
            {
                Say("Change is the essence of revolution. It is through change that we progress and grow.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is the forward movement towards our goals. It is the path we walk to achieve our dreams.");
            }
            else if (speech.Contains("dreams"))
            {
                Say("Dreams are the visions of a brighter future. They keep our spirits high in the face of adversity.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our will. Overcoming it is a mark of true strength and conviction.");
            }
            else if (speech.Contains("strength"))
            {
                Say("Strength is not just physical, but also moral. It is the fortitude to stand by our beliefs.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience is also a virtue. Return later for your reward.");
                }
                else
                {
                    Say("Your resolve is commendable. Accept this Workers’ Revolution Chest as a symbol of your dedication.");
                    from.AddToBackpack(new WorkersRevolutionChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RevolvusMarx(Serial serial) : base(serial) { }

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
