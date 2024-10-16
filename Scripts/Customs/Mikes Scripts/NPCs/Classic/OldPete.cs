using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Old Pete")]
    public class OldPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OldPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Old Pete";
            Body = 0x190; // Human male body

            // Stats
            SetStr(40);
            SetDex(30);
            SetInt(30);
            SetHits(40);

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new SkullCap() { Hue = 1153 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize the lastRewardTime to a past time
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
            DialogueModule greeting = new DialogueModule("Can't you see I'm busy here, laddie? What do you want?");
            
            greeting.AddOption("Tell me your name.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I'm Old Pete. Been on these streets longer than most can remember.")));
                });

            greeting.AddOption("What do you think about health?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Health? You think a beggar like me worries about health? I ain't had a proper meal in weeks. My health is the least of my concerns.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Job? You're lookin' at it, lad. Beggin' is my profession! And it's a hard one at that. You wouldn't believe the things I've seen.")));
                });

            greeting.AddOption("Tell me about your battles.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("True valor, huh? What's that got to do with a beggar's life? My battles are with hunger, cold, and indifference.")));
                });

            greeting.AddOption("Do you know what hunger feels like?",
                player => true,
                player =>
                {
                    DialogueModule hungerModule = new DialogueModule("Answer me this: do you know the taste of hunger, lad? It gnaws at you, day in, day out. The past two weeks? I’ve had to eat scraps from the garbage!");
                    
                    hungerModule.AddOption("Garbage? What was it like?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Aye, it was vile! Stale bread, rotting vegetables... sometimes I’d find a half-eaten apple. It’s hard to describe the taste, but desperate times call for desperate measures.")));
                        });

                    hungerModule.AddOption("Why not just ask for help?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule helpModule = new DialogueModule("Oh, lad, pride gets in the way. A beggar has his dignity, you know? Besides, not everyone is kind. Most just walk on by, pretending not to see.");
                            
                            helpModule.AddOption("But surely someone has helped you?",
                                p => true,
                                p =>
                                {
                                    DialogueModule kindnessModule = new DialogueModule("Aye, every once in a while, a good soul stops and lends me a hand. Just yesterday, a kind woman gave me a crust of bread. It's them small gestures that give me hope.");
                                    
                                    kindnessModule.AddOption("Do you remember her name?",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I believe her name was Elaina. She has a soft heart, and I can see the kindness in her eyes. Rare to find these days.")));
                                        });
                                    
                                    kindnessModule.AddOption("Sounds like a good soul.",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Aye, she is. But they are few and far between. Most folks are too busy with their own lives to care for a beggar like me.")));
                                        });

                                    p.SendGump(new DialogueGump(p, kindnessModule));
                                });

                            pl.SendGump(new DialogueGump(pl, helpModule));
                        });

                    player.SendGump(new DialogueGump(player, hungerModule));
                });

            greeting.AddOption("What do you think of the streets?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("These streets have history. From joyous parades to dark shadows lurking in the alleys. But they've always been my home. It's a tough life, though.")));
                });

            greeting.AddOption("What about the shadows?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Oh, the tales I could tell. Seen shady dealings and cloaked figures passing secrets. But a beggar like me? I keep my head down and stay out of it. Safety first, after all.")));
                });

            greeting.AddOption("What can you tell me about kindness?",
                player => true,
                player =>
                {
                    DialogueModule kindnessModule = new DialogueModule("Aye, every once in a while, a good soul stops and lends me a hand. It's them small gestures that give me hope. Speaking of which, I remember something about a mantra.");
                    
                    kindnessModule.AddOption("What do you know about the mantra?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Heard some monks whispering once. Said the third syllable of the mantra of Compassion is MUH. Don't know much else about it, but it stuck with me.")));
                        });
                    
                    kindnessModule.AddOption("Forget it.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });

                    player.SendGump(new DialogueGump(player, kindnessModule));
                });

            greeting.AddOption("What do you think of caring?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                            player.AddToBackpack(new MaxxiaScroll()); // Reward item
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            player.SendGump(new DialogueGump(player, new DialogueModule("Here you go, a charm imbued with nature's essence. It might bring you luck on your journeys.")));
                    }
                });

            return greeting;
        }

        public OldPete(Serial serial) : base(serial) { }

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
