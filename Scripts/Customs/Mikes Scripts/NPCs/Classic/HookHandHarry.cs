using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hook Hand Harry")]
    public class HookHandHarry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HookHandHarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hook Hand Harry";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 40;
            Hits = 80;

            // Appearance
            AddItem(new LongPants() { Hue = 1904 }); // Long pants with hue 1904
            AddItem(new FancyShirt() { Hue = 64 }); // Fancy shirt with hue 64
            AddItem(new Boots() { Hue = 64 }); // Boots with hue 64
            AddItem(new TricorneHat() { Hue = 1904 }); // Tricorne hat with hue 1904
            AddItem(new Cutlass() { Name = "Harry's Hook" }); // Cutlass with a name
            AddItem(new GoldRing() { Name = "Harry's Eye Patch" }); // Eye patch with a name

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
                Say("Ahoy, matey! I be Hook Hand Harry, the fiercest pirate to sail these treacherous seas!");
            }
            else if (speech.Contains("health"))
            {
                Say("Me health be as sturdy as a ship's hull in a storm!");
            }
            else if (speech.Contains("job"))
            {
                Say("I be a pirate through and through, a swashbuckler of the high seas!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Ye see, true valor be in outwittin' yer foes and takin' risks when the booty be temptin'! Be ye valiant, matey?");
            }
            else if (speech.Contains("yes") && !lastRewardTime.Equals(DateTime.MinValue) && DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
            {
                Say("Aye, that be the spirit! Ne'er turn yer back on a chance for adventure!");
            }
            else if (speech.Contains("harry"))
            {
                Say("Aye, most call me Hook Hand Harry on account of me missin' hand. Lost it to a monstrous shark I did, but that be another tale.");
            }
            else if (speech.Contains("sturdy"))
            {
                Say("Aye, bein' sturdy ain't just about the body, mate. It's about the spirit, too. Keeps a pirate goin' when the winds be against 'em.");
            }
            else if (speech.Contains("swashbuckler"))
            {
                Say("As a swashbuckler, I've crossed blades with many, and raided countless treasures. But there's one treasure I've yet to find...");
            }
            else if (speech.Contains("shark"))
            {
                Say("That beast was the biggest I've ever seen, with teeth sharp as daggers. I may have lost me hand, but I won the battle. Got this hook as a reminder of that fateful day.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of a pirate be his true compass. It guides ye to uncharted waters, to places where maps have no names. And if ye prove yer spirit to me, I might just reward ye.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Legends speak of the 'Heart of the Sea', a gem so radiant it's said to outshine the moon. I've been searchin' for it me whole life.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, so ye wish to earn me reward? Very well. Bring me a rare golden doubloon from the sunken ship 'Siren's Lament', and I shall reward ye handsomely. A taste, here.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example item; replace with the actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public HookHandHarry(Serial serial) : base(serial) { }

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
