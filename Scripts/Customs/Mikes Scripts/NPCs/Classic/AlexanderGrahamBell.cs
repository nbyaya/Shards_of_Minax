using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alexander Graham Bell")]
    public class AlexanderGrahamBell : BaseCreature
    {
        [Constructable]
        public AlexanderGrahamBell() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alexander Graham Bell";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new LongPants(1153));
            AddItem(new FancyShirt(1153));
            AddItem(new Boots(1153));
            AddItem(new Dagger { Name = "Alexander's Invention" });
			
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

            if (Insensitive.Contains(e.Speech, "name"))
            {
                Say("I'm Alexander Graham Bell, the genius from Canada. What do you want?");
            }
            else if (Insensitive.Contains(e.Speech, "health"))
            {
                Say("As if I care about your health. I'm not your physician.");
            }
            else if (Insensitive.Contains(e.Speech, "job"))
            {
                Say("Job? Do I look like I have a job? My job is listening to idiots like you.");
            }
            else if (Insensitive.Contains(e.Speech, "invention"))
            {
                Say("Do you even know the first thing about communication? Tell me, what's the most important invention in history?");
            }
            else if (Insensitive.Contains(e.Speech, "telephone"))
            {
                Say("Hah! You're even more ignorant than I thought. It's the telephone, you imbecile! Alexander Graham Bell's greatest creation.");
            }

            base.OnSpeech(e);
        }

        public AlexanderGrahamBell(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
