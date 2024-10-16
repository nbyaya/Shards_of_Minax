using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Lorenzo the Lyrical")]
public class LorenzoTheLyrical : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LorenzoTheLyrical() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lorenzo the Lyrical";
        Body = 0x190; // Human male body

        // Stats
        SetStr(40);
        SetDex(80);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new FancyShirt() { Hue = 1153 });
        AddItem(new LongPants() { Hue = 1153 });
        AddItem(new Boots() { Hue = 1153 });
        AddItem(new FeatheredHat() { Hue = 1153 });
        AddItem(new Lute() { Name = "Lorenzo's Lute" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue; // Initialize reward time
        SpeechHue = 0; // Default speech hue
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler! I am Lorenzo the Lyrical, a rogue with a silver tongue!");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
                player.SendGump(new DialogueGump(player, new DialogueModule("My health is as unpredictable as a gambler's luck, my friend."))));

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My craft is in the shadows, dancing with the moonlight and whispering secrets in the dark. I am a rogue.");
                jobModule.AddOption("What do you mean by shadows?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule shadowsModule = new DialogueModule("The shadows are my sanctuary. It's where I can be myself, away from the eyes of the world. Do you fear the shadows or embrace them?");
                        shadowsModule.AddOption("I embrace them.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Then you understand the freedom they provide. There is much to learn in the dark."))));
                        shadowsModule.AddOption("I fear them.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Fear not, my friend! The dark holds wonders and secrets that can enlighten the heart."))));
                        pl.SendGump(new DialogueGump(pl, shadowsModule));
                    });
                jobModule.AddOption("What skills do you have?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule skillsModule = new DialogueModule("I am skilled in the art of deception, charming others with words, and slipping unnoticed through the shadows. Would you like to learn more about my tricks?");
                        skillsModule.AddOption("Yes, tell me about your tricks.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Ah, there are many! From sleight of hand to sweet-talking a guard, every rogue has their methods."))));
                        skillsModule.AddOption("No, I'm not interested.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, skillsModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What about virtue?",
            player => true,
            player => 
            {
                DialogueModule virtueModule = new DialogueModule("Life is a game of chance, my friend. What virtue guides a rogue like me?");
                virtueModule.AddOption("Is there a virtue in deception?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule deceptionModule = new DialogueModule("Deception, when used wisely, can serve greater purposes. A well-placed lie can protect those you care about.");
                        deceptionModule.AddOption("That sounds justifiable.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! Sometimes the truth can be more harmful than a gentle falsehood."))));
                        deceptionModule.AddOption("But isn't it wrong?",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Ah, but morality is subjective. Each rogue must find their own balance."))));
                        pl.SendGump(new DialogueGump(pl, deceptionModule));
                    });
                virtueModule.AddOption("What about honor?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule honorModule = new DialogueModule("Honor? In the world of shadows, honor often takes a back seat. But a rogue can choose to uphold their own code.");
                        honorModule.AddOption("What is your code?",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("My code is simple: harm not the innocent, and protect those who cannot protect themselves."))));
                        honorModule.AddOption("Isn't that contradictory?",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("Not at all! It's a fine line, but one worth walking."))));
                        pl.SendGump(new DialogueGump(pl, honorModule));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("What do you think of balance?",
            player => true,
            player => 
            {
                DialogueModule balanceModule = new DialogueModule("Interesting choice. Remember, even in the shadows, there is room for virtue. Seek the balance, my friend.");
                balanceModule.AddOption("Can you teach me about balance?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule teachBalanceModule = new DialogueModule("Balance in life is crucial. Too much of anything can lead to ruin. It's like a tightrope walk, isn't it?");
                        teachBalanceModule.AddOption("How can I achieve this balance?",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("It requires self-awareness, understanding your desires, and knowing when to hold back."))));
                        teachBalanceModule.AddOption("Sounds complicated.",
                            p => true,
                            p => 
                                p.SendGump(new DialogueGump(p, new DialogueModule("It can be, but every journey begins with a single step. Would you take that step?"))));
                        pl.SendGump(new DialogueGump(pl, teachBalanceModule));
                    });
                balanceModule.AddOption("What about rewards?",
                    pl => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    pl =>
                    {
                        pl.AddToBackpack(new SwordsmanshipAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the eternal struggle for balance. Here is a token of appreciation.")));
                    });
                balanceModule.AddOption("I have to go now.",
                    pl => true,
                    pl => 
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, balanceModule));
            });

        greeting.AddOption("What do you think about silver?",
            player => true,
            player => 
            {
                DialogueModule silverModule = new DialogueModule("Ah, my silver tongue! It's been my savior in more than one tight spot. Have you ever been swayed by a tale or song?");
                silverModule.AddOption("Yes, tales are enchanting.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! Stories weave the fabric of our lives, connecting us through shared experiences."))));
                silverModule.AddOption("No, I prefer honesty.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, but honesty can sometimes be the harshest of truths. A little embellishment never hurt anyone!"))));
                player.SendGump(new DialogueGump(player, silverModule));
            });

        greeting.AddOption("What about gambling?",
            player => true,
            player => 
            {
                DialogueModule gamblingModule = new DialogueModule("Gambling is a game of chance and fate. I've had my fair share of losses, but the thrill of the game always brings me back. Do you feel fate has a hand in your life?");
                gamblingModule.AddOption("I believe in fate.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Fate is a mysterious force, guiding us toward our destiny. But remember, we also hold the power to shape our own paths."))));
                gamblingModule.AddOption("I don't trust fate.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("A wise choice! Taking control of your own fate can lead to a more fulfilling life."))));
                player.SendGump(new DialogueGump(player, gamblingModule));
            });

        greeting.AddOption("What do you think of moonlight?",
            player => true,
            player => 
            {
                DialogueModule moonlightModule = new DialogueModule("Moonlight has always been my guide, casting the world in a gentle, silvery glow. It's the time when the world is at its most honest, don't you think?");
                moonlightModule.AddOption("Yes, it’s magical.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Magic resides in those quiet moments, where secrets are unveiled under the soft light."))));
                moonlightModule.AddOption("No, I prefer daylight.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Daylight can be blinding, but it also reveals truths hidden in the dark."))));
                player.SendGump(new DialogueGump(player, moonlightModule));
            });

        greeting.AddOption("What about the dark?",
            player => true,
            player => 
            {
                DialogueModule darkModule = new DialogueModule("In the dark, one can find both danger and opportunity. It's a time when the world sleeps, but rogues like me are wide awake. Tell me, do you fear the dark or embrace it?");
                darkModule.AddOption("I embrace the dark.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Then you understand the allure! The dark is where one can truly be free."))));
                darkModule.AddOption("I fear the dark.",
                    pl => true,
                    pl =>
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Fear not! Embrace the shadows, and you may find comfort and strength there."))));
                player.SendGump(new DialogueGump(player, darkModule));
            });

        return greeting;
    }

    public LorenzoTheLyrical(Serial serial) : base(serial) { }

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
