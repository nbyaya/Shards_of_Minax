using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Talor the Tameless")]
    public class TalorTheTameless : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TalorTheTameless() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Talor the Tameless";
            Body = 0x190; // Human male body

            // Stats
            SetStr(96);
            SetDex(90);
            SetInt(85);
            SetHits(96);

            // Appearance
            AddItem(new Kilt() { Hue = 64 });
            AddItem(new Doublet() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1174 });
            AddItem(new ShepherdsCrook() { Name = "Talor's Wild Rod" });

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
            DialogueModule greeting = new DialogueModule("I am Talor the Tameless, the master of creatures! How may I assist you today?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateHealthModule())); });
                
            greeting.AddOption("What is your job?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateJobModule())); });
                
            greeting.AddOption("What virtues do you hold dear?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateVirtuesModule())); });
                
            greeting.AddOption("Tell me about the creatures.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateCreaturesModule())); });
                
            greeting.AddOption("Do you have any secrets?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateSecretModule(player))); });
                
            greeting.AddOption("What do you know about nature's medicine?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateMedicineModule())); });

            greeting.AddOption("What happened to your donkey?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDonkeyModule())); });

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            return new DialogueModule("I am in perfect health, thanks to my animal friends!");
        }

        private DialogueModule CreateJobModule()
        {
            return new DialogueModule("My job is to tame and care for the creatures of this land.");
        }

        private DialogueModule CreateVirtuesModule()
        {
            return new DialogueModule("Compassion is my guiding virtue, for it takes a kind heart to understand and tame wild creatures. What virtues do you hold dear?");
        }

        private DialogueModule CreateCreaturesModule()
        {
            return new DialogueModule("Ah, creatures! From the mighty dragon to the meek rabbit, I've befriended them all. One creature, in particular, holds a secret. Care to know more?");
        }

        private DialogueModule CreateSecretModule(PlayerMobile player)
        {
            var secretModule = new DialogueModule("The secret lies with the enigmatic Griffin. A creature of majesty and mystery. If you ever find one, remember to approach with respect. For your efforts in seeking knowledge, accept this gift.");
            if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
            {
                lastRewardTime = DateTime.UtcNow;
                player.AddToBackpack(new EnergyResistAugmentCrystal()); // Reward
                secretModule.NPCText += " You've received an Energy Resist Augment Crystal!";
            }
            else
            {
                secretModule.NPCText = "Please return later for your reward.";
            }
            return secretModule;
        }

        private DialogueModule CreateMedicineModule()
        {
            return new DialogueModule("Nature's medicine is powerful. From the bark of the elder tree to the petals of the moonflower, each has its healing property. Seek the Grove of Tranquility for rare herbs.");
        }

        private DialogueModule CreateDonkeyModule()
        {
            DialogueModule donkeyModule = new DialogueModule("Ah, my beloved donkey, Bramble! He was no ordinary donkey; he was a rare creature known for his keen sense of direction and remarkable strength. I lost him while foraging for herbs in the Whispering Forest.");
            
            donkeyModule.AddOption("What makes Bramble so special?",
                player => true,
                player =>
                {
                    DialogueModule specialModule = new DialogueModule("Bramble has a unique ability to find hidden paths and rare plants. His coat shines like silver under the moonlight, and he has a gentle heart.");
                    specialModule.AddOption("What do you plan to do to find him?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule planModule = new DialogueModule("I plan to search the Whispering Forest and ask other creatures if they've seen him. If only I had a way to track him!");
                            planModule.AddOption("Maybe I can help you find Bramble.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule helpModule = new DialogueModule("Would you? That would mean the world to me! Bramble loves apples. If you can find some, it might lure him back to me!");
                                    helpModule.AddOption("Where can I find these apples?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule locationModule = new DialogueModule("You can find the Golden Apples in the Orchard of Elders, just south of here. Be wary; the path can be treacherous!");
                                            locationModule.AddOption("I'll go to the Orchard and return with apples.",
                                                plq => true,
                                                plq =>
                                                {
                                                    pl.SendMessage("You set off towards the Orchard of Elders.");
                                                });
                                            locationModule.AddOption("That sounds too dangerous for me.",
                                                plw => true,
                                                plw =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, locationModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, helpModule));
                                });
                            planModule.AddOption("I hope you find him soon.",
                                p => true,
                                p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                            player.SendGump(new DialogueGump(player, planModule));
                        });
                    specialModule.AddOption("I'd like to help find him.",
                        playere => true,
                        playere =>
                        {
                            player.SendGump(new DialogueGump(player, CreateDonkeyModule())); // Loop back for more discussion
                        });
                    player.SendGump(new DialogueGump(player, specialModule));
                });
            
            donkeyModule.AddOption("What do you miss most about him?",
                player => true,
                player =>
                {
                    DialogueModule missModule = new DialogueModule("I miss his gentle braying and the way he would nuzzle against me. He had a calming presence that could soothe even the fiercest beast.");
                    missModule.AddOption("Have you tried calling for him?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule callModule = new DialogueModule("I have, but it seems my voice is lost in the rustling leaves. Perhaps if I had someone to help me call for him!");
                            callModule.AddOption("I can help you call for him.",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("You call for Bramble, hoping he hears you.");
                                });
                            callModule.AddOption("Maybe we should continue searching.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateDonkeyModule())); // Loop back for more discussion
                                });
                            player.SendGump(new DialogueGump(player, callModule));
                        });
                    player.SendGump(new DialogueGump(player, missModule));
                });
            
            donkeyModule.AddOption("Tell me about the Whispering Forest.",
                player => true,
                player =>
                {
                    DialogueModule forestModule = new DialogueModule("The Whispering Forest is filled with ancient trees that seem to talk to one another. The air is thick with magic, and many creatures call it home. It can be dangerous, but it holds secrets.");
                    forestModule.AddOption("What kind of dangers should I expect?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangerModule = new DialogueModule("There are many wild creatures in the forest. Some are protective of their territory, while others are curious but not necessarily friendly. Keep your wits about you!");
                            dangerModule.AddOption("Thank you for the warning.",
                                p => true,
                                p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                            player.SendGump(new DialogueGump(player, dangerModule));
                        });
                    forestModule.AddOption("What secrets do you mean?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretsModule = new DialogueModule("The forest holds ancient runes and forgotten paths. Many believe it leads to hidden treasures and powerful magic. Perhaps Bramble found one of these paths.");
                            secretsModule.AddOption("I'd love to explore the forest with you.",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("You and Talor begin to discuss the possibilities of exploring together.");
                                });
                            secretsModule.AddOption("Maybe I should go alone.",
                                p => true,
                                p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                            player.SendGump(new DialogueGump(player, secretsModule));
                        });
                    player.SendGump(new DialogueGump(player, forestModule));
                });

            return donkeyModule;
        }

        public TalorTheTameless(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
