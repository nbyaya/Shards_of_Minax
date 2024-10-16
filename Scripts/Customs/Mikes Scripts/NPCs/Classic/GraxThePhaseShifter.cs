using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grax the Phase Shifter")]
    public class GraxThePhaseShifter : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GraxThePhaseShifter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grax the Phase Shifter";
            Body = 0x190; // Human male body
            Hue = 1190;

            // Stats
            SetStr(90);
            SetDex(80);
            SetInt(130);
            SetHits(95);

            // Appearance
            AddItem(new Tunic() { Hue = 1192 });
            AddItem(new LongPants() { Hue = 1191 });
            AddItem(new Dagger() { Name = "Grax's Quantum Blade" });

            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public GraxThePhaseShifter(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Grax the Phase Shifter, at your service. What brings you to my realm?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My 'job'? I shift phases, unlike the mundane tasks most people have. In the 43rd century, we didn't merely live; we experienced existence across multiple dimensions simultaneously.");
                    jobModule.AddOption("What was life like in the 43rd century?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule lifeModule = new DialogueModule("Ah, the 43rd century was a time of unparalleled innovation. We had technologies that made the impossible possible—quantum teleportation, consciousness transfer, and even interdimensional travel. But it was also a time of existential dilemmas.");
                            lifeModule.AddOption("Existential dilemmas? What do you mean?",
                                p => true,
                                p =>
                                {
                                    DialogueModule dilemmaModule = new DialogueModule("With great power comes great responsibility. Many questioned whether we were still truly living or merely existing as echoes in a digital landscape. The essence of humanity felt diluted amidst such vast technological advancements.");
                                    dilemmaModule.AddOption("Sounds like a heavy burden to bear.",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, dilemmaModule));
                                });
                            lifeModule.AddOption("What was the most remarkable technology?",
                                p => true,
                                p =>
                                {
                                    DialogueModule techModule = new DialogueModule("The most remarkable? Quantum leaping, of course! It allowed individuals to experience different realities by shifting their consciousness into parallel timelines. Each leap brought forth new experiences, but it was also risky. Not everyone returned the same.");
                                    techModule.AddOption("Were there any dangers in quantum leaping?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule dangerModule = new DialogueModule("Indeed! Many lost their sense of self, becoming fragmented beings. Some found themselves trapped in loops of time or displaced in alternate dimensions. It's a complex art, requiring a strong will to navigate.");
                                            dangerModule.AddOption("I see. How did you end up here?",
                                                pe => true,
                                                pe =>
                                                {
                                                    DialogueModule endModule = new DialogueModule("My last quantum leap was interrupted, and I found myself stranded in this realm. The transition was disorienting, like waking from a dream into a nightmare. Now, I seek a way back, but the pathways are shrouded in mystery.");
                                                    endModule.AddOption("That sounds tragic. Have you found a way to return?",
                                                        plr => true,
                                                        plr =>
                                                        {
                                                            DialogueModule returnModule = new DialogueModule("Not yet. I gather knowledge and artifacts that may help me navigate the chaotic energies of this world. Each encounter brings me closer to understanding how to reconnect with my time.");
                                                            returnModule.AddOption("Is there anything I can do to assist?",
                                                                pt => true,
                                                                pt =>
                                                                {
                                                                    p.SendGump(new DialogueGump(p, CreateHelpModule()));
                                                                });
                                                            pl.SendGump(new DialogueGump(pl, returnModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, endModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, dangerModule));
                                        });
                                    p.SendGump(new DialogueGump(p, techModule));
                                });
                            pl.SendGump(new DialogueGump(pl, lifeModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you know about oblivion?",
                player => true,
                player =>
                {
                    DialogueModule oblivionModule = new DialogueModule("Ha! You know nothing of my world. Begone, before I shift you into oblivion! Yet, perhaps you are curious about what lies beyond.");
                    oblivionModule.AddOption("What lies beyond?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule beyondModule = new DialogueModule("Beyond this realm is the Abyss, a chaotic expanse where time and reality intertwine. Some believe it holds the secrets of the universe, while others fear its unpredictability.");
                            beyondModule.AddOption("How do you navigate the Abyss?",
                                p => true,
                                p =>
                                {
                                    DialogueModule navigateModule = new DialogueModule("Navigating the Abyss requires intuition and knowledge of dimensional currents. One must be attuned to the energies around them, lest they become lost forever. It's a dance of survival.");
                                    navigateModule.AddOption("You make it sound both thrilling and terrifying.",
                                        ply => true,
                                        ply =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, navigateModule));
                                });
                            pl.SendGump(new DialogueGump(pl, beyondModule));
                        });
                    player.SendGump(new DialogueGump(player, oblivionModule));
                });

            greeting.AddOption("What are your experiences in other dimensions?",
                player => true,
                player =>
                {
                    DialogueModule dimensionsModule = new DialogueModule("I've journeyed to realms of pure energy, lands frozen in time, and worlds that defy logic. Each journey changes you a little.");
                    dimensionsModule.AddOption("What was the most incredible world you've seen?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule incredibleModule = new DialogueModule("I once visited a world where thoughts manifested into reality. Creatures of pure light danced in a symphony of colors, and time flowed differently—one could experience a lifetime in mere moments.");
                            incredibleModule.AddOption("That sounds surreal! Did you bring anything back?",
                                p => true,
                                p =>
                                {
                                    DialogueModule bringBackModule = new DialogueModule("I managed to gather some essence of that realm—fragments of thought that can inspire or enlighten. They’re potent and dangerous if mishandled.");
                                    bringBackModule.AddOption("How can I use them?",
                                        plu => true,
                                        plu =>
                                        {
                                            pl.SendMessage("These essences can enhance your abilities or offer glimpses of potential futures. However, use them wisely; they can just as easily lead to madness.");
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, bringBackModule));
                                });
                            pl.SendGump(new DialogueGump(pl, incredibleModule));
                        });
                    player.SendGump(new DialogueGump(player, dimensionsModule));
                });

            greeting.AddOption("Can you teach me about phase shifting?",
                player => true,
                player =>
                {
                    DialogueModule teachModule = new DialogueModule("Dedication in phase shifting means endless study and practice. It's a devotion few can understand unless they experience it.");
                    teachModule.AddOption("What should I know before I start?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule startModule = new DialogueModule("You must be mentally prepared to confront the unknown. Each phase shift can change your perception of reality, and one must remain grounded amidst the chaos.");
                            startModule.AddOption("Grounded? How do I stay grounded?",
                                p => true,
                                p =>
                                {
                                    DialogueModule groundedModule = new DialogueModule("Focus on your physical sensations—your breath, heartbeat, and the world around you. It's essential to maintain a connection to your current reality.");
                                    groundedModule.AddOption("I understand. And what about the risks?",
                                        pli => true,
                                        pli =>
                                        {
                                            DialogueModule risksModule = new DialogueModule("The risks include losing oneself in the experience. It's vital to have a mentor or guide when you first attempt phase shifting to avoid irreversible consequences.");
                                            risksModule.AddOption("Thank you for the warning. I'm intrigued!",
                                                po => true,
                                                po =>
                                                {
                                                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                                                    {
                                                        p.SendMessage("You've shown a genuine interest, unlike many before you. But I cannot reward you yet.");
                                                    }
                                                    else
                                                    {
                                                        p.AddToBackpack(new InscriptionAugmentCrystal()); // Give the reward
                                                        p.SendMessage("Here, take this. It might help you on your own adventures.");
                                                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                                    }
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, risksModule));
                                        });
                                    p.SendGump(new DialogueGump(p, groundedModule));
                                });
                            player.SendGump(new DialogueGump(player, startModule));
                        });
                    player.SendGump(new DialogueGump(player, teachModule));
                });

            greeting.AddOption("What do you think about mundane tasks?",
                player => true,
                player =>
                {
                    DialogueModule mundaneModule = new DialogueModule("The mundane tasks might seem insignificant to me, but they keep the world spinning. Every role is vital.");
                    mundaneModule.AddOption("Can you give an example?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule exampleModule = new DialogueModule("Even the simplest of tasks, like tending to a garden or forging a tool, contribute to the tapestry of life. Each act, no matter how small, resonates through the fabric of existence.");
                            exampleModule.AddOption("I see. There is beauty in simplicity.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, exampleModule));
                        });
                    player.SendGump(new DialogueGump(player, mundaneModule));
                });

            return greeting;
        }

        private DialogueModule CreateHelpModule()
        {
            DialogueModule helpModule = new DialogueModule("Ah, assistance! I appreciate your willingness to help. There are many artifacts and knowledge pieces I seek.");

            helpModule.AddOption("What do you need help with?",
                player => true,
                player =>
                {
                    DialogueModule taskModule = new DialogueModule("I require components from various dimensions to stabilize my quantum shifts. Specifically, I need:");
                    taskModule.AddOption("What components do you need?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule componentsModule = new DialogueModule("1. Essence of a Fallen Star - Found in the depths of the Celestial Abyss. 2. A Shard of Time - Gathered from the Ruins of the Eternal Clock. 3. Liquid Void - Collected from the streams of the Abyssal Plane.");
                            componentsModule.AddOption("I'll help you gather them!",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Thank you! Your courage is commendable. Return to me with these components, and I shall reward you handsomely.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, componentsModule));
                        });
                    player.SendGump(new DialogueGump(player, taskModule));
                });

            return helpModule;
        }

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
