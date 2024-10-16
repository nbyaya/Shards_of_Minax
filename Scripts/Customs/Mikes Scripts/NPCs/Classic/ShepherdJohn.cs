using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd John")]
    public class ShepherdJohn : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdJohn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd John";
            Body = 0x190; // Human male body

            // Stats
            SetStr(85);
            SetDex(60);
            SetInt(45);
            SetHits(75);

            // Appearance
            AddItem(new LongPants() { Hue = 1121 });
            AddItem(new FancyShirt() { Hue = 1122 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd John's Crook" });

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Shepherd John, a humble shepherd. But if you ask me, cows are where the heart is! What brings you here?");

            greeting.AddOption("Why do you prefer cows over sheep?",
                player => true,
                player =>
                {
                    DialogueModule cowPreferenceModule = new DialogueModule("Cows have a charm that sheep just can't match. They're strong, majestic creatures, and they have a way of understanding me. Unlike sheep, who just bleat and wander!");
                    
                    cowPreferenceModule.AddOption("What do you like most about cows?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cowLikesModule = new DialogueModule("For one, they're more reliable! Cows have this calm demeanor that makes them easier to manage. And have you ever seen a cow galloping? It's quite the sight!");
                            
                            cowLikesModule.AddOption("Do you have a favorite breed?",
                                p => true,
                                p =>
                                {
                                    DialogueModule breedModule = new DialogueModule("I’m particularly fond of the Hereford breed. Their reddish-brown color and white faces make them stand out. Plus, they're great milk producers!");
                                    
                                    breedModule.AddOption("What about sheep?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule sheepModule = new DialogueModule("Sheep are alright, but they can be so skittish! They seem to panic over the slightest thing. I prefer the steadiness of cows.");
                                            sheepModule.AddOption("Have you always been a shepherd?",
                                                pw => true,
                                                pw =>
                                                {
                                                    p.SendGump(new DialogueGump(p, CreateLifeStoryModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, sheepModule));
                                        });
                                    
                                    player.SendGump(new DialogueGump(player, breedModule));
                                });

                            player.SendGump(new DialogueGump(player, cowLikesModule));
                        });

                    cowPreferenceModule.AddOption("Do you wish you could be something else?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule wishModule = new DialogueModule("Oh, absolutely! I often dream of being a cool cowboy, riding the open plains, lasso in hand, living free under the stars.");
                            
                            wishModule.AddOption("What would you do as a cowboy?",
                                p => true,
                                p =>
                                {
                                    DialogueModule cowboyDreamsModule = new DialogueModule("I'd roam the vast fields, drive cattle, and maybe even have a trusty steed by my side. There's something romantic about that life, don’t you think?");
                                    
                                    cowboyDreamsModule.AddOption("That sounds adventurous!",
                                        ple => true,
                                        ple =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    
                                    cowboyDreamsModule.AddOption("Do you have a cowboy name?",
                                        plr => true,
                                        plr =>
                                        {
                                            DialogueModule nameModule = new DialogueModule("Ha! I'd like to think of myself as 'Cowboy John,' the swift and clever cattle wrangler! Just picture it—me in a hat, boots, and a bandana!");
                                            nameModule.AddOption("You should totally do that!",
                                                pt => true,
                                                pt =>
                                                {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, nameModule));
                                        });

                                    player.SendGump(new DialogueGump(player, cowboyDreamsModule));
                                });

                            player.SendGump(new DialogueGump(player, wishModule));
                        });

                    player.SendGump(new DialogueGump(player, cowPreferenceModule));
                });

            greeting.AddOption("What do you enjoy about being a shepherd?",
                player => true,
                player =>
                {
                    DialogueModule joyModule = new DialogueModule("Being a shepherd allows me to connect with nature. The peaceful grazing of my flock and the fresh air—it’s a simple, yet fulfilling life.");
                    
                    joyModule.AddOption("Do you have any interesting stories?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule storyModule = new DialogueModule("Oh, plenty! There was a time I had to chase a runaway sheep through the hills. It was quite the adventure!");
                            
                            storyModule.AddOption("What happened?",
                                p => true,
                                p =>
                                {
                                    DialogueModule chaseModule = new DialogueModule("That little rascal dashed off, thinking it was a game! I ended up slipping and tumbling down a hill, covered in mud, but I caught him in the end.");
                                    
                                    chaseModule.AddOption("Sounds hilarious!",
                                        ply => true,
                                        ply =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    
                                    chaseModule.AddOption("I’d love to hear more!",
                                        plu => true,
                                        plu =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateLifeStoryModule())); // Loop back to life story
                                        });

                                    player.SendGump(new DialogueGump(player, chaseModule));
                                });
                            
                            player.SendGump(new DialogueGump(player, storyModule));
                        });

                    player.SendGump(new DialogueGump(player, joyModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Safe travels, traveler. Remember, cows are the real treasures of the land!");
                });

            return greeting;
        }

        private DialogueModule CreateLifeStoryModule()
        {
            DialogueModule lifeStoryModule = new DialogueModule("Ah, my life story! It’s a simple one. Born in a small village, I grew up surrounded by nature and animals. My parents were farmers, and I was always drawn to the animals, especially cows.");

            lifeStoryModule.AddOption("What was your childhood like?",
                player => true,
                player =>
                {
                    DialogueModule childhoodModule = new DialogueModule("My childhood was filled with laughter and adventure! I spent my days exploring the fields and learning the ways of the land. I often played with the calves, dreaming of the day I'd be a great shepherd.");
                    
                    childhoodModule.AddOption("Did you have any friends?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule friendsModule = new DialogueModule("I did! My best friend was a boy named Tommy. We shared dreams of being cowboys and spent countless hours pretending to ride the range.");
                            
                            friendsModule.AddOption("What happened to Tommy?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Loop back to greeting
                                });
                            
                            player.SendGump(new DialogueGump(player, friendsModule));
                        });

                    player.SendGump(new DialogueGump(player, childhoodModule));
                });

            return lifeStoryModule;
        }

        public ShepherdJohn(Serial serial) : base(serial) { }

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
