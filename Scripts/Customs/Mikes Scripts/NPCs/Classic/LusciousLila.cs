using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Luscious Lila")]
public class LusciousLila : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LusciousLila() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Luscious Lila";
        Body = 0x191; // Human female body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();
        
        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(50);
        SetHits(60);

        // Appearance
        AddItem(new Skirt() { Hue = 2951 });
        AddItem(new Tunic() { Hue = 2950 });
        AddItem(new Sandals() { Hue = 2965 });

        // Initialize lastRewardTime
        lastRewardTime = DateTime.MinValue;
    }

    public LusciousLila(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Welcome, darling. I am Luscious Lila. How may I assist you?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player => { player.SendGump(new DialogueGump(player, CreateAboutMeModule())); });
        
        greeting.AddOption("What services do you offer?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateServicesModule()));
            });
        
        greeting.AddOption("What can you tell me about the world?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateWorldModule()));
            });

        return greeting;
    }

    private DialogueModule CreateAboutMeModule()
    {
        DialogueModule aboutMe = new DialogueModule("Oh, my health? Just dandy, my sweet. And yours?");
        
        aboutMe.AddOption("What's your story?",
            player => true,
            player => {
                DialogueModule storyModule = new DialogueModule("Ah, darling, my life is a tapestry woven with threads of adventure and intrigue. I grew up in a small village, surrounded by whispers of magic. It was there I discovered my passion for the mystical arts.");
                storyModule.AddOption("What kind of magic?",
                    playera => true,
                    playera => {
                        storyModule.AddOption("Tell me about potion-making.",
                            pl => true,
                            pl => {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        storyModule.AddOption("What about divination?",
                            pl => true,
                            pl => {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        playera.SendGump(new DialogueGump(playera, storyModule));
                    });
                player.SendGump(new DialogueGump(player, storyModule));
            });
        
        aboutMe.AddOption("What do you enjoy most?",
            player => true,
            player => {
                aboutMe.AddOption("I love the thrill of the unknown.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateUnknownModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutMe));
            });

        return aboutMe;
    }

    private DialogueModule CreateUnknownModule()
    {
        DialogueModule unknownModule = new DialogueModule("The world is filled with mysteries, darling. Every corner has a story waiting to unfold. I often wonder what lies beyond the next hill.");
        unknownModule.AddOption("Have you ever ventured far?",
            player => true,
            player => {
                unknownModule.AddOption("Yes, to the Shadow Realm.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateShadowRealmModule()));
                    });
                unknownModule.AddOption("Not really, I'm more cautious.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateCautiousModule()));
                    });
                player.SendGump(new DialogueGump(player, unknownModule));
            });
        return unknownModule;
    }

    private DialogueModule CreateShadowRealmModule()
    {
        DialogueModule shadowRealmModule = new DialogueModule("Ah, the Shadow Realm! It’s a place of darkness but also of hidden treasures. I encountered many spectral beings there, each with their own tales.");
        shadowRealmModule.AddOption("What kind of treasures?",
            player => true,
            player => {
                shadowRealmModule.AddOption("I found an ancient tome.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateTomeModule()));
                    });
                shadowRealmModule.AddOption("A crystal that glows with light.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateCrystalModule()));
                    });
                player.SendGump(new DialogueGump(player, shadowRealmModule));
            });
        return shadowRealmModule;
    }

    private DialogueModule CreateTomeModule()
    {
        return new DialogueModule("The tome contained spells of incredible power, but also warned of dire consequences if misused. Knowledge is a double-edged sword, darling.");
    }

    private DialogueModule CreateCrystalModule()
    {
        return new DialogueModule("The crystal pulses with a soft glow, said to guide lost souls. It's a rare find, one that many would covet!");
    }

    private DialogueModule CreateCautiousModule()
    {
        DialogueModule cautiousModule = new DialogueModule("Ah, a wise choice! Caution is often the better part of valor. There are many dangers lurking in the shadows, waiting for the unwary.");
        cautiousModule.AddOption("What kind of dangers?",
            player => true,
            player => {
                cautiousModule.AddOption("I've heard of dark spirits.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateSpiritsModule()));
                    });
                cautiousModule.AddOption("What about bandits?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateBanditsModule()));
                    });
                player.SendGump(new DialogueGump(player, cautiousModule));
            });
        return cautiousModule;
    }

    private DialogueModule CreateSpiritsModule()
    {
        return new DialogueModule("Yes, dark spirits that seek to ensnare the unwary traveler. It's said they can whisper your deepest fears and draw you into their realm.");
    }

    private DialogueModule CreateBanditsModule()
    {
        return new DialogueModule("Ah, bandits. Greedy and ruthless, they lie in wait for travelers. Always stay alert and keep your belongings close, darling.");
    }

    private DialogueModule CreateServicesModule()
    {
        DialogueModule servicesModule = new DialogueModule("I can read palms, dance like a dream, and brew the most curious of potions. Ever tried a love potion, darling?");
        servicesModule.AddOption("I'd like to purchase a potion.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateLovePotionModule()));
            });
        servicesModule.AddOption("What potions do you have?",
            player => true,
            player => {
                servicesModule.AddOption("Tell me about love potions.",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateLovePotionModule()));
                    });
                servicesModule.AddOption("What about healing potions?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateHealingPotionModule()));
                    });
                player.SendGump(new DialogueGump(player, servicesModule));
            });
        return servicesModule;
    }

    private DialogueModule CreateLovePotionModule()
    {
        DialogueModule lovePotionModule = new DialogueModule("Ah, love potions! A delicate brew that can spark affection in even the coldest of hearts. But beware, darling, they come with a price.");
        lovePotionModule.AddOption("What price?",
            player => true,
            player => {
                lovePotionModule.AddOption("Is it gold?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateGoldPriceModule()));
                    });
                lovePotionModule.AddOption("What about a secret?",
                    pl => true,
                    pl => {
                        pl.SendGump(new DialogueGump(pl, CreateSecretPriceModule()));
                    });
                player.SendGump(new DialogueGump(player, lovePotionModule));
            });
        return lovePotionModule;
    }

    private DialogueModule CreateGoldPriceModule()
    {
        return new DialogueModule("For you, darling, it's a mere 50 gold coins. But remember, true love cannot be bought! It must be earned.");
    }

    private DialogueModule CreateSecretPriceModule()
    {
        return new DialogueModule("Ah, a secret! Secrets are powerful. Share one with me, and perhaps the potion will work even better.");
    }

    private DialogueModule CreateHealingPotionModule()
    {
        return new DialogueModule("Healing potions are a staple for adventurers. They can mend wounds and restore vitality. Just be sure to use them wisely!");
    }

    private DialogueModule CreateWorldModule()
    {
        DialogueModule worldModule = new DialogueModule("Ah, this world is a tapestry of tales. Some of joy, some of sorrow. Do you have any favorite places you've been?");
        worldModule.AddOption("What about the Whispering Woods?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateWoodsModule()));
            });
        return worldModule;
    }

    private DialogueModule CreateWoodsModule()
    {
        DialogueModule woodsModule = new DialogueModule("Ah, the Whispering Woods. A place where trees speak if you listen closely. But it's not for the faint of heart.");
        woodsModule.AddOption("What kind of creatures are there?",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateCreaturesModule()));
            });
        return woodsModule;
    }

    private DialogueModule CreateCreaturesModule()
    {
        DialogueModule creaturesModule = new DialogueModule("Yes, creatures like shadows that move without sound and eerie spirits. But if you're brave, treasures await the adventurous. Ever gone treasure hunting?");
        creaturesModule.AddOption("Tell me more about the treasures.",
            player => true,
            player => {
                player.SendGump(new DialogueGump(player, CreateTreasureModule()));
            });
        return creaturesModule;
    }

    private DialogueModule CreateTreasureModule()
    {
        DialogueModule treasureModule = new DialogueModule("Treasures are stories, memories, experiences. If it's gold you seek, there's a hidden chest in the woods, guarded by an ancient spirit. Good luck, darling.");
        return treasureModule;
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
