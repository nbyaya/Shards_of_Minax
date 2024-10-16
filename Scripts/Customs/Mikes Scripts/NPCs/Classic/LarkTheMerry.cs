using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lark the Merry")]
    public class LarkTheMerry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LarkTheMerry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lark the Merry";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 90;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new JesterSuit() { Hue = 1153 });
            AddItem(new JesterHat() { Hue = 1153 });
            
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
            DialogueModule greeting = new DialogueModule("Ho ho! Art thou in need of some merriment?");

            greeting.AddOption("What is your name?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I am Lark the Merry, here to jest and entertain!")));
                });

            greeting.AddOption("What do you do?",
                player => true,
                player =>
                {
                    DialogueModule workModule = new DialogueModule("I jest and entertain, and sometimes share riddles! Would you like to hear one?");
                    workModule.AddOption("Yes, tell me a riddle.",
                        pl => true,
                        pl => 
                        {
                            TellRiddle(pl);
                        });
                    workModule.AddOption("No, tell me more about your performances.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule performanceModule = new DialogueModule("Ah, my performances are filled with jests and tales! I often perform at the local tavern. Would you like to join me there sometime?");
                            performanceModule.AddOption("Yes, I would love to!",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Splendid! The tavern is always filled with laughter!")));
                                });
                            performanceModule.AddOption("Maybe another time.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Ah, perhaps I shall see you there another time!")));
                                });
                            pl.SendGump(new DialogueGump(pl, performanceModule));
                        });
                    player.SendGump(new DialogueGump(player, workModule));
                });

            greeting.AddOption("Tell me a riddle.",
                player => true,
                player =>
                {
                    TellRiddle(player);
                });

            greeting.AddOption("What about laughter?",
                player => true,
                player =>
                {
                    DialogueModule laughterModule = new DialogueModule("Ah, the sweet sound of laughter, a balm for weary souls. There's a tale I once heard about a village where laughter was the most treasured possession. Would you like to hear it?");
                    laughterModule.AddOption("Yes, tell me the tale!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("In a faraway village, the people believed that laughter was a gift from the gods. They held an annual festival to celebrate it, where the best jesters competed for the title of 'Jester of Joy'. It was said that the laughter echoed through the mountains for days!")));
                        });
                    laughterModule.AddOption("Maybe later.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("As you wish, my friend! Laughter will wait for no one.")));
                        });
                    player.SendGump(new DialogueGump(player, laughterModule));
                });

            greeting.AddOption("Can you reward me for my wit?",
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
                        player.SendGump(new DialogueGump(player, new DialogueModule("Your wit is commendable! For your keen intellect, I bestow upon you a small reward.")));
                        player.AddToBackpack(new Gold(1000)); // Give 1000 gold as a reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            return greeting;
        }

        private void TellRiddle(PlayerMobile player)
        {
            DialogueModule riddleModule = new DialogueModule("Ah, a riddle for thee! What has keys but opens no doors?");
            riddleModule.AddOption("A piano?",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed, a piano it is! Well guessed! Here’s another: What can you catch but not throw?")));
                    TellSecondRiddle(pl);
                });
            riddleModule.AddOption("Tell me another.",
                pl => true,
                pl =>
                {
                    pl.SendGump(new DialogueGump(pl, new DialogueModule("I have no other riddles at the moment, but do come back!")));
                });
            player.SendGump(new DialogueGump(player, riddleModule));
        }

        private void TellSecondRiddle(PlayerMobile player)
        {
            DialogueModule secondRiddleModule = new DialogueModule("What can you catch but not throw?");
            secondRiddleModule.AddOption("A cold?",
                pl => true,
                pl =>
                {
                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Correct! You're quite sharp, my friend!")));
                });
            secondRiddleModule.AddOption("I don't know.",
                pl => true,
                pl =>
                {
                    pl.SendGump(new DialogueGump(pl, new DialogueModule("The answer is 'a cold.' It's a tricky one, isn't it?")));
                });
            player.SendGump(new DialogueGump(player, secondRiddleModule));
        }

        public LarkTheMerry(Serial serial) : base(serial) { }

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
