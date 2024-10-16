using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class TavaraTheMystic : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TavaraTheMystic() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tavara the Mystic";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 1000;

        // Outfit
        AddItem(new Robe(1157)); // Robe with a mystical dark blue hue
        AddItem(new Sandals(1175)); // Sandals with a light blue hue
        AddItem(new WizardsHat(1153)); // A wizard's hat with a unique hue
        AddItem(new HotFlamingScarecrow()); // A unique staff, also in inventory for potential stealing

        VirtualArmor = 15;
    }

    public TavaraTheMystic(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Tavara, a seeker of ancient knowledge and a guardian of mystical artifacts. Have you come seeking secrets of the arcane?");

        // Start with dialogue about his knowledge
        greeting.AddOption("Tell me about the secrets you guard.", 
            p => true, 
            p =>
            {
                DialogueModule secretsModule = new DialogueModule("The ancient knowledge I guard spans realms beyond this one. The Virtues, the runes, and the forgotten arts... all are threads of the grand tapestry. Would you like to hear about the Rune of Virtue or the Enigma of Flames?");
                
                // Nested options for each topic
                secretsModule.AddOption("Tell me about the Rune of Virtue.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule virtueRuneModule = new DialogueModule("The Rune of Virtue is a powerful artifact, representing the highest ideals one can aspire to. It grants its bearer strength in spirit and wisdom beyond measure. I have seen travelers grow in power by aligning themselves with its magic.");
                        virtueRuneModule.AddOption("How can I obtain one?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule obtainModule = new DialogueModule("The Rune is not easily found. It requires purity of heart and courage. Seek places of virtue and ancient trials, for only there can the Rune be earned.");
                                obtainModule.AddOption("Thank you for the guidance.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, obtainModule));
                            });
                        virtueRuneModule.AddOption("Thank you, that is all I needed.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, virtueRuneModule));
                    });

                secretsModule.AddOption("Tell me about the Enigma of Flames.", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule flamesModule = new DialogueModule("The Enigma of Flames is an ancient spell, powerful yet unpredictable. It can both protect and destroy, depending on the intent of the caster. Legends speak of a Hot Flaming Scarecrow crafted using this magic, a rare and dangerous item.");
                        flamesModule.AddOption("How can I acquire such an item?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I can create a Hot Flaming Scarecrow, but it requires something in return. Do you happen to possess a VirtueRune? In exchange, I can offer you the Scarecrow, along with a MaxxiaScroll, but I can only perform this ritual once every 10 minutes.");
                                tradeIntroductionModule.AddOption("I have a VirtueRune for you.", 
                                    plaa => HasVirtueRune(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeIntroductionModule.AddOption("I don't have a VirtueRune right now.", 
                                    plaa => !HasVirtueRune(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have obtained a VirtueRune.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeIntroductionModule.AddOption("I have traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        flamesModule.AddOption("That is too dangerous for me.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, flamesModule));
                    });

                p.SendGump(new DialogueGump(p, secretsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Tavara nods and returns to his musings.");
            });

        return greeting;
    }

    private bool HasVirtueRune(PlayerMobile player)
    {
        // Check the player's inventory for VirtueRune
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(VirtueRune)) != null;
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
        // Remove the VirtueRune and give the HotFlamingScarecrow and MaxxiaScroll, then set the cooldown timer
        Item virtueRune = player.Backpack.FindItemByType(typeof(VirtueRune));
        if (virtueRune != null)
        {
            virtueRune.Delete();
            player.AddToBackpack(new HotFlamingScarecrow());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the VirtueRune and receive a Hot Flaming Scarecrow and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a VirtueRune.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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