using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sultry Sally")]
    public class SultrySally : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SultrySally() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sultry Sally";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 50;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 1150 });
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 2126 });
            AddItem(new GoldEarrings() { Name = "Sally's Earrings" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
            DialogueModule greeting = new DialogueModule("Greetings, darling. I am Sultry Sally. Care for a tale or two about my... colorful clientele?");
            
            greeting.AddOption("Tell me about your worst customers.",
                player => true,
                player =>
                {
                    DialogueModule worstCustomersModule = new DialogueModule("Oh, where do I even begin? There are so many! One of the worst was a man named Boris who thought he could charm me with gold and empty promises.");
                    
                    worstCustomersModule.AddOption("What did he do?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule borisModule = new DialogueModule("He would come in every week, flaunting his wealth and boasting about his adventures. But darling, when it came to paying, he'd always have an excuse. 'My purse was stolen!' he would say. How predictable!");
                            borisModule.AddOption("Did you ever confront him?",
                                p => true,
                                p =>
                                {
                                    DialogueModule confrontationModule = new DialogueModule("Oh, I tried! I told him that if he couldn't pay, he should stop wasting my time. He simply laughed and promised to make it up to me. But he never did.");
                                    confrontationModule.AddOption("What happened to him?",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendMessage("He eventually stopped coming around. Word got out, and I suspect the other ladies had a few words with him.");
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, confrontationModule));
                                });
                            pl.SendGump(new DialogueGump(pl, borisModule));
                        });

                    worstCustomersModule.AddOption("Any other memorable customers?",
                        playerw => true,
                        playerw =>
                        {
                            DialogueModule otherModule = new DialogueModule("Absolutely! There's also the tale of Lady Gwendolyn, a rather... demanding patron. She would arrive with a long list of expectations and a very short temper.");
                            otherModule.AddOption("What did she expect?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule expectationsModule = new DialogueModule("She wanted everything to be perfect—candlelit ambiance, fresh flowers, and the finest wines. But, oh, the moment anything went wrong, her wrath was terrifying!");
                                    expectationsModule.AddOption("Did you ever make a mistake?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule mistakeModule = new DialogueModule("Once, I accidentally served her the wrong vintage. She nearly threw the glass at my head! I learned very quickly to triple-check her orders.");
                                            mistakeModule.AddOption("How did you handle her?",
                                                ple => true,
                                                ple =>
                                                {
                                                    pl.SendMessage("I had to calm her down and promise her a complimentary evening next time. I even apologized profusely, and thankfully, she returned.");
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, mistakeModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, expectationsModule));
                                });
                            player.SendGump(new DialogueGump(player, otherModule));
                        });

                    worstCustomersModule.AddOption("Anyone else worth mentioning?",
                        playerr => true,
                        playerr =>
                        {
                            DialogueModule mentionModule = new DialogueModule("Oh yes! There's that infamous rogue, Jarek. He was charming but utterly unpredictable. You never knew what mood he'd be in.");
                            mentionModule.AddOption("What was he like?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule jarekModule = new DialogueModule("One moment he’d shower you with affection, the next he’d be shouting at the shadows! He once tried to pick a fight with a lantern!");
                                    jarekModule.AddOption("What did you do?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule responseModule = new DialogueModule("I merely stepped aside and let him tire himself out. Eventually, he realized how ridiculous he was being. I think he left embarrassed.");
                                            responseModule.AddOption("Did he ever return?",
                                                pla => true,
                                                pla =>
                                                {
                                                    pla.SendMessage("Oh yes, he came back, but with a much calmer demeanor. I suppose he learned not to provoke the very shadows he feared.");
                                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, responseModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, jarekModule));
                                });
                            player.SendGump(new DialogueGump(player, mentionModule));
                        });

                    player.SendGump(new DialogueGump(player, worstCustomersModule));
                });

            greeting.AddOption("What tales do you have?",
                player => true,
                player =>
                {
                    DialogueModule talesModule = new DialogueModule("Over the years, I've heard many stories. One tale that intrigued me the most is that of the Moonlit Grotto.");
                    talesModule.AddOption("What about the grotto?",
                        pl => true,
                        pl =>
                        {
                            if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                            {
                                pl.SendMessage("I have no reward right now. Please return later.");
                            }
                            else
                            {
                                pl.SendMessage("Ah, the mysterious cavern where legends say the water glows under the moonlight. Here, take this token. It might help you on your way.");
                                pl.AddToBackpack(new BraceletSlotChangeDeed()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            }
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, talesModule));
                });

            greeting.AddOption("Do you have secrets to share?",
                player => true,
                player =>
                {
                    DialogueModule secretsModule = new DialogueModule("Darling, the world holds many mysteries. The best secret? That's the power of love and connection.");
                    secretsModule.AddOption("What do you think about love?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loveModule = new DialogueModule("Love is a force that moves us, binds us, and sometimes breaks us. Have you ever been in love, darling?");
                            loveModule.AddOption("Yes.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            loveModule.AddOption("No.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, loveModule));
                        });
                    player.SendGump(new DialogueGump(player, secretsModule));
                });

            return greeting;
        }

        public SultrySally(Serial serial) : base(serial) { }

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
