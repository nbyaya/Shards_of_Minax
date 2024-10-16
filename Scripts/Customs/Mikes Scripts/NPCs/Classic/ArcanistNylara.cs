using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ArcanistNylara : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ArcanistNylara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Arcanist Nylara";
        Body = 0x191; // Human female body

        // Stats
        SetStr(150);
        SetDex(50);
        SetInt(100);

        SetHits(100);

        // Appearance
        AddItem(new Robe() { Hue = 1260 });
        AddItem(new Boots() { Hue = 1910 });
        AddItem(new TallStrawHat() { Hue = 1260 });
        AddItem(new LeatherGloves() { Hue = 1260 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Arcanist Nylara, dedicated to the study of ancient magic. How can I assist you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Arcanist Nylara. I come from a lineage of powerful mages, the Nylara clan, and we have been guarding these realms for generations.");
                aboutModule.AddOption("Tell me more about your lineage.",
                    p => true,
                    p =>
                    {
                        DialogueModule lineageModule = new DialogueModule("My ancestors played crucial roles during the arcane wars, battling dark forces to keep our world safe. Our family has always been dedicated to the pursuit of knowledge and the protection of the realms.");
                        lineageModule.AddOption("That sounds impressive.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, lineageModule));
                    });
                aboutModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you tell me about the arcane forces?",
            player => true,
            player =>
            {
                DialogueModule forcesModule = new DialogueModule("True wisdom lies in understanding the balance of forces. Only by understanding the harmony between opposing forces can one truly master the arcane arts.");
                forcesModule.AddOption("What do you mean by balance?",
                    p => true,
                    p =>
                    {
                        DialogueModule balanceModule = new DialogueModule("Balance is the foundation of magic. The arcane arts require a deep understanding of both creation and destruction, light and dark. Only then can a mage truly harness its power.");
                        balanceModule.AddOption("Thank you for the explanation.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, balanceModule));
                    });
                forcesModule.AddOption("I see. Thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, forcesModule));
            });

        greeting.AddOption("I heard you need to brew a love potion?",
            player => true,
            player =>
            {
                DialogueModule lovePotionModule = new DialogueModule("Ah, you've heard of my... predicament. Yes, it's true. I seek to brew a love potion, but not for a trivial purpose. I desire the affections of Lord Blackthorn, for with his power and influence, we could change the realms for the better.");
                lovePotionModule.AddOption("Why do you want Lord Blackthorn to marry you?",
                    p => true,
                    p =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Lord Blackthorn possesses a rare combination of charisma, wisdom, and strength. He is the key to uniting the fractured factions of this realm. Together, I believe we can bring peace and prosperity, but only if he loves me truly.");
                        reasonModule.AddOption("That sounds ambitious. How can I help?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule helpModule = new DialogueModule("To brew the potion, I require several rare ingredients: a Rose of Eternal Bloom, a Dragon's Tear, and Stardust collected under a full moon. Each of these is incredibly rare and difficult to obtain.");
                                helpModule.AddOption("Where can I find the Rose of Eternal Bloom?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule roseModule = new DialogueModule("The Rose of Eternal Bloom can be found deep within the Enchanted Forest. It is guarded by a spirit of nature who only allows those with pure intentions to approach. You must prove your worth to the spirit before it will part with the rose.");
                                        roseModule.AddOption("I'll try to prove myself to the spirit.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You set off towards the Enchanted Forest, determined to prove your worth.");
                                            });
                                        roseModule.AddOption("That sounds challenging. Maybe later.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, roseModule));
                                    });
                                helpModule.AddOption("What about the Dragon's Tear?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule tearModule = new DialogueModule("The Dragon's Tear is a gemstone created when a dragon mourns the loss of a loved one. Such a tear can only be obtained by gaining the trust of a dragon and sharing in its sorrow. It is said that the dragons of the Crimson Peak have recently experienced a great loss.");
                                        tearModule.AddOption("I will seek out the dragons of Crimson Peak.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You head towards Crimson Peak, hoping to earn the trust of the dragons.");
                                            });
                                        tearModule.AddOption("That seems too dangerous for me.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, tearModule));
                                    });
                                helpModule.AddOption("And the Stardust?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stardustModule = new DialogueModule("The Stardust must be collected under a full moon at the top of the Whispering Mountain. The winds there are fierce, and the path is treacherous. Many who have attempted the climb have never returned. You must be prepared for the journey.");
                                        stardustModule.AddOption("I will climb the Whispering Mountain.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("You begin your preparations to climb the Whispering Mountain under the full moon.");
                                            });
                                        stardustModule.AddOption("That sounds too risky.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, stardustModule));
                                    });
                                helpModule.AddOption("I don't think I can help right now.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, helpModule));
                            });
                        reasonModule.AddOption("I think this is a bad idea.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, reasonModule));
                    });
                lovePotionModule.AddOption("What ingredients do you need?",
                    p => true,
                    p =>
                    {
                        DialogueModule ingredientsModule = new DialogueModule("To create the love potion, I need a Rose of Eternal Bloom, a Dragon's Tear, and Stardust collected under a full moon. Each of these ingredients holds immense magical properties and is essential for the potion's success.");
                        ingredientsModule.AddOption("Where can I find these ingredients?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, ingredientsModule));
                    });
                lovePotionModule.AddOption("I wish you luck with your potion.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, lovePotionModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    noRewardModule.AddOption("I'll come back later.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    player.AddToBackpack(new CurseAugmentCrystal());
                    lastRewardTime = DateTime.UtcNow;
                    DialogueModule rewardModule = new DialogueModule("Here is a reward imbued with the energy of the ancients. Use it wisely.");
                    rewardModule.AddOption("Thank you.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Tell me about the arcane wars.",
            player => true,
            player =>
            {
                DialogueModule warsModule = new DialogueModule("The arcane wars were fought between mages and sorcerers for control over powerful artifacts. Many lives were lost, and the echoes of those battles still resonate today.");
                warsModule.AddOption("What kind of artifacts?",
                    p => true,
                    p =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("These artifacts were sources of immense power. Some could command the elements, while others had the power to bend time and space. They are incredibly dangerous in the wrong hands.");
                        artifactsModule.AddOption("Thank you for the information.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, artifactsModule));
                    });
                warsModule.AddOption("Sounds like a difficult time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, warsModule));
            });

        return greeting;
    }

    public ArcanistNylara(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}