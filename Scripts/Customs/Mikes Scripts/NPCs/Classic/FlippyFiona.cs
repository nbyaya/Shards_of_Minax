using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Flippy Fiona")]
    public class FlippyFiona : BaseCreature
    {
        [Constructable]
        public FlippyFiona() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Flippy Fiona";
            Body = 0x190; // Human female body

            // Stats
            SetStr(90);
            SetDex(75);
            SetInt(50);
            SetHits(90);

            // Appearance
            AddItem(new Skirt() { Hue = 53 });
            AddItem(new FancyShirt() { Hue = 53 });
            AddItem(new Sandals() { Hue = 1194 });
            AddItem(new LeatherGloves() { Name = "Fiona's Flipping Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            // Set direction
            Direction = Direction.North;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public FlippyFiona(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Oh, hello there! I’m Flippy Fiona, the wrestler! I’ve mastered the art of flipping twice in a single jump. Would you like to hear about it?");

            greeting.AddOption("Yes, tell me how you learned to flip twice!",
                player => true,
                player =>
                {
                    DialogueModule learningModule = new DialogueModule("It all began when I was just a child in the local wrestling ring. I watched the greats perform, and their agility fascinated me. One day, I decided to try flipping myself!");
                    
                    learningModule.AddOption("What was your first attempt like?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule firstAttemptModule = new DialogueModule("Oh, it was quite the sight! I jumped and did a flip, but I only managed to rotate once and landed flat on my back! I got up laughing, though; it was exhilarating!");
                            
                            firstAttemptModule.AddOption("Did that discourage you?",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, new DialogueModule("Not at all! In fact, it motivated me even more. I practiced day and night, trying to perfect my jump and flip technique.")));
                                });

                            firstAttemptModule.AddOption("How did you improve?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule improvementModule = new DialogueModule("I began training with a local coach, a former champion. He taught me the mechanics of jumps, and gradually I became more confident. I discovered how to time my flips better.");
                                    
                                    improvementModule.AddOption("What exercises helped you?",
                                        plaa => true,
                                        plaa =>
                                        {
                                            plaa.SendGump(new DialogueGump(plaa, new DialogueModule("I practiced jumping drills, strength training, and, of course, plenty of practice flips on soft mats. Each session made me feel like I was flying!")));
                                        });

                                    improvementModule.AddOption("What about your coach?",
                                        plaa => true,
                                        plaa =>
                                        {
                                            DialogueModule coachModule = new DialogueModule("My coach was a tough one! He'd shout encouragements, but also critique every little mistake. His motto was, 'Every mistake is a step closer to greatness!'");
                                            
                                            coachModule.AddOption("What did you learn from him?",
                                                plaaa => true,
                                                plaaa =>
                                                {
                                                    plaaa.SendGump(new DialogueGump(plaaa, new DialogueModule("I learned discipline, determination, and the importance of believing in myself. Those lessons shaped me not just as a wrestler but as a person.")));
                                                });
                                            plaa.SendGump(new DialogueGump(plaa, coachModule));
                                        });

                                    pl.SendGump(new DialogueGump(pl, improvementModule));
                                });

                            learningModule.AddOption("Did you have any setbacks?",
                                plaa => true,
                                plaa =>
                                {
                                    DialogueModule setbacksModule = new DialogueModule("Absolutely! There were times I fell hard, even sprained my ankle once. But every setback taught me resilience. I learned to get back up and try again.");
                                    
                                    setbacksModule.AddOption("That sounds tough! What kept you going?",
                                        plaaa => true,
                                        plaaa =>
                                        {
                                            plaaa.SendGump(new DialogueGump(plaaa, new DialogueModule("The thrill of performing in front of a crowd! I loved their cheers and support. It fueled my passion like nothing else!")));
                                        });
                                    
                                    plaa.SendGump(new DialogueGump(plaa, setbacksModule));
                                });

                            pl.SendGump(new DialogueGump(pl, firstAttemptModule));
                        });

                    learningModule.AddOption("What was the breakthrough moment?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule breakthroughModule = new DialogueModule("One fateful day during practice, I felt the perfect balance and control in my jump. I leaped and managed to flip twice, landing perfectly! I couldn't believe it!");
                            
                            breakthroughModule.AddOption("How did it feel?",
                                plaa => true,
                                plaa =>
                                {
                                    plaa.SendGump(new DialogueGump(plaa, new DialogueModule("It was euphoric! I felt like I was soaring through the air, like a bird! The crowd would love this move!")));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, breakthroughModule));
                        });

                    player.SendGump(new DialogueGump(player, learningModule));
                });

            greeting.AddOption("Can you show me a flip?",
                player => true,
                player =>
                {
                    player.SendMessage("Flippy Fiona grins and performs an impressive flip, executing two rotations flawlessly before landing gracefully.");
                });

            greeting.AddOption("Good luck on your journey.",
                player => true,
                player =>
                {
                    player.SendMessage("Flippy Fiona smiles warmly at you, thanking you for your kind words.");
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
