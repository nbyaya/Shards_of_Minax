using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kain")]
    public class Kain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Kain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kain";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(120);
            SetInt(40);
            SetHits(120);

            // Appearance
            AddItem(new PlateLegs() { Hue = 2335 });
            AddItem(new PlateChest() { Hue = 1745 });
            AddItem(new PlateHelm() { Hue = 1955 });
            AddItem(new PlateGloves() { Hue = 1843 });
            AddItem(new Boots() { Hue = 2322 });
            AddItem(new Halberd() { Name = "Kain's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Kain, the Dragoon from Baron Castle. What brings you to this realm?");

            greeting.AddOption("What is your health?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });

            greeting.AddOption("What is your job?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });

            greeting.AddOption("Tell me about battle.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateBattleModule())); });

            greeting.AddOption("What about Baron Castle?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateBaronModule())); });

            greeting.AddOption("Can you tell me about Dragoons?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDragoonModule())); });

            greeting.AddOption("What does honor mean to you?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateHonorModule())); });

            greeting.AddOption("How do you protect the castle?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateProtectModule())); });

            greeting.AddOption("Can you share any secrets?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateSecretsModule(player))); });

            greeting.AddOption("How can I help?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateHelpModule())); });

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            return new DialogueModule("I'm in good shape, ready for battle! Every day is a test of strength and skill. What about you?");
        }

        private DialogueModule CreateJobModule()
        {
            DialogueModule jobModule = new DialogueModule("My duty is to protect Baron Castle and its people. It’s a solemn responsibility, and one I take to heart.");

            jobModule.AddOption("What challenges do you face?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateChallengesModule())); });

            jobModule.AddOption("How can I assist you in your duty?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateAssistModule())); });

            return jobModule;
        }

        private DialogueModule CreateChallengesModule()
        {
            DialogueModule challengesModule = new DialogueModule("The threats are many: marauding beasts, treacherous bandits, and dark sorcery. Each day is a new challenge.");

            challengesModule.AddOption("What kind of beasts do you encounter?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateBeastsModule())); });

            challengesModule.AddOption("Have you faced sorcery?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateSorceryModule())); });

            return challengesModule;
        }

        private DialogueModule CreateBeastsModule()
        {
            return new DialogueModule("We often face wolves, bears, and even dragons that wander too close to the castle. Each encounter tests our skills and resolve.");
        }

        private DialogueModule CreateSorceryModule()
        {
            return new DialogueModule("Indeed, dark sorcery is a constant threat. Wizards and witches seek to exploit the castle’s secrets. It is a perilous game of wits and power.");
        }

        private DialogueModule CreateAssistModule()
        {
            DialogueModule assistModule = new DialogueModule("Your offer is noble! There are many ways you could help: scouting the perimeter, gathering supplies, or even training with us.");

            assistModule.AddOption("I would like to scout the perimeter.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateScoutModule())); });

            assistModule.AddOption("I can gather supplies.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateGatherModule())); });

            return assistModule;
        }

        private DialogueModule CreateScoutModule()
        {
            return new DialogueModule("Scouting is vital! Ensure the paths around the castle are safe. Report back any unusual sightings.");
        }

        private DialogueModule CreateGatherModule()
        {
            return new DialogueModule("Gathering supplies is equally important. We need potions and rations. Return with whatever you can find.");
        }

        private DialogueModule CreateBattleModule()
        {
            DialogueModule battleModule = new DialogueModule("The path of a Dragoon is one of honor and valor. We fight for our homeland. Are you familiar with our ways?");

            battleModule.AddOption("Tell me more about the Dragoons.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDragoonDetailsModule())); });

            battleModule.AddOption("What is your battle style?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateBattleStyleModule())); });

            return battleModule;
        }

        private DialogueModule CreateDragoonDetailsModule()
        {
            return new DialogueModule("Dragoons are elite warriors trained in aerial combat. We leap into the skies and dive onto our foes, striking with deadly precision.");
        }

        private DialogueModule CreateBattleStyleModule()
        {
            return new DialogueModule("My battle style focuses on agility and power. Using my halberd, I can strike swiftly and retreat before the enemy can respond.");
        }

        private DialogueModule CreateBaronModule()
        {
            return new DialogueModule("Baron Castle is an ancient stronghold, rich with history. Many secrets lie within its walls, waiting to be uncovered.");
        }

        private DialogueModule CreateDragoonModule()
        {
            DialogueModule dragoonModule = new DialogueModule("A Dragoon is a noble warrior trained in the art of dragon combat. We leap into the skies and dive onto our foes.");

            dragoonModule.AddOption("What is your training like?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateTrainingModule())); });

            dragoonModule.AddOption("What about your equipment?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEquipmentModule())); });

            return dragoonModule;
        }

        private DialogueModule CreateTrainingModule()
        {
            return new DialogueModule("Training is grueling, focusing on strength, agility, and mental fortitude. We practice for hours until our skills become second nature.");
        }

        private DialogueModule CreateEquipmentModule()
        {
            return new DialogueModule("I wear heavy armor to protect myself, along with my trusty halberd. Each piece is chosen for its strength and balance in combat.");
        }

        private DialogueModule CreateHonorModule()
        {
            return new DialogueModule("Honor means fighting for what is right and just, standing up for those who cannot defend themselves. It is a code we live by.");

        }

        private DialogueModule CreateProtectModule()
        {
            return new DialogueModule("The threats to our lands are endless. But I swear on my life to keep the castle safe. I will not falter in my duty.");
        }

        private DialogueModule CreateSecretsModule(PlayerMobile player)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                return new DialogueModule("I have no reward right now. Please return later.");
            }
            else
            {
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                player.AddToBackpack(new NeckSlotChangeDeed()); // Give the reward
                return new DialogueModule("Ah, many have tried to uncover the mysteries of Baron Castle. Here, take this as a token of trust. Use it wisely.");
            }
        }

        private DialogueModule CreateHelpModule()
        {
            DialogueModule helpModule = new DialogueModule("Many need assistance in these trying times. From fetching medicines to defending against foes, there's always a way to aid.");

            helpModule.AddOption("What kind of tasks are needed?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateTaskDetailsModule())); });

            return helpModule;
        }

        private DialogueModule CreateTaskDetailsModule()
        {
            return new DialogueModule("Tasks vary from gathering herbs in the forest to patrolling the outskirts for bandit activity. Choose what you feel capable of!");
        }

        public Kain(Serial serial) : base(serial) { }

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
