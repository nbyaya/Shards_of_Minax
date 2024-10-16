using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CarmenTheCrimson : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CarmenTheCrimson() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Carmen the Crimson";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(50);
        SetHits(75);

        // Appearance
        AddItem(new FancyDress() { Hue = 1156 });
        AddItem(new GoldNecklace());
        AddItem(new Boots() { Hue = 1174 });
        AddItem(new Dagger() { Name = "Carmen's Blade" });

        // Hair and facial features
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = 0x2041;
        FacialHairHue = 38;

        SpeechHue = 0;

        lastRewardTime = DateTime.MinValue;
    }

    public CarmenTheCrimson(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I'm Carmen the Crimson, the sharpest blade in this wretched town. What do you want?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I'm Carmen, known as the Crimson. I relieve folks of their burdens, for a fee. What else would you like to know?");
                identityModule.AddOption("What burdens do you relieve?",
                    p => true,
                    p =>
                    {
                        DialogueModule burdenModule = new DialogueModule("Yes, heavy pockets weigh one down. I help lighten the load by taking... 'donations'. Curious about my collection?");
                        burdenModule.AddOption("Tell me about your collection.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule collectionModule = new DialogueModule("Over the years, I've amassed quite a hoard. Say, I could use someone like you to fetch a particular item. Interested?");
                                collectionModule.AddOption("Yes, I'm interested.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule taskModule = new DialogueModule("There's a pendant, once owned by a powerful mage. It's rumored to be in the catacombs. Retrieve it, and I might reward you handsomely.");
                                        taskModule.AddOption("I'll retrieve the pendant for you.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You set off to retrieve the pendant from the catacombs.");
                                            });
                                        taskModule.AddOption("Maybe another time.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, taskModule));
                                    });
                                collectionModule.AddOption("Not right now.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, collectionModule));
                            });
                        burdenModule.AddOption("I'm not interested in your collection.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, burdenModule));
                    });
                identityModule.AddOption("What is your job?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("Job, you say? I relieve folks of their burdens, for a fee. True valor is a myth in this town, my friend. Tell me, do you have the guts to survive here?");
                        jobModule.AddOption("Yes, I have the guts.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Guts alone won't get you far. You need cunning, like me. Got it?");
                            });
                        jobModule.AddOption("No, I think I'll pass.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("Tell me about your favorite kills.",
                    p => true,
                    p =>
                    {
                        DialogueModule killModule = new DialogueModule("Ah, my favorite kills... I've had many, but some stand out more than others. Would you like to hear about the merchant in the woods, the nobleman in his bed, or the band of thieves?");
                        killModule.AddOption("Tell me about the merchant in the woods.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule merchantModule = new DialogueModule("The merchant thought he could travel alone with a chest full of gold. Foolish, really. I tracked him through the woods for days, watching his every move. When the time was right, I struck. His screams were swallowed by the trees, and his gold became mine. It was almost poetic. Do you want to hear more?");
                                merchantModule.AddOption("How did you track him?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule trackingModule = new DialogueModule("Tracking is an art. You watch for broken branches, disturbed earth, even the faint smell of sweat on the wind. The merchant was careless—he left a trail a child could follow. But patience is key. I waited until he was far from help, tired, and vulnerable.");
                                        trackingModule.AddOption("That sounds impressive.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen smirks, clearly pleased. 'It was. One of my finer moments.'");
                                            });
                                        trackingModule.AddOption("That's brutal.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen shrugs. 'Survival is brutal, my friend. He should have known better.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, trackingModule));
                                    });
                                merchantModule.AddOption("What did you do with the gold?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule goldModule = new DialogueModule("The gold? I used it to buy information, favors, and weapons. Gold is only as good as what it can get you. And I always make sure it gets me power.");
                                        goldModule.AddOption("Sounds like you know how to use your resources.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen nods approvingly. 'Exactly. Resources are meant to be used, not hoarded.'");
                                            });
                                        goldModule.AddOption("That's a dangerous way to live.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen laughs. 'Danger is my business, darling. I wouldn't have it any other way.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, goldModule));
                                    });
                                merchantModule.AddOption("I've heard enough.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, merchantModule));
                            });
                        killModule.AddOption("Tell me about the nobleman in his bed.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule noblemanModule = new DialogueModule("The nobleman was arrogant, living in luxury while others starved. He had guards, but they were lazy, complacent. I slipped into his estate under the cover of darkness, found him in his bed, and ended his life before he even woke up. It was almost too easy.");
                                noblemanModule.AddOption("How did you get past the guards?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule guardModule = new DialogueModule("Guards are predictable. They get bored, they get distracted. A bribe here, a distraction there, and suddenly the way is clear. People think a fortress keeps them safe, but it's the people inside who are the weak links.");
                                        guardModule.AddOption("You're very resourceful.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen smiles. 'I have to be. Resourcefulness is what keeps me alive.'");
                                            });
                                        guardModule.AddOption("That's risky.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen shrugs. 'Risk is part of the thrill. Without it, life would be dull.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, guardModule));
                                    });
                                noblemanModule.AddOption("Why did you kill him?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule reasonModule = new DialogueModule("He was exploiting the people, taking their money and giving nothing in return. Someone had to put an end to it. I may be a killer, but even I have my standards.");
                                        reasonModule.AddOption("Sounds like he deserved it.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen nods. 'He did. And I don't lose sleep over it.'");
                                            });
                                        reasonModule.AddOption("Killing is still wrong.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen frowns. 'Perhaps. But in this world, sometimes it's the only option.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, reasonModule));
                                    });
                                noblemanModule.AddOption("I've heard enough.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, noblemanModule));
                            });
                        killModule.AddOption("Tell me about the band of thieves.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule thievesModule = new DialogueModule("The band of thieves thought they could muscle in on my territory. They were wrong. I waited until they were gathered around their campfire, drunk and overconfident. One by one, I took them out, until none were left. They never saw me coming.");
                                thievesModule.AddOption("Why didn't they see you coming?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stealthModule = new DialogueModule("They were careless, and I am very, very good at what I do. The shadows are my ally, and I know how to move without a sound. By the time they realized what was happening, it was too late.");
                                        stealthModule.AddOption("You're a true predator.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen grins. 'A predator, yes. And they were easy prey.'");
                                            });
                                        stealthModule.AddOption("That sounds terrifying.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen's smile fades slightly. 'It should be. Fear keeps people alive... sometimes.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, stealthModule));
                                    });
                                thievesModule.AddOption("What did you do with their loot?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule lootModule = new DialogueModule("Their loot was nothing special—trinkets, coins, a few weapons. I kept what was useful and sold the rest. Waste not, want not.");
                                        lootModule.AddOption("Efficient.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen nods. 'That's the way to survive.'");
                                            });
                                        lootModule.AddOption("Not very glamorous.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Carmen laughs. 'Not everything is glamorous. Sometimes it's just about making it to the next day.'");
                                            });
                                        pla.SendGump(new DialogueGump(pla, lootModule));
                                    });
                                thievesModule.AddOption("I've heard enough.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, thievesModule));
                            });
                        killModule.AddOption("I've heard enough.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, killModule));
                    });
                identityModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Carmen nods and turns away.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Can you tell me about your blade?",
            player => true,
            player =>
            {
                DialogueModule bladeModule = new DialogueModule("Not just any blade, a blade bathed in the blood of a dragon. It gives me an edge in my line of work. Ever seen dragon's blood?");
                bladeModule.AddOption("No, I've never seen it.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("It's a rare sight. The crimson hue, the fiery essence. But I know where one can find some. Perhaps that's a tale for another time.");
                    });
                bladeModule.AddOption("Yes, I have.",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        p.SendMessage("It's a rare sight. Here, take this as a token of my regard.");
                        p.AddToBackpack(new ArcheryAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow;
                    });
                bladeModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Carmen nods and turns away.");
                    });
                player.SendGump(new DialogueGump(player, bladeModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Carmen smirks and turns away.");
            });

        return greeting;
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