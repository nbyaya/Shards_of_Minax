using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class RulvynTheRustedCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RulvynTheRustedCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rulvyn the Rusted Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(2413)); // Rusty orange shroud
        AddItem(new Kilt(1255)); // Dark brown kilt
        AddItem(new Sandals(1415)); // Black sandals
        AddItem(new BodySash(2125)); // Deep green sash
        AddItem(new QuarterStaff()); // Holds a staff that appears ancient

        VirtualArmor = 15;
    }

    public RulvynTheRustedCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hail, traveler. I am Rulvyn, collector of all things curious and forgotten. Perhaps you have something of interest for me?");

        // Dialogue options
        greeting.AddOption("What kind of curiosities are you interested in?",
            p => true,
            p =>
            {
                DialogueModule curiositiesModule = new DialogueModule("I have an eye for the rare and the bizarre. Items that tell a story. For instance, a SpikedChair would be just the thing. If you have one, I can offer you a choice of a KrakenTrophy or a ZttyCrystal in return.");

                curiositiesModule.AddOption("Why a SpikedChair specifically?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule spikedChairExplanation = new DialogueModule("Ah, the SpikedChair... it holds a peculiar significance to me. You see, I used to be a pharmacist, working with herbs and mixtures, not unlike a healer of sorts. During those days, I began experimenting with substances that could alter the mind. It was during one such session, under the effects of my own concoction, that I saw visions of other dimensions—terrifying, wondrous, and vivid. In one of these hallucinations, I saw the SpikedChair, an item that seemed to exist between worlds, a link to the unknown.");
                        
                        spikedChairExplanation.AddOption("Hallucinations? Were you experimenting with drugs?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule drugExplanation = new DialogueModule("Yes, yes... I was a pharmacist, but more than that, I was a seeker of knowledge. I synthesized drugs that would help me glimpse beyond the mundane. Some might call it madness, but I was convinced I was on the verge of discovering something greater—a bridge to other realms. I thought I was seeing other dimensions, but as time went on, the line between reality and my visions blurred.");

                                drugExplanation.AddOption("That sounds dangerous. Did it affect you negatively?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule negativeEffectsModule = new DialogueModule("Dangerous, yes... In truth, I lost everything: my reputation, my friends, even my sense of self. My anxiety grew worse with every trip I took to those bizarre worlds. I could never tell if the things I saw were real or just figments of my decaying mind. And yet, I still can't help but yearn for the thrill, the glimpse into something beyond comprehension.");

                                        negativeEffectsModule.AddOption("Why do you continue down this path, then?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule obsessionExplanation = new DialogueModule("It's an obsession, you see. Once you've seen what's beyond the veil, you cannot go back to ignorance. I know it has driven me to the brink of madness, but there's a part of me that believes there's a purpose behind it. The SpikedChair is just one of the pieces. The visions showed me that collecting certain objects would reveal a greater truth—a map of sorts that might lead me to answers, or perhaps... to an escape.");

                                                obsessionExplanation.AddOption("What do you mean by an escape?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule escapeExplanation = new DialogueModule("An escape from this torment. I fear that whatever I glimpsed has taken hold of me, like a parasite that feeds on my sanity. Maybe, just maybe, if I gather all the pieces, I can find a way to rid myself of this burden, or at least understand why I was chosen to witness these other realms. Until then, I am trapped in this cycle—searching, collecting, hoping.");

                                                        plaaaa.SendGump(new DialogueGump(plaaaa, escapeExplanation));
                                                    });

                                                obsessionExplanation.AddOption("I don't have all the answers, but I hope you find what you're looking for.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Rulvyn looks at you with a haunted expression, nodding solemnly.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, obsessionExplanation));
                                            });

                                        negativeEffectsModule.AddOption("I see. Thank you for sharing your story.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rulvyn's eyes are downcast, as though lost in thought.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, negativeEffectsModule));
                                    });

                                drugExplanation.AddOption("You were really willing to risk everything for knowledge?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule riskExplanation = new DialogueModule("Knowledge has always been worth the risk, at least to me. The things I saw... some were beyond beautiful, but others were grotesque, unsettling. I had hoped to harness the power of what I glimpsed, to bring some part of it back to this world. But instead, it has left me with questions, fears, and an insatiable curiosity. And now I search for relics, artifacts like the SpikedChair, that might provide me with further insight.");

                                        plaa.SendGump(new DialogueGump(plaa, riskExplanation));
                                    });

                                pla.SendGump(new DialogueGump(pla, drugExplanation));
                            });

                        spikedChairExplanation.AddOption("That sounds fascinating, albeit frightening.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Rulvyn smiles, though there's a hint of anxiety in his eyes.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, spikedChairExplanation));
                    });

                curiositiesModule.AddOption("Do you want to make a trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Do you possess a SpikedChair? If so, I would be happy to make the trade.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SpikedChair for me?");
                                tradeModule.AddOption("Yes, I have a SpikedChair.",
                                    plaa => HasSpikedChair(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasSpikedChair(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SpikedChair.");
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
                        tradeIntroductionModule.AddOption("Maybe another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, curiositiesModule));
            });

        greeting.AddOption("Farewell.",
            p => true,
            p =>
            {
                p.SendMessage("Rulvyn nods and smiles slightly, his eyes glinting with curiosity, but also with a touch of sadness.");
            });

        return greeting;
    }

    private bool HasSpikedChair(PlayerMobile player)
    {
        // Check the player's inventory for SpikedChair
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SpikedChair)) != null;
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
        // Remove the SpikedChair and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item spikedChair = player.Backpack.FindItemByType(typeof(SpikedChair));
        if (spikedChair != null)
        {
            spikedChair.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for KrakenTrophy and ZttyCrystal
            rewardChoiceModule.AddOption("KrakenTrophy", pl => true, pl =>
            {
                pl.AddToBackpack(new KrakenTrophy());
                pl.SendMessage("You receive a KrakenTrophy!");
            });

            rewardChoiceModule.AddOption("ZttyCrystal", pl => true, pl =>
            {
                pl.AddToBackpack(new ZttyCrystal());
                pl.SendMessage("You receive a ZttyCrystal!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SpikedChair.");
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