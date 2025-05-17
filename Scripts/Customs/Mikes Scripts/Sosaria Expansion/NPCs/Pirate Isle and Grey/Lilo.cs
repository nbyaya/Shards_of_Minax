using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network; // Required for NetState

namespace Server.Mobiles
{
    [CorpseName("the remains of the discreet Lilo")]
    public class Lilo : BaseCreature
    {
        [Constructable]
		public Lilo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lilo";
            Body = 0x190; // Human male body

            // Set basic stats
            SetStr(90);
            SetDex(110);
            SetInt(100);
            SetHits(80);

            // Appearance items: a mix of urban rogue and dashing mariner styles
            AddItem(new FancyShirt() { Hue = 0x5A });
            AddItem(new LongPants() { Hue = 0x4B });
            AddItem(new Boots() { Hue = 0x501 });
            AddItem(new Cloak() { Hue = 0x455 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Lilo(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greeting = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greeting));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Lilo's initial greeting is polished and mysterious,
            // peppered with hints of secret lives and dangerous allegiances.
            DialogueModule greeting = new DialogueModule(
                "Ahoy there, traveler. I’m Lilo—the shadow upon the docks, the whisper in the wind. " +
                "By day I’m known as a rum-runner, ferrying covert messages to Cassian of Fawn and Mino of East Montor; " +
                "by night, I confide in Kel of Devil Guard, a scholar of ancient lore. " +
                "But between these worlds lies a secret deeper than the darkest tide… " +
                "What intrigues you this evening?"
            );

            // Option 1: Rum-running operations
            greeting.AddOption("Tell me about your rum-running operations.",
                player => true,
                player =>
                {
                    DialogueModule rumModule = new DialogueModule(
                        "The rum trade isn’t mere smuggling—it’s an art of subterfuge. " +
                        "I secure the finest, elusive spirits from hidden distilleries that only the daring dare speak of. " +
                        "Every bottle bears not only exotic flavor but also the burden of trust and risk. " +
                        "Would you like to hear about our clandestine drop-offs or the perils we face in these shadowed waters?"
                    );

                    rumModule.AddOption("I’d like to hear about the drop-offs.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dropOffModule = new DialogueModule(
                                "Our drop-offs are choreographed like a symphony in the dark. " +
                                "Cassian, ever the master of subterfuge in Fawn, signals via his enigmatic paintings that a shipment is in motion. " +
                                "Meanwhile, Mino in East Montor has marked secure rendezvous points known only to our inner circle. " +
                                "Do you admire the precision of our covert artistry, or does the danger unsettle you?"
                            );

                            dropOffModule.AddOption("I admire the precision.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));

                            dropOffModule.AddOption("It sounds too dangerous.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule dangerModule = new DialogueModule(
                                        "Danger, my friend, is the rhythm of our existence. " +
                                        "Each delivery is a dance with fate—a misstep could be your last. Yet, perfection is the only path to survival. " +
                                        "We leave no room for error."
                                    );
                                    pla.SendGump(new DialogueGump(pla, dangerModule));
                                });

                            pl.SendGump(new DialogueGump(pl, dropOffModule));
                        });

                    rumModule.AddOption("And what perils do you face?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule perilModule = new DialogueModule(
                                "Perils come as naturally as the tide. " +
                                "Ambushes by rival gangs, double-crosses from corrupt officials, and the ever-present specter of betrayal haunt our every move. " +
                                "I've skirted death more times than I care to recount, each encounter refining my resolve like a blade on the whetstone of fate. " +
                                "Would you like to hear of a specific narrow escape or how our network anticipates such threats?"
                            );

