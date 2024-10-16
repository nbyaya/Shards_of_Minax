using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd Tom")]
    public class ShepherdTom : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdTom() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd Tom";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(60);
            SetInt(50);
            SetHits(70);

            // Appearance
            AddItem(new PlainDress() { Hue = 1113 });
            AddItem(new Sandals() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd Tom's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue;
        }

        public ShepherdTom(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Shepherd Tom. I have a curious tale to share if you're interested. What would you like to know?");
            
            greeting.AddOption("Tell me your tale.",
                player => true,
                player =>
                {
                    DialogueModule taleModule = new DialogueModule("Once, while tending to my flock, I discovered a shimmering portal hidden within the meadows. A mystical sheep wandered through it, beckoning me to follow. It led me to a wondrous world beyond imagination. Would you care to hear more?");
                    taleModule.AddOption("Yes, please continue.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule portalModule = new DialogueModule("As I stepped through the portal, I felt a strange energy coursing through me. The world on the other side was vibrant, filled with colors I've never seen. The sheep danced joyously, and their wool shimmered like starlight. But there was a challenge ahead. Would you dare to follow the sheep?");
                            portalModule.AddOption("I would love to follow the sheep!",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("You feel an irresistible urge to follow the sheep through the shimmering portal!");
                                    pla.SendGump(new DialogueGump(pla, CreateFollowSheepModule()));
                                });
                            portalModule.AddOption("What kind of challenges?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule challengeModule = new DialogueModule("Beyond the portal, I faced trials that tested my courage and wit. Creatures of light and shadow roamed the lands, and I had to earn their trust. Some sought to protect the sheep, while others wished to capture them. Would you be ready for such a journey?");
                                    challengeModule.AddOption("I am ready for anything!",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendMessage("Your determination is admirable! Let us embark on this adventure together!");
                                            pl.SendGump(new DialogueGump(pl, CreateFollowSheepModule()));
                                        });
                                    challengeModule.AddOption("Maybe I'll just listen for now.",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, portalModule));
                                        });
                                    player.SendGump(new DialogueGump(player, challengeModule));
                                });
                            player.SendGump(new DialogueGump(player, portalModule));
                        });
                    taleModule.AddOption("No, I’m not interested.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("Very well! If you change your mind, I'll be here.");
                        });
                    player.SendGump(new DialogueGump(player, taleModule));
                });

            greeting.AddOption("What happened to the sheep?",
                player => true,
                player =>
                {
                    DialogueModule sheepModule = new DialogueModule("The sheep, radiant and wise, became my guide in that world. They shared secrets of the land, teaching me about the balance of nature and magic. Have you ever wondered what wisdom an animal might share?");
                    sheepModule.AddOption("Yes, I believe animals know much.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("Indeed! In that world, the sheep spoke of harmony and the need to protect our realms. Each sheep held a unique story, woven into the fabric of that magical place.");
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    sheepModule.AddOption("I think they're just animals.",
                        pl => true,
                        pl =>
                        {
                            player.SendMessage("Sometimes, wisdom can be found in the most unexpected places. Don't underestimate them!");
                            player.SendGump(new DialogueGump(player, sheepModule));
                        });
                    player.SendGump(new DialogueGump(player, sheepModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
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
                        player.SendMessage("Here, take this token of appreciation for your interest.");
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            return greeting;
        }

        private DialogueModule CreateFollowSheepModule()
        {
            DialogueModule followModule = new DialogueModule("You approach the shimmering portal, your heart racing. As you step through, the world around you transforms into a landscape of wonder. You see the mystical sheep ahead, beckoning you to follow. Where do you wish to go?");
            
            followModule.AddOption("Follow the sheep deeper into the forest.",
                player => true,
                player =>
                {
                    DialogueModule forestModule = new DialogueModule("As you delve deeper, the trees shimmer with magical light, and you hear whispers of ancient spirits. The sheep pause, sensing your presence. They seem to know something you don’t. What will you do?");
                    forestModule.AddOption("Ask the sheep what lies ahead.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("You kneel beside the sheep, seeking their wisdom.");
                            pl.SendGump(new DialogueGump(pl, CreateWisdomModule()));
                        });
                    forestModule.AddOption("Keep moving forward.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("You decide to trust your instincts and push on, the sheep leading the way.");
                            // Continue the adventure logic here...
                        });
                    player.SendGump(new DialogueGump(player, forestModule));
                });

            followModule.AddOption("Investigate a nearby shimmering stream.",
                player => true,
                player =>
                {
                    DialogueModule streamModule = new DialogueModule("The stream sparkles with ethereal light. The sheep gather around it, appearing enchanted. Will you touch the water?");
                    streamModule.AddOption("Yes, I’ll touch the water.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("As your fingers brush the surface, visions of the world flash before your eyes. You see glimpses of the challenges ahead and the rewards for those brave enough to face them.");
                            // Continue the adventure logic here...
                        });
                    streamModule.AddOption("No, let’s keep following the sheep.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("You choose to stay focused on the journey ahead.");
                            // Continue the adventure logic here...
                        });
                    player.SendGump(new DialogueGump(player, streamModule));
                });

            return followModule;
        }

        private DialogueModule CreateWisdomModule()
        {
            DialogueModule wisdomModule = new DialogueModule("The sheep, with wise eyes, seem to ponder your question. They reply in a soft, melodic tone, 'The path ahead is filled with light and shadow. Trust in your heart, for it will guide you through.' What do you wish to ask next?");
            wisdomModule.AddOption("What dangers should I be aware of?",
                pl => true,
                pl =>
                {
                    pl.SendMessage("The sheep inform you of shadowy creatures lurking in the dark corners of the realm, waiting to test your courage. 'But fear not,' they say, 'for courage shines brightest in the face of fear.'");
                    pl.SendGump(new DialogueGump(pl, wisdomModule));
                });
            wisdomModule.AddOption("How can I help the sheep?",
                pl => true,
                pl =>
                {
                    pl.SendMessage("The sheep encourage you to seek the lost ones, for they are in need of guidance. 'Together, we can restore balance to our world.'");
                    pl.SendGump(new DialogueGump(pl, wisdomModule));
                });
            wisdomModule.AddOption("Thank you for your wisdom.",
                pl => true,
                pl =>
                {
                    pl.SendMessage("The sheep nod in appreciation. 'May your journey be blessed.'");
                    pl.SendGump(new DialogueGump(pl, CreateFollowSheepModule()));
                });

            return wisdomModule;
        }

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
