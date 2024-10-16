using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Melody the Harpist")]
    public class MelodyTheHarpist : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MelodyTheHarpist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Melody the Harpist";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 80;
            Int = 80;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Name = "Melody's Shroud", Hue = 1156 });
            AddItem(new Spellbook() { Name = "Melody's Songs" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            SpeechHue = 0;

            lastRewardTime = DateTime.MinValue;
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
            DialogueModule greeting = new DialogueModule("I am Melody the Harpist, a bard by trade. What would you like to discuss?");

            greeting.AddOption("Tell me about your music.",
                player => true,
                player =>
                {
                    DialogueModule musicModule = new DialogueModule("Music can lift even the heaviest of hearts. Would you like to hear a melody or learn about my favorite instruments?");
                    musicModule.AddOption("Yes, please play a tune.",
                        pl => true,
                        pl => 
                        {
                            pl.SendMessage("Melody plays a beautiful tune that resonates in your heart.");
                        });
                    musicModule.AddOption("What are your favorite instruments?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule instrumentsModule = new DialogueModule("I adore the harp, of course! But I also have a soft spot for the lute and the flute. Each instrument has its unique voice.");
                            instrumentsModule.AddOption("What makes the harp special?",
                                pla => true,
                                pla => 
                                {
                                    pla.SendMessage("The harp's strings can evoke deep emotions. Its melodies are soothing, like a gentle breeze on a summer's day.");
                                });
                            instrumentsModule.AddOption("What about the lute?",
                                pla => true,
                                pla => 
                                {
                                    pla.SendMessage("The lute has a rich, warm tone, perfect for storytelling and lively dances.");
                                });
                            instrumentsModule.AddOption("Tell me about the flute.",
                                pla => true,
                                pla => 
                                {
                                    pla.SendMessage("The flute is light and airy, its notes dancing like a butterfly. It brings a joyful spirit to any gathering.");
                                });
                            pl.SendGump(new DialogueGump(pl, instrumentsModule));
                        });
                    musicModule.AddOption("No, thank you.",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, musicModule));
                });

            greeting.AddOption("What do you think of heroes?",
                player => true,
                player =>
                {
                    DialogueModule heroesModule = new DialogueModule("The heroes of our land have faced great evils and brought peace to many. Their tales inspire me to write songs of valor.");
                    heroesModule.AddOption("Can you tell me a hero's story?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule storyModule = new DialogueModule("Ah, there are so many! Let me tell you about Sir Cedric, a knight who fought bravely against a dragon.");
                            storyModule.AddOption("What happened in the battle?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Sir Cedric faced the dragon with courage. With a swift strike of his sword, he wounded it and sent it fleeing.");
                                });
                            storyModule.AddOption("Did he win the war?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Yes, his bravery rallied the troops, and together they drove the dragon from the realm, securing peace for many years.");
                                });
                            pl.SendGump(new DialogueGump(pl, storyModule));
                        });
                    heroesModule.AddOption("That's inspiring!",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, heroesModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
                player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                player =>
                {
                    DialogueModule rewardModule = new DialogueModule("Music has a way of connecting souls. For your appreciation, I'd like to give you a small reward.");
                    rewardModule.AddOption("Thank you!",
                        pl => true,
                        pl =>
                        {
                            pl.AddToBackpack(new BanishingOrb());
                            lastRewardTime = DateTime.UtcNow;
                            pl.SendMessage("You receive a Banishing Orb as a reward!");
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                });

            greeting.AddOption("Tell me about your adventures.",
                player => true,
                player =>
                {
                    DialogueModule adventuresModule = new DialogueModule("I've traveled to distant lands and faced many challenges. My adventures are as varied as the tunes I play.");
                    adventuresModule.AddOption("What was your most dangerous adventure?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangerModule = new DialogueModule("Once, I traversed the Cursed Woods, where shadows whisper and danger lurks behind every tree.");
                            dangerModule.AddOption("What did you encounter there?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("I met a wraith that tried to steal my songs. With my music, I calmed its spirit and was allowed to pass.");
                                });
                            dangerModule.AddOption("Were you afraid?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Fear is natural, but music gives me strength. It guided me through the darkness.");
                                });
                            pl.SendGump(new DialogueGump(pl, dangerModule));
                        });
                    adventuresModule.AddOption("What other places have you visited?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule placesModule = new DialogueModule("I've been to the bustling markets of Rivenport and the serene valleys of Eldoria, each filled with stories.");
                            placesModule.AddOption("Tell me about Rivenport.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Rivenport is alive with colors and sounds. Merchants sell exotic wares, and the air is filled with laughter.");
                                });
                            placesModule.AddOption("And Eldoria?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Eldoria is peaceful, with rolling hills and streams. The villagers often gather for festivals, celebrating life through music.");
                                });
                            pl.SendGump(new DialogueGump(pl, placesModule));
                        });
                    player.SendGump(new DialogueGump(player, adventuresModule));
                });

            greeting.AddOption("What do you enjoy the most about being a bard?",
                player => true,
                player =>
                {
                    DialogueModule joyModule = new DialogueModule("Ah, it's the connection with others! Music creates bonds and evokes memories, both sweet and bitter.");
                    joyModule.AddOption("Do you have a favorite memory?",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("One of my fondest memories is playing at a wedding, where the joy of love filled the air, making every note magical.");
                        });
                    joyModule.AddOption("What about sad memories?",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("There have been many farewells. Each note played at a departure carries a weight, but it's the hope that lingers that matters.");
                        });
                    player.SendGump(new DialogueGump(player, joyModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Melody smiles warmly at you, her eyes twinkling with stories yet to be told.");
                });

            return greeting;
        }

        public MelodyTheHarpist(Serial serial) : base(serial) { }

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
