using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EliraTheStarSeer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EliraTheStarSeer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elira the Star-Seer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1366)); // Midnight blue robe
        AddItem(new Sandals(1109)); // Dark sandals
        AddItem(new Cloak(1150)); // Starry-patterned cloak
        AddItem(new WizardsHat(1153)); // A deep blue wizard's hat
        AddItem(new CrystalBall()); // Decorative crystal ball in her inventory

        VirtualArmor = 15;
    }

    public EliraTheStarSeer(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule(player);
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule(PlayerMobile player)
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Elira, a seeker of the secrets hidden in the stars. Do you wish to peer into the unknown?");
        
        // Dialogue options
        greeting.AddOption("What secrets do you see?", 
            p => true, 
            p =>
            {
                DialogueModule starsModule = new DialogueModule("The stars speak of many things—fates untold, the paths we walk, and even treasures hidden from common eyes. I sense you may hold a SilverMirror, a relic that I could use for my scrying.");
                
                starsModule.AddOption("Do you wish to trade for the SilverMirror?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a SilverMirror, I would gladly offer you a choice between a MarbleHourglass or a KnightStone, along with a special scroll.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SilverMirror for me?");
                                tradeModule.AddOption("Yes, I have a SilverMirror.", 
                                    plaa => HasSilverMirror(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasSilverMirror(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SilverMirror.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });
                        
                        tradeIntroductionModule.AddOption("Before we trade, tell me more about you.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule backstoryModule = new DialogueModule("I was once a simple barmaid in a small village, serving drinks to travelers and listening to their tales. But the horrors I saw one fateful night changed everything. A darkness crept into our tavern, and I watched as shadows took the lives of everyone I knew. The stars called to me after that—they were my only solace, and I learned to read their wisdom. Now I pass those warnings onto others, in hopes they might avoid the tragedies I could not.");
                                backstoryModule.AddOption("That must have been difficult. How do you carry on?", 
                                    plb => true, 
                                    plb =>
                                    {
                                        DialogueModule resilienceModule = new DialogueModule("It is difficult, yes. But life does not allow us to simply stop, does it? I find purpose in warning others. The stars tell me things that many cannot comprehend—sometimes they show me visions of those who will face dangers, and I do my best to steer them away. Even if my words seem cryptic, it is all in the hope that someone will listen and survive.");
                                        resilienceModule.AddOption("I understand. The world can be cruel.", 
                                            plc => true, 
                                            plc =>
                                            {
                                                plc.SendMessage("Elira nods solemnly, her eyes glinting with distant memories.");
                                            });
                                        resilienceModule.AddOption("The stars must hold incredible power.", 
                                            plc => true, 
                                            plc =>
                                            {
                                                DialogueModule powerModule = new DialogueModule("They do indeed. The stars guide us—they see far beyond what our simple eyes can perceive. They have been my teachers and my companions when all else was lost. I share this knowledge with those who are willing to listen, but it comes with a price. The SilverMirror, for instance, is not just any trinket—it reflects more than mere appearance, it reflects one's essence.");
                                                powerModule.AddOption("What does it show you?", 
                                                    pld => true, 
                                                    pld =>
                                                    {
                                                        DialogueModule essenceModule = new DialogueModule("The SilverMirror shows me the potential within. It is a rare relic, one that can be used to glimpse the truth hidden beneath layers of one's being. In the hands of a seer, it can reveal paths and choices that others cannot see. But it is not without its risks—the truth can be both enlightening and dangerous.");
                                                        essenceModule.AddOption("I see. Perhaps it is best to keep such power in your hands.", 
                                                            ple => true, 
                                                            ple =>
                                                            {
                                                                ple.SendMessage("Elira smiles faintly, her gaze softening. 'Perhaps, traveler. But should you find one, I hope you will consider bringing it to me.'");
                                                            });
                                                        essenceModule.AddOption("I'm intrigued. I will seek a SilverMirror.", 
                                                            ple => true, 
                                                            ple =>
                                                            {
                                                                ple.SendMessage("Elira nods. 'Then may the stars guide you, brave traveler.'");
                                                            });
                                                        pld.SendGump(new DialogueGump(pld, essenceModule));
                                                    });
                                                plc.SendGump(new DialogueGump(plc, powerModule));
                                            });
                                        plb.SendGump(new DialogueGump(plb, resilienceModule));
                                    });
                                
                                backstoryModule.AddOption("The stars must tell you many things.", 
                                    plb => true, 
                                    plb =>
                                    {
                                        DialogueModule knowledgeModule = new DialogueModule("Yes, indeed. They tell of danger, of opportunity, of the unseen currents that shape our world. The stars have no favorites, they simply illuminate what is to come. Many would think me mad for listening, but those who have heeded my warnings often find themselves spared from ill fate. Perhaps the stars brought you here for a reason, as well.");
                                        knowledgeModule.AddOption("Perhaps they did. I will listen closely.", 
                                            plc => true, 
                                            plc =>
                                            {
                                                plc.SendMessage("Elira smiles, a hint of warmth breaking through her otherwise distant demeanor.");
                                            });
                                        knowledgeModule.AddOption("I will heed your words, Elira.", 
                                            plc => true, 
                                            plc =>
                                            {
                                                plc.SendMessage("Elira bows her head slightly. 'May you walk safely, wherever the stars lead you.'");
                                            });
                                        plb.SendGump(new DialogueGump(plb, knowledgeModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, backstoryModule));
                            });

                        tradeIntroductionModule.AddOption("Maybe another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, starsModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Elira nods, her eyes shimmering with the light of distant stars.");
            });

        return greeting;
    }

    private bool HasSilverMirror(PlayerMobile player)
    {
        // Check the player's inventory for SilverMirror
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SilverMirror)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check if the player can trade, based on the 10-minute cooldown
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the SilverMirror and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item silverMirror = player.Backpack.FindItemByType(typeof(SilverMirror));
        if (silverMirror != null)
        {
            silverMirror.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for MarbleHourglass and KnightStone
            rewardChoiceModule.AddOption("MarbleHourglass", pl => true, pl => 
            {
                pl.AddToBackpack(new MarbleHourglass());
                pl.SendMessage("You receive a MarbleHourglass!");
            });
            
            rewardChoiceModule.AddOption("KnightStone", pl => true, pl =>
            {
                pl.AddToBackpack(new KnightStone());
                pl.SendMessage("You receive a KnightStone!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SilverMirror.");
        }
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}