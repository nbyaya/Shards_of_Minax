using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Euclid")]
    public class ProfessorEuclid : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorEuclid() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Euclid";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(60);
            SetInt(110);
            SetHits(60);

            // Appearance
            AddItem(new Robe() { Hue = 1155 });
            AddItem(new Sandals() { Hue = 1155 });
            AddItem(new Spellbook() { Name = "Euclid's Elements" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
            SpeechHue = 0; // Default speech hue
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Professor Euclid, a humble philosopher. What thoughts weigh upon your mind?");

            greeting.AddOption("Tell me about geometry.",
                player => true,
                player =>
                {
                    DialogueModule geometryModule = new DialogueModule("Geometry is not merely a branch of mathematics; it is the language in which the universe is written. Would you like to explore its philosophical implications?");
                    geometryModule.AddOption("Yes, please elaborate.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule philosophyModule = new DialogueModule("Geometry embodies the interplay between form and abstraction. It raises profound questions about existence, space, and the nature of reality. What aspect interests you most?");
                            philosophyModule.AddOption("The nature of space.",
                                p => true,
                                p =>
                                {
                                    DialogueModule spaceModule = new DialogueModule("Space, dear traveler, is the canvas upon which the universe paints its myriad wonders. It compels us to ponder: Is space an entity of its own, or merely a context for objects?");
                                    spaceModule.AddOption("Is space infinite?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule infiniteSpaceModule = new DialogueModule("Ah, the infinite! Many philosophers and mathematicians have debated whether space extends forever or is bounded by some cosmic boundary. What do you think?");
                                            infiniteSpaceModule.AddOption("I believe space is infinite.",
                                                pw => true,
                                                pw =>
                                                {
                                                    p.SendMessage("Professor Euclid nods thoughtfully. 'Indeed, the concept of infinity challenges our understanding and imagination.'");
                                                });
                                            infiniteSpaceModule.AddOption("I think it must be finite.",
                                                pe => true,
                                                pe =>
                                                {
                                                    p.SendMessage("Professor Euclid raises an eyebrow. 'A curious perspective! If space is finite, what lies beyond its edges? Such questions inspire deeper exploration.'");
                                                });
                                            pl.SendGump(new DialogueGump(pl, infiniteSpaceModule));
                                        });
                                    spaceModule.AddOption("What about dimensions?",
                                        plr => true,
                                        plr =>
                                        {
                                            DialogueModule dimensionModule = new DialogueModule("Dimensions are fascinating constructs. In our realm, we perceive three dimensions—length, width, and height. But what of higher dimensions? How do they shape our understanding of reality?");
                                            dimensionModule.AddOption("What are higher dimensions?",
                                                pt => true,
                                                pt =>
                                                {
                                                    DialogueModule higherDimModule = new DialogueModule("Higher dimensions can be visualized as extensions beyond our perception. For instance, a tesseract is a four-dimensional cube. It challenges our spatial intuitions and reveals deeper truths about the universe.");
                                                    higherDimModule.AddOption("That sounds intriguing!",
                                                        ply => true,
                                                        ply =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, higherDimModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, higherDimModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, dimensionModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, spaceModule));
                                });
                            philosophyModule.AddOption("The relationship between form and function.",
                                p => true,
                                p =>
                                {
                                    DialogueModule formFunctionModule = new DialogueModule("The relationship between form and function in geometry is paramount. Form provides structure, while function lends purpose. Consider the shapes in nature—how they serve both aesthetic and practical roles.");
                                    formFunctionModule.AddOption("Can you give an example?",
                                        plu => true,
                                        plu =>
                                        {
                                            DialogueModule exampleModule = new DialogueModule("Take the honeycomb, for instance. Its hexagonal structure maximizes space and efficiency, embodying nature's geometric genius. Such harmony between form and function invites us to appreciate the design of the world around us.");
                                            pl.SendGump(new DialogueGump(pl, exampleModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, formFunctionModule));
                                });
                            player.SendGump(new DialogueGump(player, philosophyModule));
                        });

                    geometryModule.AddOption("How does geometry relate to art?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule artModule = new DialogueModule("Geometry has a profound influence on art. Artists utilize shapes, lines, and symmetry to create harmony and balance in their work. The Golden Ratio, for example, is often employed to achieve aesthetic appeal.");
                            artModule.AddOption("What is the Golden Ratio?",
                                p => true,
                                p =>
                                {
                                    DialogueModule goldenRatioModule = new DialogueModule("The Golden Ratio, approximately 1.618, is a mathematical ratio that appears in nature, architecture, and art. It creates balance and beauty, guiding artists in their compositions.");
                                    goldenRatioModule.AddOption("Can you show me examples?",
                                        pli => true,
                                        pli =>
                                        {
                                            DialogueModule examplesModule = new DialogueModule("You can observe the Golden Ratio in the Parthenon, da Vinci's works, and even in the spiral patterns of seashells. Its prevalence underscores the intrinsic connection between mathematics and beauty.");
                                            pl.SendGump(new DialogueGump(pl, examplesModule));
                                        });
                                    p.SendGump(new DialogueGump(p, goldenRatioModule));
                                });
                            player.SendGump(new DialogueGump(player, artModule));
                        });
                    player.SendGump(new DialogueGump(player, geometryModule));
                });

            greeting.AddOption("What do you think about the philosophy of knowledge?",
                player => true,
                player =>
                {
                    DialogueModule knowledgePhilosophyModule = new DialogueModule("The philosophy of knowledge intertwines with geometry in profound ways. Knowledge shapes our understanding of reality, and geometry provides the framework for that understanding. What intrigues you?");
                    knowledgePhilosophyModule.AddOption("How do we acquire knowledge?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule acquisitionModule = new DialogueModule("Knowledge is acquired through experience, observation, and reason. Geometry teaches us to observe patterns and relationships, enhancing our comprehension of the world.");
                            player.SendGump(new DialogueGump(player, acquisitionModule));
                        });
                    knowledgePhilosophyModule.AddOption("Is knowledge objective or subjective?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule objectivityModule = new DialogueModule("Ah, the eternal debate! Some argue that knowledge is objective, rooted in universal truths, while others contend it is subjective, shaped by personal experiences. Geometry often straddles both realms.");
                            player.SendGump(new DialogueGump(player, objectivityModule));
                        });
                    player.SendGump(new DialogueGump(player, knowledgePhilosophyModule));
                });

            greeting.AddOption("What about the intersection of geometry and nature?",
                player => true,
                player =>
                {
                    DialogueModule natureGeometryModule = new DialogueModule("Nature is a master of geometry. From the Fibonacci sequence in sunflowers to the hexagonal patterns in beehives, geometry reveals itself in the most unexpected places.");
                    natureGeometryModule.AddOption("Can you explain the Fibonacci sequence?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule fibonacciModule = new DialogueModule("The Fibonacci sequence is a series of numbers where each number is the sum of the two preceding ones. It appears frequently in nature, such as in the arrangement of leaves, the branching of trees, and the patterns of flowers.");
                            player.SendGump(new DialogueGump(player, fibonacciModule));
                        });
                    player.SendGump(new DialogueGump(player, natureGeometryModule));
                });

            greeting.AddOption("What are your thoughts on destiny?",
                player => true,
                player =>
                {
                    DialogueModule destinyModule = new DialogueModule("Destiny, like geometry, raises profound questions. Is our path predetermined, or do we carve it through our choices? Just as a line extends infinitely, so too does our potential.");
                    destinyModule.AddOption("Is destiny fixed?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule fixedDestinyModule = new DialogueModule("Some philosophies suggest that destiny is fixed, akin to a geometric shape defined by its angles and sides. Others argue that we possess the power to alter our trajectory, like adjusting the path of a line.");
                            player.SendGump(new DialogueGump(player, fixedDestinyModule));
                        });
                    destinyModule.AddOption("How do choices influence destiny?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule choicesModule = new DialogueModule("Each choice we make carves a path through the fabric of destiny, much like a geometric proof builds upon previous axioms. Our decisions shape the outcome of our lives and the world around us.");
                            player.SendGump(new DialogueGump(player, choicesModule));
                        });
                    player.SendGump(new DialogueGump(player, destinyModule));
                });

            greeting.AddOption("Farewell.",
                player => true,
                player =>
                {
                    player.SendMessage("Very well, traveler. Your thoughts are your own to ponder. Farewell.");
                });

            return greeting;
        }

        public ProfessorEuclid(Serial serial) : base(serial) { }

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
