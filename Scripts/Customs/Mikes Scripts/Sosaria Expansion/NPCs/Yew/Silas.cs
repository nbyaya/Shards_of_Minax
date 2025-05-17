using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network; // Needed for NetState

namespace Server.Mobiles
{
    public class Silas : BaseCreature
    {
        [Constructable]
        public Silas() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Silas";
            Body = 0x190; // Human male body

            // Set basic stats – a man of both physical and mental fortitude
            SetStr(100);
            SetDex(90);
            SetInt(120); // Elevated intellect to reflect his fortune-teller nature
            SetHits(100);

            // Appearance: mysterious and reserved – a rugged figure cloaked in omens
            AddItem(new Cloak() { Hue = 1100 });
            AddItem(new Boots() { Hue = 1100 });
            AddItem(new LongPants() { Hue = 1100 });
            AddItem(new Shirt() { Hue = 1100 });
            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.Random(0x203B, 5);
            HairHue = Utility.RandomHairHue();
        }

        public Silas(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule(
                "Greetings, seeker. I am Silas—a wanderer of fate, supplier of game to Bryn at Dawn, and, some whisper, a fortune-teller " +
                "who has bargained with destiny itself. They say I traded a fragment of my soul for uncanny knowledge. " +
                "What do you wish to know about my work, my visions, or the secrets that haunt these woods?"
            );

            // Option 1: Work for Bryn and life in Dawn
            greeting.AddOption("Tell me about your work for Bryn at Dawn.", 
                player => true,
                player =>
                {
                    DialogueModule brynModule = new DialogueModule(
                        "Bryn, the humble baker of Dawn, relies on me to bring forth the finest game and freshest meat from these haunted woods. " +
                        "Every venture is laced with omens and curious encounters. Would you prefer to hear of the creatures I hunt, " +
                        "or the tales of mystic encounters that have shaped my journey?"
                    );

                    brynModule.AddOption("I want to hear about the creatures and your hunts.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule huntModule = new DialogueModule(
                                "In the dim light of dusk, these woods come alive with both the mundane and the otherworldly. I've stalked great wild boars, " +
                                "elusive stags, and even sensed the silent tread of spectral hounds. On one fateful night, a stag with antlers aglow " +
                                "appeared as if summoned by the spirits. Would you like to know every detail of that mystic encounter?"
                            );

                            huntModule.AddOption("Yes, recount the tale of the glowing stag.",
                                plh => true,
                                plh =>
                                {
                                    DialogueModule stagModule = new DialogueModule(
                                        "Under a crescent moon and in a clearing where time itself seemed to pause, I beheld a stag. Its antlers shone " +
                                        "with an ethereal radiance, and its eyes—filled with sorrow and ancient wisdom—spoke silently of fate's design. " +
                                        "For one brief moment, I felt as though the forest itself had chosen to reveal a secret. Do you feel that the " +
                                        "magic of such a vision resonates with you?"
                                    );

                                    stagModule.AddOption("Indeed, the magic is palpable.",
                                        plc => true,
                                        plc =>
                                        {
                                            DialogueModule magicModule = new DialogueModule(
                                                "Magic courses through every living thing here, though it is often veiled in mystery. " +
                                                "I have seen wonders and horrors alike, each a reminder that the fabric of our world is far more complex " +
                                                "than it seems. Yet, not every revelation is a blessing—some secrets are best left unspoken."
                                            );
                                            plc.SendGump(new DialogueGump(plc, magicModule));
                                        });
                                    stagModule.AddOption("I remain skeptical of such omens.",
                                        plc => true,
                                        plc =>
                                        {
                                            DialogueModule skepticModule = new DialogueModule(
                                                "Skepticism is a wise guard against the unknown, yet even the most rational heart cannot ignore the inexplicable glow " +
                                                "of destiny when it manifests. The stag's light, however brief, challenges our notions of reality."
                                            );
                                            plc.SendGump(new DialogueGump(plc, skepticModule));
                                        });
                                    plh.SendGump(new DialogueGump(plh, stagModule));
                                });

                            huntModule.AddOption("No, describe the other dangerous creatures instead.",
                                plh => true,
                                plh =>
                                {
                                    DialogueModule dangerousModule = new DialogueModule(
                                        "Beyond the well-known beasts, the forest hides entities that defy mortal comprehension. Ghostly wolves with eyes like dying embers " +
                                        "roam silently, and wild dogs vanish into the shadows as if swallowed by the night itself. Their elusive forms serve as a " +
                                        "constant reminder that nature and the supernatural share an uneasy truce."
                                    );

                                    dangerousModule.AddOption("Tell me more about these spectral wolves.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule wolvesModule = new DialogueModule(
                                                "The spectral wolves appear without warning—fleeting glimpses of otherworldly sorrow. Their howls are not merely sounds, " +
                                                "but echoes of long-forgotten grief. They seem to herald changes in fate, their presence a silent omen of events yet to unfold."
                                            );
                                            plx.SendGump(new DialogueGump(plx, wolvesModule));
                                        });
                                    dangerousModule.AddOption("What of the vanishing wild dogs?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule dogsModule = new DialogueModule(
                                                "The wild dogs are as enigmatic as the shifting mists; they emerge and fade with such swiftness that one wonders if they " +
                                                "are real at all, or simply a manifestation of the forest’s many secrets. Their fleeting presence leaves behind a chill " +
                                                "that lingers in the air."
                                            );
                                            plx.SendGump(new DialogueGump(plx, dogsModule));
                                        });
                                    plh.SendGump(new DialogueGump(plh, dangerousModule));
                                });
                            pl.SendGump(new DialogueGump(pl, huntModule));
                        });

                    brynModule.AddOption("Tell me about your bond with Bryn and the Dawn community.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule communityModule = new DialogueModule(
                                "The community of Dawn is built on the simple yet profound values of trust, perseverance, and shared hope. " +
                                "Bryn, with his gentle laugh and steadfast nature, is like a beacon of warmth in the cold of night. " +
                                "Our conversations over freshly baked bread and savory stews reveal truths about life and fate that resonate deeply. " +
                                "Would you care to hear a personal memory that binds us?"
                            );

                            communityModule.AddOption("Yes, share a memory you have with Bryn.",
                                plm => true,
                                plm =>
                                {
                                    DialogueModule memoryModule = new DialogueModule(
                                        "I recall a bitter winter’s night when the world was blanketed in silence and snow. " +
                                        "Bryn’s smile broke through the gloom as we huddled by a roaring fire, speaking softly of omens and the " +
                                        "subtle language of nature. It was a time when even the smallest act—a shared loaf, a word of comfort—" +
                                        "became a beacon of hope against the encroaching dark."
                                    );
                                    memoryModule.AddOption("And what omens did you speak of?",
                                        pln => true,
                                        pln =>
                                        {
                                            DialogueModule fateModule = new DialogueModule(
                                                "We discussed the delicate balance of fate—how a single snowfall or the pattern of frost on a window " +
                                                "might foretell change. It was as if the very air whispered secrets of what was to come, a tapestry " +
                                                "woven by time and chance."
                                            );
                                            pln.SendGump(new DialogueGump(pln, fateModule));
                                        });
                                    plm.SendGump(new DialogueGump(plm, memoryModule));
                                });
                            communityModule.AddOption("I would rather know about the community’s values.",
                                plm => true,
                                plm =>
                                {
                                    DialogueModule valuesModule = new DialogueModule(
                                        "The people of Dawn hold dear the virtues of simplicity and kinship. " +
                                        "Every shared meal and quiet smile is a thread in the fabric of their collective destiny. " +
                                        "In a world shadowed by uncertainty, their unity stands as a quiet defiance against despair."
                                    );
                                    plm.SendGump(new DialogueGump(plm, valuesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, communityModule));
                        });

                    player.SendGump(new DialogueGump(player, brynModule));
                });

            // Option 2: Dark arts in Fawn and Cassian’s obsessions
            greeting.AddOption("I’ve heard whispers about Fawn’s eerie arts and Cassian’s dark obsessions. What do you make of it?",
                player => true,
                player =>
                {
                    DialogueModule fawnModule = new DialogueModule(
                        "Fawn is a realm where beauty and darkness entwine. Cassian, its troubled artist, has delved deep into secrets " +
                        "that border on the forbidden. His canvases are as much a window to his inner torment as they are a reflection " +
                        "of the abyss. Would you prefer to explore the depths of his work or to hear my own reservations about tampering with " +
                        "forces beyond our ken?"
                    );

                    fawnModule.AddOption("Delve into the depths of Cassian’s work.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cassianDeepModule = new DialogueModule(
                                "Cassian’s art is both mesmerizing and disturbing—an echo of a soul in torment. " +
                                "Late into the night, I have witnessed him hunched over canvases, his eyes reflecting visions of another realm. " +
                                "He speaks in riddles of voices and visions, as if drawing upon a well of forbidden knowledge. " +
                                "Do you believe that art can serve as a conduit to such hidden realms?"
                            );

                            cassianDeepModule.AddOption("Yes, art can reveal hidden truths.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule artTruthModule = new DialogueModule(
                                        "Indeed, art holds the power to unmask the unseen. Yet, Cassian's creations venture into a territory " +
                                        "where the cost of enlightenment may be far too steep—a realm where beauty is shadowed by an undercurrent " +
                                        "of peril."
                                    );
                                    plx.SendGump(new DialogueGump(plx, artTruthModule));
                                });
                            cassianDeepModule.AddOption("No, some mysteries should remain hidden.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule hiddenModule = new DialogueModule(
                                        "Sometimes, the unknown is best left undisturbed. The darkness in Cassian’s work may be a mirror to " +
                                        "the fragile boundaries between genius and madness—boundaries that are not meant to be crossed lightly."
                                    );
                                    plx.SendGump(new DialogueGump(plx, hiddenModule));
                                });
                            cassianDeepModule.AddOption("What drives him to such dark obsessions?",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule driveModule = new DialogueModule(
                                        "It seems that Cassian is haunted by a personal tragedy—a loss so profound that it drove him " +
                                        "to barter a piece of his own innocence for forbidden insight. His art may be the cry of a soul " +
                                        "seeking solace in the dark."
                                    );
                                    driveModule.AddOption("Has he ever spoken of this past in detail?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule pastModule = new DialogueModule(
                                                "Rarely does he speak of it openly, but in a moment of vulnerability, he hinted at an event " +
                                                "that shattered his world. A memory too painful to relive, yet it fuels the passion behind his art."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, pastModule));
                                        });
                                    driveModule.AddOption("Perhaps his inner torment is reflected in his art.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule innerModule = new DialogueModule(
                                                "Your insight is keen. Often, the darkness one bears within is channeled into creative expression— " +
                                                "a testimony to the fragile balance between light and shadow within the human soul."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, innerModule));
                                        });
                                    ply.SendGump(new DialogueGump(ply, driveModule));
                                });
                            pl.SendGump(new DialogueGump(pl, cassianDeepModule));
                        });

                    fawnModule.AddOption("Express your personal concerns about these dark pursuits.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule personalModule = new DialogueModule(
                                "My heart is heavy when I ponder the consequences of delving too deeply into forbidden lore. " +
                                "I have seen visions that warn of an imbalance—of fates disrupted by relentless curiosity. " +
                                "While Cassian chases the allure of the abyss, I remain cautious, for I have borne the price of " +
                                "such pursuits in my own life."
                            );

                            personalModule.AddOption("What visions have you seen?",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule visionsModule = new DialogueModule(
                                        "In the quiet of night, I have dreamt of shattered mirrors and fractured time. " +
                                        "I witnessed scenes of converging destinies—leaves inscribed with names and dates falling like rain, " +
                                        "each a silent chronicle of lives intertwined with fate. These visions speak of both despair and hope."
                                    );
                                    visionsModule.AddOption("Tell me more about these dreams.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule dreamsModule = new DialogueModule(
                                                "I have seen a twilight realm where past and future merge, where each decision ripples " +
                                                "into the fabric of destiny. It is a place both beautiful and terrifying—an invitation to " +
                                                "change that comes at a cost."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, dreamsModule));
                                        });
                                    visionsModule.AddOption("Are these visions a curse upon you?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule curseModule = new DialogueModule(
                                                "Perhaps they are a curse—or a burden of insight. I once bargained with fate itself, trading " +
                                                "a fragment of my soul for this uncanny clarity. Every vision reminds me of that sacrifice, " +
                                                "a price that isolates me even as it guides me."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, curseModule));
                                        });
                                    ply.SendGump(new DialogueGump(ply, visionsModule));
                                });

                            personalModule.AddOption("Do you regret your bargain with fate?",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule regretModule = new DialogueModule(
                                        "Regret is a shadow that flits at the edge of memory. There are moments when the loss weighs heavily on me, " +
                                        "yet I cannot deny the clarity it has granted. It is a trade-off—sacrifice for the sake of vision."
                                    );
                                    regretModule.AddOption("That is a heavy price indeed.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule heavyModule = new DialogueModule(
                                                "Indeed, the cost is steep. But without that sacrifice, I would be blind to the intricate dance of fate " +
                                                "that unfolds around us."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, heavyModule));
                                        });
                                    regretModule.AddOption("Perhaps the gain outweighs the loss.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule gainModule = new DialogueModule(
                                                "Every glimpse of the future, every whispered secret of destiny, is a testament to the price I paid. " +
                                                "It is a burden I bear, yet it grants me the rare privilege of seeing beyond the veil."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, gainModule));
                                        });
                                    ply.SendGump(new DialogueGump(ply, regretModule));
                                });

                            pl.SendGump(new DialogueGump(pl, personalModule));
                        });

                    fawnModule.AddOption("What is your own destiny, Silas?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule destinyModule = new DialogueModule(
                                "Ah, destiny—a tapestry of shadows and light, woven by choices and omens. I have seen futures where the paths of many converge " +
                                "and diverge like the twisting branches of an ancient tree. My own journey is steeped in mystery and sacrifice. " +
                                "Would you like to hear of the visions that guide me, or the price I paid to glimpse the unknown?"
                            );

                            destinyModule.AddOption("Tell me about the visions that guide you.",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule guideModule = new DialogueModule(
                                        "My visions are subtle yet relentless—a murmur in the rustle of leaves, a glimmer in the dark. They come as fleeting whispers, " +
                                        "each hinting at the hidden threads of fate that bind us all. In one vision, I saw a convergence of lives, where every small act " +
                                        "rippled into destiny. It is a reminder that even the smallest choice can alter the course of history."
                                    );
                                    guideModule.AddOption("What did that convergence look like?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule convergeModule = new DialogueModule(
                                                "I saw a cascade of falling leaves, each inscribed with names and dates—a record of lives intertwined in an endless dance. " +
                                                "Leaders rose, empires fell, and love blossomed amidst chaos. It was both a dirge and a hymn to the resilience of the human spirit."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, convergeModule));
                                        });
                                    guideModule.AddOption("Do these visions ever change your course?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule changeModule = new DialogueModule(
                                                "They guide me like stars in a twilight sky. Though the visions inform my path, I choose each step with careful deliberation, " +
                                                "balancing fate with free will. The future is not set in stone, but a fluid tapestry that I strive to understand."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, changeModule));
                                        });
                                    ply.SendGump(new DialogueGump(ply, guideModule));
                                });

                            destinyModule.AddOption("What sacrifices have you made for this insight?",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule sacrificeModule = new DialogueModule(
                                        "In my relentless pursuit of hidden truth, I bargained with fate—trading a fragment of my soul for the gift of foresight. " +
                                        "It is said that such a price can never be fully repaid. I have lost a part of myself, yet gained the clarity to see " +
                                        "beyond the veil. Do you wonder if that sacrifice still haunts me?"
                                    );
                                    sacrificeModule.AddOption("Do you regret this sacrifice?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule noRegretModule = new DialogueModule(
                                                "Regret is an ever-flickering shadow. I bear the cost, yet without it, I would be blind to the intricate web " +
                                                "of destiny that surrounds us. The pain is real, but so too is the wisdom it has bestowed upon me."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, noRegretModule));
                                        });
                                    sacrificeModule.AddOption("Is any part of you left untouched by this bargain?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule untouchedModule = new DialogueModule(
                                                "Curious indeed. Though I sacrificed a part of my soul, I remain whole enough to feel, to dream, and to share " +
                                                "the visions I gather. That loss has made me a guardian of secrets—a bridge between the mundane and the mysterious."
                                            );
                                            plyx.SendGump(new DialogueGump(plyx, untouchedModule));
                                        });
                                    ply.SendGump(new DialogueGump(ply, sacrificeModule));
                                });

                            destinyModule.AddOption("I must say, your fate is as enigmatic as you are.",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule enigmaticModule = new DialogueModule(
                                        "Enigmatic, indeed. My journey is a mosaic of unspoken truths and quiet revelations—a path reserved for those who dare to seek " +
                                        "knowledge beyond the veil. Each step forward is both a revelation and a mystery in itself."
                                    );
                                    ply.SendGump(new DialogueGump(ply, enigmaticModule));
                                });
                            pl.SendGump(new DialogueGump(pl, destinyModule));
                        });

                    fawnModule.AddOption("I’m content for now. Thank you for sharing your wisdom, Silas.",
                        pl => true,
                        pl =>
                        {
                            player.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    
                    player.SendGump(new DialogueGump(player, fawnModule));
                });

            // Option 3: Farewell / End conversation
            greeting.AddOption("I’m just passing by. Farewell, Silas.",
                player => true,
                player =>
                {
                    // End conversation by returning to the greeting (or closing the gump)
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
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
