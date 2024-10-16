using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Razor Rina")]
    public class RazorRina : BaseCreature
    {
        private DateTime lastLessonTime;
        private DateTime lastManualTime;

        [Constructable]
        public RazorRina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Razor Rina";
            Body = 0x191; // Human female body

            // Stats
            SetStr(90);
            SetDex(70);
            SetInt(80);
            SetHits(90);

            // Appearance
            AddItem(new LeatherCap() { Name = "Rina's Razor Cap" });
            AddItem(new LeatherGloves() { Name = "Rina's Razor Gloves" });
            AddItem(new LeatherArms() { Name = "Rina's Razor Arms" });
            AddItem(new LeatherLegs() { Name = "Rina's Razor Legs" });
            AddItem(new LeatherChest() { Name = "Rina's Razor Chest" });
            AddItem(new Boots() { Name = "Rina's Razor Boots" });
            AddItem(new Kryss() { Name = "Rina's Razor Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the last reward times to a past time
            lastLessonTime = DateTime.MinValue;
            lastManualTime = DateTime.MinValue;
        }

        public RazorRina(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Razor Rina, the rogue. I find great amusement in dealing with the snobby folk around here. What would you like to discuss?");

            greeting.AddOption("What do you have against snobby people?",
                player => true,
                player => 
                {
                    DialogueModule snobbyModule = new DialogueModule("Oh, where do I begin? They strut about like peacocks, thinking they're better than everyone else. They don’t realize how fragile their lives are. I'd love to see them squirm!");
                    
                    snobbyModule.AddOption("That sounds interesting. Tell me more.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule moreAboutSnobsModule = new DialogueModule("It's the attitude that gets to me. They flaunt their wealth and connections, but it’s all just a facade. I’ve made it my personal mission to cut them down to size. It’s quite satisfying, really.");
                            
                            moreAboutSnobsModule.AddOption("How do you plan to do that?",
                                plq => true,
                                plq => 
                                {
                                    DialogueModule planModule = new DialogueModule("Oh, I've got my ways. A little trickery here, a well-placed dagger there... it's all in the art of stealth. You'd be surprised at how easily their arrogance can lead to their downfall.");
                                    
                                    planModule.AddOption("What if they catch you?",
                                        plw => true,
                                        plw => 
                                        {
                                            DialogueModule catchModule = new DialogueModule("Let them try! A true rogue knows how to slip away like a shadow. Besides, I thrive in chaos; it fuels my heart! Their screams of surprise are like music to my ears.");
                                            player.SendGump(new DialogueGump(player, catchModule));
                                        });

                                    planModule.AddOption("What’s the best snobby person you’ve taken down?",
                                        ple => true,
                                        ple => 
                                        {
                                            DialogueModule storyModule = new DialogueModule("Ah, there was this nobleman named Lord Aric. He thought he was untouchable. I snuck into his manor during a grand ball and turned his own guards against him. The look on his face was priceless! His reputation was ruined, and it made my day!");
                                            player.SendGump(new DialogueGump(player, storyModule));
                                        });

                                    player.SendGump(new DialogueGump(player, planModule));
                                });

                            moreAboutSnobsModule.AddOption("Do you think it’s worth the risk?",
                                plt => true,
                                plt => 
                                {
                                    DialogueModule worthModule = new DialogueModule("Absolutely! Each encounter is a thrill. It’s like a dance on the edge of a blade. If you’re not living on the edge, you’re taking up too much space!");
                                    player.SendGump(new DialogueGump(player, worthModule));
                                });

                            player.SendGump(new DialogueGump(player, moreAboutSnobsModule));
                        });

                    snobbyModule.AddOption("I share your disdain for them.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule sharedDisdainModule = new DialogueModule("Ah, a kindred spirit! Together, we could orchestrate some delightful chaos! Imagine the look on their faces when we turn their world upside down!");
                            sharedDisdainModule.AddOption("Count me in! What’s the first step?",
                                ply => true,
                                ply => 
                                {
                                    DialogueModule firstStepModule = new DialogueModule("First, we need to scout out a target. Perhaps a high-profile event where the snobs gather? We could create distractions and make our move while they’re distracted.");
                                    player.SendGump(new DialogueGump(player, firstStepModule));
                                });
                            player.SendGump(new DialogueGump(player, sharedDisdainModule));
                        });

                    player.SendGump(new DialogueGump(player, snobbyModule));
                });

            greeting.AddOption("What’s your favorite method of dealing with them?",
                player => true,
                player => 
                {
                    DialogueModule methodModule = new DialogueModule("A well-placed rumor can do wonders. I love watching them turn on each other! They’re like rats in a cage, and I’m just the one shaking the bars. And if that doesn’t work, a good old-fashioned knife will do!");
                    methodModule.AddOption("You seem to enjoy this a lot.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule enjoyModule = new DialogueModule("Oh, it’s more than just enjoyment; it’s a passion! Each victory against a snob fills me with an exhilarating rush. It’s my art form, my rebellion against their pretentiousness!");
                            player.SendGump(new DialogueGump(player, enjoyModule));
                        });
                    player.SendGump(new DialogueGump(player, methodModule));
                });

            greeting.AddOption("Can you teach me some tricks?",
                player => true,
                player => 
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastLessonTime < cooldown)
                    {
                        DialogueModule lessonCooldownModule = new DialogueModule("I have no lesson right now. Please return later.");
                        player.SendGump(new DialogueGump(player, lessonCooldownModule));
                    }
                    else
                    {
                        Say("Alright then! Here’s a little trick I use to throw them off. Observe and learn.");
                        player.AddToBackpack(new LockpickingAugmentCrystal());
                        lastLessonTime = DateTime.UtcNow; // Update the timestamp
                        DialogueModule lessonModule = new DialogueModule("I've given you a lesson. May it serve you well. Now go, and put it to use against those high-and-mighty snobs!");
                        player.SendGump(new DialogueGump(player, lessonModule));
                    }
                });

            greeting.AddOption("Do you have any stories about your adventures?",
                player => true,
                player => 
                {
                    DialogueModule adventureModule = new DialogueModule("Oh, I've got a treasure trove of stories! Once, I infiltrated a lavish party dressed as a servant. They were so caught up in their own grandeur that they didn’t see me swiping their treasures right under their noses.");
                    
                    adventureModule.AddOption("What did you steal?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule stealModule = new DialogueModule("Jewels, of course! They hoard them like dragons. But it’s not just the loot; it’s the thrill of the hunt! Watching their faces drop when they realize what’s gone—priceless!");
                            player.SendGump(new DialogueGump(player, stealModule));
                        });
                    
                    adventureModule.AddOption("What happened when they found out?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule foundOutModule = new DialogueModule("Chaos erupted! They turned on each other, blaming everyone in sight. I stood back, watching the spectacle unfold. Nothing quite compares to the sound of their panic!");
                            player.SendGump(new DialogueGump(player, foundOutModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, adventureModule));
                });

            greeting.AddOption("What do you think about the Thieves Guild?",
                player => true,
                player => 
                {
                    DialogueModule guildOpinionModule = new DialogueModule("The Thieves Guild? They have their uses, but too many rules! I prefer my freedom to act. No one tells me who to target or how to play the game. I’m a rogue, not a puppet!");
                    
                    guildOpinionModule.AddOption("So you go against their code?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule codeModule = new DialogueModule("Absolutely! I have my own code: take what you want, and never let them see you coming. If the Guild can’t handle my style, that’s their loss!");
                            player.SendGump(new DialogueGump(player, codeModule));
                        });

                    player.SendGump(new DialogueGump(player, guildOpinionModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(lastLessonTime);
            writer.Write(lastManualTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastLessonTime = reader.ReadDateTime();
            lastManualTime = reader.ReadDateTime();
        }
    }
}
