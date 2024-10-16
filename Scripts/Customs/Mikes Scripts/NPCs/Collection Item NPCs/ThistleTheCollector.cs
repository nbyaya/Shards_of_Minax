using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThistleTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThistleTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thistle the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(100);

        SetHits(120);
        SetMana(150);
        SetStam(70);

        Fame = 500;
        Karma = 1000;

        // Outfit
        AddItem(new FancyShirt(2157)); // A deep purple shirt
        AddItem(new LongPants(2128)); // Dark green pants
        AddItem(new FeatheredHat(1109)); // A vibrant blue feathered hat
        AddItem(new Sandals(141)); // Simple gray sandals
        AddItem(new Backpack()); // A collector's backpack

        VirtualArmor = 15;
    }

    public ThistleTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Thistle, a collector of rare and peculiar items. Do you have anything of interest for me today?");

        // Dialogue options
        greeting.AddOption("What kind of items do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectModule = new DialogueModule("I collect all manner of strange and exotic items. Currently, I'm searching for a GargoyleLamp. Should you have one, I can offer you something of equal value in exchange.");

                // Additional superstitious dialogue options
                collectModule.AddOption("Why are you searching for a GargoyleLamp?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("Ah, the GargoyleLamp... it's no ordinary trinket. It has a story of its own, one intertwined with the restless spirits of the sea. You see, I was once a sailor, a damned one at that. We sailed into a storm that seemed conjured by the depths themselves. The ship was lost, and I alone survived. Ever since, the visions haunt me.");

                        storyModule.AddOption("Visions? What do you mean?",
                            pq => true,
                            pq =>
                            {
                                DialogueModule visionModule = new DialogueModule("Visions of the deep, of creatures that no man should ever lay eyes upon. They whisper to me in the dead of night, promising my doom unless I pay tribute. The GargoyleLamp... it's said to keep the darkness at bay, to hold back the tide of nightmares. I need it to keep my mind intact.");

                                visionModule.AddOption("That sounds terrifying. How have you managed to survive this long?",
                                    pw => true,
                                    pw =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("Survival? Ha! It's not survival, it's endurance. I move from town to town, collecting relics, anything that might ease the weight of this cursed existence. I have charms, tokens, and now you - perhaps you can help me end this torment.");

                                        survivalModule.AddOption("I'll help you if I can.",
                                            pe => true,
                                            pe =>
                                            {
                                                p.SendMessage("Thistle's eyes brighten momentarily, and he nods gratefully.");
                                                DialogueModule helpModule = new DialogueModule("If you have a GargoyleLamp, I will trade you a relic of equal value. But heed my warning, these items are not without their own mysteries and burdens.");

                                                helpModule.AddOption("I have a GargoyleLamp. What will you offer me?",
                                                    plr => HasGargoyleLamp(pl) && CanTradeWithPlayer(pl),
                                                    plr =>
                                                    {
                                                        CompleteTrade(pl);
                                                    });

                                                helpModule.AddOption("I don't have a GargoyleLamp right now.",
                                                    plt => !HasGargoyleLamp(pl),
                                                    plt =>
                                                    {
                                                        pl.SendMessage("Come back when you have a GargoyleLamp to trade.");
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                                                    });

                                                helpModule.AddOption("I traded recently; I'll come back later.",
                                                    ply => !CanTradeWithPlayer(pl),
                                                    ply =>
                                                    {
                                                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                                                    });

                                                p.SendGump(new DialogueGump(p, helpModule));
                                            });

                                        survivalModule.AddOption("I can't help you. This is too much for me.",
                                            pu => true,
                                            pu =>
                                            {
                                                p.SendMessage("Thistle's expression darkens, and he gives a weary nod. 'I understand, traveler. This curse is mine alone to bear.'");
                                            });

                                        p.SendGump(new DialogueGump(p, survivalModule));
                                    });

                                visionModule.AddOption("What kind of creatures did you see?",
                                    pi => true,
                                    pi =>
                                    {
                                        DialogueModule creaturesModule = new DialogueModule("The kind that haunt your worst nightmares. Twisted things with eyes like abyssal voids, tendrils that could crush a ship's mast, and voices like the wailing of the damned. They call to me even now, just beneath the surface of my mind.");

                                        creaturesModule.AddOption("That's horrifying. I hope the GargoyleLamp helps you.",
                                            po => true,
                                            po =>
                                            {
                                                p.SendMessage("Thistle nods solemnly, 'As do I, traveler. As do I.'");
                                            });

                                        p.SendGump(new DialogueGump(p, creaturesModule));
                                    });

                                p.SendGump(new DialogueGump(p, visionModule));
                            });

                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });

                // Trade option after story
                collectModule.AddOption("I have a GargoyleLamp. What will you offer me?",
                    pl => HasGargoyleLamp(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                collectModule.AddOption("I don't have a GargoyleLamp right now.",
                    pl => !HasGargoyleLamp(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a GargoyleLamp to trade.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                collectModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, collectModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Thistle nods and returns to examining his collection.");
            });

        return greeting;
    }

    private bool HasGargoyleLamp(PlayerMobile player)
    {
        // Check the player's inventory for GargoyleLamp
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GargoyleLamp)) != null;
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
        // Remove the GargoyleLamp and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item gargoyleLamp = player.Backpack.FindItemByType(typeof(GargoyleLamp));
        if (gargoyleLamp != null)
        {
            gargoyleLamp.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for AnimalTopiary and TinCowbell
            rewardChoiceModule.AddOption("AnimalTopiary", pl => true, pl =>
            {
                pl.AddToBackpack(new AnimalTopiary());
                pl.SendMessage("You receive an AnimalTopiary!");
            });

            rewardChoiceModule.AddOption("TinCowbell", pl => true, pl =>
            {
                pl.AddToBackpack(new TinCowbell());
                pl.SendMessage("You receive a TinCowbell!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a GargoyleLamp.");
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