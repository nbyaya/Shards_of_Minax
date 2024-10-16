using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Brutus")]
    public class SirBrutus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirBrutus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Brutus";
            Body = 0x190; // Human male body

            // Stats
            SetStr(140);
            SetDex(60);
            SetInt(20);
            SetHits(100);

            // Appearance
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new PlateLegs() { Hue = 1175 });
            AddItem(new PlateHelm() { Hue = 1175 });
            AddItem(new PlateGloves() { Hue = 1175 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new WarAxe() { Name = "Sir Brutus’s Axe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

		public SirBrutus(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("I am Sir Brutus, the silent shadow in the night. How may I assist you?");

            greeting.AddOption("Tell me about your journey through the moongate.",
                player => true,
                player =>
                {
                    DialogueModule journeyModule = new DialogueModule("Ah, the moongate. A mysterious portal, it is. One moment, I was shrouded in darkness, and the next, I was pulled through a swirling vortex of light. It felt as if the very fabric of reality was unraveling around me.");
                    journeyModule.AddOption("What happened when you arrived?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule arrivalModule = new DialogueModule("I found myself in a strange wood, filled with sounds I had never heard before. The air was thick with the scent of damp earth and unknown flora. I was disoriented, a mere shadow of my former self, lost in a realm I did not understand.");
                            arrivalModule.AddOption("Did you encounter any creatures?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule creaturesModule = new DialogueModule("Indeed, I encountered strange beasts and mystical beings. Some were hostile, while others seemed merely curious about my presence. I remember a peculiar fae creature that flitted around, giggling as it watched me stumble through the underbrush.");
                                    creaturesModule.AddOption("What did the fae say?",
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule faeModule = new DialogueModule("It whispered riddles and warned me of dangers lurking in the shadows. It claimed that not all who wander here return to their own world. Its words were both amusing and terrifying.");
                                            faeModule.AddOption("Did you feel threatened?",
                                                pl4 => true,
                                                pl4 =>
                                                {
                                                    player.SendGump(new DialogueGump(player, new DialogueModule("At first, yes. The woods felt alive, and I was but a trespasser. However, as I spent more time there, I learned to respect its rhythms and secrets.")));
                                                });
                                            pl3.SendGump(new DialogueGump(pl3, faeModule));
                                        });
                                    arrivalModule.AddOption("What else did you see?",
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule sightsModule = new DialogueModule("The trees were enormous, their trunks gnarled and ancient. I stumbled upon a crystal-clear stream that sparkled like stars. It was enchanting and filled with vibrant life, but I could sense lurking dangers.");
                                            sightsModule.AddOption("What kind of dangers?",
                                                pl4 => true,
                                                pl4 =>
                                                {
                                                    player.SendGump(new DialogueGump(player, new DialogueModule("Creatures of shadow, predators unseen, awaited the unwary. I learned quickly to tread lightly and to listen closely to the whispers of the woods.")));
                                                });
                                            pl3.SendGump(new DialogueGump(pl3, sightsModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, creaturesModule));
                                });
                            journeyModule.AddOption("How did you survive?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule survivalModule = new DialogueModule("I relied on my instincts and the skills honed through years of experience. I hunted when I could and foraged for wild edibles. The woods taught me the balance of life and survival.");
                                    survivalModule.AddOption("Did you ever feel hopeless?",
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            player.SendGump(new DialogueGump(player, new DialogueModule("There were dark moments when despair threatened to consume me. But I remembered my training and the resolve that brought me here. Hope is a powerful ally.")));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, survivalModule));
                                });
                            player.SendGump(new DialogueGump(player, arrivalModule));
                        });
                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            greeting.AddOption("What about your thoughts on justice?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Justice is subjective. To some, it is vengeance. To others, it is retribution. In my world, it is about maintaining balance, whether in the shadows or the light.")));
                });

            greeting.AddOption("What is your view on honor?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Honor is a personal code. Light or shadow, it's our actions that define its true meaning. My journey has taught me that honor is often blurred by necessity.")));
                });

            greeting.AddOption("Do you believe in compassion?",
                player => true,
                player =>
                {
                    player.AddToBackpack(new FencingAugmentCrystal()); // Give the reward
                    player.SendGump(new DialogueGump(player, new DialogueModule("Even in my line of work, compassion is not entirely lost. It has saved lives, including my own. Here, take this as a token of my belief.")));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
