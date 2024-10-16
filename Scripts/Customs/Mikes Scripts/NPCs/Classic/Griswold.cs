using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Griswold")]
    public class Griswold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Griswold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Griswold";
            Body = 0x190; // Human male body

            // Stats
            Str = 130;
            Dex = 70;
            Int = 40;
            Hits = 120;

            // Appearance
            AddItem(new SmithHammer() { Name = "Griswold's Hammer" });
            AddItem(new FullApron() { Hue = 1109 });
            AddItem(new Shoes() { Hue = 0 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
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
            DialogueModule greeting = new DialogueModule("I am Griswold, the blacksmith. But don't expect me to be all smiles and sunshine.");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthModule())));

            greeting.AddOption("What is your job?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJobModule())));

            greeting.AddOption("What do you think about heroes?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHeroesModule())));

            greeting.AddOption("Can I receive a reward?",
                player => CanReceiveReward(player),
                player => GiveReward(player));

            greeting.AddOption("What do you think about your ancestors?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateAncestorsModule())));

            greeting.AddOption("What advice do you have for my journey?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJourneyModule())));

            return greeting;
        }

        private bool CanReceiveReward(PlayerMobile player)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            return DateTime.UtcNow - lastRewardTime >= cooldown;
        }

        private void GiveReward(PlayerMobile player)
        {
            if (CanReceiveReward(player))
            {
                player.AddToBackpack(new Gold(1000)); // Example reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                player.SendMessage("Here, take this token of my appreciation. You've proven yourself to be a true hero.");
            }
            else
            {
                player.SendMessage("I have no reward right now. Please return later.");
            }
        }

        private DialogueModule CreateHealthModule()
        {
            DialogueModule healthModule = new DialogueModule("Health? What do you care? I've seen better days.");

            healthModule.AddOption("What happened to you?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthDetailsModule())));

            healthModule.AddOption("Is there anything I can do to help?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthHelpModule())));

            return healthModule;
        }

        private DialogueModule CreateHealthDetailsModule()
        {
            return new DialogueModule("Ah, it's a long tale. I was once a warrior, strong and capable. But after many battles, I chose the anvil over the battlefield. Yet, the scars remain.");
        }

        private DialogueModule CreateHealthHelpModule()
        {
            DialogueModule helpModule = new DialogueModule("If you truly wish to help, you could fetch me some rare herbs known for their healing properties. They grow in the Whispering Forest.");

            helpModule.AddOption("What herbs do you need?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHerbDetailsModule())));

            helpModule.AddOption("That sounds dangerous. Maybe another time.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return helpModule;
        }

        private DialogueModule CreateHerbDetailsModule()
        {
            return new DialogueModule("I need Moonlit Petals and Crystal Dew. The petals bloom only at night, and the dew can be collected at dawn.");
        }

        private DialogueModule CreateJobModule()
        {
            DialogueModule jobModule = new DialogueModule("I forge weapons and armor for all you heroes who waltz in here like you own the place.");

            jobModule.AddOption("What kind of weapons do you forge?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateWeaponDetailsModule())));

            jobModule.AddOption("Do you offer any special services?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateSpecialServicesModule())));

            return jobModule;
        }

        private DialogueModule CreateWeaponDetailsModule()
        {
            return new DialogueModule("Ah, I've crafted many weapons: swords, axes, and even a few magical items for those who can afford them. Each has a story.");
        }

        private DialogueModule CreateSpecialServicesModule()
        {
            DialogueModule servicesModule = new DialogueModule("I can enhance your weapons for the right price. Or, if you bring me rare materials, I might create something extraordinary.");

            servicesModule.AddOption("What rare materials do you need?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateRareMaterialsModule())));

            servicesModule.AddOption("Maybe another time.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return servicesModule;
        }

        private DialogueModule CreateRareMaterialsModule()
        {
            return new DialogueModule("Materials like Dragon's Breath, Celestial Essence, or even the elusive Star Dust can make all the difference.");
        }

        private DialogueModule CreateHeroesModule()
        {
            DialogueModule heroesModule = new DialogueModule("True heroes know when to fight and when to parley. The world isn't as black and white as it seems.");

            heroesModule.AddOption("What do you mean by that?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHeroPhilosophyModule())));

            heroesModule.AddOption("Can I become a true hero?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHeroJourneyModule())));

            return heroesModule;
        }

        private DialogueModule CreateHeroPhilosophyModule()
        {
            return new DialogueModule("Valor isn't just about the battles you win, but also the ones you choose to avoid. Not every conflict needs a blade.");
        }

        private DialogueModule CreateHeroJourneyModule()
        {
            DialogueModule journeyModule = new DialogueModule("You can become a true hero by helping others, proving your worth in deeds, and showing kindness.");

            journeyModule.AddOption("How can I help others?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHelpWaysModule())));

            journeyModule.AddOption("What if I fail?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateFailureModule())));

            return journeyModule;
        }

        private DialogueModule CreateHelpWaysModule()
        {
            return new DialogueModule("You can help by defending the weak, bringing supplies to those in need, or even just lending a listening ear.");
        }

        private DialogueModule CreateFailureModule()
        {
            return new DialogueModule("Failure is part of growth. Even the strongest warriors fall. What matters is that you get back up and try again.");
        }

        private DialogueModule CreateAncestorsModule()
        {
            DialogueModule ancestorsModule = new DialogueModule("My ancestors were blacksmiths too. Some say our hammers are blessed by the old gods, making our creations strong.");

            ancestorsModule.AddOption("What were they like?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateAncestorsDetailsModule())));

            ancestorsModule.AddOption("Do you carry on their legacy?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateLegacyModule())));

            return ancestorsModule;
        }

        private DialogueModule CreateAncestorsDetailsModule()
        {
            return new DialogueModule("They were renowned for their craftsmanship. Legends say one of my ancestors once forged a blade that could cut through the very fabric of time.");
        }

        private DialogueModule CreateLegacyModule()
        {
            return new DialogueModule("I strive to honor their memory by crafting the best weapons I can. Every strike of the hammer is a tribute to them.");
        }

        private DialogueModule CreateJourneyModule()
        {
            DialogueModule journeyModule = new DialogueModule("Every quest is a chance to prove oneself. Choose wisely and remember that the journey is as important as the destination.");

            journeyModule.AddOption("What should I prioritize on my journey?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJourneyAdviceModule())));

            return journeyModule;
        }

        private DialogueModule CreateJourneyAdviceModule()
        {
            return new DialogueModule("Prioritize kindness, strength, and wisdom. They will guide you through the toughest of times.");
        }

        public Griswold(Serial serial) : base(serial) { }

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
