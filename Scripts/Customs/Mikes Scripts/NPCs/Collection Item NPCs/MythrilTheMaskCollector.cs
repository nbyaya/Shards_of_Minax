using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class MythrilTheMaskCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MythrilTheMaskCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mythril the Mask Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new FancyShirt(1153)); // Fancy shirt with a dark blue hue
        AddItem(new LongPants(1109)); // Long pants with a greyish hue
        AddItem(new Boots(1175)); // Boots with a dark hue
        AddItem(new Cloak(1157)); // A cloak with a shadowy hue
        AddItem(new FeatheredHat(1175)); // Feathered hat, matching the boots
        AddItem(new HorribleMask()); // Mythril also wears a HorribleMask for added mystery

        VirtualArmor = 15;
    }

    public MythrilTheMaskCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Mythril, collector of rare and bizarre masks. Tell me, do you happen to possess a HorribleMask? I could offer something valuable in return.");

        // Start with dialogue about his collection
        greeting.AddOption("Tell me more about your mask collection.",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("Masks are more than mere disguises; they are gateways to other identities, other worlds. Each mask I collect has its own story. The HorribleMask, for instance, is said to be cursed, driving away spirits of ill fortune.");
                collectionModule.AddOption("Why do you want the HorribleMask?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("The HorribleMask holds a strange power that I am keen to study. I believe it could help me create a ward against dark forces. If you bring me one, I can reward you with FixedScales, an item of great value to certain alchemists.");
                        reasonModule.AddOption("I have a HorribleMask. Can we trade?",
                            pla => HasHorribleMask(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        reasonModule.AddOption("I don't have one right now.",
                            pla => !HasHorribleMask(pla),
                            pla =>
                            {
                                pla.SendMessage("Come back when you have a HorribleMask.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        reasonModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                
                // Adding more detailed dialogue about various masks Mythril has collected
                collectionModule.AddOption("What kind of masks do you collect?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule maskTypesModule = new DialogueModule("Ah, I collect masks from all corners of the world. Each culture has its own unique tradition of mask-making. There are the Oni masks of Tokuno, the Shamanic masks of the Savage Tribes, the Festival masks of Nujel'm, and the Spirit masks of the Orcs.");
                        
                        maskTypesModule.AddOption("Tell me about the Oni masks.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule oniModule = new DialogueModule("The Oni masks come from the far-off lands of Tokuno. They are crafted to resemble fierce demons, used to ward off evil spirits. It is said that those who wear them can harness a fragment of the Oni's power, though at a cost to their own sanity.");
                                oniModule.AddOption("What kind of power?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule powerModule = new DialogueModule("The Oni masks grant great strength and a fearsome presence in battle. However, prolonged use can corrupt the wearer, blurring the line between man and beast. Many warriors have lost themselves entirely, becoming little more than monsters.");
                                        powerModule.AddOption("That sounds dangerous. I'll pass.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, powerModule));
                                    });
                                oniModule.AddOption("Fascinating. Tell me about another mask.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, maskTypesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, oniModule));
                            });
                        
                        maskTypesModule.AddOption("Tell me about the Shamanic masks.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule shamanicModule = new DialogueModule("The Shamanic masks are used by the Savage Tribes in their rituals. They are adorned with feathers, bones, and gemstones, each one representing a spirit of the wild. Wearing these masks allows the shaman to commune with the spirits, gaining visions and guidance from the ancestors.");
                                shamanicModule.AddOption("Can anyone use them?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule usageModule = new DialogueModule("Not quite. Only those who have undergone the rites of the Savage Tribes can truly harness the power of these masks. Outsiders who try to use them may find themselves haunted by restless spirits, their dreams turning into nightmares.");
                                        usageModule.AddOption("I think I'll avoid those.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, usageModule));
                                    });
                                shamanicModule.AddOption("Tell me about another mask.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, maskTypesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, shamanicModule));
                            });
                        
                        maskTypesModule.AddOption("Tell me about the Festival masks of Nujel'm.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule festivalModule = new DialogueModule("The Festival masks of Nujel'm are worn during grand celebrations and masquerades. They are ornate, often decorated with gold leaf and precious stones. These masks are meant to hide one's identity, allowing nobles and commoners alike to mingle freely, if only for a night.");
                                festivalModule.AddOption("What happens during these festivals?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule festivalDetailsModule = new DialogueModule("The festivals are a time of revelry, music, and dance. Many secrets are shared, and many plots are hatched behind the anonymity of these masks. Some say the masks themselves remember these secrets, whispering them to their next wearer.");
                                        festivalDetailsModule.AddOption("Sounds intriguing, but risky.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, festivalDetailsModule));
                                    });
                                festivalModule.AddOption("Tell me about another mask.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, maskTypesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, festivalModule));
                            });
                        
                        maskTypesModule.AddOption("Tell me about the Spirit masks of the Orcs.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule spiritModule = new DialogueModule("The Spirit masks of the Orcs are carved from wood and painted with bright colors. They are worn by Orc shamans to connect with their ancestors and to draw power from the spirits of the land. Each mask is unique, representing the spirit of a specific ancestor.");
                                spiritModule.AddOption("Do they actually work?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule spiritPowerModule = new DialogueModule("The Orcs believe so. They claim that wearing these masks allows them to channel the strength and wisdom of their ancestors. Many who have faced Orc shamans in battle say they fight as if possessed, their eyes burning with an otherworldly light.");
                                        spiritPowerModule.AddOption("I'd rather not test that myself.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, spiritPowerModule));
                                    });
                                spiritModule.AddOption("Tell me about another mask.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, maskTypesModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, spiritModule));
                            });
                        
                        maskTypesModule.AddOption("That's enough about masks for now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, maskTypesModule));
                    });
                
                collectionModule.AddOption("That's interesting. Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Mythril nods, his eyes glinting mysteriously beneath the mask.");
            });

        return greeting;
    }

    private bool HasHorribleMask(PlayerMobile player)
    {
        // Check the player's inventory for HorribleMask
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HorribleMask)) != null;
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
        // Remove the HorribleMask and give the FixedScales, then set the cooldown timer
        Item horribleMask = player.Backpack.FindItemByType(typeof(HorribleMask));
        if (horribleMask != null)
        {
            horribleMask.Delete();
            player.AddToBackpack(new FixedScales());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the HorribleMask and receive FixedScales in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a HorribleMask.");
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