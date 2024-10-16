using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nox Nora")]
    public class NoxNora : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NoxNora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nox Nora";
            Body = 0x190; // Human male body

            // Stats
            SetStr(125);
            SetDex(82);
            SetInt(75);
            SetHits(82);

            // Appearance
            AddItem(new LeatherChest() { Hue = 1260 });
            AddItem(new LeatherGorget() { Hue = 1260 });
            AddItem(new Kryss()); // Ensure you have this item type or adjust accordingly
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Nox Nora the Rogue! What brings you to my shadows?");

            greeting.AddOption("Tell me about yourself.",
                player => true,
                player =>
                {
                    DialogueModule selfModule = new DialogueModule("Ah, a seeker of knowledge! I walk the path of shadows and secrets. My true passion lies in the art of poisoning. Efficiency is the key, my friend.");
                    selfModule.AddOption("Poisoning? That sounds dangerous.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreatePoisoningModule())));
                    selfModule.AddOption("What else do you enjoy?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateHobbiesModule())));
                    player.SendGump(new DialogueGump(player, selfModule));
                });

            greeting.AddOption("Do you know any secrets?",
                player => true,
                player =>
                {
                    DialogueModule secretsModule = new DialogueModule("Secrets are the currency of rogues. Share a secret of yours, and perhaps I'll share one in return.");
                    secretsModule.AddOption("What kind of secrets?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateSecretModule())));
                    player.SendGump(new DialogueGump(player, secretsModule));
                });

            greeting.AddOption("Can you teach me something?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow;
                        DialogueModule teachModule = new DialogueModule("Bring me the feather of a raven as a token of your commitment, and I will share my secrets with you.");
                        teachModule.AddOption("I'll find a raven's feather.",
                            p => true,
                            p => p.SendMessage("You set off to find a raven's feather."));
                        teachModule.AddOption("Never mind.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, teachModule));
                    }
                });

            return greeting;
        }

        private DialogueModule CreatePoisoningModule()
        {
            DialogueModule poisoningModule = new DialogueModule("Poisoning is an art form that requires precision and an understanding of one's target. The right poison can be a masterstroke in a rogue's repertoire.");
            poisoningModule.AddOption("Why do you prefer poisoning over other methods?",
                p => true,
                p => 
                {
                    DialogueModule preferenceModule = new DialogueModule("Poisoning allows for subtlety. Why engage in a messy fight when a single drop can accomplish the same? The efficiency of a well-placed poison is unmatched.");
                    preferenceModule.AddOption("What poisons do you recommend?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreatePoisonTypesModule())));
                    preferenceModule.AddOption("Isn't poisoning morally questionable?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateMoralityModule())));
                    p.SendGump(new DialogueGump(p, preferenceModule));
                });
            return poisoningModule;
        }

        private DialogueModule CreatePoisonTypesModule()
        {
            DialogueModule typesModule = new DialogueModule("There are many types of poisons, each with its unique properties. Here are a few I find particularly effective:");
            typesModule.AddOption("Tell me more about the poisons.",
                pl => true,
                pl => 
                {
                    DialogueModule detailsModule = new DialogueModule("1. **Nightshade Essence**: Causes sleep and paralysis. Perfect for stealthy approaches. \n2. **Crimson Hemlock**: A slow-acting poison that makes the victim's heart race before stopping it. \n3. **Wyrmwood Extract**: Induces hallucinations and confusion. Ideal for creating chaos.");
                    detailsModule.AddOption("Sounds fascinating!",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                    typesModule.AddOption("How do you obtain these poisons?",
                        plq => true,
                        plq => pl.SendGump(new DialogueGump(pl, CreateObtainPoisonModule())));
                    pl.SendGump(new DialogueGump(pl, detailsModule));
                });
            return typesModule;
        }

        private DialogueModule CreateObtainPoisonModule()
        {
            DialogueModule obtainModule = new DialogueModule("Many ingredients can be gathered from the wild or purchased from dubious merchants. Some require delicate extraction methods, while others may need a skilled alchemist.");
            obtainModule.AddOption("Can you teach me to create poisons?",
                pl => true,
                pl => 
                {
                    DialogueModule teachPoisonModule = new DialogueModule("To create poisons, one must first learn the art of alchemy. Combine specific ingredients in the right ratios. I can teach you, but first, you must prove your worth.");
                    teachPoisonModule.AddOption("What do I need to do?",
                        p => true,
                        p => p.SendMessage("Bring me a vial of Dragon's Blood and a Mandrake Root. Only then will I share my secrets with you."));
                    obtainModule.AddOption("I'll find those ingredients.",
                        plw => true,
                        plw => pl.SendMessage("You set off to gather Dragon's Blood and Mandrake Root."));
                    pl.SendGump(new DialogueGump(pl, teachPoisonModule));
                });
            return obtainModule;
        }

        private DialogueModule CreateMoralityModule()
        {
            DialogueModule moralityModule = new DialogueModule("Morality is a construct of the weak. In this world, power is what matters. If you can eliminate your enemies without bloodshed, why not embrace it?");
            moralityModule.AddOption("You seem to have a dark view of the world.",
                p => true,
                p => p.SendMessage("Darkness is where I thrive. The light is blinding and reveals nothing. Embrace the shadows, and you will find the truth."));
            moralityModule.AddOption("What about the victims of your poisons?",
                pl => true,
                pl => 
                {
                    DialogueModule victimModule = new DialogueModule("The weak should fear the strong. Those who cannot protect themselves deserve their fate. Efficiency in death is a service to the world.");
                    victimModule.AddOption("You have a chilling perspective.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                    victimModule.AddOption("Perhaps there's a better way.",
                        p => true,
                        p => 
                        {
                            DialogueModule betterWayModule = new DialogueModule("Better? Perhaps. But better is often a mask for weakness. In the end, only the cunning survive.");
                            betterWayModule.AddOption("I think I prefer my own path.",
                                ple => true,
                                ple => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            victimModule.AddOption("You may be right.",
                                plr => true,
                                plr => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, betterWayModule));
                        });
                    pl.SendGump(new DialogueGump(pl, victimModule));
                });
            return moralityModule;
        }

        private DialogueModule CreateHobbiesModule()
        {
            DialogueModule hobbiesModule = new DialogueModule("I enjoy studying the anatomy of creatures and experimenting with different toxins. It’s a thrill to see how each reacts.");
            hobbiesModule.AddOption("What do you gain from this knowledge?",
                p => true,
                p => 
                {
                    DialogueModule gainModule = new DialogueModule("Knowledge is power, my friend. With the right poison, I can become untouchable. Imagine the enemies that will fall before me without a fight.");
                    gainModule.AddOption("You seem obsessed with power.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    gainModule.AddOption("Is power all that matters?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule powerModule = new DialogueModule("Power is everything in this world. Without it, you are prey to those who are stronger. I have no intention of being hunted.");
                            powerModule.AddOption("I see your point.",
                                pz => true,
                                pz => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                            powerModule.AddOption("There must be more to life.",
                                px => true,
                                px => 
                                {
                                    DialogueModule moreModule = new DialogueModule("More? Perhaps. But I choose to embrace my nature. Shadows are where I feel most alive.");
                                    moreModule.AddOption("I will take my leave then.",
                                        plc => true,
                                        plc => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    p.SendGump(new DialogueGump(p, moreModule));
                                });
                            p.SendGump(new DialogueGump(p, powerModule));
                        });
                    p.SendGump(new DialogueGump(p, gainModule));
                });
            return hobbiesModule;
        }

        private DialogueModule CreateSecretModule()
        {
            DialogueModule secretModule = new DialogueModule("Ravens are symbols of mystery and change. Their feathers hold power and significance. Return with one, and we shall proceed.");
            secretModule.AddOption("I'll search for a raven's feather.",
                p => true,
                p => p.SendMessage("You set off to find a raven's feather."));
            return secretModule;
        }

        public NoxNora(Serial serial) : base(serial) { }

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
