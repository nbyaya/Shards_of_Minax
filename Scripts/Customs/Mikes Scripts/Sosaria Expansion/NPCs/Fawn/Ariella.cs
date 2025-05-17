using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remnants of a shattered canvas")]
    public class Ariella : BaseCreature
    {
        [Constructable]
		public Ariella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ariella, the Dream Painter";
            Body = 0x191; // Human body

            // Stats
            SetStr(90);
            SetDex(80);
            SetInt(120);
            SetHits(100);

            // Appearance
            AddItem(new Robe() { Hue = 2650 });
            AddItem(new TricorneHat() { Hue = 2750 });
            AddItem(new Sandals() { Hue = 2850 });
        }

        public Ariella(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Ah, welcome traveler. I am Ariella, the dream painter—whose gentle hand captures visions both wondrous and woeful. " +
                "My art is a tapestry woven from dreams, healing, and deep sorrow. What part of my story speaks to you today?"
            );
            
            // Branch: Art & Visions
            greeting.AddOption("Tell me about your art and the visions that inspire you.", 
                player => true, 
                player =>
                {
                    DialogueModule artModule = new DialogueModule(
                        "My canvases are no mere collection of colors—they are echoes of a realm where life and death intertwine. " +
                        "I capture visions of fading light, of souls adrift between hope and despair. At times, Elena the Archivist insists these images " +
                        "unveil secrets too perilous for mortal eyes. Which aspect intrigues you most?"
                    );
                    
                    artModule.AddOption("What visions haunt your creations?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule visionModule = new DialogueModule(
                                "In the quiet of midnight, when the veil between worlds grows thin, I see the dying—faces etched in sorrow and grace. " +
                                "One painting, 'The Fading Echo', mirrors these fleeting images. Would you like to know how one fateful night birthed these visions?"
                            );
                            
                            visionModule.AddOption("Tell me about that fateful night.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule nightModule = new DialogueModule(
                                        "It was a tempestuous night when the boundaries blurred. I witnessed an endless procession of lost souls, " +
                                        "each bearing the weight of unfulfilled dreams. One spectral visage, gentle yet desperate, called out for salvation. " +
                                        "That moment changed me forever. How do you feel when fate intervenes so starkly?"
                                    );
                                    
                                    nightModule.AddOption("I feel awe mixed with sorrow.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    nightModule.AddOption("Such moments can only deepen one's melancholy.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, nightModule));
                                });
                            
                            visionModule.AddOption("I sense hope even in your darkness.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule hopeModule = new DialogueModule(
                                        "Hope, indeed, is the fragile spark amid despair. Even as I see the dying, I also witness the promise of rebirth. " +
                                        "Each healed soul, each act of compassion, ignites a tiny flame against the vast darkness. Would you like to hear " +
                                        "how I nurture that hope through my art and healing?"
                                    );
                                    
                                    hopeModule.AddOption("Yes, share how you nurture hope.",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule nurtureModule = new DialogueModule(
                                                "I wander from town to town, offering gentle healing and solace. My methods blend secret herbal lore, " +
                                                "quiet incantations, and heartfelt empathy. In every tender act, I strive to mend not just the body, but the spirit. " +
                                                "Do you believe that such acts can heal even the deepest wounds?"
                                            );
                                            
                                            nurtureModule.AddOption("Absolutely, healing is a profound art.",
                                                plya => true,
                                                plya => plya.SendGump(new DialogueGump(plya, CreateGreetingModule())));
                                            nurtureModule.AddOption("Sometimes even the smallest spark can light the darkness.",
                                                plya => true,
                                                plya => plya.SendGump(new DialogueGump(plya, CreateGreetingModule())));
                                            
                                            ply.SendGump(new DialogueGump(ply, nurtureModule));
                                        });
                                    
                                    hopeModule.AddOption("I fear hope might be as fleeting as the visions you see.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, hopeModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, visionModule));
                        });
                    
                    artModule.AddOption("What secrets lie hidden in your brushstrokes?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "Every brushstroke carries a secret—a silent ode to forbidden lore and veiled prophecies. " +
                                "I often confer with Kel at Devil Guard, who deciphers the ancient symbols interwoven with my work. " +
                                "Shall I recount one such clandestine encounter?"
                            );
                            
                            secretModule.AddOption("Yes, please tell me about your encounter with Kel.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule kelModule = new DialogueModule(
                                        "One twilight, as shadows danced upon the walls, Kel arrived with an archaic scroll. " +
                                        "In hushed tones, he explained that a recurring motif in my paintings foretold calamity—a delicate warning " +
                                        "enshrouded in metaphor. Do you feel the weight of such omens?"
                                    );
                                    
                                    kelModule.AddOption("The balance of art and warning is fascinating.",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    kelModule.AddOption("Perhaps some secrets are best left untouched.",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, kelModule));
                                });
                            
                            secretModule.AddOption("No, let those secrets remain veiled.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, secretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, artModule));
                });
            
            // Branch: Elena's Critique
            greeting.AddOption("I heard Elena the Archivist finds your paintings unsettling. Why is that?",
                player => true,
                player =>
                {
                    DialogueModule elenaInquiryModule = new DialogueModule(
                        "Elena, ever the meticulous scholar, insists my work is more than art—it is a mirror of forbidden truths. " +
                        "She claims my canvases echo the sorrow of a healer’s past failure and reveal secrets that should perhaps remain buried. " +
                        "Would you like to hear how she interprets these mysteries?"
                    );
                    
                    elenaInquiryModule.AddOption("Yes, what secrets does she see?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "She speaks in hushed tones of lost rituals and curses woven into each pigment. To her, my work carries the gentle anguish of a healer who once failed, a burden that now whispers through every color. " +
                                "Do you think such sorrow can be both a curse and a muse?"
                            );
                            
                            secretModule.AddOption("Sorrow can inspire profound beauty.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            secretModule.AddOption("It is dangerous to dwell in such despair.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, secretModule));
                        });
                    
                    elenaInquiryModule.AddOption("No, I prefer to admire your art without its burdens.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, elenaInquiryModule));
                });
            
            // Branch: Jasper's Influence
            greeting.AddOption("I’m curious about your meetings with Jasper the Scribe. How does he influence your work?",
                player => true,
                player =>
                {
                    DialogueModule jasperModule = new DialogueModule(
                        "Jasper, the eloquent chronicler, challenges me to unearth the deeper symbolism hidden within my dreams. " +
                        "Our conversations are a dance of metaphor and memory, pushing me to reveal layers of emotion I once concealed. " +
                        "Shall I share one of our recent debates?"
                    );
                    
                    jasperModule.AddOption("Yes, recount your latest discussion.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule discussionModule = new DialogueModule(
                                "We recently discussed shattered mirrors—a motif for the fragmentation of self and memory. Jasper argued each shard held a unique recollection of regret, while I saw them as portals to lost hope. " +
                                "Do you feel that our inner selves can be as fragmented as these reflections?"
                            );
                            
                            discussionModule.AddOption("Indeed, our memories can be scattered and multifaceted.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            discussionModule.AddOption("I prefer a unified view of the self.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, discussionModule));
                        });
                    
                    jasperModule.AddOption("What makes his perspective so distinct?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule insightModule = new DialogueModule(
                                "Jasper weaves together history, myth, and raw emotion in a tapestry that challenges my own perceptions. " +
                                "He recently mentioned a dream where I, as a healer, stood amidst grieving faces—a vivid reminder of my past failure. " +
                                "Can you imagine carrying such a burden?"
                            );
                            
                            insightModule.AddOption("That burden must be overwhelming.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            insightModule.AddOption("Perhaps it fuels your creative spirit.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, insightModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, jasperModule));
                });
            
            // Branch: Secret Healer Past
            greeting.AddOption("Your gentle demeanor hints at a deeper sorrow. Tell me about your hidden past as a healer.",
                player => true,
                player =>
                {
                    DialogueModule healerModule = new DialogueModule(
                        "Ah, you perceive well. Beneath the guise of an artist lies a soul once devoted to healing—a path marred by visions of the dying and a tragic failure that haunts me still. " +
                        "I wandered from town to town, easing the suffering of others, even as the memory of loss clung to me like a specter. " +
                        "What would you like to know about those days?"
                    );
                    
                    healerModule.AddOption("Tell me about your days as a healer.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule daysModule = new DialogueModule(
                                "In my healing days, I traveled to the farthest reaches of Sosaria. I administered remedies crafted from rare herbs, whispered ancient incantations, " +
                                "and offered solace to those on the brink of despair. Yet, with every life I saved, I was reminded of the one soul I could not rescue. " +
                                "Would you care to hear a specific tale of my healing?"
                            );
                            
                            daysModule.AddOption("Yes, share a tale of your healing.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule taleModule = new DialogueModule(
                                        "There was a humble farmer stricken by a mysterious fever. I stayed by his side day and night, my heart heavy with hope. " +
                                        "In the silent hours, a vision of his young daughter—her eyes void of life—haunted me, a stark reminder of my greatest failure. " +
                                        "That loss still echoes in every healing touch I offer. Does this sorrow not stir something within you?"
                                    );
                                    
                                    taleModule.AddOption("It is truly heartbreaking.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    taleModule.AddOption("Perhaps such tragedy refines the soul.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, taleModule));
                                });
                            
                            daysModule.AddOption("No, tell me about your healing methods instead.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule methodsModule = new DialogueModule(
                                        "My methods were as secretive as they were gentle. I combined the lore of forgotten herbal remedies with the soft murmur of incantations " +
                                        "passed down through generations. Each treatment was a ritual—a quiet communion with the forces that govern life and death. " +
                                        "Do you believe that healing is as much an art as it is a science?"
                                    );
                                    
                                    methodsModule.AddOption("Yes, healing is a profound art.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    methodsModule.AddOption("I see it as nature’s simple gift.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, methodsModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, daysModule));
                        });
                    
                    healerModule.AddOption("How do you cope with the visions of the dying?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule visionsModule = new DialogueModule(
                                "The visions of the dying are a constant reminder of all I lost. Each image—the gentle eyes, the silent pleas—etch themselves into my soul. " +
                                "To cope, I retreat into quiet rituals: a candle lit at dawn, whispered prayers, and long walks under the moonlit sky. " +
                                "These small acts bring me fleeting solace. Would you like to learn about my personal rituals of mourning and hope?"
                            );
                            
                            visionsModule.AddOption("Yes, share your rituals.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule ritualsModule = new DialogueModule(
                                        "Every morning, before the world awakens, I gather a single bloom from a hidden glen and light a candle in its honor. " +
                                        "I whisper words of gratitude and lament, offering a prayer to both the departed and the living. " +
                                        "These rituals, though simple, are my way of honoring those lost and mending my fractured heart. " +
                                        "Do you find that such quiet acts can heal the deepest wounds?"
                                    );
                                    
                                    ritualsModule.AddOption("Indeed, even the smallest ritual can restore hope.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    ritualsModule.AddOption("I wonder if time alone can mend such scars.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, ritualsModule));
                                });
                            
                            visionsModule.AddOption("No, the burden of those visions is beyond words.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, visionsModule));
                        });
                    
                    healerModule.AddOption("I understand your burden, though it must be a lonely path.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, healerModule));
                });
            
            // Branch: Personal Reflections
            greeting.AddOption("Is there anything else you'd share about your life and art?",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule(
                        "Every moment of my life adds a brushstroke to the canvas of my soul. I am gentle, secretive, and haunted by memories that whisper in the silence. " +
                        "What more would you like to uncover about my journey?"
                    );
                    
                    personalModule.AddOption("Tell me more about your personal journey.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule journeyModule = new DialogueModule(
                                "I was born beneath a weeping sky, my early days filled with wonder and quiet sorrow. " +
                                "The loss of a cherished soul set me on this wandering path—one of healing, art, and endless introspection. " +
                                "Do you feel that our past shapes the art we create?"
                            );
                            
                            journeyModule.AddOption("Yes, our past is the soil from which art grows.",
                                ply => true,
                                ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                            journeyModule.AddOption("I prefer to forge my own destiny regardless of the past.",
                                ply => true,
                                ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, journeyModule));
                        });
                    
                    personalModule.AddOption("Perhaps some secrets are best left in the shadows.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, personalModule));
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
