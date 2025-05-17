using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Yorn")]
    public class Yorn : BaseCreature
    {
        [Constructable]
		public Yorn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Yorn";
            Body = 0x190; // Standard humanoid body

            // Set up basic stats (adjust as needed)
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance and attire for a bathhouse keeper
            AddItem(new FancyShirt() { Hue = 2750 });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new FullApron() { Hue = 1100 });

            // Additional initialization can go here
        }

        public Yorn(Serial serial) : base(serial) { }

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
                "Welcome, traveler. I am Yorn—a keeper of baths and bearer of hidden wisdom. " +
                "My body bears the scars of a golem’s fury, and I have learned the arts of healing " +
                "from Sir Thomund, exchanged care tips with Heidi of Dawn, and shared mining lore " +
                "with Crag from the Devil Guard. Yet, beneath these humble occupations lies a secret " +
                "past: long ago, I walked the path of a fortune-teller, trading part of my soul for " +
                "insight into the future. What facet of my tale would you like to explore?"
            );

            // Option 1: The Golem Wound
            greeting.AddOption("Tell me about the golem wound you endured.",
                player => true,
                player =>
                {
                    DialogueModule golemModule = new DialogueModule(
                        "The golem was not merely a behemoth of stone—it was a manifestation of unruly, " +
                        "primordial magic. I remember the shock and pain, as its crushing force etched " +
                        "a wound not only upon my flesh but deep into my soul. This encounter opened my eyes " +
                        "to the delicate interplay between suffering and strength. Do you wish to know " +
                        "how I found recovery, or shall I describe the nature of that fearsome entity?"
                    );

                    golemModule.AddOption("How did you recover from your injury?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule recoveryModule = new DialogueModule(
                                "My recovery was a long, introspective journey. Under the watchful eye of Sir " +
                                "Thomund of Castle British, I embraced ancient herbal infusions and meditative " +
                                "rituals. In these very baths, warmed by secret spring water and enriched with " +
                                "rare botanicals, the bond between body and spirit was slowly restored."
                            );
                            pl.SendGump(new DialogueGump(pl, recoveryModule));
                        });

                    golemModule.AddOption("What can you tell me about the golem itself?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule golemDetailModule = new DialogueModule(
                                "I saw in its eyes a burning intensity—a reminder that even stone may harbor " +
                                "the spark of wild magic. The golem was both a creature of destruction and a " +
                                "harbinger, hinting at darker mysteries in the realms beyond our own."
                            );
                            pl.SendGump(new DialogueGump(pl, golemDetailModule));
                        });

                    player.SendGump(new DialogueGump(player, golemModule));
                });

            // Option 2: Healing Lore with Sir Thomund
            greeting.AddOption("I hear you discuss healing lore with Sir Thomund. Tell me more.",
                player => true,
                player =>
                {
                    DialogueModule thomundModule = new DialogueModule(
                        "Indeed, Sir Thomund is far more than a warrior; he is a sage who has unlocked " +
                        "the secrets of restorative arts. Together, we have delved into archaic manuscripts " +
                        "and discovered rituals that bind the physical and the spiritual. Would you like " +
                        "to uncover our sacred healing rituals or understand the philosophy that guides us?"
                    );

                    thomundModule.AddOption("Reveal your secret healing rituals.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule ritualsModule = new DialogueModule(
                                "Within the gentle warmth of the bathhouse, we combine rare herbs like sage " +
                                "and mint with a drop of our own life's essence. As the water steams, incantations " +
                                "echo softly, invoking blessings from unseen guardians. Each gesture is as precise " +
                                "as a ritual passed down through time."
                            );
                            pl.SendGump(new DialogueGump(pl, ritualsModule));
                        });

                    thomundModule.AddOption("What is the philosophy behind these rituals?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loreModule = new DialogueModule(
                                "Our philosophy is one of balance. Healing, to us, is the art of renewing both the " +
                                "body and spirit. Every wound teaches a lesson, and every scar is a testament to " +
                                "resilience. Sir Thomund taught me that true restoration comes when we acknowledge " +
                                "our suffering and, in doing so, open the way for profound transformation."
                            );
                            pl.SendGump(new DialogueGump(pl, loreModule));
                        });

                    player.SendGump(new DialogueGump(player, thomundModule));
                });

            // Option 3: Care Tips with Heidi
            greeting.AddOption("Share the insights you have learned from your friend Heidi of Dawn.",
                player => true,
                player =>
                {
                    DialogueModule heidiModule = new DialogueModule(
                        "Heidi’s gentle wisdom is like the first light of dawn. In her quiet world of natural " +
                        "remedies, she crafts tonics from wild chamomile and thyme, and extols the virtues of mindful " +
                        "living. Would you like to hear about one of her specific remedies, or shall I recount the guiding " +
                        "principles she holds dear?"
                    );

                    heidiModule.AddOption("Describe one of her specific remedies.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule remedyModule = new DialogueModule(
                                "Just a fortnight ago, Heidi presented me with a tonic brewed from dew-drenched chamomile " +
                                "and hand-picked thyme. This elixir, with a trace of honey, not only soothes minor wounds " +
                                "but also eases the burden of the spirit—a gentle reminder that healing can be as simple " +
                                "as nature itself."
                            );
                            pl.SendGump(new DialogueGump(pl, remedyModule));
                        });

                    heidiModule.AddOption("Explain her general care principles.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule carePrinciplesModule = new DialogueModule(
                                "For Heidi, care is an act of mindfulness. She preaches that true well-being comes from " +
                                "pausing to savor each breath, nourishing the body with wholesome foods, and giving space " +
                                "to the quiet cadence of the natural world. Every ritual, no matter how small, is a step " +
                                "toward inner harmony."
                            );
                            pl.SendGump(new DialogueGump(pl, carePrinciplesModule));
                        });

                    player.SendGump(new DialogueGump(player, heidiModule));
                });

            // Option 4: Mining Lore with Crag
            greeting.AddOption("What tales do you exchange with Crag about the ancient mines?",
                player => true,
                player =>
                {
                    DialogueModule cragModule = new DialogueModule(
                        "Crag, though rough of voice, speaks with the poetry of the earth itself. " +
                        "Deep within the Devil Guard mines, he has uncovered the secrets of stone and time. " +
                        "Would you hear one of his epic tales, or would you prefer to learn about the ancient " +
                        "mining techniques he holds sacred?"
                    );

                    cragModule.AddOption("Relate one of Crag's epic tales.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule epicTaleModule = new DialogueModule(
                                "Crag once told me of a luminous quartz vein that pulsed with the heartbeat of the earth—" +
                                "a moment when miners claimed to hear the ancient song of creation itself. It was a flash " +
                                "of insight that spoke of the land’s immortal spirit."
                            );
                            pl.SendGump(new DialogueGump(pl, epicTaleModule));
                        });

                    cragModule.AddOption("What mining techniques does he honor?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule methodModule = new DialogueModule(
                                "He preaches respectful extraction—a method where each strike into the earth " +
                                "is a respectful caress upon the ancient stone. Every tool is chosen with deliberation, " +
                                "and every movement is a ritual acknowledging the legacy of the land."
                            );
                            pl.SendGump(new DialogueGump(pl, methodModule));
                        });

                    player.SendGump(new DialogueGump(player, cragModule));
                });

            // Option 5: The Secret Past—Revealing the Fortune-Teller
            greeting.AddOption("I sense a mysterious aura about you... Are you really only a bathhouse keeper?",
                player => true,
                player =>
                {
                    DialogueModule secretModule = new DialogueModule(
                        "Your intuition is sharp. Beneath this humble exterior lies a hidden past—a time when I " +
                        "wandered as a fortune-teller, deciphering destinies and unravelling the tapestry of fate. " +
                        "They say I traded a portion of my soul to glimpse the secrets of the cosmos. What would " +
                        "you like to know about this clandestine chapter of my life?"
                    );

                    secretModule.AddOption("What do you mean by trading part of your soul?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule soulModule = new DialogueModule(
                                "In a time steeped in desperation and longing for answers, I made a pact with an ancient, " +
                                "nameless force. In exchange for visions that pierced the veil of time, I relinquished a fragment " +
                                "of my very essence. It was a sacrifice that has left me with both profound insight and lingering sorrow."
                            );
                            
                            soulModule.AddOption("And what visions has that sacrifice granted you?",
                                pb => true,
                                pb =>
                                {
                                    DialogueModule visionsModule = new DialogueModule(
                                        "The visions flow like an endless river—fleeting yet undeniable. I have seen empires " +
                                        "rise and crumble, lost loves rekindled, and destinies forged in the crucible of fate. " +
                                        "Each glimpse is a cryptic lesson, urging me to share this wisdom with those brave enough to listen."
                                    );
                                    
                                    visionsModule.AddOption("Do you see a destiny for me in those visions?",
                                        pc => true,
                                        pc =>
                                        {
                                            DialogueModule destinyModule = new DialogueModule(
                                                "I sense in you a latent power—a crossroads where choices ripple into eternity. " +
                                                "The future is not set, but I glimpse both peril and promise in your path. Your journey " +
                                                "will be marked by moments of clarity and shadows of uncertainty. Trust your intuition, " +
                                                "for it is the beacon that will guide you through the mists of tomorrow."
                                            );
                                            
                                            destinyModule.AddOption("Tell me more about this crossroads of fate.",
                                                pd => true,
                                                pd =>
                                                {
                                                    DialogueModule crossroadsModule = new DialogueModule(
                                                        "Imagine a place where every step alters the weave of destiny—a juncture " +
                                                        "where the past, present, and future converge. There lie choices that could lead " +
                                                        "to greatness or plunge one into despair. Your heart, though burdened, holds the key " +
                                                        "to unlocking these mysteries."
                                                    );
                                                    pd.SendGump(new DialogueGump(pd, crossroadsModule));
                                                });
                                            
                                            destinyModule.AddOption("I need time to ponder this fate.",
                                                pd => true,
                                                pd =>
                                                {
                                                    pd.SendMessage("Yorn's eyes glow with quiet understanding as he offers a slight nod.");
                                                });
                                            
                                            pc.SendGump(new DialogueGump(pc, destinyModule));
                                        });
                                    
                                    visionsModule.AddOption("Do you ever regret making that sacrifice?",
                                        pc => true,
                                        pc =>
                                        {
                                            DialogueModule regretModule = new DialogueModule(
                                                "Regret is a shadow that dwells in the corridors of the soul. There are nights " +
                                                "when the echo of what I lost reverberates in the silence. Yet, each vision " +
                                                "and every truth I've gleaned has made the burden bearable—a bittersweet testament " +
                                                "to the price of knowledge."
                                            );
                                            
                                            regretModule.AddOption("It sounds like a heavy burden.",
                                                pd => true,
                                                pd =>
                                                {
                                                    pd.SendMessage("Yorn murmurs, 'Indeed, it is a burden I carry with both sorrow and pride.'");
                                                });
                                            
                                            regretModule.AddOption("And yet, your insight is remarkable.",
                                                pd => true,
                                                pd =>
                                                {
                                                    pd.SendMessage("A faint smile crosses Yorn's features, as if acknowledging a secret victory.");
                                                });
                                            
                                            pc.SendGump(new DialogueGump(pc, regretModule));
                                        });
                                    
                                    pb.SendGump(new DialogueGump(pb, visionsModule));
                                });
                            
                            soulModule.AddOption("Do you ever wish you could reclaim what you lost?",
                                pb => true,
                                pb =>
                                {
                                    DialogueModule reclaimModule = new DialogueModule(
                                        "There are moments in the stillness of night when the emptiness echoes deeply. " +
                                        "But I have learned that every loss births a new insight, and every sacrifice carves " +
                                        "the path to wisdom. To reclaim what is lost is to forsake the growth that follows."
                                    );
                                    
                                    reclaimModule.AddOption("Your resolve is truly admirable.",
                                        pc => true,
                                        pc =>
                                        {
                                            pc.SendMessage("Yorn nods slowly, his eyes reflecting both pain and enlightenment.");
                                        });
                                    
                                    pb.SendGump(new DialogueGump(pb, reclaimModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, soulModule));
                        });

                    secretModule.AddOption("What visions guide you today?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule guidanceModule = new DialogueModule(
                                "In the murmur of the bathhouse steam and the flickering candlelight, visions come " +
                                "and go—fleeting flashes of what may yet be. I see whispers of change, unspoken promises " +
                                "and the delicate balance between hope and despair."
                            );
                            
                            guidanceModule.AddOption("Can you foretell my future?",
                                pb => true,
                                pb =>
                                {
                                    DialogueModule futureModule = new DialogueModule(
                                        "I peer into the swirling mists of fate and see many possibilities—none are absolute, " +
                                        "only opportunities waiting for your own hand to shape them. The future is a canvas " +
                                        "splashed with uncertainty and hope alike."
                                    );
                                    
                                    futureModule.AddOption("I appreciate your insight.",
                                        pc => true,
                                        pc =>
                                        {
                                            pc.SendMessage("Yorn's eyes glimmer with a reserved satisfaction, as if sharing a precious secret.");
                                        });
                                    
                                    pb.SendGump(new DialogueGump(pb, futureModule));
                                });
                            
                            guidanceModule.AddOption("What must I do to shape my destiny?",
                                pb => true,
                                pb =>
                                {
                                    DialogueModule shapeModule = new DialogueModule(
                                        "Destiny is not a fixed road, but a path you carve with every decision. Embrace uncertainty, " +
                                        "listen to the quiet voice within, and dare to step beyond what is safe. Each step is a seed " +
                                        "from which your future blossoms."
                                    );
                                    
                                    shapeModule.AddOption("I will remember your words.",
                                        pc => true,
                                        pc =>
                                        {
                                            pc.SendMessage("Yorn's soft, mysterious nod speaks volumes in the silence.");
                                        });
                                    
                                    pb.SendGump(new DialogueGump(pb, shapeModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, guidanceModule));
                        });

                    secretModule.AddOption("I must leave you to your quiet contemplations.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("Yorn's expression grows pensive as the unspoken weight of destiny lingers in the air.");
                        });

                    player.SendGump(new DialogueGump(player, secretModule));
                });

            // Option 6: End conversation politely
            greeting.AddOption("Thank you, Yorn. Your words illuminate more than they conceal.",
                player => true,
                player =>
                {
                    player.SendMessage("Yorn offers a reserved smile, his eyes hinting at mysteries known only to the silent stars.");
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
