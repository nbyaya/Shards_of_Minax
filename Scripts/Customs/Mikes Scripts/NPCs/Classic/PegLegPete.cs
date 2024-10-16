using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class PegLegPete : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public PegLegPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Peg-leg Pete";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        
        // Stats
        SetStr(120);
        SetDex(55);
        SetInt(20);
        SetHits(85);

        // Appearance
        AddItem(new TricorneHat() { Hue = 2123 });
        AddItem(new FancyShirt() { Hue = 38 });
        AddItem(new ShortPants() { Hue = 2124 });
        AddItem(new Boots() { Hue = 1170 });
        AddItem(new Longsword() { Name = "Pete's Blade" });

        lastRewardTime = DateTime.MinValue; // Initialize the last reward time
    }

    public PegLegPete(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Arrr, I'm Peg-leg Pete, the one-legged pirate! What ye want, landlubber?");
        
        greeting.AddOption("Tell me about your adventures.",
            player => true,
            player =>
            {
                DialogueModule adventuresModule = new DialogueModule("Life on the sea be tough, lad. It takes more than a sharp blade to survive. Be ye up to the challenge?");
                adventuresModule.AddOption("Aye, I be ready for the challenge!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateChallengeModule()));
                    });
                adventuresModule.AddOption("I think I'll pass.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, adventuresModule));
            });

        greeting.AddOption("What do you know about the Lost Jewel of the Sea?",
            player => true,
            player =>
            {
                DialogueModule jewelModule = new DialogueModule("Aye, the Lost Jewel o' the Sea be a fabled gem, said to be as big as a fist and shinin' brighter than the North Star. Many have sought it, few have lived to tell the tale. If ye ever find it, bring it to me, and I'll make it worth yer while.");
                jewelModule.AddOption("I'll keep an eye out for it.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jewelModule));
            });

        greeting.AddOption("Tell me about your crew.",
            player => true,
            player =>
            {
                DialogueModule crewModule = new DialogueModule("Aye, me crew be the saltiest bunch of scallywags ye ever did see! But they're loyal to a fault. Once saved me life from a kraken's grasp, they did! Have ye ever seen such a beast?");
                crewModule.AddOption("I've heard tales of the kraken.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateKrakenModule()));
                    });
                crewModule.AddOption("I haven't, but I don't want to!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, crewModule));
            });

        greeting.AddOption("What led you to piracy?",
            player => true,
            player =>
            {
                DialogueModule piracyModule = new DialogueModule("Ah, that be a tale worth tellin'. I once had dreams of bein' a librarian. But they laughed me out, said I was too rough 'n tumble for the bookshelves.");
                piracyModule.AddOption("Why did you want to be a librarian?",
                    p => true,
                    p =>
                    {
                        DialogueModule librarianDreamsModule = new DialogueModule("I always believed knowledge should be free for all! I wanted to share stories and wisdom, to create a world where everyone could read and learn.");
                        librarianDreamsModule.AddOption("That sounds noble. What happened?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule rejectionModule = new DialogueModule("They said I didn't fit their image—too many scars, too much grit. I was crushed, lad. The world ain't fair, and I sought a different path.");
                                rejectionModule.AddOption("So you turned to piracy for equality?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule equalityModule = new DialogueModule("Aye! On the high seas, I found me true calling. Here, I could take what was denied to me and share it with me crew. A more equal world, that's what I seek!");
                                        equalityModule.AddOption("How can piracy create equality?",
                                            pq => true,
                                            pq =>
                                            {
                                                DialogueModule createEqualityModule = new DialogueModule("By redistributing the wealth of the rich! The sea be filled with treasures hoarded by the greedy. I take from them and give to those in need. That's me code.");
                                                createEqualityModule.AddOption("What about the consequences?",
                                                    plw => true,
                                                    plw =>
                                                    {
                                                        DialogueModule consequencesModule = new DialogueModule("Aye, there be risks, but life on the sea be a gamble. Better to live free and wild than caged in the libraries, don’t ye think?");
                                                        consequencesModule.AddOption("I suppose freedom is worth it.",
                                                            plae => true,
                                                            plae =>
                                                            {
                                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                            });
                                                        consequencesModule.AddOption("It sounds dangerous.",
                                                            plar => true,
                                                            plar =>
                                                            {
                                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                            });
                                                        p.SendGump(new DialogueGump(p, consequencesModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, createEqualityModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, equalityModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, rejectionModule));
                            });
                        p.SendGump(new DialogueGump(p, librarianDreamsModule));
                    });
                player.SendGump(new DialogueGump(player, piracyModule));
            });

        greeting.AddOption("Do you have any quests for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule questModule = new DialogueModule("Lillian was a gem, not just for her voice, but her spirit. She left me crew years ago in search of her own destiny. If ye ever find her, give her this old locket from me. And for yer trouble, here's somethin' for ye.");
                    player.AddToBackpack(new FishingAugmentCrystal()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    questModule.AddOption("Thank you, I'll keep an eye out for her.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, questModule));
                }
            });

        return greeting;
    }

    private DialogueModule CreateChallengeModule()
    {
        DialogueModule challengeModule = new DialogueModule("Har! A brave one, ye be! But remember, in the pirate's code, there be no fleein'. What say ye?");
        challengeModule.AddOption("I'm ready to face any danger!",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
            });
        challengeModule.AddOption("Perhaps another time.",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
            });
        return challengeModule;
    }

    private DialogueModule CreateKrakenModule()
    {
        DialogueModule krakenModule = new DialogueModule("The kraken be a monster from the deep, tentacles longer than masts, and a hunger for ships and men alike! If ye ever encounter one, be sure to have a bard with ye. Their songs can soothe the beast.");
        krakenModule.AddOption("Bards can calm the kraken?",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
            });
        return krakenModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}
