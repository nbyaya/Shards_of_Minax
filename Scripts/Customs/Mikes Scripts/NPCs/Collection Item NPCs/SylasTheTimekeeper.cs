using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class SylasTheTimekeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public SylasTheTimekeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Sylas the Timekeeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2125)); // A deep blue robe
        AddItem(new Sandals(1150)); // Light blue sandals
        AddItem(new WideBrimHat(2406)); // A dark grey hat
        AddItem(new ClockworkAssembly()); // A unique trinket visible on the character

        VirtualArmor = 15;
    }

    public SylasTheTimekeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Sylas, the keeper of moments, memories, and melodies. Do you know, I believe that music can bring joy and hope to even the darkest of places? Have you perhaps stumbled upon something rare, like a MarbleHourglass?");
        
        // Dialogue options
        greeting.AddOption("Who are you, really?", 
            p => true, 
            p =>
            {
                DialogueModule whoAreYouModule = new DialogueModule("Ah, a curious spirit! I like that. Allow me to introduce myself properly. I am Sylas, a wandering bard, a weaver of tales, and a keeper of time. I have traveled far and wide, spreading songs of heroism, love, and hope. The world may be broken, but with music, we can bind the pieces together. If you have a MarbleHourglass, I may have something special for you.");
                
                whoAreYouModule.AddOption("Tell me more about your travels.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule travelsModule = new DialogueModule("Ah, where to begin? I've journeyed through the sun-baked sands of the Desert of Forgotten Kings, where I sang songs to lift the spirits of weary travelers. I've seen the crystal caverns of Elyndor, where echoes of my lute intertwined with the shimmering stones to create melodies that even the stones seemed to dance to. Every place has a story, and every story has a song. Perhaps, one day, I'll sing one of your tales as well.");
                        travelsModule.AddOption("That sounds wonderful! Could you share one of your songs?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule songModule = new DialogueModule("Ah, a fellow lover of music! Let me share with you the Ballad of the Endless Dawn, a tale of resilience and hope. *Sylas begins to play his lute, his fingers dancing across the strings as he sings of a hero who faced the darkness and never gave up, bringing light to the world once more.*");
                                songModule.AddOption("That was beautiful. Thank you, Sylas.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Sylas smiles warmly, his eyes twinkling. 'The pleasure was mine, friend. May your path be filled with light.'");
                                    });
                                songModule.AddOption("Do you have any more stories to share?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule moreStoriesModule = new DialogueModule("Oh, I have countless tales! There is the story of the Skyfire Phoenix, a majestic bird that rose from the ashes to bring warmth to a frozen village. Or the Legend of the Whispering Woods, where the trees themselves whispered secrets to those who dared to listen. Every story holds a lesson, and every lesson a spark of hope.");
                                        moreStoriesModule.AddOption("Tell me about the Skyfire Phoenix.",
                                            plaax => true,
                                            plaax =>
                                            {
                                                plaax.SendMessage("Sylas's eyes light up as he begins. 'Ah, the Skyfire Phoenix... A bird of fire and rebirth. It came to a village on the brink of despair, its flames melting the ice that had gripped their homes and hearts. It taught them that no matter how cold and dark things become, warmth and hope are always possible.'");
                                            });
                                        moreStoriesModule.AddOption("Tell me about the Whispering Woods.",
                                            plaax => true,
                                            plaax =>
                                            {
                                                plaax.SendMessage("Sylas lowers his voice, as if sharing a secret. 'The Whispering Woods... A place where the trees themselves hold ancient knowledge. The villagers learned to listen to the whispers, discovering secrets that saved them from an unseen danger. It is said that those who enter with a pure heart can still hear the whispers today.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, moreStoriesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, songModule));
                            });
                        travelsModule.AddOption("What kind of bargain are you offering?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("If you bring me a MarbleHourglass, I will offer you a choice: the ImperiumCrest, a symbol of power, or a FancyShipWheel, a fitting prize for any seasoned sailor. And because every good deed deserves a song, I'll also give you a MaxxiaScroll. A little extra for those who believe in hope.");
                                tradeIntroductionModule.AddOption("Let's make the trade.",
                                    plaa => CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have the MarbleHourglass?");
                                        tradeModule.AddOption("Yes, here it is.",
                                            plaaa => HasMarbleHourglass(plaaa) && CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        tradeModule.AddOption("No, I don't have one yet.",
                                            plaaa => !HasMarbleHourglass(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Come back when you have a MarbleHourglass.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll return later.",
                                            plaaa => !CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                tradeIntroductionModule.AddOption("Maybe some other time.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, travelsModule));
                    });
                p.SendGump(new DialogueGump(p, whoAreYouModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Sylas nods knowingly, his eyes glinting with hidden knowledge and warmth. 'May your path be filled with melodies and moments worth keeping.'");
            });

        return greeting;
    }

    private bool HasMarbleHourglass(PlayerMobile player)
    {
        // Check the player's inventory for MarbleHourglass
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MarbleHourglass)) != null;
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
        // Remove the MarbleHourglass and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item marbleHourglass = player.Backpack.FindItemByType(typeof(MarbleHourglass));
        if (marbleHourglass != null)
        {
            marbleHourglass.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for ImperiumCrest and FancyShipWheel
            rewardChoiceModule.AddOption("ImperiumCrest", pl => true, pl =>
            {
                pl.AddToBackpack(new ImperiumCrest());
                pl.SendMessage("You receive an ImperiumCrest!");
            });

            rewardChoiceModule.AddOption("FancyShipWheel", pl => true, pl =>
            {
                pl.AddToBackpack(new FancyShipWheel());
                pl.SendMessage("You receive a FancyShipWheel!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a MarbleHourglass.");
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