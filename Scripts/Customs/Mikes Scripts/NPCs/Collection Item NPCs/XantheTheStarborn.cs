using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class XantheTheStarborn : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public XantheTheStarborn() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Xanthe the Starborn";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit - Unique and star-themed
        AddItem(new Robe(1157)); // Robe with a deep blue hue, like the night sky
        AddItem(new Sandals(1150)); // Silver sandals
        AddItem(new Circlet() { Hue = 1153 }); // A silver circlet with a celestial hue
        AddItem(new Lantern() { Movable = false, Name = "Star Lantern", Hue = 1154 });

        VirtualArmor = 20;
    }

    public XantheTheStarborn(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Xanthe, born beneath the falling stars. Have you ever gazed into the night sky and wondered at its mysteries?");

        // Start with dialogue about her origins
        greeting.AddOption("Tell me about your origins.",
            p => true,
            p =>
            {
                DialogueModule originModule = new DialogueModule("I was born on the eve of a rare celestial event, a night when the sky was alight with falling stars. It is said that those born under such a sky are gifted with the ability to perceive the unseen. I was once a leading researcher, studying the effects of cosmic radiation on life forms, until circumstances forced me into seclusion. Now, I live in a dilapidated lab, obsessed with understanding and possibly reversing the mutations caused by radiation.");
                originModule.AddOption("Why did you leave your research position?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("The Institute I worked for... they didn't share my passion for uncovering the truth. They wanted results, something tangible, something they could exploit. But knowledge is not always about immediate gain. It's about understanding, expanding our horizons, and sometimes that journey takes us down dark and unexpected paths.");
                        reasonModule.AddOption("That sounds difficult. Do you miss it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule missingModule = new DialogueModule("Miss it? Hah, perhaps. I miss the ability to delve into the mysteries without restrictions, without the constant pressure for weaponization. My work was not about power—it was about discovery, but my colleagues could not see beyond their narrow ambitions. Now I continue my research alone, far from their meddling.");
                                missingModule.AddOption("What exactly are you researching now?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule researchModule = new DialogueModule("I study the effects of cosmic radiation, the mutations it creates, and how they might be reversed. Radiation is both a curse and a gift—many of the strange creatures you see today are results of this mutation. But I am determined to find a way to reverse the harmful changes, to restore those who suffer to their original forms. It may sound ambitious, but ambition is what brought me here.");
                                        researchModule.AddOption("How do you plan to reverse the mutations?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule reverseModule = new DialogueModule("Ah, the key lies in the properties of certain artifacts—one of which is the MutantStarfish. Its unique composition holds the potential for neutralizing radiation effects, or so I believe. There are, of course, challenges. The process requires delicate calibration, precision, and, above all, the right components.");
                                                reverseModule.AddOption("What kind of components do you need?",
                                                    plabc => true,
                                                    plabc =>
                                                    {
                                                        DialogueModule componentsModule = new DialogueModule("The components are rare—artifacts from the stars, minerals that react to cosmic energy, and living samples affected by radiation. One such component is the InfinitySymbol, an object capable of stabilizing volatile energies. It's ironic, isn't it? Something as simple as a symbol can wield such profound power.");
                                                        componentsModule.AddOption("I see. Can I help you in any way?",
                                                            plabcd => true,
                                                            plabcd =>
                                                            {
                                                                DialogueModule helpModule = new DialogueModule("Perhaps. If you come across any artifacts of the cosmos, such as the MutantStarfish or others that resonate with stellar energy, I would be interested. Knowledge, power, tools—they are all connected. You would be rewarded for your efforts, of course.");
                                                                helpModule.AddOption("I do have a MutantStarfish. What can you offer in return?",
                                                                    plaabcd => HasMutantStarfish(plaabcd) && CanTradeWithPlayer(plaabcd),
                                                                    plaabcd =>
                                                                    {
                                                                        CompleteTrade(plaabcd);
                                                                    });
                                                                helpModule.AddOption("No, I don't have one right now.",
                                                                    plaabcd => !HasMutantStarfish(plaabcd),
                                                                    plaabcd =>
                                                                    {
                                                                        plaabcd.SendMessage("The MutantStarfish is quite rare. Return if you happen upon one.");
                                                                        plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                                    });
                                                                helpModule.AddOption("I traded recently; I'll come back later.",
                                                                    plaabcd => !CanTradeWithPlayer(plaabcd),
                                                                    plaabcd =>
                                                                    {
                                                                        plaabcd.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                                    });
                                                                plabcd.SendGump(new DialogueGump(plabcd, helpModule));
                                                            });
                                                        componentsModule.AddOption("Good luck with your research, Xanthe.",
                                                            plabcde => true,
                                                            plabcde =>
                                                            {
                                                                plabcde.SendMessage("Xanthe nods, her eyes filled with determination. 'The journey of knowledge is often a lonely one, but it must be taken.'");
                                                                plabcde.SendGump(new DialogueGump(plabcde, CreateGreetingModule(plabcde)));
                                                            });
                                                        plabc.SendGump(new DialogueGump(plabc, componentsModule));
                                                    });
                                                reverseModule.AddOption("I hope you succeed, Xanthe.",
                                                    plaabe => true,
                                                    plaabe =>
                                                    {
                                                        plaabe.SendMessage("Success is relative, traveler. In this pursuit, every step forward is a success of its own.");
                                                        plaabe.SendGump(new DialogueGump(plaabe, CreateGreetingModule(plaabe)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, reverseModule));
                                            });
                                        researchModule.AddOption("That sounds incredibly challenging.",
                                            plaaf => true,
                                            plaaf =>
                                            {
                                                plaaf.SendMessage("Indeed, it is. But what else is worth doing if not the impossible?");
                                                plaaf.SendGump(new DialogueGump(plaaf, CreateGreetingModule(plaaf)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, researchModule));
                                    });
                                missingModule.AddOption("Thank you for sharing your story.",
                                    plaag => true,
                                    plaag =>
                                    {
                                        plaag.SendMessage("Xanthe gives a small, wry smile. 'Stories are all we have, in the end.'");
                                        plaag.SendGump(new DialogueGump(plaag, CreateGreetingModule(plaag)));
                                    });
                                pla.SendGump(new DialogueGump(pla, missingModule));
                            });
                        reasonModule.AddOption("I'm sorry that happened to you.",
                            plaah => true,
                            plaah =>
                            {
                                plaah.SendMessage("Xanthe nods solemnly. 'We must all endure the consequences of our pursuits.'");
                                plaah.SendGump(new DialogueGump(plaah, CreateGreetingModule(plaah)));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                originModule.AddOption("Thank you for sharing.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, originModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Xanthe nods knowingly, her eyes reflecting the starlight.");
            });

        return greeting;
    }

    private bool HasMutantStarfish(PlayerMobile player)
    {
        // Check the player's inventory for MutantStarfish
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MutantStarfish)) != null;
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
        // Remove the MutantStarfish and give the InfinitySymbol and MaxxiaScroll, then set the cooldown timer
        Item mutantStarfish = player.Backpack.FindItemByType(typeof(MutantStarfish));
        if (mutantStarfish != null)
        {
            mutantStarfish.Delete();
            player.AddToBackpack(new InfinitySymbol());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MutantStarfish and receive an InfinitySymbol and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a MutantStarfish.");
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