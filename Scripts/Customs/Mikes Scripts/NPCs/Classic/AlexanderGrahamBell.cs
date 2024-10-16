using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Alexander Graham Bell")]
    public class AlexanderGrahamBell : BaseCreature
    {
        [Constructable]
        public AlexanderGrahamBell() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Alexander Graham Bell";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(70);
            SetInt(80);
            SetHits(60);

            // Appearance
            AddItem(new LongPants(1153));
            AddItem(new FancyShirt(1153));
            AddItem(new Boots(1153));
            AddItem(new Dagger { Name = "Alexander's Invention" });

            Hue = Utility.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            SpeechHue = 0; // Default speech hue
        }

        public AlexanderGrahamBell(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I'm Alexander Graham Bell, the genius from Canada. What do you want?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("As if I care about your health. I'm not your physician.");
                    healthModule.AddOption("Fair enough.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("Job? Do I look like I have a job? My job is listening to idiots like you.");
                    jobModule.AddOption("You seem knowledgeable.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateInventionModule())));
                    jobModule.AddOption("Fair enough.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you think of your inventions?",
                player => true,
                player =>
                {
                    DialogueModule inventionModule = new DialogueModule("Do you even know the first thing about communication? Tell me, what's the most important invention in history?");
                    inventionModule.AddOption("Is it the telephone?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule responseModule = new DialogueModule("Hah! You're even more ignorant than I thought. It's the telephone, you imbecile! Alexander Graham Bell's greatest creation.");
                            responseModule.AddOption("That's impressive.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                            responseModule.AddOption("What inspired you to invent it?",
                                p => true,
                                p =>
                                {
                                    DialogueModule inspirationModule = new DialogueModule("I was inspired by my mother, who was deaf. I wanted to create a way for people to communicate over distances, bridging gaps that separate us.");
                                    inspirationModule.AddOption("That's touching.",
                                        pla => true,
                                        pla => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    p.SendGump(new DialogueGump(p, inspirationModule));
                                });
                            pl.SendGump(new DialogueGump(pl, responseModule));
                        });
                    inventionModule.AddOption("Maybe I should ask someone else.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, inventionModule));
                });

            greeting.AddOption("Tell me about your greatest challenge.",
                player => true,
                player =>
                {
                    DialogueModule challengeModule = new DialogueModule("Every invention faces challenges. The telephone was dismissed by many as a folly. Convincing people of its potential was no easy task.");
                    challengeModule.AddOption("How did you overcome that?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule overcomeModule = new DialogueModule("Persistence, my friend! I demonstrated the telephone's capabilities to skeptics, showing them how it could change the world. Belief in my invention kept me going.");
                            overcomeModule.AddOption("Your determination is admirable.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, overcomeModule));
                        });
                    challengeModule.AddOption("Interesting.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, challengeModule));
                });

            return greeting;
        }

        private DialogueModule CreateInventionModule()
        {
            DialogueModule inventionModule = new DialogueModule("I create inventions that revolutionize communication. The telephone was merely the beginning.");
            inventionModule.AddOption("What other inventions have you created?",
                player => true,
                player =>
                {
                    DialogueModule inventionsListModule = new DialogueModule("Aside from the telephone, I developed improvements in telegraphy and even worked on methods for teaching the deaf. Each invention aims to bring people closer.");
                    inventionsListModule.AddOption("That's quite a legacy.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, inventionsListModule));
                });
            inventionModule.AddOption("Your inventions sound fascinating!",
                player => true,
                player =>
                {
                    DialogueModule complimentModule = new DialogueModule("Thank you! Innovation requires curiosity and bravery. Always question the norm.");
                    complimentModule.AddOption("I will remember that.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, complimentModule));
                });
            return inventionModule;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
