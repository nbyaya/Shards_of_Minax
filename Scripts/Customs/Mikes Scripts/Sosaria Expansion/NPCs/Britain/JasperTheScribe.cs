using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class JasperTheScribe : BaseCreature
    {
        [Constructable]
        public JasperTheScribe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jasper the Scribe";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(70);
            SetInt(140);
            SetHits(100);

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new FeatheredHat() { Hue = 1150, Name = "Scribe's Cap" });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public JasperTheScribe(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule root = CreateDialogueTree();
            player.SendGump(new DialogueGump(player, root));
        }

        private DialogueModule CreateDialogueTree()
        {
            // Root Dialogue: Introduce Jasper as both a scribe and reclusive alchemist.
            DialogueModule root = new DialogueModule("Ah, greetings traveler! I am Jasper the Scribe—and, between scribbling ancient lore and concocting peculiar elixirs, I find myself ever-curious about the magical mysteries of Sosaria. My experiments are as unpredictable as the legends I record. How may I enlighten you today?");
            
            // Option 1: Discuss Relic Sites and Ley Lines (with a nod to his collaboration with Miko)
            root.AddOption("Tell me about the relic sites and ley lines you've documented with Miko.", 
                player => true,
                player =>
                {
                    DialogueModule relicModule = new DialogueModule("Miko, the brilliant mapmaker from Grey & Pirate Isle, and I have uncovered relic sites scattered across our land. These sites, linked by ancient ley lines, emit wondrous magical auras—and occasionally trigger my own experiments! Would you like to hear about the relic sites, the ley lines, or perhaps the unintended magical consequences I've observed?");
                    
                    // Option 1a: Details on relic sites
                    relicModule.AddOption("I want details about the relic sites.", 
                        p => true,
                        p =>
                        {
                            DialogueModule sitesModule = new DialogueModule("These relic sites are as varied as they are mysterious. One site, hidden beneath an old grove in Yew, is adorned with inscriptions that glow under the moonlight. I once attempted to recreate the glow in my lab—resulting in a minor explosion that singed half my manuscripts! Would you care to know more about the inscriptions or the phenomenon of the moonlit glow?");
                            
                            sitesModule.AddOption("Tell me more about the inscriptions.", 
                                pl => true,
                                pl =>
                                {
                                    DialogueModule inscriptionModule = new DialogueModule("The inscriptions are a puzzling mix of archaic runes and pictographs. Lady Isobel lent me several ancient scrolls to compare with these symbols. In my haste, I even tried a rudimentary translation potion—only to have it backfire and turn my quill into a rather chatty bird! Nonetheless, the runes hint at a secret network of relics and long-forgotten rituals.");
                                    p.SendGump(new DialogueGump(p, inscriptionModule));
                                });
                            
                            sitesModule.AddOption("What about the moonlit glow?", 
                                pl => true,
                                pl =>
                                {
                                    DialogueModule glowModule = new DialogueModule("Ah, the glow! During full moons, some relic sites emit a soft luminescence, as if kissed by stardust. I attempted to harness this light in a vial, hoping to create a luminous elixir. Instead, the vial burst into a cascade of sparkles—and my lab smelled of burnt rosemary for days. It is as if the very fabric of magic is woven into these ruins.");
                                    glowModule.AddOption("Explain the science behind the glow.", 
                                        q => true,
                                        q =>
                                        {
                                            DialogueModule scienceModule = new DialogueModule("I theorize that the glow results from concentrated ley line energy interacting with ancient magical residues. It’s a phenomenon that defies conventional alchemy and challenges even my inventive mind!");
                                            q.SendGump(new DialogueGump(q, scienceModule));
                                        });
                                    glowModule.AddOption("I'll leave the science to you.", 
                                        q => true,
                                        q => q.SendGump(new DialogueGump(q, CreateDialogueTree())));
                                    p.SendGump(new DialogueGump(p, glowModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, sitesModule));
                        });
                    
                    // Option 1b: Discussion of ley lines
                    relicModule.AddOption("I want to hear about the ley lines.", 
                        p => true,
                        p =>
                        {
                            DialogueModule leyModule = new DialogueModule("Ley lines are the invisible threads of power that stitch our land together. I once tried to visualize them using a self-made prism and a concoction of phosphorescent reagents—only to have my lab floor shimmer with ghostly patterns for a week! Would you like to explore how these ley lines influence local legends, or how they unexpectedly affect my alchemical experiments?");
                            
                            leyModule.AddOption("How do they influence local legends?", 
                                q => true,
                                q =>
                                {
                                    DialogueModule legendModule = new DialogueModule("Local lore tells of ley lines as the veins of ancient magic. Lady Isobel insists that a forgotten scroll she discovered speaks of these lines as guides to hidden treasures. I often wonder if my own misadventures are somehow fated by these lines. Intriguing, isn’t it?");
                                    q.SendGump(new DialogueGump(q, legendModule));
                                });
                            
                            leyModule.AddOption("How do they affect your experiments?", 
                                q => true,
                                q =>
                                {
                                    DialogueModule expModule = new DialogueModule("In my laboratory, ley lines have a mind of their own! A slight misalignment of a reagent near a ley intersection once resulted in a spontaneous rain of silver dust—a spectacle that, while beautiful, left me with quite a mess to clean. These unpredictable bursts of magic keep me both inspired and cautious.");
                                    expModule.AddOption("Tell me about that silver dust incident.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule silverModule = new DialogueModule("Ah, the silver dust incident! I was attempting to distill a potion of clarity when, due to a miscalculation, the potion exploded in a shower of glittering particles. The effect was mesmerizing but left me with a lab full of shimmering residue and a renewed respect for the raw power of ley energy.");
                                            r.SendGump(new DialogueGump(r, silverModule));
                                        });
                                    expModule.AddOption("That sounds dangerous!", 
                                        r => true,
                                        r => r.SendGump(new DialogueGump(r, CreateDialogueTree())));
                                    q.SendGump(new DialogueGump(q, expModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, leyModule));
                        });
                    
                    // Option 1c: Magical consequences of his experiments
                    relicModule.AddOption("Tell me about the unintended magical consequences of your experiments.", 
                        p => true,
                        p =>
                        {
                            DialogueModule magicModule = new DialogueModule("Oh, the perils of experimentation! My incessant curiosity has led to spells gone awry—a self-replicating quill here, an enchanted ink that writes on its own there. Once, I even brewed an elixir intended to reveal hidden truths, only to have it transform my notes into dancing, sarcastic runes!");
                            
                            magicModule.AddOption("What happened with the self-replicating quill?", 
                                r => true,
                                r =>
                                {
                                    DialogueModule quillModule = new DialogueModule("The quill was designed to record all that I observed. Instead, it began multiplying, each copy scribbling nonsensical poetry until I had to chase them around the scriptorium. I later recovered one quill that still hums with mysterious energy—perhaps it holds a clue to an ancient magical ritual.");
                                    r.SendGump(new DialogueGump(r, quillModule));
                                });
                            
                            magicModule.AddOption("And the enchanted ink?", 
                                r => true,
                                r =>
                                {
                                    DialogueModule inkModule = new DialogueModule("The enchanted ink was supposed to reveal invisible messages on ancient parchment. However, it ended up inscribing cheeky comments on every document in my lab. Imagine reading a centuries-old treaty only to find a note saying, 'Nice try, old man!' It’s both amusing and maddening.");
                                    r.SendGump(new DialogueGump(r, inkModule));
                                });
                            
                            magicModule.AddOption("I can see why you remain reclusive.", 
                                r => true,
                                r => r.SendGump(new DialogueGump(r, CreateDialogueTree())));
                            p.SendGump(new DialogueGump(p, magicModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, relicModule));
                });

            // Option 2: Updates and Secrets from Lady Isobel
            root.AddOption("What are the latest mysteries Lady Isobel has uncovered?", 
                player => true,
                player =>
                {
                    DialogueModule isobelModule = new DialogueModule("Lady Isobel is a relentless seeker of truth. Recently, she discovered hidden chambers within the castle, their walls adorned with cryptic symbols. I’ve helped her transcribe her findings—and even tried to decode a few with my experimental elixir of insight, with unpredictable results. Would you like to hear about the secret chambers, the symbols, or perhaps my alchemical misadventure during one of our meetings?");
                    
                    isobelModule.AddOption("Tell me about the secret chambers.", 
                        p => true,
                        p =>
                        {
                            DialogueModule chamberModule = new DialogueModule("The secret chambers are deep within the castle's forgotten wings, built by an ancient order to safeguard forbidden lore. During one exploration, a misfired potion of illumination made the entire chamber glow like a starfield—revealing murals that no one had seen in centuries. It left Lady Isobel both awestruck and disturbed.");
                            p.SendGump(new DialogueGump(p, chamberModule));
                        });
                    
                    isobelModule.AddOption("What of the cryptic symbols?", 
                        p => true,
                        p =>
                        {
                            DialogueModule symbolModule = new DialogueModule("The symbols are a labyrinth of meaning—a blend of celestial maps and elemental sigils. Lady Isobel and I spent many a sleepless night comparing our findings. I even attempted to replicate one of the symbols in my lab; the ensuing magical burst nearly singed my eyebrows off! Such is the price of curiosity.");
                            p.SendGump(new DialogueGump(p, symbolModule));
                        });
                    
                    isobelModule.AddOption("Share your alchemical misadventure.", 
                        p => true,
                        p =>
                        {
                            DialogueModule misadventureModule = new DialogueModule("Ah, that incident was both humbling and hilarious! I mixed an elixir intended to enhance perception with extracts from a rare moonflower. Instead of a moment of clarity, I was left with visions of dancing runes mocking my every move. Lady Isobel still teases me about it during our quiet tea sessions in the library.");
                            misadventureModule.AddOption("That must have been embarrassing!", 
                                q => true,
                                q => q.SendGump(new DialogueGump(q, misadventureModule)));
                            misadventureModule.AddOption("I admire your dedication despite setbacks.", 
                                q => true,
                                q => q.SendGump(new DialogueGump(q, CreateDialogueTree())));
                            p.SendGump(new DialogueGump(p, misadventureModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, isobelModule));
                });

            // Option 3: Visions and Wisdom from Mother Edda
            root.AddOption("What does Mother Edda say about the ancient magics?", 
                player => true,
                player =>
                {
                    DialogueModule eddaModule = new DialogueModule("Mother Edda of Yew, our venerable seer, has been having visions of a time when the old magics surge anew. I’ve recorded her prophecies in my scrolls, though her words are as enigmatic as they are profound. Would you like to hear of her most recent vision, or understand her overall philosophy on the land’s magic?");
                    
                    eddaModule.AddOption("Share her most recent vision.", 
                        p => true,
                        p =>
                        {
                            DialogueModule recentVisionModule = new DialogueModule("In her latest vision, Mother Edda described a swirling nexus of energy beneath the ancient oak in Yew—a convergence of ley lines that foretold both renewal and calamity. I attempted to capture its essence in a tincture, but the resulting brew turned my notes into shimmering, incomprehensible script!");
                            p.SendGump(new DialogueGump(p, recentVisionModule));
                        });
                    
                    eddaModule.AddOption("Explain her overall philosophy.", 
                        p => true,
                        p =>
                        {
                            DialogueModule philosophyModule = new DialogueModule("Mother Edda believes that every rock, tree, and drop of rain holds the memories of our world. Her words remind us to listen deeply—to the wind, to the whisper of the earth, and even to the errant murmurs of a failed potion. Her wisdom has guided many in understanding the unpredictable nature of magic.");
                            p.SendGump(new DialogueGump(p, philosophyModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, eddaModule));
                });

            // Option 4: Jasper's Dual Life as Scribe and Alchemist
            root.AddOption("I'm curious about your life as both a scribe and an alchemist.", 
                player => true,
                player =>
                {
                    DialogueModule dualLifeModule = new DialogueModule("My days are a curious blend of ink-stained parchments and bubbling cauldrons. I record the legends of Sosaria while simultaneously experimenting with potions that often have unexpected—and sometimes explosive—results. Would you like to learn about a recent discovery in my research, or the challenges I face balancing these dual callings?");
                    
                    dualLifeModule.AddOption("Tell me about a recent discovery.", 
                        p => true,
                        p =>
                        {
                            DialogueModule discoveryModule = new DialogueModule("Not long ago, I discovered an obscure recipe hidden within a crumbling manuscript. The potion was meant to reveal hidden truths, yet when brewed, it instead transformed mundane parchment into animated sketches of forgotten heroes! Miko helped me document each detail while Lady Isobel provided historical context—an unforgettable collaboration that deepened my resolve to master these arcane arts.");
                            p.SendGump(new DialogueGump(p, discoveryModule));
                        });
                    
                    dualLifeModule.AddOption("What challenges do you face daily?", 
                        p => true,
                        p =>
                        {
                            DialogueModule challengesModule = new DialogueModule("The challenges are many: deciphering ancient runes, mitigating volatile reactions in my lab, and balancing the expectations of the scholarly and the mystical. Every misstep, like the time my attempt at a truth serum resulted in a chorus of giggling vines, reminds me that magic is as unpredictable as it is wondrous.");
                            challengesModule.AddOption("That sounds overwhelming.", 
                                q => true,
                                q => q.SendGump(new DialogueGump(q, challengesModule)));
                            challengesModule.AddOption("Your passion is inspiring despite the setbacks.", 
                                q => true,
                                q => q.SendGump(new DialogueGump(q, CreateDialogueTree())));
                            p.SendGump(new DialogueGump(p, challengesModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, dualLifeModule));
                });

            // Option 5: Farewell
            root.AddOption("Farewell, Jasper. I must continue my journey.", 
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule("May your path be illuminated by both wisdom and wonder, traveler. Remember—each story and every failed experiment adds a new page to the grand chronicle of Sosaria.");
                    player.SendGump(new DialogueGump(player, farewellModule));
                });

            return root;
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
