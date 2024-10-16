using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class ThaliaTheEnchantress : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaliaTheEnchantress() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalia the Enchantress";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(90);
        SetMana(200);
        SetStam(60);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1153)); // Deep blue robe
        AddItem(new Sandals(1109)); // Dark grey sandals
        AddItem(new WizardsHat(1153)); // Deep blue wizard hat
        AddItem(new GoldBracelet());
        AddItem(new Spellbook()); // Spellbook to signify her magical expertise

        VirtualArmor = 12;
    }

    public ThaliaTheEnchantress(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Thalia, an enchantress studying the ancient arts of transformation and illusion. Perhaps you can aid me in my research?");

        // Start with dialogue about her work
        greeting.AddOption("What kind of research do you do?",
            p => true,
            p =>
            {
                DialogueModule researchModule = new DialogueModule("I delve into the magic of the natural world and its connection to transformation. Have you heard of the enchanted creatures of the Topiary Grove?");

                // Introduce enchanted creatures
                researchModule.AddOption("Tell me about the enchanted creatures.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule creaturesModule = new DialogueModule("The Topiary creatures are shaped from flora and imbued with magic. They serve as guardians, their forms sculpted by master carpenters. Their essence is highly sought after for its magical properties.");
                        creaturesModule.AddOption("How can one harness their magic?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule magicModule = new DialogueModule("To harness their magic, one must possess a rare item known as the AnimalTopiary. These items contain the very essence of the Topiary creatures and are crucial for powerful enchantments.");
                                magicModule.AddOption("Interesting. Can I help you with this?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateTradeModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, magicModule));
                            });
                        creaturesModule.AddOption("Fascinating. Thank you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        // Additional dialogue about Thalia's backstory
                        creaturesModule.AddOption("You seem quite knowledgeable. How did you come to study these creatures?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule backstoryModule = new DialogueModule("Ah, well, it's quite the story. You see, I wasn't always an enchantress. I grew up in the ruins, a street-smart teenager, scavenging for whatever I could find to survive. It was dangerous, and I had to be agile and resourceful. One day, while navigating the old ruins, I stumbled upon an ancient library of magic scrolls. That's where I found my calling.");
                                backstoryModule.AddOption("That sounds challenging. How did you manage to survive?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("It was definitely not easy. The wasteland is full of dangers—wild beasts, hostile scavengers, and even worse things lurking in the shadows. I learned to stay optimistic no matter the situation. I used my agility to avoid danger and my resourcefulness to find food, water, and shelter. I suppose it's that same resourcefulness that helps me now in my studies.");
                                        survivalModule.AddOption("That's inspiring. Do you think the ruins still hold more secrets?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule secretsModule = new DialogueModule("Oh, absolutely. The ruins are filled with hidden treasures and ancient knowledge, just waiting to be uncovered. I believe that, one day, we could rebuild civilization, using what we find. It's a dream I hold onto, even though it's hard. Each new discovery brings us one step closer to understanding the old world—and maybe even surpassing it.");
                                                secretsModule.AddOption("I admire your vision. How can I help?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateTradeModule(plaaa)));
                                                    });
                                                secretsModule.AddOption("I wish you luck in achieving that dream.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, secretsModule));
                                            });
                                        survivalModule.AddOption("It sounds like you've been through a lot. Thank you for sharing.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, survivalModule));
                                    });
                                backstoryModule.AddOption("The library must have been an incredible find.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule libraryModule = new DialogueModule("Indeed it was. The scrolls there taught me the basics of magic—spells to light my way, to protect myself, and eventually, to transform and manipulate the world around me. It wasn't long before I realized I could use these skills to help others and maybe, just maybe, rebuild what was lost.");
                                        libraryModule.AddOption("You have a noble goal. I would like to help.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateTradeModule(plaab)));
                                            });
                                        libraryModule.AddOption("Thank you for sharing your story.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, libraryModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, backstoryModule));
                            });

                        pl.SendGump(new DialogueGump(pl, creaturesModule));
                    });

                researchModule.AddOption("What can I do to help?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTradeModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, researchModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Thalia nods and returns to her studies.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeModule(PlayerMobile player)
    {
        DialogueModule tradeModule = new DialogueModule("I am in need of an AnimalTopiary for my research. If you bring me one, I can reward you with a CarpentryTalisman and a MaxxiaScroll.");
        tradeModule.AddOption("I have an AnimalTopiary to trade.",
            pl => HasAnimalTopiary(pl) && CanTradeWithPlayer(pl),
            pl =>
            {
                CompleteTrade(pl);
            });
        tradeModule.AddOption("I don't have one right now.",
            pl => !HasAnimalTopiary(pl),
            pl =>
            {
                pl.SendMessage("Come back when you have an AnimalTopiary.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });
        tradeModule.AddOption("I traded recently; I'll come back later.",
            pl => !CanTradeWithPlayer(pl),
            pl =>
            {
                pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });

        return tradeModule;
    }

    private bool HasAnimalTopiary(PlayerMobile player)
    {
        // Check the player's inventory for AnimalTopiary
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AnimalTopiary)) != null;
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
        // Remove the AnimalTopiary and give the CarpentryTalisman and MaxxiaScroll, then set the cooldown timer
        Item animalTopiary = player.Backpack.FindItemByType(typeof(AnimalTopiary));
        if (animalTopiary != null)
        {
            animalTopiary.Delete();
            player.AddToBackpack(new CarpentryTalisman());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the AnimalTopiary and receive a CarpentryTalisman and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have an AnimalTopiary.");
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