using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ScallywagSteve : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ScallywagSteve() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Scallywag Steve";
        Body = 0x190; // Human male body

        // Stats
        SetStr(135);
        SetDex(65);
        SetInt(20);
        SetHits(92);

        // Appearance
        AddItem(new TricorneHat() { Hue = 2122 });
        AddItem(new FancyShirt() { Hue = 1154 });
        AddItem(new ShortPants() { Hue = 1158 });
        AddItem(new Boots() { Hue = 38 });
        AddItem(new Longsword() { Name = "Steve's Sneaky Saber" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ScallywagSteve(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Arrr, ye be talkin' to Scallywag Steve, the saltiest pirate this side of the Seven Seas! But I have a secret... me heart belongs to rum!");

        greeting.AddOption("Tell me about your love for rum.",
            player => true,
            player =>
            {
                DialogueModule rumLoveModule = new DialogueModule("Ah, rum be the finest nectar of the gods! I wish I could be drunk 24/7! Life’s troubles just vanish when ye have a good mug of rum in hand.");
                rumLoveModule.AddOption("What's your favorite rum?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule favoriteRumModule = new DialogueModule("Aye, it be the Dark Spiced Rum from the Isle of the Dead Man's Chest. It warms me soul and fuels me spirit! But ye can’t just drink any rum; it has to be the finest!");
                        favoriteRumModule.AddOption("Can I try some?",
                            p => true,
                            p =>
                            {
                                p.SendMessage("Scallywag Steve offers you a mug of his finest rum!");
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        favoriteRumModule.AddOption("What makes it special?",
                            p => true,
                            p =>
                            {
                                DialogueModule specialModule = new DialogueModule("Ah, it be aged in barrels made from ancient trees, and infused with spices from distant lands! Just a sip, and ye feel like a true pirate!");
                                specialModule.AddOption("Sounds tempting! Where can I get it?",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendMessage("Rum be found in the taverns of Port Royal, or ye might need to plunder a ship or two!");
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, specialModule));
                            });
                        rumLoveModule.AddOption("Maybe I should stick to water.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, favoriteRumModule));
                    });

                rumLoveModule.AddOption("What’s the best way to enjoy rum?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule enjoyRumModule = new DialogueModule("Ye should drink it with friends, on a moonlit night, while tellin' tales of adventure and treasure! Aye, that's the life for a pirate!");
                        enjoyRumModule.AddOption("What kind of tales do you tell?",
                            p => true,
                            p =>
                            {
                                DialogueModule talesModule = new DialogueModule("Ah, tales of daring raids, mythical sea beasts, and cursed treasures! One time, I battled a giant octopus just to protect me rum stash!");
                                talesModule.AddOption("I want to hear more about that!",
                                    plw => true,
                                    plw =>
                                    {
                                        DialogueModule octopusTaleModule = new DialogueModule("It was a dark and stormy night when the beast rose from the depths! Tentacles flailing, it tried to snatch me barrel of rum! But with a fierce yell and a good swing of me sword, I sent it back to the depths!");
                                        octopusTaleModule.AddOption("That sounds epic! Did you win the battle?",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendMessage("Scallywag Steve winks at you.");
                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, octopusTaleModule));
                                    });
                                p.SendGump(new DialogueGump(p, talesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, enjoyRumModule));
                    });

                rumLoveModule.AddOption("Rum be dangerous, isn't it?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule dangerModule = new DialogueModule("Aye, it can lead ye to trouble! I've lost me ship in a game of dice after a few too many mugs! But what’s life without a little risk?");
                        dangerModule.AddOption("True, but ye have to be careful!",
                            pla => true,
                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, dangerModule));
                    });

                player.SendGump(new DialogueGump(player, rumLoveModule));
            });

        greeting.AddOption("What do ye do when ye can't have rum?",
            player => true,
            player =>
            {
                DialogueModule noRumModule = new DialogueModule("Arrr, that be a sad day! I long for the taste of rum! I fill me time with tales and daydreams of me next mug.");
                noRumModule.AddOption("Tell me about your daydreams.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule daydreamsModule = new DialogueModule("I dream of grand feasts filled with endless rum and songs sung by me hearty crew! Aye, a pirate's life be grand when rum flows like water!");
                        daydreamsModule.AddOption("That sounds delightful!",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, daydreamsModule));
                    });
                noRumModule.AddOption("Do you have a backup drink?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backupDrinkModule = new DialogueModule("Aye, I sometimes settle for grog, but it just ain't the same! It be like sailing a ship with one sail!");
                        backupDrinkModule.AddOption("What’s in grog?",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, backupDrinkModule));
                    });
                player.SendGump(new DialogueGump(player, noRumModule));
            });

        greeting.AddOption("Good luck with your rum adventures!",
            player => true,
            player =>
            {
                player.SendMessage("Scallywag Steve grins widely. 'Aye, may ye always find the finest rum!'");
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
