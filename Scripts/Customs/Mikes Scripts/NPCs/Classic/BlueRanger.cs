using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BlueRanger : BaseCreature
{
    [Constructable]
    public BlueRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Blue Ranger";
        Body = 0x190; // Human male body

        // Stats
        SetStr(105);
        SetDex(115);
        SetInt(55);

        SetHits(80);

        // Appearance
        AddItem(new RingmailLegs() { Hue = 2 });
        AddItem(new RingmailChest() { Hue = 2 });
        AddItem(new CloseHelm() { Hue = 2 });
        AddItem(new RingmailGloves() { Hue = 2 });
        AddItem(new Boots() { Hue = 2 });
        AddItem(new Halberd { Name = "Blue Ranger's Halberd" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        Direction = Direction.East;

        SpeechHue = 0; // Default speech hue
    }

    public BlueRanger(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am the Blue Ranger. What do you want, puny mortal?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am the Blue Ranger. A so-called 'Power Ranger,' fighting for justice and all that nonsense. What a joke!");
                whoModule.AddOption("Why do you say it's a joke?",
                    p => true,
                    p =>
                    {
                        DialogueModule jokeModule = new DialogueModule("Justice? What is justice in a world filled with chaos and despair? I once believed in it, but now? I'm not so sure.");
                        jokeModule.AddOption("Have you seen true justice?",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("The Blue Ranger narrows his eyes, lost in thought, but does not answer.");
                            });
                        jokeModule.AddOption("Perhaps you can still find hope.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Hope? Maybe... but it's a fragile thing.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        jokeModule.AddOption("What was it like being a Power Ranger?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule pastModule = new DialogueModule("Being a Power Ranger... It was exhilarating, terrifying, and everything in between. The power we wielded was beyond imagination, but it came at a cost.");
                                pastModule.AddOption("What kind of power did you have?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule powerModule = new DialogueModule("The technology we used was centuries ahead of anything I had ever seen. We had Zords—giant mechanical beasts that responded to our commands—and suits that enhanced our abilities. It felt like being invincible, but that kind of power... it changes you.");
                                        powerModule.AddOption("How did it change you?",
                                            plla => true,
                                            plla =>
                                            {
                                                DialogueModule changeModule = new DialogueModule("The power made us feel like gods. It gave us a sense of invulnerability, but also a sense of distance from those we protected. The people started to look... smaller, more fragile. It was difficult to stay grounded, to remember why we fought.");
                                                changeModule.AddOption("Did you struggle to stay grounded?",
                                                    pllb => true,
                                                    pllb =>
                                                    {
                                                        DialogueModule groundedModule = new DialogueModule("Yes. We all did, in our own ways. Some of us tried to keep connections with friends and family, others lost themselves entirely in the fight. I tried to focus on understanding the technology—learning about the advanced systems we used, hoping it would keep me human, give me purpose.");
                                                        groundedModule.AddOption("Tell me more about the technology.",
                                                            pllc => true,
                                                            pllc =>
                                                            {
                                                                DialogueModule techModule = new DialogueModule("The technology was incredible. Our suits were made of an unknown alloy that provided protection beyond anything normal armor could. The Zords were biomechanical, almost alive in a sense, and they responded to our thoughts and emotions. I spent countless nights trying to understand them, but they were far beyond human science.");
                                                                techModule.AddOption("Did you ever understand the Zords?",
                                                                    plld => true,
                                                                    plld =>
                                                                    {
                                                                        DialogueModule zordModule = new DialogueModule("Not entirely. They seemed to have a will of their own, as if they were ancient beings that chose to help us. I remember one night, after a particularly difficult battle, I stayed up, trying to communicate with my Zord. It felt like... it understood me, in a way no human ever could.");
                                                                        zordModule.AddOption("That sounds lonely.",
                                                                            plle => true,
                                                                            plle =>
                                                                            {
                                                                                plle.SendMessage("The Blue Ranger looks away, his eyes distant. 'It was. But it was also comforting, in a strange way.'");
                                                                            });
                                                                        zordModule.AddOption("Did you feel connected to your Zord?",
                                                                            plle => true,
                                                                            plle =>
                                                                            {
                                                                                DialogueModule connectionModule = new DialogueModule("Yes, deeply connected. The Zord wasn't just a machine. It was like a partner, a guardian. In battle, we moved as one, our minds linked. It gave me a strength I never thought possible, but also a vulnerability. If the Zord was damaged, I felt it, emotionally, even physically.");
                                                                                connectionModule.AddOption("That sounds like a powerful bond.",
                                                                                    pllf => true,
                                                                                    pllf =>
                                                                                    {
                                                                                        pllf.SendMessage("The Blue Ranger nods slowly. 'It was. Perhaps the strongest bond I've ever had.'");
                                                                                    });
                                                                                plle.SendGump(new DialogueGump(plle, connectionModule));
                                                                            });
                                                                        plld.SendGump(new DialogueGump(plld, zordModule));
                                                                    });
                                                                techModule.AddOption("Why did you want to understand the technology?",
                                                                    plld => true,
                                                                    plld =>
                                                                    {
                                                                        DialogueModule reasonModule = new DialogueModule("I thought that if I could understand it, maybe I could control it better. Maybe I could find a way to make it safer for us, for the people we protected. But the more I learned, the more I realized how little I knew. The technology was beyond us—it was almost like it had a mind of its own.");
                                                                        reasonModule.AddOption("Did that scare you?",
                                                                            plle => true,
                                                                            plle =>
                                                                            {
                                                                                plle.SendMessage("The Blue Ranger exhales deeply. 'Yes. It scared all of us. But we couldn't let fear control us. We had to keep going, no matter the cost.'");
                                                                            });
                                                                        reasonModule.AddOption("Did you ever regret it?",
                                                                            plle => true,
                                                                            plle =>
                                                                            {
                                                                                DialogueModule regretModule = new DialogueModule("Sometimes. There were moments when I wondered if we were in over our heads, if we were meddling with forces we couldn't understand. But then I'd see the people we saved, the lives we protected, and it made the fear worth it.");
                                                                                regretModule.AddOption("You did what you had to do.",
                                                                                    pllf => true,
                                                                                    pllf =>
                                                                                    {
                                                                                        pllf.SendMessage("The Blue Ranger nods, a hint of a smile on his lips. 'Yes, we did.'");
                                                                                    });
                                                                                regretModule.AddOption("That must have been hard.",
                                                                                    pllf => true,
                                                                                    pllf =>
                                                                                    {
                                                                                        pllf.SendMessage("The Blue Ranger's eyes darken. 'It was. But no one said being a Ranger would be easy.'");
                                                                                    });
                                                                                plle.SendGump(new DialogueGump(plle, regretModule));
                                                                            });
                                                                        plld.SendGump(new DialogueGump(plld, reasonModule));
                                                                    });
                                                                pllc.SendGump(new DialogueGump(pllc, techModule));
                                                            });
                                                        pllb.SendGump(new DialogueGump(pllb, groundedModule));
                                                    });
                                                plla.SendGump(new DialogueGump(plla, changeModule));
                                            });
                                        pll.SendGump(new DialogueGump(pll, powerModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, pastModule));
                            });
                        p.SendGump(new DialogueGump(p, jokeModule));
                    });
                whoModule.AddOption("You sound disillusioned.",
                    p => true,
                    p =>
                    {
                        DialogueModule disillusionModule = new DialogueModule("Disillusioned? Ha! You don't know the half of it. Valor, justice, righteousness... all meaningless pursuits, like my existence as a Ranger.");
                        disillusionModule.AddOption("Why do you continue then?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule continueModule = new DialogueModule("Why? Maybe it's because I don't know how to do anything else. Or maybe, just maybe, I still cling to some foolish dream of redemption.");
                                continueModule.AddOption("Redemption is not foolish.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("The Blue Ranger remains silent, but a flicker of emotion crosses his face.");
                                    });
                                continueModule.AddOption("What kind of redemption are you seeking?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule redemptionModule = new DialogueModule("I don't even know. Maybe it's a way to atone for the lives I couldn't save, or the mistakes I made along the way. Being a Ranger wasn't always about victory. We failed, more times than I'd like to admit.");
                                        redemptionModule.AddOption("Failure is part of being human.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("The Blue Ranger nods slowly. 'Perhaps. But those failures still haunt me.'");
                                            });
                                        redemptionModule.AddOption("Tell me about one of those failures.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule failureModule = new DialogueModule("There was a mission... a young village was under attack by a monstrous creature. We thought we could handle it, but we underestimated its power. By the time we defeated it, half the village was gone. We saved who we could, but the cost was too great.");
                                                failureModule.AddOption("That must have been devastating.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("The Blue Ranger looks away, his face etched with pain. 'It was. Those faces... they never leave you.'");
                                                    });
                                                failureModule.AddOption("You did your best.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("The Blue Ranger sighs. 'Sometimes your best isn't enough. But you keep fighting, because what else can you do?'");
                                                    });
                                                plb.SendGump(new DialogueGump(plb, failureModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, redemptionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, continueModule));
                            });
                        p.SendGump(new DialogueGump(p, disillusionModule));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("Are you still standing after all your battles?",
            player => true,
            player =>
            {
                DialogueModule standingModule = new DialogueModule("Yes, I'm still standing, but that doesn't mean I'm not haunted by the memories of past battles. Every scar on my body has a tale to tell.");
                standingModule.AddOption("Do you carry many scars?",
                    p => true,
                    p =>
                    {
                        DialogueModule scarModule = new DialogueModule("Each scar is a mark of a battle fought, a life saved, or a mistake made. They remind me of the price of being a Ranger. Do you carry such reminders?");
                        scarModule.AddOption("I do. We all have our burdens.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("The Blue Ranger nods solemnly.");
                            });
                        scarModule.AddOption("No, I don't have such scars.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Consider yourself fortunate then. Scars are a heavy burden to bear.");
                            });
                        p.SendGump(new DialogueGump(p, scarModule));
                    });
                player.SendGump(new DialogueGump(player, standingModule));
            });

        greeting.AddOption("Do you believe in valor?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor? Ha! If you truly consider yourself valiant, then perhaps you can prove it. Complete a task for me, and I might reward you.");
                valorModule.AddOption("What task do you have for me?",
                    p => true,
                    p =>
                    {
                        DialogueModule taskModule = new DialogueModule("There's a dangerous beast lurking nearby. Defeat it, and bring me proof. If you succeed, I'll give you a reward worthy of your valor.");
                        taskModule.AddOption("I will take on this task.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("The Blue Ranger nods. 'Very well. Return to me with proof, and we shall speak again.'");
                            });
                        taskModule.AddOption("I am not ready for such a challenge.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, taskModule));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });

        greeting.AddOption("Goodbye, Blue Ranger.",
            player => true,
            player =>
            {
                player.SendMessage("The Blue Ranger grunts in acknowledgment.");
            });

        return greeting;
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