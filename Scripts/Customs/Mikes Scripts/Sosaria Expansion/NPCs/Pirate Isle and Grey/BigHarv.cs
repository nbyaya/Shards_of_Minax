using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Big Harv")]
    public class BigHarv : BaseCreature
    {
        [Constructable]
        public BigHarv() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Big Harv";
            Body = 0x190; // Using a human male body for Big Harv

            // Set Stats
            SetStr(150);
            SetDex(70);
            SetInt(90);
            SetHits(120);

            // Appearance and Items: Big Harv wears a rugged leather apron with a well-worn smith’s hammer at his side.
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new ShortPants() { Hue = 1010 });
            AddItem(new Boots() { Hue = 1010 });
            AddItem(new SmithHammer() { Name = "Harv's Legendary Hammer", Hue = 1175 });
            AddItem(new HalfApron() { Hue = 1020 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public BigHarv(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Ahoy there, traveler! I’m Big Harv – master smith, keeper of shipbuilding secrets, and a man with a past as fierce as the crashing waves. I've forged mighty tools for Galven, exchanged shipwright lore with Marin, and helped Captain Waylon restore ancient maritime relics. But there’s another side to me—one of battle, honor, and a secret legacy as a leader of a once-mighty warrior clan. What would you like to chat about?");

            greeting.AddOption("Tell me about the tools you’ve forged for Galven.", 
                player => true, 
                player =>
                {
                    DialogueModule forgingModule = CreateForgingModule();
                    player.SendGump(new DialogueGump(player, forgingModule));
                });

            greeting.AddOption("I heard you share shipbuilding secrets with Marin. What’s the story there?",
                player => true,
                player =>
                {
                    DialogueModule shipModule = CreateShipbuildingModule();
                    player.SendGump(new DialogueGump(player, shipModule));
                });

            greeting.AddOption("What can you tell me about your work with Captain Waylon and those maritime relics?",
                player => true,
                player =>
                {
                    DialogueModule relicModule = CreateRelicsModule();
                    player.SendGump(new DialogueGump(player, relicModule));
                });

            greeting.AddOption("Tell me about your secret warrior past.", 
                player => true,
                player =>
                {
                    DialogueModule warriorModule = CreateWarriorModule();
                    player.SendGump(new DialogueGump(player, warriorModule));
                });

            greeting.AddOption("I’d like to know more about you – your personal journey as a craftsman.",
                player => true,
                player =>
                {
                    DialogueModule personalModule = CreatePersonalModule();
                    player.SendGump(new DialogueGump(player, personalModule));
                });

            greeting.AddOption("I’m all set, thanks.", 
                player => true,
                player => {
                    player.SendMessage("Safe travels, friend.");
                });

            return greeting;
        }

        private DialogueModule CreateForgingModule()
        {
            DialogueModule forging = new DialogueModule("Aye, forging tools for Galven is more than mere labor—it’s an art that speaks of tradition and magic. I recall the day I completed a set of enchanted chisels and hammers, each imbued with a spark of my own passion. Every tool is a testament not only to my skill, but to the legacy of my craft.");
            
            forging.AddOption("What makes your forged tools so unique?",
                player => true,
                player =>
                {
                    DialogueModule detailsModule = new DialogueModule("The secret lies in the rare metals and enchanted ores I gather from deep within the mountains. I blend ancient smithing techniques with arcane fire—a careful dance between raw strength and subtle magic. Every strike of my hammer sings of the old ways.");
                    detailsModule.AddOption("Tell me more about these enchanted ores.",
                        p => true,
                        p =>
                        {
                            DialogueModule oreModule = new DialogueModule("These ores aren’t ordinary—they carry the inner light of elemental fire and water. Tempered with ancient rituals, they glow like embers under the night sky. Each piece of metal feels alive in my hands.");
                            p.SendGump(new DialogueGump(p, oreModule));
                        });
                    detailsModule.AddOption("How do you perfect the balance of strength and magic?",
                        p => true,
                        p =>
                        {
                            DialogueModule balanceModule = new DialogueModule("Through years of relentless study and practice, I honed my craft under both the tutelage of master smiths and the whispers of the elemental spirits. Every tool bears the mark of countless trials—a blend of meticulous technique and fierce determination.");
                            p.SendGump(new DialogueGump(p, balanceModule));
                        });
                    player.SendGump(new DialogueGump(player, detailsModule));
                });

            forging.AddOption("Is there a legendary tool you once forged for Galven?",
                player => true,
                player =>
                {
                    DialogueModule legendModule = new DialogueModule("Ah, the 'Anchorbreaker' stands as my finest work. A hammer with a spirit as fierce as a tempest—it could shatter the hull of a ghostly ship with a single blow. Galven swears its enchantment guided him through the darkest storms.");
                    legendModule.AddOption("How did you craft 'Anchorbreaker'?",
                        p => true,
                        p =>
                        {
                            DialogueModule creationModule = new DialogueModule("I labored for a full lunar cycle, gathering meteorite iron and mixing it with enchanted brine from the deep. Every strike was a prayer for strength and endurance—a blend of art and valor.");
                            p.SendGump(new DialogueGump(p, creationModule));
                        });
                    legendModule.AddOption("Has 'Anchorbreaker' ever faltered in battle?",
                        p => true,
                        p =>
                        {
                            DialogueModule failureModule = new DialogueModule("Never! That hammer has witnessed more storms and bloodshed than most can imagine. Even when fate pressed hard against our backs, its spirit—like my own—remained unbroken.");
                            p.SendGump(new DialogueGump(p, failureModule));
                        });
                    player.SendGump(new DialogueGump(player, legendModule));
                });

            forging.AddOption("Return to previous menu.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return forging;
        }

        private DialogueModule CreateShipbuildingModule()
        {
            DialogueModule shipbuilding = new DialogueModule("Marin and I often steal quiet hours on the docks, debating the curvature of hulls, the subtleties of weight balance, and the ancient runes that protect our vessels. Our nights are filled with the sound of waves and whispered secrets of a craft as old as time.");
            
            shipbuilding.AddOption("What kind of secrets do you and Marin exchange?",
                player => true,
                player =>
                {
                    DialogueModule secretsModule = new DialogueModule("Marin is a master of the old maritime ways. Together we decode timeless blueprints, discussing the delicate intricacies of waterproofing techniques using rare sea herbs, and even the mystical art of rune-etching to channel the protective forces of the moon.");
                    secretsModule.AddOption("Tell me about the waterproofing technique.",
                        p => true,
                        p =>
                        {
                            DialogueModule waterproofModule = new DialogueModule("It uses an extract from the sea fern—a ritual passed down over generations. Applied with care, it repels saltwater while reinforcing the ship’s wooden soul against decay. A secret worth its weight in legends.");
                            p.SendGump(new DialogueGump(p, waterproofModule));
                        });
                    secretsModule.AddOption("How do the runes protect the ships?",
                        p => true,
                        p =>
                        {
                            DialogueModule runesModule = new DialogueModule("Each rune is painstakingly inscribed by hand. They are not mere decorations but channels for protective energies drawn from the moon and tides. Marin insists they are as vital as a captain’s steady hand on deck.");
                            p.SendGump(new DialogueGump(p, runesModule));
                        });
                    player.SendGump(new DialogueGump(player, secretsModule));
                });

            shipbuilding.AddOption("Have you ever built a ship with Marin?",
                player => true,
                player =>
                {
                    DialogueModule buildModule = new DialogueModule("Aye, there was one starry night when inspiration struck like lightning. Together, we built a swift clipper destined to outrun even the fiercest tempests. The process was one of both art and valor—a true blend of our shared vision for a better tomorrow.");
                    buildModule.AddOption("What made that ship so special?",
                        p => true,
                        p =>
                        {
                            DialogueModule specialModule = new DialogueModule("The balance was flawless, the enchantments subtle yet potent—a masterpiece born of friendship, passion, and a yearning for adventure. It remains a symbol of our unbreakable bond and creative spirit.");
                            p.SendGump(new DialogueGump(p, specialModule));
                        });
                    buildModule.AddOption("I’d love to see that ship someday.",
                        p => true,
                        p =>
                        {
                            DialogueModule dreamModule = new DialogueModule("Perhaps one day, under fate's kind eye, you'll witness her gleaming silhouette against the setting sun. Until then, let her legend live on in our stories.");
                            p.SendGump(new DialogueGump(p, dreamModule));
                        });
                    player.SendGump(new DialogueGump(player, buildModule));
                });

            shipbuilding.AddOption("Return to previous menu.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return shipbuilding;
        }

        private DialogueModule CreateRelicsModule()
        {
            DialogueModule relics = new DialogueModule("Captain Waylon and I share a solemn duty: safeguarding the relics of maritime lore. From ancient compasses to mystic astrolabes, each artifact is imbued with histories of heroism and sacrifice—whispers of a bygone era that guide us still.");
            
            relics.AddOption("What kind of relics do you maintain?",
                player => true,
                player =>
                {
                    DialogueModule typeModule = new DialogueModule("We care for relics like the 'Compass of the Endless Tide'—which is said to direct not just the ship, but the very course of destiny—and the 'Astrolabe of the Lost Horizon,' a device that still hints at hidden realms beneath the waves.");
                    typeModule.AddOption("Tell me about the Compass of the Endless Tide.",
                        p => true,
                        p =>
                        {
                            DialogueModule compassModule = new DialogueModule("They say the Compass of the Endless Tide shivers in the presence of both danger and fortune. Its needle seems almost sentient, guiding sailors to their true fates.");
                            p.SendGump(new DialogueGump(p, compassModule));
                        });
                    typeModule.AddOption("What about the Astrolabe of the Lost Horizon?",
                        p => true,
                        p =>
                        {
                            DialogueModule astroModule = new DialogueModule("Crafted by an ancient navigator, this astrolabe channels the celestial bodies’ secrets. Its intricate design continues to baffle even the most seasoned sailors.");
                            p.SendGump(new DialogueGump(p, astroModule));
                        });
                    player.SendGump(new DialogueGump(player, typeModule));
                });

            relics.AddOption("How do you and Captain Waylon collaborate?",
                player => true,
                player =>
                {
                    DialogueModule teamworkModule = new DialogueModule("Captain Waylon is a man of iron resolve and salty wit. We meet by the old lighthouse to discuss restoration techniques, trade battle-worn tales, and even strategize secret missions to reclaim relics lost to time. His insights echo with wisdom earned on turbulent seas.");
                    teamworkModule.AddOption("What was the toughest relic restoration you ever undertook?",
                        p => true,
                        p =>
                        {
                            DialogueModule challengeModule = new DialogueModule("Restoring an ancient navigational sextant—a relic once wielded by a pirate king—tested every ounce of my craft. Weeks of tireless work and stubborn resolve made that achievement a beacon of honor in my career.");
                            p.SendGump(new DialogueGump(p, challengeModule));
                        });
                    teamworkModule.AddOption("Do you ever clash over methods?",
                        p => true,
                        p =>
                        {
                            DialogueModule banterModule = new DialogueModule("Oh, our debates are as spirited as a squall at sea! Yet, even when passions flare, our respect for each other’s expertise prevails. In the end, every argument refines our methods into something greater.");
                            p.SendGump(new DialogueGump(p, banterModule));
                        });
                    player.SendGump(new DialogueGump(player, teamworkModule));
                });

            relics.AddOption("Return to previous menu.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return relics;
        }

        private DialogueModule CreatePersonalModule()
        {
            DialogueModule personal = new DialogueModule("I’ve spent years at the anvil and amidst the roar of the sea. Every scar on my hands tells a story—from early mornings at the forge to moments of quiet reflection under the watchful gaze of the stars. My journey is as rich in hardship as it is in triumph.");
            
            personal.AddOption("Tell me about your early days at the forge.",
                player => true,
                player =>
                {
                    DialogueModule earlyDaysModule = new DialogueModule("Back in my youth, as an eager apprentice, I learned that every blow of the hammer carried not just metal but the very spirit of our ancestors. It was a time of sweat, determination, and unspoken promises—a period that shaped my destiny.");
                    earlyDaysModule.AddOption("What did you learn that still guides you?",
                        p => true,
                        p =>
                        {
                            DialogueModule lessonsModule = new DialogueModule("I learned to honor every piece of metal, for in each was a fragment of a grander story. Patience, precision, and honor became my steadfast companions, guiding my hands and heart as I forged my path.");
                            p.SendGump(new DialogueGump(p, lessonsModule));
                        });
                    earlyDaysModule.AddOption("It sounds like those days were hard but rewarding.",
                        p => true,
                        p =>
                        {
                            DialogueModule rewardModule = new DialogueModule("They were indeed grueling, yet every hardship fueled my resolve. The warm glow of a freshly forged blade and the knowledge of a job well done—these were my rewards, more enduring than any coin.");
                            p.SendGump(new DialogueGump(p, rewardModule));
                        });
                    player.SendGump(new DialogueGump(player, earlyDaysModule));
                });

            personal.AddOption("How do you balance your work and your inner journey?",
                player => true,
                player =>
                {
                    DialogueModule balanceModule = new DialogueModule("Balancing the weight of iron and the spirit within isn’t easy. I find solace in the bond with my fellows at the docks, in quiet evenings sharing tales and in memories of battles past. Every day is a lesson in resilience and fortitude.");
                    balanceModule.AddOption("Your journey sounds full of rich stories.",
                        p => true,
                        p =>
                        {
                            DialogueModule storyModule = new DialogueModule("Indeed, each scar, each line on my face is a chapter—a reminder of triumphs and losses. They speak of close calls with fate and the relentless pursuit of honor.");
                            p.SendGump(new DialogueGump(p, storyModule));
                        });
                    balanceModule.AddOption("Thank you for sharing these insights.",
                        p => true,
                        p =>
                        {
                            p.SendMessage("Big Harv nods solemnly. 'Honor is both burden and beacon, friend.'");
                        });
                    player.SendGump(new DialogueGump(player, balanceModule));
                });

            personal.AddOption("Return to previous menu.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return personal;
        }

        private DialogueModule CreateWarriorModule()
        {
            DialogueModule warrior = new DialogueModule("So, you wish to hear of my secret warrior past? Beneath this weathered exterior beats the heart of a fierce and proud warrior—once the leader of the Iron Wolf Clan. Though my days at the forge and docks have defined much of my present, my blood still burns with the memory of glorious battle and honor lost—and soon to be regained.");
            
            warrior.AddOption("Tell me about the Iron Wolf Clan.",
                player => true,
                player =>
                {
                    DialogueModule clanModule = new DialogueModule("The Iron Wolf Clan was once renowned for its fierce valor and unyielding honor. We were warriors of unmatched spirit, our names spoken in both fear and reverence across the lands. I was chosen to lead them—a responsibility that still weighs on me.");
                    clanModule.AddOption("What were the clan's founding legends?",
                        p => true,
                        p =>
                        {
                            DialogueModule legendsModule = new DialogueModule("Legends say that our ancestors were born from the heart of a storm and tempered in battle. They fought side by side with wolves—a symbol of loyalty and ferocity. Every scar and medal of our past tells a story of bravery and sacrifice.");
                            legendsModule.AddOption("How did these legends inspire you?",
                                q => true,
                                q =>
                                {
                                    DialogueModule inspireModule = new DialogueModule("They instilled in me an unbreakable will. Every time I swing my hammer, I feel the echo of my ancestors urging me to reclaim our lost glory and restore honor to our family name.");
                                    q.SendGump(new DialogueGump(q, inspireModule));
                                });
                            p.SendGump(new DialogueGump(p, legendsModule));
                        });
                    clanModule.AddOption("What happened to the clan?",
                        p => true,
                        p =>
                        {
                            DialogueModule downfallModule = new DialogueModule("Betrayal from within and the ravages of time decimated our numbers. We fell from our lofty perch, scattered like fallen leaves in a storm. Yet, in the depths of defeat, my resolve was forged anew—to rise, reclaim, and let our legacy roar back into life.");
                            downfallModule.AddOption("How do you plan to restore your clan's honor?",
                                q => true,
                                q =>
                                {
                                    DialogueModule restoreModule = new DialogueModule("I am gathering every remaining warrior, every spark of our lost pride. I train relentlessly and forge weapons not only for trade but as symbols of our imminent resurgence. Our return shall be marked by glorious battle—a reckoning written in the annals of time.");
                                    restoreModule.AddOption("What is your next step in this quest?",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule nextStepModule = new DialogueModule("I plan to lead a small band on a daring raid against marauders who've defiled our sacred grounds. It will be a test of both strength and honor—a battle that will mark the beginning of our resurgence. Every clash of steel is a promise to our forebears.");
                                            r.SendGump(new DialogueGump(r, nextStepModule));
                                        });
                                    q.SendGump(new DialogueGump(q, restoreModule));
                                });
                            p.SendGump(new DialogueGump(p, downfallModule));
                        });
                    player.SendGump(new DialogueGump(player, clanModule));
                });

            warrior.AddOption("Have you fought in battle recently?",
                player => true,
                player =>
                {
                    DialogueModule battleModule = new DialogueModule("Aye, just beyond the coastal cliffs, I led a skirmish against marauders who dared disrespect our ancient lands. The clash was brutal—steel met steel, and every cry carried the weight of my clan's honor. Even now, my battle scars speak for the price of pride.");
                    battleModule.AddOption("Tell me more about that battle.",
                        p => true,
                        p =>
                        {
                            DialogueModule detailsBattle = new DialogueModule("Under a blood-red sunset, our band clashed with foes twice our number. Every parry and strike was a testament to our unyielding spirit. I fought with the fury of a man possessed—each victory a small rekindling of our once-glorious legacy.");
                            detailsBattle.AddOption("What did you learn from that fight?",
                                q => true,
                                q =>
                                {
                                    DialogueModule lessonsBattle = new DialogueModule("I learned that honor is earned in the crucible of combat—sacrifices made and blood spilled forge a warrior stronger than any steel. It reminded me that our clan’s spirit still burns fiercely in my veins.");
                                    q.SendGump(new DialogueGump(q, lessonsBattle));
                                });
                            p.SendGump(new DialogueGump(p, detailsBattle));
                        });
                    battleModule.AddOption("Do you regret returning to battle?",
                        p => true,
                        p =>
                        {
                            DialogueModule regretModule = new DialogueModule("Regret has no place in a warrior’s heart. Every battle, no matter how grueling, is a step toward reclaiming our honor. Though the price is high, I embrace the fire of combat as both penance and promise.");
                            p.SendGump(new DialogueGump(p, regretModule));
                        });
                    player.SendGump(new DialogueGump(player, battleModule));
                });

            warrior.AddOption("What does being a proud, honorable warrior mean to you?",
                player => true,
                player =>
                {
                    DialogueModule honorModule = new DialogueModule("To me, honor is the sacred bond that ties us to our ancestors. It is living with integrity, fighting for justice, and bearing the scars of battle as emblems of courage. I am proud to stand by my clan’s legacy, fierce and unyielding in the face of adversity.");
                    honorModule.AddOption("Your words are inspiring. How do you keep that fire alive?",
                        p => true,
                        p =>
                        {
                            DialogueModule fireModule = new DialogueModule("I keep the fire alive by never forgetting who I am—a warrior forged in the heat of battle, with a destiny to restore our faded glory. Every day I train, every blow I strike is an oath to my forefathers and to myself.");
                            p.SendGump(new DialogueGump(p, fireModule));
                        });
                    honorModule.AddOption("It must be a heavy burden to carry.",
                        p => true,
                        p =>
                        {
                            DialogueModule burdenModule = new DialogueModule("Indeed, the burden of honor is heavy—but it is also my strength. I bear it with pride, for it fuels my determination to return our clan to its rightful place among legends.");
                            p.SendGump(new DialogueGump(p, burdenModule));
                        });
                    player.SendGump(new DialogueGump(player, honorModule));
                });

            warrior.AddOption("Return to previous menu.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return warrior;
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
