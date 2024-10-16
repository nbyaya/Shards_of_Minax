using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Trini")]
    public class LadyTrini : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyTrini() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Trini";
            Body = 0x191; // Human female body

            // Stats
            SetStr(80);
            SetDex(120);
            SetInt(60);
            SetHits(80);

            // Appearance
            AddItem(new RingmailLegs() { Hue = 53 });
            AddItem(new RingmailChest() { Hue = 53 });
            AddItem(new RingmailGloves() { Hue = 53 });
            AddItem(new ChainCoif() { Hue = 53 });
            AddItem(new Boots() { Hue = 53 });
            AddItem(new Dagger() { Name = "Lady Trini's Dagger" });

            // Random Hair and Hue
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Trini, a seeker of knowledge and a student of the virtues.");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthModule())));

            greeting.AddOption("What is your job?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJobModule())));

            greeting.AddOption("What can you tell me about the virtues?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateVirtuesModule())));

            greeting.AddOption("Do you have any tasks for me?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateTasksModule())));

            greeting.AddOption("Goodbye.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGoodbyeModule())));

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            return new DialogueModule("I am in good health, thank you for asking. The study of virtues keeps my spirit strong.");
        }

        private DialogueModule CreateJobModule()
        {
            DialogueModule jobModule = new DialogueModule("My calling is that of a scholar and a seeker of knowledge. I delve into the mysteries of the world.");

            jobModule.AddOption("What kind of knowledge do you seek?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateKnowledgeModule())));

            jobModule.AddOption("Can you teach me?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateTeachModule())));

            return jobModule;
        }

        private DialogueModule CreateKnowledgeModule()
        {
            DialogueModule knowledgeModule = new DialogueModule("I seek knowledge of the ancient tomes and the secrets of alchemy. Every piece of information is a key to understanding our world.");

            knowledgeModule.AddOption("What about the ancient tomes?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateTomeModule())));

            knowledgeModule.AddOption("How does alchemy work?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateAlchemyModule())));

            return knowledgeModule;
        }

        private DialogueModule CreateTomeModule()
        {
            DialogueModule tomeModule = new DialogueModule("The ancient tomes hold wisdom from centuries past. Each page is a treasure that unveils the mysteries of existence.");

            tomeModule.AddOption("Where can I find these tomes?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateFindTomeModule())));

            tomeModule.AddOption("What tomes do you have?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateMyTomesModule())));

            return tomeModule;
        }

        private DialogueModule CreateFindTomeModule()
        {
            return new DialogueModule("Many tomes are hidden in libraries long forgotten or guarded by powerful creatures. A wise adventurer should tread carefully.");
        }

        private DialogueModule CreateMyTomesModule()
        {
            return new DialogueModule("I possess a few tomes myself, but they are not for sale. They serve as my companions on this journey of discovery.");
        }

        private DialogueModule CreateAlchemyModule()
        {
            DialogueModule alchemyModule = new DialogueModule("Alchemy is the art of transformation. It requires not just ingredients, but also knowledge and intuition.");

            alchemyModule.AddOption("What do I need to know about alchemy?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateAlchemyKnowledgeModule())));

            alchemyModule.AddOption("Can you show me an example?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateAlchemyKnowledgeModule())));

            return alchemyModule;
        }

        private DialogueModule CreateAlchemyKnowledgeModule()
        {
            DialogueModule alchemyKnowledgeModule = new DialogueModule("To succeed in alchemy, one must understand the properties of each ingredient. Combining them at the right ratios can yield powerful effects.");

            alchemyKnowledgeModule.AddOption("What are some key ingredients?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateKeyIngredientsModule())));

            alchemyKnowledgeModule.AddOption("What are the risks of alchemy?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateRisksModule())));

            return alchemyKnowledgeModule;
        }

        private DialogueModule CreateKeyIngredientsModule()
        {
            return new DialogueModule("Some key ingredients include Moonlit Petals, Crystal Dew, and Dragon's Breath. Each has unique properties that can greatly affect your results.");
        }

        private DialogueModule CreateRisksModule()
        {
            return new DialogueModule("The greatest risk is failure. A miscalculation can lead to explosive results or potions with dangerous side effects.");
        }

        private DialogueModule CreateTeachModule()
        {
            DialogueModule teachModule = new DialogueModule("I would be happy to share what I've learned. Knowledge is meant to be shared.");

            teachModule.AddOption("What shall we learn together?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateTeachTopicsModule())));

            return teachModule;
        }

        private DialogueModule CreateTeachTopicsModule()
        {
            DialogueModule topicsModule = new DialogueModule("We can explore various topics such as potions, elixirs, or even the historical aspects of alchemy.");

            topicsModule.AddOption("Tell me about potions.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreatePotionsModule())));

            topicsModule.AddOption("What about elixirs?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateElixirsModule())));

            return topicsModule;
        }

        private DialogueModule CreatePotionsModule()
        {
            return new DialogueModule("Potions are mixtures that grant effects when consumed. Understanding the properties of each ingredient is crucial for success.");
        }

        private DialogueModule CreateElixirsModule()
        {
            return new DialogueModule("Elixirs are more potent than potions and often require rare ingredients. They can provide extraordinary benefits.");
        }

        private DialogueModule CreateVirtuesModule()
        {
            DialogueModule virtuesModule = new DialogueModule("The eight virtues—Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility—are the guiding principles of a righteous life.");

            virtuesModule.AddOption("How do I embody these virtues?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateEmbodimentModule())));

            virtuesModule.AddOption("Which virtue do you think is most important?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateImportantVirtueModule())));

            return virtuesModule;
        }

        private DialogueModule CreateEmbodimentModule()
        {
            return new DialogueModule("To embody these virtues, one must practice them daily. Each interaction with others is an opportunity to live by these principles.");
        }

        private DialogueModule CreateImportantVirtueModule()
        {
            return new DialogueModule("Many would argue that Compassion is the most important. It allows us to understand others and act with kindness.");
        }

        private DialogueModule CreateTasksModule()
        {
            DialogueModule tasksModule = new DialogueModule("If you find the tome of alchemy, I'd be grateful enough to offer you a reward.");

            tasksModule.AddOption("Where can I find this tome?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateFindTomeQuestModule())));

            return tasksModule;
        }

        private DialogueModule CreateFindTomeQuestModule()
        {
            DialogueModule findTomeModule = new DialogueModule("The tome is rumored to be hidden in the ancient library, protected by magical wards and dangerous creatures.");

            findTomeModule.AddOption("What dangers should I expect?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateDangersModule())));

            return findTomeModule;
        }

        private DialogueModule CreateDangersModule()
        {
            return new DialogueModule("The library is said to be haunted by spirits of those who perished seeking its knowledge. Be prepared for anything.");
        }

        private DialogueModule CreateGoodbyeModule()
        {
            return new DialogueModule("Farewell, traveler. May wisdom guide your path. Return if you seek more knowledge.");
        }

        public LadyTrini(Serial serial) : base(serial) { }

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
