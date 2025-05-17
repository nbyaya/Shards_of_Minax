using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Captain Waylon")]
    public class CaptainWaylon : BaseCreature
    {
        [Constructable]
		public CaptainWaylon() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Waylon";
            Body = 0x190; // Human male body

            // Stats
            SetStr(110);
            SetDex(90);
            SetInt(95);
            SetHits(160);

            // Appearance and Equipment
            AddItem(new FancyShirt() { Hue = 0x47E });   // An ocean-blue, finely embroidered shirt
            AddItem(new Bandana() { Hue = 0x47E });      // A worn bandana, a memento of his rebellious days
            AddItem(new LongPants() { Hue = 0x47E });
            AddItem(new ThighBoots() { Name = "Boots of Travel" });                // Sturdy boots well-suited for unpredictable tides
            AddItem(new Cutlass() { Name = "Old Cutlass of the Sea" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Utility.RandomHairHue();
        }

        public CaptainWaylon(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Ahoy, traveler! I am Captain Waylon—once a fearsome marauder, a gambler of fate, and now the keeper of relics and secrets. My life’s journey has been as wild and unpredictable as the sea itself. What tale do you wish to hear?");
            
            // Option 1: Secret Relic
            greeting.AddOption("Tell me about the secret relic you guard.",
                player => true,
                player =>
                {
                    DialogueModule relicModule = new DialogueModule("Arrr, the relic I guard is steeped in mystery. It is said to be blessed by the very spirits of the deep, a charm of both fortune and doom. I came upon it during a ferocious squall near Devil Guard—when the heavens roared and fate itself intervened.");
                    
                    relicModule.AddOption("How did you first come by the relic?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule relicOrigin = new DialogueModule("It was a night of chaos—cannon fire lit up the black sky and enemy ships encircled us. Amid the turmoil, a dying pirate captain pressed it into my hands. I've never known if it was destiny or pure luck that kept me alive that night. My very soul is bound to this relic.");
                            
                            relicOrigin.AddOption("It sounds both fated and perilous.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            relicOrigin.AddOption("Does this relic grant you any luck?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule relicLuck = new DialogueModule("Aye, many a time I've credited this relic for my lucky escapes. But beware—fortune has a fickle nature. To rely solely on luck is to dance with danger. I've seen storms calm and curses lifted, yet I've also witnessed misfortune strike even the boldest souls.");
                                    
                                    relicLuck.AddOption("Fascinating. I sense a dangerous allure in that power.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, relicLuck));
                                });
                            pl.SendGump(new DialogueGump(pl, relicOrigin));
                        });

                    relicModule.AddOption("What power does the relic hold?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule relicPower = new DialogueModule("Legends claim it can calm the wildest storms, reveal secret coves, and even open portals to realms unknown. Yet each boon comes with a price, a reminder that the line between fortune and folly is ever so thin.");
                            
                            relicPower.AddOption("A heavy price indeed, but one that sings of adventure.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            relicPower.AddOption("Tell me, does it ever betray you?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule relicBetrayal = new DialogueModule("Oh, on a few cursed nights it has. When I pushed my luck too far, the relic's glow would dim—a sign that my time to gamble with fate was nearing its end. Yet, miraculously, I always emerged, though scarred by the close call.");
                                    relicBetrayal.AddOption("Your survival defies all reason!",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, relicBetrayal));
                                });
                            pl.SendGump(new DialogueGump(pl, relicPower));
                        });
                    player.SendGump(new DialogueGump(player, relicModule));
                });

            // Option 2: Maritime Adventures
            greeting.AddOption("Share your maritime adventures with me.",
                player => true,
                player =>
                {
                    DialogueModule adventureModule = new DialogueModule("The vast sea has been both my playground and my battleground. I've weathered storms that threatened to tear my ship apart, navigated through spectral mists, and survived encounters that would chill the heart of any man.");
                    
                    adventureModule.AddOption("Tell me about a truly reckless adventure.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule recklessModule = new DialogueModule("Ah, there was one harrowing night aboard The Tempest's Fury. A rogue tempest battered us, and in the midst of chaos, I leaped from a shattered mast—barely clinging to a splintered beam. With naught but a grin and a prayer, I found myself on a nearby rock, bewildered yet alive. My recklessness has often been my saving grace.");
                            
                            recklessModule.AddOption("That is sheer lunacy—and miraculous luck!",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            recklessModule.AddOption("How did you maintain such a cheerful spirit?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule cheerfulModule = new DialogueModule("The sea teaches you laughter in the face of death. I believe that a hearty chuckle in dangerous times can charm fate itself. Every narrow escape reminded me that life is far too grand to be taken too seriously.");
                                    cheerfulModule.AddOption("Your joy in the face of danger is inspiring!",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, cheerfulModule));
                                });
                            pl.SendGump(new DialogueGump(pl, recklessModule));
                        });

                    adventureModule.AddOption("How have your friends shaped your journeys?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule friendsModule = new DialogueModule("Aye, Jasper the Scribe chronicled each twist of fate with his quill, while Edda from East Montor lent her prophetic insights when the waters ran dark. Their unwavering support and counsel have been as invaluable as any navigational chart.");
                            
                            friendsModule.AddOption("It sounds as though their wisdom is your true compass.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            friendsModule.AddOption("Do they know of your reckless gambles?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule gambleTalk = new DialogueModule("Ha! They know me as a man who flirts with destiny at every turn. Jasper sometimes raises an eyebrow at my wild bets, and Edda merely smiles, as if she’s seen it all before in the tapestry of fate.");
                                    gambleTalk.AddOption("A fine balance indeed between recklessness and fate.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, gambleTalk));
                                });
                            pl.SendGump(new DialogueGump(pl, friendsModule));
                        });
                    player.SendGump(new DialogueGump(player, adventureModule));
                });

            // Option 3: Mentoring Young Miko
            greeting.AddOption("How did you come to mentor young Miko?",
                player => true,
                player =>
                {
                    DialogueModule mentorModule = new DialogueModule("Young Miko arrived at the docks clutching an old, tattered chart and a boundless hunger for adventure. I saw in him the spark of daring—and perhaps a touch of the same reckless luck that has been my companion through countless trials.");
                    
                    mentorModule.AddOption("What lessons do you share with him?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule lessonsModule = new DialogueModule("I teach him to read the subtle signs of the horizon, to trust his gut when the waves whisper secrets, and never to fear taking a chance—even when the odds seem impossibly stacked against him. Life, after all, is the grandest gamble of all.");
                            lessonsModule.AddOption("A bold philosophy—one that has served you well.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, lessonsModule));
                        });
                    
                    mentorModule.AddOption("Where can I catch a glimpse of Miko these days?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule mikoModule = new DialogueModule("You'll likely find him poring over ancient maps near the docks of Grey & Pirate Isle or sharing a hearty laugh in the tavern's shadow. His journey is just beginning, and every port holds a new secret waiting to be unraveled.");
                            mikoModule.AddOption("I may seek him out someday.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, mikoModule));
                        });
                    player.SendGump(new DialogueGump(player, mentorModule));
                });

            // Option 4: Life as a Pirate
            greeting.AddOption("Tell me more about your life as a pirate.",
                player => true,
                player =>
                {
                    DialogueModule pirateLifeModule = new DialogueModule("Oh, the pirate’s life! I once sailed aboard The Tempest's Fury—a ship that danced with the winds and defied the fury of storms. Every day was a new wager with death, yet I wore my scars like badges of honor.");
                    
                    pirateLifeModule.AddOption("What drove you away from that life?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule leavingModule = new DialogueModule("After countless nights of wild chases and near-misses with a watery grave, I realized that survival meant embracing a deeper purpose. I chose to preserve the lore of those tempestuous days, passing on my secrets to those brave enough to follow in my wake.");
                            leavingModule.AddOption("A change of course forged by fate and wisdom.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, leavingModule));
                        });
                    
                    pirateLifeModule.AddOption("Tell me about The Tempest's Fury.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule shipModule = new DialogueModule("The Tempest's Fury was our home—a vessel that harnessed the raw energy of the sea. With sails that caught the very breath of storms and a hull that bore witness to countless duels with fate, she was as much a character in our saga as any man.");
                            shipModule.AddOption("Her legend is as enduring as the sea itself.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, shipModule));
                        });
                    player.SendGump(new DialogueGump(player, pirateLifeModule));
                });

            // Option 5: Gambling Days & Reckless Luck
            greeting.AddOption("What about your gambling days? I've heard you push your luck like none other.",
                player => true,
                player =>
                {
                    DialogueModule gambleModule = new DialogueModule("Ah, now ye've struck upon a hidden truth of me past! Before I became the keeper of relics, I was a gambler—a man who wagered his very life on the turn of a card, the roll of a die, and the fickle favor of fortune. My heart was always light, my spirit ever cheerful, even as I danced on the edge of disaster.");
                    
                    gambleModule.AddOption("Tell me about your luckiest wager.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule luckiestWager = new DialogueModule("There was a fabled night in a cursed gambling hall deep in Renika. I was cornered by a notorious loan shark who declared that fate had marked me for oblivion. With all odds against me, I bet my last doubloon on a seemingly impossible hand—lo and behold, the cards revealed a royal flush! I escaped with not only my life but a treasure that changed my destiny.");
                            
                            luckiestWager.AddOption("Unbelievable! Did that wild risk ever change your nature?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule natureModule = new DialogueModule("It taught me that fortune favors the bold. I learned to greet danger with a grin and to never let the specter of fear steal the joy of life's gamble. I owe much of my luck to that relentless spirit and, perhaps, to a touch of the supernatural.");
                                    natureModule.AddOption("Your fearless cheer is remarkable.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, natureModule));
                                });
                            luckiestWager.AddOption("I can see how recklessness and luck walk hand in hand.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, luckiestWager));
                        });
                    
                    gambleModule.AddOption("What is the most reckless bet you ever made?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule recklessBet = new DialogueModule("There was a time when I wagered my honor on the outcome of a duel against a ghostly opponent in the heart of a storm. The winds howled, the seas raged, and yet—by some miracle—I emerged unscathed. My wild, reckless gamble defied every law of nature, and I swear that even the fates chuckled at my insolence.");
                            
                            recklessBet.AddOption("That is both terrifying and awe-inspiring.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            recklessBet.AddOption("How did you manage to survive such odds?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule survivalModule = new DialogueModule("I reckon it was a mix of sheer recklessness, a buoyant heart, and a streak of inexplicable luck. Perhaps the gods themselves tossed a coin in my favor that night, ensuring that my adventures would continue.");
                                    survivalModule.AddOption("Your survival is nothing short of miraculous!",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, survivalModule));
                                });
                            pl.SendGump(new DialogueGump(pl, recklessBet));
                        });
                    
                    gambleModule.AddOption("Do you ever miss the thrill of the gamble?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule missGamble = new DialogueModule("Oh, the thrill of betting it all—be it coin or courage—still quickens me pulse! There are nights when I reminisce about the clatter of dice and the roar of a jubilant crowd, and though I now tread a steadier course, that spark of wild abandon remains a cherished ember in my soul.");
                            missGamble.AddOption("That ember seems to light your eyes even now.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, missGamble));
                        });
                    player.SendGump(new DialogueGump(player, gambleModule));
                });

            // Option 6: Farewell
            greeting.AddOption("Farewell, Captain.",
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule("May the winds be ever in your favor, and may fortune smile upon your every step. Until we meet again, traveler.");
                    player.SendGump(new DialogueGump(player, farewellModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
