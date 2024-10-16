using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TiroTheLamplighter : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TiroTheLamplighter() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tiro the Lamplighter";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(80);

        SetHits(100);
        SetMana(100);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Cloak(1255)); // Dark blue cloak
        AddItem(new FancyShirt(1150)); // Black shirt
        AddItem(new LongPants(1109)); // Dark green pants
        AddItem(new Boots(1109)); // Black boots
        AddItem(new Lantern() { Movable = false }); // Lantern in hand to fit his lore

        VirtualArmor = 15;
    }

    public TiroTheLamplighter(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Tiro, keeper of the lampposts that light our paths. Do you happen to have a ColoredLamppost on you? I may have something for you in return.");

        // Dialogue options
        greeting.AddOption("Who are you, Tiro?", 
            p => true, 
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I have been wandering these lands for years, lighting lamps to guide lost souls in the dark. Every lamppost I maintain is a beacon of hope, but it is always a challenge to find enough materials to keep the lights burning brightly.");
                aboutModule.AddOption("Can I help with something?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Indeed you can! If you have a ColoredLamppost, I can offer you a FishBasket or TinTub in exchange. You'll also get a MaxxiaScroll as a token of my appreciation. Remember, I can only trade once every 10 minutes per traveler.");
                        tradeModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule confirmTradeModule = new DialogueModule("Do you have a ColoredLamppost for me?");
                                confirmTradeModule.AddOption("Yes, I have a ColoredLamppost.", 
                                    plaa => HasColoredLamppost(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                confirmTradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasColoredLamppost(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a ColoredLamppost.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                confirmTradeModule.AddOption("I traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, confirmTradeModule));
                            });
                        tradeModule.AddOption("Maybe another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        // Add paranoid ramblings due to Tiro's investigative background
                        tradeModule.AddOption("Why are you really out here, Tiro?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule paranoidModule = new DialogueModule("Why am I out here? Ah, you must be one of them, aren't you? I knew it. They're always watching, always listening. The Cult... they think I don't know, but I see their shadows everywhere. The lamps, they keep the darkness at bay, but it's never enough.");
                                paranoidModule.AddOption("The Cult? Tell me more.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule cultModule = new DialogueModule("The Cult of the Shrouded Flame. They control the dark corners of this world, twisting minds and spreading their corruption. I uncovered their secrets once, and now I can't escape them. Every flicker of a lamppost is a sign—sometimes hope, sometimes warning. They want me silenced.");
                                        cultModule.AddOption("How do you fight them?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule fightModule = new DialogueModule("Fight them? Ha! You can't fight shadows with a sword. You need light, truth, and a relentless spirit. That's why I need the lampposts. They think I'm just a lamplighter, but I'm keeping them away, exposing their movements. The more lights we have, the fewer shadows they can hide in.");
                                                fightModule.AddOption("Can I help you fight them?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("You wish to help? You're either brave or foolish. If you truly want to assist, bring me ColoredLampposts, and I shall continue my work. But beware, once you get involved, there's no turning back. They will come for you too.");
                                                        helpModule.AddOption("I'm ready for whatever comes.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Tiro nods with a mix of admiration and sadness. 'Then we stand together, against the darkness.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        helpModule.AddOption("This is too much for me.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Tiro looks at you with a knowing gaze. 'Few have the courage. I understand. Stay safe, traveler.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });
                                                fightModule.AddOption("I need time to think.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Tiro nods. 'Take all the time you need. Just remember, the shadows don't wait.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, fightModule));
                                            });
                                        cultModule.AddOption("This sounds dangerous. I should go.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Tiro sighs deeply. 'Yes, it's not for the faint of heart. Stay vigilant, traveler.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, cultModule));
                                    });
                                paranoidModule.AddOption("You're just being paranoid.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Tiro narrows his eyes. 'Perhaps. Or perhaps you're too blind to see. Either way, be careful in the dark.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, paranoidModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                p.SendGump(new DialogueGump(p, aboutModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Tiro nods solemnly and continues his watch over the lamps.");
            });

        return greeting;
    }

    private bool HasColoredLamppost(PlayerMobile player)
    {
        // Check the player's inventory for ColoredLamppost
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ColoredLamppost)) != null;
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
        // Remove the ColoredLamppost and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item coloredLamppost = player.Backpack.FindItemByType(typeof(ColoredLamppost));
        if (coloredLamppost != null)
        {
            coloredLamppost.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for FishBasket and TinTub
            rewardChoiceModule.AddOption("FishBasket", pl => true, pl =>
            {
                pl.AddToBackpack(new FishBasket());
                pl.SendMessage("You receive a FishBasket!");
            });

            rewardChoiceModule.AddOption("TinTub", pl => true, pl =>
            {
                pl.AddToBackpack(new TinTub());
                pl.SendMessage("You receive a TinTub!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a ColoredLamppost.");
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