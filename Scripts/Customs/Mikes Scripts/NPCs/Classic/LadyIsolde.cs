using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Isolde")]
    public class LadyIsolde : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyIsolde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Isolde";
            Body = 0x191; // Human female body

            // Stats
            Str = 155;
            Dex = 65;
            Int = 25;
            Hits = 110;

            // Appearance
            AddItem(new PlateChest() { Hue = 1122 });
            AddItem(new PlateArms() { Hue = 1122 });
            AddItem(new PlateHelm() { Hue = 1122 });
            AddItem(new PlateGloves() { Hue = 1122 });
            AddItem(new ThighBoots() { Hue = 1157 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

		public LadyIsolde(Serial serial) : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Lady Isolde, a knight of this wretched realm. How may I assist you?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("Health? Bah! What does it matter? We're all doomed in the end anyway."))));

            greeting.AddOption("What is your job?",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("My so-called 'job' is to uphold the honor of this crumbling kingdom. A futile task if there ever was one."))));

            greeting.AddOption("What do you think of valor?",
                player => true,
                player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor? Hah! Valor is a fairy tale told to keep us fools fighting in this eternal war. Are you valiant, or just another pawn?");
                valorModule.AddOption("I am valiant.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Valiant, you say? Well, if you believe in such nonsense, then never flee unless you're staring death in the face."))));
                valorModule.AddOption("I am just a pawn.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Many have become pawns in this grand game of power and deceit. I fight to reclaim my agency, my freedom."))));
                player.SendGump(new DialogueGump(player, valorModule));
            });

            greeting.AddOption("Tell me about this realm.",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("This realm was once a beacon of hope, now overshadowed by darkness and despair."))));

            greeting.AddOption("Why do you feel doomed?",
                player => true,
                player =>
            {
                DialogueModule doomModule = new DialogueModule("Why are we doomed? Legends speak of a cursed artifact that brought misfortune upon us. I seek it, hoping to reverse our fate.");
                doomModule.AddOption("How can I help?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule helpModule = new DialogueModule("If you assist me in finding this cursed artifact, I might just have a reward for you. Bring me the map of the lost catacombs.");
                        helpModule.AddOption("Where can I find this map?",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Legend has it that the map is held by the Oracle of the East. Seek her guidance."))));
                        player.SendGump(new DialogueGump(player, helpModule));
                    });
                player.SendGump(new DialogueGump(player, doomModule));
            });

            // Adding more depth to the dialogue tree
            greeting.AddOption("What do you know of the artifact?",
                player => true,
                player =>
            {
                DialogueModule artifactModule = new DialogueModule("The artifact is said to be a dark relic, imbued with a power that corrupts all it touches. It can either doom us further or save us, depending on who wields it.");
                artifactModule.AddOption("What kind of power does it hold?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Its power is a double-edged sword. Some say it grants unparalleled strength, while others believe it drives the bearer mad."))));
                artifactModule.AddOption("Have you ever seen it?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("No, but I have heard tales of it being guarded by a fearsome beast deep within the Shadowfang Caverns."))));
                player.SendGump(new DialogueGump(player, artifactModule));
            });

            greeting.AddOption("What is your view on the current war?",
                player => true,
                player =>
            {
                DialogueModule warModule = new DialogueModule("The war has raged for as long as I can remember. It's consumed the best of us, turning friends into foes. Yet, I believe there is a glimmer of hope.");
                warModule.AddOption("What hope do you speak of?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hopeModule = new DialogueModule("There are whispers of a hidden alliance forming among the realms, seeking peace. But such alliances are fragile.");
                        hopeModule.AddOption("How can I help foster this alliance?",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("You must earn the trust of key figures. Speak to them, prove your worth, and perhaps they will listen."))));
                        player.SendGump(new DialogueGump(player, hopeModule));
                    });
                player.SendGump(new DialogueGump(player, warModule));
            });

            greeting.AddOption("Tell me about your past.",
                player => true,
                player =>
            {
                DialogueModule pastModule = new DialogueModule("My past is filled with battles and loss. I once led a charge against an invading force, but we were outnumbered and fell.");
                pastModule.AddOption("Do you regret it?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Regret? Yes, in some ways. But I would not change my choices. I fought for honor and for those I loved."))));
                pastModule.AddOption("What did you learn from that experience?",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("I learned that honor can often be a heavy burden. One must weigh the cost of their convictions."))));
                player.SendGump(new DialogueGump(player, pastModule));
            });

            greeting.AddOption("What do you know about your kingdom's history?",
                player => true,
                player =>
            {
                DialogueModule historyModule = new DialogueModule("This kingdom was once glorious, filled with life and laughter. Now it's but a shadow, a haunting reminder of the past.");
                historyModule.AddOption("What events led to its downfall?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule eventsModule = new DialogueModule("Greed and betrayal. Once united under a just ruler, the nobles turned against each other, igniting a civil war that consumed us.");
                        eventsModule.AddOption("Is there any hope for redemption?",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, new DialogueModule("Only if we learn from our past mistakes. Unity is our strength, and we must fight to reclaim it."))));
                        player.SendGump(new DialogueGump(player, eventsModule));
                    });
                player.SendGump(new DialogueGump(player, historyModule));
            });

            return greeting;
        }

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
