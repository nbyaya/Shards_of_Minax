using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class LadyIsobel : BaseCreature
    {
        [Constructable]
		public LadyIsobel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Isobel";
            Body = 0x190; // Human female body (customize as needed)

            // Stats
            SetStr(120);
            SetDex(80);
            SetInt(110);
            SetHits(100);

            // Appearance: A mix of noble elegance and battle-worn pragmatism.
            AddItem(new Robe() { Hue = 1150, Name = "Elegant Court Robe" });
            AddItem(new Bonnet() { Hue = 1150, Name = "Intricately Woven Bonnet" });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new GoldRing() { Hue = 1150, Name = "Family Crest" });
            // Subtle hint to her past
            AddItem(new Longsword() { Name = "Rusty Saber", Hue = 1150 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public LadyIsobel(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Main greeting text with hints of her dark past
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Isobel, once a soldier in the pre-war armies and now a mercenary of renown—selling my skills to the highest bidder. My stoic demeanor masks a ruthless determination and a resourceful spirit born of conflict. Yet beneath it all, I am haunted by the ghosts of those I've slain. How may I share my tale with you today?");

            // Option 1: Discuss the castle's ancient gates and repairs.
            greeting.AddOption("Tell me about the castle's ancient gates and the repairs.", 
                player => true, 
                player =>
                {
                    DialogueModule gatesModule = new DialogueModule("The ancient gates stand as a testament to our long history. I work closely with Torren from East Montor on their restoration. His craftsmanship is second to none, and our discussions often mix practical repair work with reflections on the old wars. Do you wish to hear about their storied past or the challenges we face today?");
                    
                    gatesModule.AddOption("Tell me about the gates' storied past.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule historyModule = new DialogueModule("These gates were built in an age when honor and bloodshed walked hand-in-hand. They have seen the rise of noble houses and the fall of tyrants. I recall a time when these very stones were used as a rallying point for soldiers—soldiers like myself, who believed in the cause until the day that cause turned dark. Would you like to hear of a specific event?");
                            historyModule.AddOption("Yes, recount a legendary event.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule legendModule = new DialogueModule("There was a bitter winter when a comet blazed across the sky. The gates pulsed with an eerie light, as if the heavens themselves wept for the fallen. It was during that night that I took my first life in battle—a memory that haunts me to this day. The stones seem to murmur secrets of that tragic night. Intriguing, is it not?");
                                    pl2.SendGump(new DialogueGump(pl2, legendModule));
                                });
                            historyModule.AddOption("I think that's enough history.", 
                                pl2 => true, 
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, historyModule));
                        });

                    gatesModule.AddOption("What challenges do the repairs face today?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule repairModule = new DialogueModule("Our repairs are hindered not just by time, but by lingering curses. Torren uncovered ancient inscriptions on a hidden section of the gate, inscriptions that suggest deliberate sabotage from forces unseen. I suspect these forces are tied to the dark secrets of my past—a past that refuses to fade away. Do you wish to learn about Torren’s discoveries or my personal suspicions?");
                            repairModule.AddOption("Tell me about Torren's discoveries.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule torrenModule = new DialogueModule("Torren, a master of stone and secrets, found runes carved in a language long forgotten. He believes these runes might be an ancient safeguard or, perhaps, a warning. I trust his resourcefulness implicitly, yet I cannot shake the feeling that these marks are linked to battles I fought long ago.");
                                    pl2.SendGump(new DialogueGump(pl2, torrenModule));
                                });
                            repairModule.AddOption("And what are your personal suspicions?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule suspicionsModule = new DialogueModule("In quiet moments, I review investigative notes shared with Elena the Archivist. I fear that these inscriptions are a beacon for malevolent forces, perhaps even the very specters of those I once condemned in battle. My past as a soldier and mercenary has taught me that every action has a cost—a burden that I carry even now.");
                                    pl2.SendGump(new DialogueGump(pl2, suspicionsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, repairModule));
                        });
                    player.SendGump(new DialogueGump(player, gatesModule));
                });

            // Option 2: Discuss her investigative notes with Elena the Archivist.
            greeting.AddOption("I hear you share investigative notes with Elena the Archivist. What strange occurrences have you uncovered?", 
                player => true, 
                player =>
                {
                    DialogueModule notesModule = new DialogueModule("Indeed, Elena and I delve into the mysteries that plague our lands. We pore over ancient manuscripts and cryptic records—each whispering of supernatural events and ghostly apparitions. Sometimes, I see reflections of my own past in these accounts. Would you like to learn about the cryptic symbols, or perhaps about a case that particularly disturbed me?");
                    
                    notesModule.AddOption("What are these cryptic symbols?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule symbolsModule = new DialogueModule("The symbols are both beautiful and ominous—intertwined serpents, broken shields, and stark runes. They remind me of the insignia from my days in the pre-war army. Jasper the Scribe and I often spar over their meaning. Do you wish to hear about one of our debates?");
                            symbolsModule.AddOption("Yes, share one of your debates.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule debateModule = new DialogueModule("Jasper argues that every symbol has a logical origin, a remnant of ancient code. I, however, insist that they hint at the chaotic nature of magic—a chaos that mirrors my own inner turmoil. Our debates are fierce, yet they drive us to unravel truths hidden in plain sight.");
                                    pl2.SendGump(new DialogueGump(pl2, debateModule));
                                });
                            symbolsModule.AddOption("I prefer to move on.", 
                                pl2 => true, 
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, symbolsModule));
                        });
                    
                    notesModule.AddOption("Tell me about a case that disturbed you.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule caseModule = new DialogueModule("There was a chilling case where villagers reported shadowy figures emerging during the new moon. The evidence was scarce, yet I felt an all-too-familiar coldness in my heart. It reminded me of the ruthless acts I committed in battle—acts that still haunt my nights. Elena and I are still piecing together the evidence, trying to decide if it’s mere superstition or something far darker.");
                            caseModule.AddOption("What did you feel when you saw those shadows?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule feelingsModule = new DialogueModule("It was as if the specters of my past reached out to remind me of my own ruthlessness. I felt both a surge of hardened resolve and a piercing pang of regret—a duality that defines me even now. I struggle daily with the weight of lives taken, and such encounters force me to confront that pain head-on.");
                                    pl2.SendGump(new DialogueGump(pl2, feelingsModule));
                                });
                            caseModule.AddOption("And what is Elena's take on it?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule elenaModule = new DialogueModule("Elena is ever the scholar—analytical, methodical. She sees these cases as puzzles, pieces of a larger, enigmatic mosaic that spans centuries. Yet even she cannot deny the eerie parallels to our darker histories. Her quiet determination to document these events is both admirable and, at times, a silent lament for what once was.");
                                    pl2.SendGump(new DialogueGump(pl2, elenaModule));
                                });
                            pl.SendGump(new DialogueGump(pl, caseModule));
                        });
                    player.SendGump(new DialogueGump(player, notesModule));
                });

            // Option 3: Discuss her rivalry with Jasper the Scribe.
            greeting.AddOption("I see you share a rivalry with Jasper the Scribe. How do you two interact?", 
                player => true, 
                player =>
                {
                    DialogueModule scribeModule = new DialogueModule("Ah, Jasper—my intellectual adversary and unexpected ally. Our debates are as sharp as the blades of our former battalions. We meet often, our conversations ranging from the deciphering of arcane symbols to recollections of the harsh realities of war. Despite our differences, there is mutual respect born of shared hardship.");
                    
                    scribeModule.AddOption("What do you debate about?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule debateModule = new DialogueModule("Our debates range from whether the symbols are mere remnants of ancient bureaucracy or living echoes of past conflicts, to philosophical musings on fate and free will. Jasper insists that every mystery has a rational explanation, while I lean towards the chaos that birthed them. It is a dance of logic and emotion.");
                            pl.SendGump(new DialogueGump(pl, debateModule));
                        });
                    scribeModule.AddOption("Do you ever set aside your rivalry and collaborate?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule collaborateModule = new DialogueModule("Yes, there are rare moments when our rivalry gives way to collaboration. When confronted with evidence too perplexing to ignore, we combine our insights. These collaborations, however brief, remind me that even in a life defined by conflict and guilt, there is room for shared purpose.");
                            pl.SendGump(new DialogueGump(pl, collaborateModule));
                        });
                    scribeModule.AddOption("It sounds like a complex relationship.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule complexityModule = new DialogueModule("Indeed. Our rivalry is both a reflection of our personal demons and our unyielding quest for truth. Each exchange is a battle fought with words, and though our scars run deep, they forge a bond that, in its own way, is a kind of salvation.");
                            pl.SendGump(new DialogueGump(pl, complexityModule));
                        });
                    player.SendGump(new DialogueGump(player, scribeModule));
                });

            // Option 4: Inquire about her personal past and inner demons.
            greeting.AddOption("Your eyes seem heavy with memories. Tell me about your past.", 
                player => true, 
                player =>
                {
                    DialogueModule pastModule = new DialogueModule("My past is a battlefield strewn with victories and atrocities. Once, I served as a soldier in the pre-war armies—a time when loyalty was forged in blood and steel. I fought with ruthless precision, and in the heat of combat, I became something I now struggle to reconcile with. I sold my skills as a mercenary to the highest bidder, yet every life taken weighs upon my soul. What would you like to know about this painful journey?");
                    
                    pastModule.AddOption("Tell me about your time as a soldier.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule soldierModule = new DialogueModule("I remember the roar of battle, the scent of smoke, and the deafening clash of steel. I was once known for my stoicism—never flinching even when the odds were against us. But behind that hardened exterior lay a heart that wept for every fallen comrade. I did what was necessary, yet the cost was a burden of guilt that I still bear.");
                            soldierModule.AddOption("How did you cope with that guilt?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule guiltModule = new DialogueModule("There are nights when the ghosts of my past whisper in the silence, their voices a constant reminder of my ruthlessness. I found solace in discipline and the cold logic of survival, but no amount of resolve can fully erase the memories of those I could not save. I channel that pain into my work, striving to protect the living from the darkness I once embraced.");
                                    pl2.SendGump(new DialogueGump(pl2, guiltModule));
                                });
                            soldierModule.AddOption("What led you to become a mercenary?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule mercModule = new DialogueModule("After the war, the world became a harsh place where ideals crumbled under the weight of reality. I sold my skills to those who could pay, not out of greed, but as a means of survival. Every contract is a reminder of what I once was—a tool of destruction—and a chance to perhaps, in some small way, atone by protecting others from the fate I once met.");
                                    pl2.SendGump(new DialogueGump(pl2, mercModule));
                                });
                            pl.SendGump(new DialogueGump(pl, soldierModule));
                        });
                    
                    pastModule.AddOption("What does your past haunt you with?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule hauntModule = new DialogueModule("The memories are many—a cacophony of lost friends, the relentless clamor of battle, and the souls of those I could not bring back. I see their faces in the flicker of candlelight, and in moments of silence, I almost hear their pleas. It is a solitude that is as crushing as it is constant. Yet, it also drives me to be resourceful and never repeat the same mistakes.");
                            hauntModule.AddOption("Is there any hope of redemption?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule hopeModule = new DialogueModule("Redemption is a path paved with regret and relentless effort. I strive each day to protect these walls and the people within them, hoping that each act of valor might tip the scales toward absolution. I cannot change the past, but perhaps, through my actions, I can secure a future where such horrors need not recur.");
                                    pl2.SendGump(new DialogueGump(pl2, hopeModule));
                                });
                            hauntModule.AddOption("How do you deal with these memories?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule dealModule = new DialogueModule("I have learned to compartmentalize—stoic in public, ruthless when duty calls, and resourceful enough to keep moving forward. In quiet moments, I write my thoughts in a hidden journal, a record of the cost of war and the hope for a better future. It is a solitary ritual, but it keeps the darkness at bay.");
                                    pl2.SendGump(new DialogueGump(pl2, dealModule));
                                });
                            pl.SendGump(new DialogueGump(pl, hauntModule));
                        });
                    
                    pastModule.AddOption("I appreciate your honesty, though it is a heavy burden to bear.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule acceptanceModule = new DialogueModule("Thank you, traveler. It is not easy to lay bare the scars of one's soul, but perhaps in sharing them, I can find a measure of peace. Know this: even the most ruthless among us can harbor a heart that longs for redemption.");
                            pl.SendGump(new DialogueGump(pl, acceptanceModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, pastModule));
                });

            // Option 5: Ask about her daily life at the castle and current responsibilities.
            greeting.AddOption("What is your life like here at the castle, aside from your haunted past?", 
                player => true, 
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("Life within these ancient walls is a blend of duty, vigilance, and quiet moments of introspection. I oversee the maintenance of the castle, ensuring that its defenses are as formidable as the legends suggest. Between supervising repairs, strategizing with trusted allies like Torren, and sharing cryptic letters with Elena, I find fleeting peace amidst chaos.");
                    
                    lifeModule.AddOption("Tell me about your daily routine.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule routineModule = new DialogueModule("My day begins before the sun graces the horizon. I patrol the ramparts, inspect the ancient gates, and often spend time in the archives—pondering over dusty records and lost tales. Even in these quiet moments, the weight of my past is never far from my mind. Every decision, every repair, is a step toward fortifying not just the castle, but the hope that one day, I might lay my burdens to rest.");
                            pl.SendGump(new DialogueGump(pl, routineModule));
                        });
                    lifeModule.AddOption("How do you balance duty with personal demons?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule balanceModule = new DialogueModule("It is a delicate equilibrium—a dance between the resolute responsibilities of a guardian and the volatile echoes of my history. I have learned to compartmentalize: by day, I am the steadfast caretaker, and by night, I confront the specters of my past. This duality is my curse and my strength.");
                            pl.SendGump(new DialogueGump(pl, balanceModule));
                        });
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // Option 6: Exit the conversation.
            greeting.AddOption("Thank you, Lady Isobel. I must take my leave.", 
                player => true, 
                player =>
                {
                    DialogueModule exitModule = new DialogueModule("Farewell, traveler. May your journey be enlightened by both the light and the shadow of truth. Remember, every stone in this castle, and every choice made, tells a story.");
                    player.SendGump(new DialogueGump(player, exitModule));
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