                            perilModule.AddOption("Tell me of a narrow escape.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule escapeModule = new DialogueModule(
                                        "I recall one stormy night when ambushes near Pirate Isle threatened to snatch away a critical shipment. " +
                                        "Cassian’s urgent coded signals alerted me; we weaved through fog-laden alleys and abandoned piers until, by the grace of a full moon, our pursuers lost our trail. " +
                                        "That night, every nerve was strung taut, and every decision mattered."
                                    );
                                    pla.SendGump(new DialogueGump(pla, escapeModule));
                                });

                            perilModule.AddOption("How do you stay ahead of such threats?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule networkModule = new DialogueModule(
                                        "Our network is our shield and sword. " +
                                        "Cassian’s art conceals intelligence, while Mino’s maps hide secret routes known only to the initiated. " +
                                        "And then there’s Kel—whose deep knowledge of ancient lore often foretells looming dangers. " +
                                        "Through relentless vigilance and perfected execution, we ensure that every risk is preempted. " +
                                        "Perfection in every move is not a luxury—it’s an obligation."
                                    );
                                    pla.SendGump(new DialogueGump(pla, networkModule));
                                });

                            pl.SendGump(new DialogueGump(pl, perilModule));
                        });

                    player.SendGump(new DialogueGump(player, rumModule));
                });

            // Option 2: Trusted contacts Cassian and Mino
            greeting.AddOption("Who are Cassian and Mino? What roles do they play?",
                player => true,
                player =>
                {
                    DialogueModule contactModule = new DialogueModule(
                        "Cassian and Mino are the silent heartbeat of our operation. " +
                        "Cassian, from Fawn, wields his brush as both art and cipher, encoding messages in vivid strokes that belie their true import. " +
                        "Mino, the shadow in East Montor, scouts and secures clandestine rendezvous—ensuring that no secret delivery is ever compromised. " +
                        "Their precision, much like my own, is uncompromising. Which of their roles would you like to explore further?"
                    );

                    contactModule.AddOption("Tell me more about Cassian.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cassianModule = new DialogueModule(
                                "Cassian is a paradox wrapped in pigment. " +
                                "In the midst of Fawn’s colorful art scene, his canvases hide intricate codes. " +
                                "Every brushstroke is deliberate, each hue a signal. His commitment to perfection mirrors my own—no detail is too small when lives hang in the balance."
                            );
                            pl.SendGump(new DialogueGump(pl, cassianModule));
                        });

                    contactModule.AddOption("And what of Mino?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule minoModule = new DialogueModule(
                                "Mino is our cartographer of the hidden world. " +
                                "Operating from East Montor, he transforms every forgotten alley and derelict warehouse into a secure haven. " +
                                "His acute sense for detail and unwavering drive ensure that when a message is sent, a safe sanctuary awaits."
                            );
                            pl.SendGump(new DialogueGump(pl, minoModule));
                        });

                    player.SendGump(new DialogueGump(player, contactModule));
                });

            // Option 3: Kel and his ancient lore
            greeting.AddOption("What can you tell me about Kel and the secrets he studies?",
                player => true,
                player =>
                {
                    DialogueModule kelModule = new DialogueModule(
                        "Kel of Devil Guard is a master of arcane wisdom—a keeper of forbidden chronicles and ancient prophecies. " +
                        "He deciphers texts said to have been authored by lost civilizations and cursed sorcerers alike. " +
                        "In his hands, the 'Codex of Shadows' whispers truths about realms beyond mortal ken. " +
                        "Would you like to know more about this manuscript or the mystical lore Kel unearths?"
                    );

                    kelModule.AddOption("What is the 'Codex of Shadows'?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule codexModule = new DialogueModule(
                                "The 'Codex of Shadows' is not merely a book but a doorway to forbidden knowledge. " +
                                "Its yellowed pages, etched with cryptic symbols, recount rituals that unlock hidden doorways—both physical and metaphysical. " +
                                "Kel believes this manuscript might even reveal how to harness the shadows themselves."
                            );
                            pl.SendGump(new DialogueGump(pl, codexModule));
                        });

                    kelModule.AddOption("Tell me about the lore Kel uncovers.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loreModule = new DialogueModule(
                                "Kel’s research spans myths of lunar rituals, ancient guardians, and prophecies foretelling the rise and fall of powers. " +
                                "Every discovery seems to nudge us closer to understanding a force older than time—one that many would kill to control. " +
                                "This knowledge, however, comes at a dire cost."
                            );
                            pl.SendGump(new DialogueGump(pl, loreModule));
                        });

                    player.SendGump(new DialogueGump(player, kelModule));
                });

            // Option 4: Lilo’s personal history and secret past
            greeting.AddOption("I’d like to know more about your own story—who are you, really?",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule(
                        "My life, like these troubled waters, runs deep and dark. " +
                        "I was born among the docks, where survival forged my mettle. But there’s more—a secret past not even the bravado of a rum-runner can conceal. " +
                        "I was once an elite assassin, trained to be flawless, driven by an unyielding quest for perfection. " +
                        "When my employers—the very ones I once trusted—turned their backs on me, I was left with nothing but betrayal. " +
                        "Now, I channel that unforgiving rage into every covert operation. " +
                        "Do you wish to delve into the details of my former life, or are you content with the mask I now wear?"
                    );

                    personalModule.AddOption("Reveal your secret past.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretPastModule = new DialogueModule(
                                "Very well… There was a time when I lived in the shadows not as a smuggler, but as a killer. " +
                                "In those days, every mission was a pursuit of perfection—a cold, calculated art where any mistake was met with swift retribution. " +
                                "I was driven by a singular purpose: vengeance against those who betrayed me. My former employers, powerful and duplicitous, believed they could discard me like yesterday’s cargo. " +
                                "They were wrong. Every scar, every sleepless night, became fuel for my relentless resolve."
                            );

                            secretPastModule.AddOption("How did you become that elite assassin?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule becomeAssassinModule = new DialogueModule(
                                        "I was forged in the crucible of betrayal. " +
                                        "Trained in the deadly arts, I learned that even the slightest error could be fatal—thus I strove for perfection in every strike. " +
                                        "No detail was overlooked, no target left unchallenged. In that unforgiving world, I was both hunter and hunted."
                                    );
                                    becomeAssassinModule.AddOption("And what happened to your employers?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule employersModule = new DialogueModule(
                                                "They thought themselves untouchable—until my blade and calculated vengeance found them. " +
                                                "Each betrayal etched a vow into my soul, a promise that I would never allow treachery to go unpunished. " +
                                                "I now pursue every lead, every whisper in the dark, to finally bring them to justice—even if it means sacrificing everything."
                                            );
                                            plb.SendGump(new DialogueGump(plb, employersModule));
                                        });
                                    becomeAssassinModule.AddOption("Your drive is unmistakable.",
                                        plb => true,
                                        plb => plb.SendGump(new DialogueGump(plb, CreateGreetingModule())));
                                    pla.SendGump(new DialogueGump(pla, becomeAssassinModule));
                                });

                            secretPastModule.AddOption("It seems that perfection is everything to you.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule perfectionModule = new DialogueModule(
                                        "Indeed. Perfection is not vanity—it is survival. " +
                                        "In the world I once inhabited, there was no room for error. " +
                                        "The smallest miscalculation meant death, so I honed my skills until every motion was flawless. " +
                                        "And if anyone dared stray from that standard, they would meet the same unforgiving fate I reserved for those who betrayed me."
                                    );
                                    pla.SendGump(new DialogueGump(pla, perfectionModule));
                                });

                            secretPastModule.AddOption("I can see the relentless drive in your eyes.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, secretPastModule));
                        });

                    personalModule.AddOption("I prefer not to delve into the darkness of your past.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));

                    player.SendGump(new DialogueGump(player, personalModule));
                });

            // Option 5: Exiting the conversation
            greeting.AddOption("I'm not interested in secrets at this time.",
                player => true,
                player =>
                {
                    DialogueModule endModule = new DialogueModule(
                        "As you wish, traveler. Sometimes the safest harbor is one of silence. " +
                        "Remember, should the lure of clandestine truths ever call to you, you know where to find me."
                    );
                    player.SendGump(new DialogueGump(player, endModule));
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
