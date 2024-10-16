using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class AlaricTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AlaricTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Alaric the Collector";
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
        AddItem(new FancyShirt(1153)); // Blue fancy shirt
        AddItem(new LongPants(1109)); // Dark gray pants
        AddItem(new Boots(775)); // Light green boots
        AddItem(new TricorneHat(1157)); // A dark blue hat
        AddItem(new Lantern()); // Carries a lantern as a collector

        VirtualArmor = 15;
    }

    public AlaricTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! You seem like someone who appreciates rare and curious artifacts. I'm Alaric, a collector of the unusual. Perhaps we can make a trade?");

        // Start with dialogue about his collection
        greeting.AddOption("What sort of items do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I collect items that are rare and unique, things with a story behind them. Among the items I seek is something called a 'RareWire'. Perhaps you have heard of it?");
                
                collectionModule.AddOption("Tell me more about your collection.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule detailModule = new DialogueModule("Oh, my collection is not just about artifacts—it's about understanding cultures, the way people think, their values, and their lives. Each item is like a window into another world. For example, the RareWire was once used in ancient rituals by an alien tribe that worshipped the sky and stars. Their understanding of the cosmos was far beyond what we know today. Isn't that fascinating?");
                        
                        detailModule.AddOption("That sounds amazing! Tell me more about the alien tribe.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tribeModule = new DialogueModule("The tribe, called the 'Astral Nomads', believed that the stars were gateways to other realms. They were a deeply spiritual people, and they used RareWire as part of their ceremonies to communicate with what they believed were celestial beings. They were optimistic and believed that every star held a secret waiting to be discovered. Their curiosity and yearning for connection resonate with me deeply.");
                                
                                tribeModule.AddOption("How did they communicate with the celestial beings?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule communicationModule = new DialogueModule("They used a combination of sound and light—a symphony of chanting and reflective surfaces that created patterns to send messages skyward. The RareWire was instrumental in these ceremonies as it could conduct not only electricity but also certain frequencies of light. They believed that these frequencies could bridge the gap between the physical and the ethereal realms. It's truly remarkable what they achieved with such limited tools.");
                                        
                                        communicationModule.AddOption("That's incredible! Have you ever tried to replicate their rituals?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule replicationModule = new DialogueModule("Oh, believe me, I've tried! I even gathered some RareWire and attempted to recreate their light patterns. I can't say that I managed to communicate with any celestial beings, but I did witness something extraordinary—a shimmering glow that seemed to pulse with energy. It's experiences like these that keep me motivated, always searching for the next mystery.");
                                                
                                                replicationModule.AddOption("I admire your passion, Alaric. What other cultures have you studied?",
                                                    plabc => true,
                                                    plabc =>
                                                    {
                                                        DialogueModule culturesModule = new DialogueModule("Oh, so many! One of my favorites is the 'Terra Drifters', a nomadic people who lived beneath the sands. They built vast underground cities and had a unique way of reading the dunes to navigate. They were empathetic towards all living things and believed in maintaining balance within their world. Every artifact they created was imbued with their philosophy of unity and respect for all forms of life.");
                                                        
                                                        culturesModule.AddOption("What sort of artifacts did they create?",
                                                            plabcd => true,
                                                            plabcd =>
                                                            {
                                                                DialogueModule artifactsModule = new DialogueModule("The Terra Drifters made incredible artifacts—most notably their 'Sandglow Orbs'. These orbs were crafted from glass found deep beneath the desert, and they glowed softly when in the presence of living beings. They used these orbs to ensure their paths were always guided by life, never harming the environment. The glow also served as a beacon in their vast underground labyrinths, lighting their way with the warmth of life itself.");
                                                                
                                                                artifactsModule.AddOption("That's such a beautiful concept.",
                                                                    plabcde => true,
                                                                    plabcde =>
                                                                    {
                                                                        plabcde.SendMessage("Alaric smiles warmly, clearly pleased by your interest. 'It really is, isn't it? The more I learn about these cultures, the more I realize how much we still have to learn from them.'");
                                                                        plabcde.SendGump(new DialogueGump(plabcde, CreateGreetingModule(plabcde)));
                                                                    });
                                                                plabcd.SendGump(new DialogueGump(plabcd, artifactsModule));
                                                            });
                                                        
                                                        culturesModule.AddOption("It sounds like they were very wise.",
                                                            plabcq => true,
                                                            plabcq =>
                                                            {
                                                                plabc.SendMessage("Alaric nods thoughtfully. 'They were indeed. Sometimes I think we have lost touch with some of that wisdom in our pursuit of progress.'");
                                                                plabc.SendGump(new DialogueGump(plabc, CreateGreetingModule(plabc)));
                                                            });
                                                        plabc.SendGump(new DialogueGump(plabc, culturesModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, replicationModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, communicationModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, tribeModule));
                            });
                        
                        detailModule.AddOption("You have such an inquisitive mind, Alaric.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Alaric beams with appreciation. 'Thank you! Curiosity is what drives us forward, wouldn't you agree? The universe is vast, and every small discovery brings us closer to understanding it.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, detailModule));
                    });
                
                collectionModule.AddOption("I think I have a RareWire. Can we trade?",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you really have a RareWire? If so, I can offer you a MemorialStone in exchange, along with a special reward.");
                        tradeModule.AddOption("Yes, I have a RareWire.",
                            plaa => HasRareWire(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasRareWire(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a RareWire.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                collectionModule.AddOption("That sounds interesting, but maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Alaric nods knowingly.");
            });

        return greeting;
    }

    private bool HasRareWire(PlayerMobile player)
    {
        // Check the player's inventory for RareWire
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RareWire)) != null;
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
        // Remove the RareWire and give the MemorialStone and MaxxiaScroll, then set the cooldown timer
        Item rareWire = player.Backpack.FindItemByType(typeof(RareWire));
        if (rareWire != null)
        {
            rareWire.Delete();
            player.AddToBackpack(new MemorialStone());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the RareWire and receive a MemorialStone and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a RareWire.");
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