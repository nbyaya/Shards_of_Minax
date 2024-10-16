using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Peg-leg Pete")]
    public class PegLegPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PegLegPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Peg-leg Pete";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 55;
            Int = 20;
            Hits = 85;

            // Appearance
            AddItem(new TricorneHat() { Hue = 2123 });
            AddItem(new FancyShirt() { Hue = 38 });
            AddItem(new ShortPants() { Hue = 2124 });
            AddItem(new Boots() { Hue = 1170 });
            AddItem(new Longsword() { Name = "Pete's Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Arrr, I'm Peg-leg Pete, the one-legged pirate! What ye want, landlubber?");
            }
            else if (speech.Contains("health"))
            {
                Say("Me leg ain't what it used to be, but I've had worse! What about ye?");
            }
            else if (speech.Contains("job"))
            {
                Say("I'm a pirate, ain't it obvious? Plunderin' and sailin' the high seas, that be me job!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Life on the sea be tough, lad. It takes more than a sharp blade to survive. Be ye up to the challenge?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Har! A brave one, ye be! But remember, in the pirate's code, there be no fleein'. What say ye?");
            }
            else if (speech.Contains("plunder"))
            {
                Say("Ah, plunderin' be the life for me! Found many a treasure chest in me days, but one still eludes me. Ever heard o' the Lost Jewel o' the Sea?");
            }
            else if (speech.Contains("jewel"))
            {
                Say("Aye, the Lost Jewel o' the Sea be a fabled gem, said to be as big as a fist and shinin' brighter than the North Star. Many have sought it, few have lived to tell the tale. If ye ever find it, bring it to me, and I'll make it worth yer while.");
            }
            else if (speech.Contains("sailin"))
            {
                Say("Sailin' be in me blood. The wind in yer hair, the salt on yer lips, and the horizon stretchin' out as far as the eye can see. Have ye ever dreamed o' becomin' a sailor?");
            }
            else if (speech.Contains("sailor"))
            {
                Say("Har har! It ain't an easy life, but it's free. If ye ever want to learn the ropes, find Old Man Jenkins at the harbor. Tell 'im Peg-leg Pete sent ye, and he might teach ye a thing or two.");
            }
            else if (speech.Contains("blade"))
            {
                Say("A sharp blade be important, but it's the hand that wields it that counts. Do ye have a trusty blade by yer side?");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust be a rare commodity on the high seas. Me crew be loyal, but there's always a dagger waitin' in the shadows. Always be watchful, lad.");
            }
            else if (speech.Contains("crew"))
            {
                Say("Aye, me crew be the saltiest bunch of scallywags ye ever did see! But they're loyal to a fault. Once saved me life from a kraken's grasp, they did! Have ye ever seen such a beast?");
            }
            else if (speech.Contains("kraken"))
            {
                Say("The kraken be a monster from the deep, tentacles longer than masts, and a hunger for ships and men alike! If ye ever encounter one, be sure to have a bard with ye. Their songs can soothe the beast.");
            }
            else if (speech.Contains("bard"))
            {
                Say("Bards have saved many a ship with their tunes. I once had a bard named Lillian in me crew. Her voice could tame the wildest of storms. Ever met a bard in yer travels?");
            }
            else if (speech.Contains("lillian"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Lillian was a gem, not just for her voice, but her spirit. She left me crew years ago in search of her own destiny. If ye ever find her, give her this old locket from me. And for yer trouble, here's somethin' for ye.");
                    from.AddToBackpack(new FishingAugmentCrystal()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public PegLegPete(Serial serial) : base(serial) { }

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
