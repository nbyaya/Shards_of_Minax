using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TazrinTheHermit : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TazrinTheHermit() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tazrin the Hermit";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows()); // Mysterious hooded cloak
        AddItem(new Sandals(2553)); // Sandals with a dark hue
        AddItem(new LeatherGloves()); // Worn leather gloves
        AddItem(new SabertoothSkull()); // A walking stick, symbolizing his life in seclusion

        VirtualArmor = 15;
    }

    public TazrinTheHermit(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a visitor... You don't see many folks around these parts. I am Tazrin, the keeper of forgotten knowledge. Do you come seeking wisdom, or perhaps a trade?");

        // Dialogue about his backstory
        greeting.AddOption("Tell me about yourself.",
            p => true,
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("I once walked among the scholars of Britain, but their pursuit of knowledge was too constrained by rules and politics. So I left, choosing a life of solitude in the wilds. Here, amidst nature, I found true enlightenment.");
                backstoryModule.AddOption("That's fascinating. What kind of knowledge did you find?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule knowledgeModule = new DialogueModule("The kind of knowledge that cannot be contained in books, my friend. The secrets of the old world, the magic in every flower, every stone. I have learned to listen to the whispers of nature, the forgotten secrets of the ancients. But alas, I digress... Is there something else you seek?");
                        knowledgeModule.AddOption("What secrets of the old world do you know?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule secretsModule = new DialogueModule("Ah, the old world was a place of wonders and dangers, filled with artifacts of unimaginable power. I collect such items, though some call me eccentric. You see, I have an obsession for the macabre and bizarre, a drive that some might call... unhealthy. But each item I collect brings me closer to understanding the ancient ways.");
                                secretsModule.AddOption("What kinds of artifacts have you collected?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule artifactsModule = new DialogueModule("I have collected many things: the petrified heart of a basilisk, the shard of an obsidian mirror used in ancient rituals, and even the skeletal hand of a long-forgotten lich. Each piece tells a story, and I guard them jealously. You see, these items are not mere curiosities; they are pieces of a larger puzzle.");
                                        artifactsModule.AddOption("Why do you collect such dangerous items?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule obsessionModule = new DialogueModule("Why indeed... It is an obsession, a hunger that cannot be sated. I must have them, regardless of the cost. Each artifact, each fragment of the past, brings me closer to the truth. The truth of what, you ask? The truth of existence, the secret that lies beneath the veil of reality. But enough about me... is there something else you wish to know?");
                                                obsessionModule.AddOption("You sound paranoid. Do you fear someone might take them?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule paranoiaModule = new DialogueModule("Paranoid? Perhaps. But it is not paranoia if the danger is real. There are those who covet my collection, who would do anything to take it from me. I have taken precautions, traps, wards, and even curses to protect what is mine. One must be meticulous, after all, to keep what is valuable safe.");
                                                        paranoiaModule.AddOption("Tell me about the wards and curses.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule wardsModule = new DialogueModule("The wards are ancient, drawn from the old languages. They weave a net of protection around my home. As for the curses... well, let's just say they are not for the faint of heart. Anyone foolish enough to try and take my treasures will find themselves marked, haunted by visions and plagued by misfortune until they return what they have stolen—or until they perish. But enough about that, it seems I've rambled again. Is there something else?");
                                                                wardsModule.AddOption("I think I've heard enough for now.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, wardsModule));
                                                            });
                                                        paranoiaModule.AddOption("That sounds dangerous. I'll be careful.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, paranoiaModule));
                                                    });
                                                obsessionModule.AddOption("I see. Thank you for sharing.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, obsessionModule));
                                            });
                                        artifactsModule.AddOption("Sounds dangerous. I think I'll pass.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, artifactsModule));
                                    });
                                secretsModule.AddOption("I think I've heard enough.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, secretsModule));
                            });
                        knowledgeModule.AddOption("I would like to hear more later.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, knowledgeModule));
                    });
                backstoryModule.AddOption("Perhaps another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        // Offer a trade if the player has the item
        greeting.AddOption("Do you need anything?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am looking for a rare herb known as the HutFlower. If you have one, I could offer you a SabertoothSkull as well as a MaxxiaScroll. I do, however, have limited resources and can only make this exchange once every 10 minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a HutFlower for me?");
                        tradeModule.AddOption("Yes, I have a HutFlower.",
                            plaa => HasHutFlower(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasHutFlower(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a HutFlower.");
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
                tradeIntroductionModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Tazrin nods and returns to his musings.");
            });

        return greeting;
    }

    private bool HasHutFlower(PlayerMobile player)
    {
        // Check the player's inventory for HutFlower
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HutFlower)) != null;
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
        // Remove the HutFlower and give the SabertoothSkull and MaxxiaScroll, then set the cooldown timer
        Item hutFlower = player.Backpack.FindItemByType(typeof(HutFlower));
        if (hutFlower != null)
        {
            hutFlower.Delete();
            player.AddToBackpack(new SabertoothSkull());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the HutFlower and receive a SabertoothSkull and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a HutFlower.");
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