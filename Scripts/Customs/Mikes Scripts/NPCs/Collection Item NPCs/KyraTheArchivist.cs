using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class KyraTheArchivist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KyraTheArchivist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kyra the Archivist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(120);

        SetHits(80);
        SetMana(200);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(1153)); // Dark blue fancy dress
        AddItem(new Shoes(1109)); // Black shoes
        AddItem(new StrappedBooks()); // Simple reading glasses
        AddItem(new QuarterStaff()); // She carries a staff as a symbol of her knowledge

        VirtualArmor = 15;
    }

    public KyraTheArchivist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Kyra, keeper of lost knowledge and ancient secrets. Do you seek wisdom, or perhaps have something of value to trade?");

        // General introduction about her background
        greeting.AddOption("Who are you, really?",
            p => true,
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("Ah, a curious soul, I see. They call me Kyra the Archivist, but I am more than just a keeper of scrolls. My origins are... elusive, like the mist that rolls through the midnight woods. I appear in settlements when the time is right, offering rare and mysterious items to those who dare to trade. Some say I am cunning, and perhaps they are right. I always know what others need, often before they know themselves.");
                
                backstoryModule.AddOption("Why do you do this?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule motivationModule = new DialogueModule("Why, indeed. My motives are my own, and they change with the wind. One day, I seek knowledge, the next, power, and sometimes, simply a rare smile from a weary traveler. Perhaps I collect secrets because I know that knowledge, in the right hands, can shape the world—or destroy it.");
                        
                        motivationModule.AddOption("You sound dangerous.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule dangerModule = new DialogueModule("Dangerous? Perhaps. But danger is a relative term, don't you think? Those who fear what they do not understand might call me dangerous, but to others, I am a beacon of hope. It depends entirely on which side of the coin you see.");
                                dangerModule.AddOption("I see. Thank you for your honesty.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, dangerModule));
                            });

                        motivationModule.AddOption("I think I understand.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Kyra nods knowingly, her eyes glinting with a mix of understanding and secrecy.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, motivationModule));
                    });

                backstoryModule.AddOption("Where do you come from?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule originModule = new DialogueModule("Where do I come from, you ask? From places unseen, from shadows unspoken. I have walked lands that most cannot imagine, touched relics older than time itself. The true question is—does it matter where I am from, or does it matter where I am now?");
                        
                        originModule.AddOption("You're right, it doesn't matter.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Kyra smiles faintly, her presence as enigmatic as ever.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        originModule.AddOption("I'd still like to know more.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule moreOriginModule = new DialogueModule("Perhaps one day, when the stars align and the truth is ready to be told, you will know. But for now, all you need to know is that I am here, and I bring opportunities to those brave enough to take them.");
                                moreOriginModule.AddOption("I understand. Thank you.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, moreOriginModule));
                            });
                        pl.SendGump(new DialogueGump(pl, originModule));
                    });

                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        greeting.AddOption("What kind of knowledge do you possess?",
            p => true,
            p =>
            {
                DialogueModule knowledgeModule = new DialogueModule("I gather scrolls and tomes that tell of forgotten times and magical wonders. Some speak of powerful artifacts, while others tell tales of ancient creatures. Which do you wish to hear about?");

                knowledgeModule.AddOption("Tell me about the ancient artifacts.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("The realm is filled with mysterious artifacts, some long lost to time. One such item is the Obsidian Mirror, said to reflect not only one's appearance but also the truth of their soul. Such artifacts can be dangerous, but also enlightening. There is also the Crown of Shadows, a relic that grants the wearer the power to command the dark, but at what cost? Would you like to hear about any of these?");

                        artifactsModule.AddOption("Tell me more about the Obsidian Mirror.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mirrorModule = new DialogueModule("The Obsidian Mirror is not just a simple artifact. Those who look into it see their truest self, their darkest desires, and their purest hopes. Many have been driven mad by what they saw, unable to face the reflection of their soul. It is said that only those with a pure heart can see beauty in the mirror's reflection.");
                                mirrorModule.AddOption("I would not dare look into it.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                mirrorModule.AddOption("Fascinating. Where can it be found?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule locationModule = new DialogueModule("Its current location is unknown, though some say it lies deep within the haunted ruins of the Shadow Keep, guarded by spirits bound to it. Only the bravest—or the most foolish—would dare seek it out.");
                                        locationModule.AddOption("I think I'll leave it be, for now.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, locationModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, mirrorModule));
                            });

                        artifactsModule.AddOption("Tell me more about the Crown of Shadows.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule crownModule = new DialogueModule("The Crown of Shadows grants its wearer dominion over the creatures of the dark—banshees, shadow hounds, and even lesser wraiths. But its power comes with a cost: it slowly corrupts the mind of its bearer, filling them with an insatiable thirst for control and despair. Many who have worn it became prisoners of their own dark desires.");
                                crownModule.AddOption("That sounds too dangerous for me.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                crownModule.AddOption("Where could such a crown be found?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule locationModule = new DialogueModule("The Crown of Shadows was last seen in the possession of the Dark Conclave, a secretive order known for their dealings with forbidden magics. They move frequently, leaving no trace, and are said to be protected by enchantments that make them nearly impossible to find.");
                                        locationModule.AddOption("Perhaps it's best left with them.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, locationModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, crownModule));
                            });
                        
                        artifactsModule.AddOption("That's enough about artifacts for now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, artifactsModule));
                    });

                knowledgeModule.AddOption("Tell me about the ancient creatures.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule creaturesModule = new DialogueModule("The ancient creatures of the realm are both beautiful and terrifying. The Astral Wyrm, for instance, is said to live between worlds, only visible under the light of a lunar eclipse. Its scales shimmer like the night sky, and it is rumored to guard gateways to other realms. There is also the Shadow Stag, a creature of legend that can pass between the mortal realm and the spirit world at will.");

                        creaturesModule.AddOption("Tell me more about the Astral Wyrm.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule wyrmModule = new DialogueModule("The Astral Wyrm is said to possess the wisdom of countless ages, having observed the world from a place between time. It is neither friend nor foe to humanity, but rather a guardian of the boundary between the realms. To glimpse it is a rare and mysterious omen—some say it brings luck, others, doom.");
                                wyrmModule.AddOption("I'd prefer not to encounter one, then.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                wyrmModule.AddOption("How might one find it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule findModule = new DialogueModule("To find the Astral Wyrm, one must journey to the Moonlit Glade during a lunar eclipse and perform the Rite of Veils. It is a dangerous undertaking, for the veil between realms is thin, and other entities might slip through.");
                                        findModule.AddOption("Sounds far too risky.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, findModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, wyrmModule));
                            });

                        creaturesModule.AddOption("Tell me more about the Shadow Stag.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule stagModule = new DialogueModule("The Shadow Stag is an elusive creature, often seen as a fleeting silhouette in the mist. It is said to be the guardian of the passage between the living and the spirit world. Those who manage to follow the stag may find themselves in a realm beyond understanding, but beware—for the way back is not always guaranteed.");
                                stagModule.AddOption("I think I'll leave the stag in peace.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                stagModule.AddOption("Could someone control the stag?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule controlModule = new DialogueModule("Control? Perhaps. But those who try to command the Shadow Stag find that it is not easily tamed. It does not bow to strength or threats, but instead to purity of purpose and sincerity. Many have tried and failed, losing themselves in the spirit realm.");
                                        controlModule.AddOption("I think I'll leave it alone.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, controlModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, stagModule));
                            });

                        creaturesModule.AddOption("That's enough about creatures for now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, creaturesModule));
                    });

                p.SendGump(new DialogueGump(p, knowledgeModule));
            });

        // Trade dialogue
        greeting.AddOption("Do you need anything in particular?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am currently in need of BioSamples for my research. In return, I can offer you a StrappedBooks and a MaxxiaScroll as a reward. However, I can only make such a trade every so often.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the BioSamples for me?");
                        tradeModule.AddOption("Yes, I have BioSamples.",
                            plaa => HasBioSamples(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have them right now.",
                            plaa => !HasBioSamples(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have BioSamples.");
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

        // Goodbye option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Kyra nods and returns to her studies.");
            });

        return greeting;
    }

    private bool HasBioSamples(PlayerMobile player)
    {
        // Check the player's inventory for BioSamples
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BioSamples)) != null;
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
        // Remove the BioSamples and give the StrappedBooks and MaxxiaScroll, then set the cooldown timer
        Item bioSamples = player.Backpack.FindItemByType(typeof(BioSamples));
        if (bioSamples != null)
        {
            bioSamples.Delete();
            player.AddToBackpack(new StrappedBooks());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BioSamples and receive StrappedBooks and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have BioSamples.");
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