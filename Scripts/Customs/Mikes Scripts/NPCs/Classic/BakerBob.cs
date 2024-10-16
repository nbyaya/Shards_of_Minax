using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Baker Bob")]
    public class BakerBob : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BakerBob() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Baker Bob";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 45;
            Int = 60;
            Hits = 65;

            // Appearance
            AddItem(new ShortPants(1153));
            AddItem(new Doublet(443));
            AddItem(new ThighBoots(1904));
            AddItem(new LeatherGloves() { Name = "Bob's Baking Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("Ye dare disturb Baker Bob while he's toilin' over these infernal ovens?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Bah, health's overrated! I've got more burns than a dragon's treasure hoard!");
            }
            else if (speech.Contains("job"))
            {
                Say("Me job? Me slavin' away in this inferno to bake them ungrateful townies their daily bread!");
            }
            else if (speech.Contains("oven"))
            {
                Say("Think ye could handle the heat in this kitchen, eh?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! You talk big, but can ye handle the heat of me ovens?");
            }
            else if (speech.Contains("toiling"))
            {
                Say("Toilin'? Aye, from morn' till dusk. But there's nothin' quite like the smell of freshly baked bread to keep me goin'.");
            }
            else if (speech.Contains("burns"))
            {
                Say("These burns ain't just for show, they're badges of honor! Every time I slip up, I remember the importance of focus. But for every burn, there's a memory of a perfectly baked loaf or a satisfied customer.");
            }
            else if (speech.Contains("townies"))
            {
                Say("Those townies? They might complain when the crust is a tad too hard or when there's not enough raisins in their bun. But let me tell ya, they'd be lost without my bakes!");
            }
            else if (speech.Contains("heat"))
            {
                Say("It ain't just about withstandin' the heat, it's about mastering it! Do ya know about the ancient ovenstone I use to maintain the perfect temperature?");
            }
            else if (speech.Contains("smell"))
            {
                Say("Ah, the aroma of freshly baked bread! It's a scent that can lure even the hungriest of adventurers from the furthest lands. Speaking of which, ever tried my special cinnamon loaf?");
            }
            else if (speech.Contains("satisfied"))
            {
                Say("Ah, there's nothing better than seeing a satisfied face after handin' them their favorite pastry. Ya know, I once made a special pie for the town's mayor. And he loved it!");
            }
            else if (speech.Contains("raisins"))
            {
                Say("Raisins, the tiny gems of sweetness. I source them from a special vineyard up north. If you ever find yourself there, tell them Baker Bob sent ya, and you might just get a sweet reward!");
            }
            else if (speech.Contains("ovenstone"))
            {
                Say("The ovenstone is an ancient relic passed down through generations. It's said to have magical properties, ensuring every bake is perfect. Want to see it?");
                from.AddToBackpack(new CookingAugmentCrystal()); // Give the reward
            }
            else if (speech.Contains("cinnamon"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("My special cinnamon loaf is a secret recipe, passed down in my family. For someone as curious as you, how about I give you a taste as a reward?");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("mayor"))
            {
                Say("Ah, the mayor. A tough one to please, but he has a soft spot for my blueberry pies. Made specially with berries from the enchanted forest!");
            }

            base.OnSpeech(e);
        }

        public BakerBob(Serial serial) : base(serial) { }

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
