using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BasilII : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BasilII() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Basil II";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(100);
        SetInt(100);

        SetHits(60);

        // Appearance
        AddItem(new Cloak(0x46F)); // BloodRed color
        AddItem(new Boots(0x46F)); // BloodRed color
        AddItem(new Longsword() { Name = "Basil II's Sword" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Basil II of Byzantium. How may I assist you?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Basil II, guardian of virtue and protector of Byzantium. You wish to know more? Oh, where do I begin? The throne, the weight, the endless sea of enemies and allies! Byzantium! It is not just an empire, it's a ravenous beast, devouring and demanding! What more would you like to know?");
                identityModule.AddOption("Tell me about Byzantium.",
                    p => true,
                    p =>
                    {
                        DialogueModule byzantiumModule = new DialogueModule("Byzantium, oh Byzantium! The walls are high but the pressure—oh, it's higher still! A land of gold, glory, and a thousand betrayals. The nobles, they scheme day and night! Do you know what it is like to sleep with one eye open, fearing that the very people you dine with will plunge a dagger into your back? Have you heard of the endless plots, the sleepless nights?");
                        byzantiumModule.AddOption("Tell me about the plots.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule plotsModule = new DialogueModule("Plots! HA! They never end! The conspiracies, the whispers behind closed doors. Each nobleman dreams of the throne, their eyes full of ambition, their smiles as poisonous as the serpents that slither in the palace garden. My own cousin tried to overthrow me twice! TWICE! Imagine having to drink your wine knowing it could be your last sip because some jester thought today would be a good day for poison!");
                                plotsModule.AddOption("How do you deal with the traitors?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule traitorModule = new DialogueModule("Deal with them? Oh, you never truly 'deal' with them. You can cut them down, exile them, have them drawn and quartered, but like the Hydra, two more heads sprout for each one you sever! It is maddening! Some days I wonder if I should just set the whole palace ablaze and dance in the ashes! Do you think I should do it? Burn it all down and be free of their plotting?");
                                        traitorModule.AddOption("That seems a bit extreme...",
                                            pq => true,
                                            pq =>
                                            {
                                                DialogueModule extremeModule = new DialogueModule("Extreme? EXTREME?! You must not understand the weight! The unbearable pressure! They say being emperor is glorious, but no one tells you about the paranoia, the dread, the sleepless nights haunted by the shadows of treachery! Glory? Bah! It's a curse!");
                                                extremeModule.AddOption("I understand, it must be difficult.",
                                                    plw => true,
                                                    plw =>
                                                    {
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, extremeModule));
                                            });
                                        traitorModule.AddOption("Burn it all, if it brings you peace.",
                                            pe => true,
                                            pe =>
                                            {
                                                DialogueModule burnModule = new DialogueModule("Ah, sweet words! The thought is tempting. To watch it all crumble, to rid myself of the endless burden! But alas, I cannot. My duty chains me to this cursed throne, and I must see Byzantium endure—even if it means my own sanity shatters into a thousand pieces.");
                                                burnModule.AddOption("You are stronger than you know.",
                                                    plr => true,
                                                    plr =>
                                                    {
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, burnModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, traitorModule));
                                    });
                                plotsModule.AddOption("Tell me about your cousin.",
                                    plt => true,
                                    plt =>
                                    {
                                        DialogueModule cousinModule = new DialogueModule("My cousin! Oh, he was a charming devil, always smiling, always laughing. But behind those eyes—oh, such ambition! He poisoned my advisors, turned my guards against me, and even bribed the cook! The cook! The one who makes my food! Imagine the audacity! I had him banished, but some nights I still see his shadow lurking in my dreams.");
                                        cousinModule.AddOption("You must be constantly vigilant.",
                                            py => true,
                                            py =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, cousinModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, plotsModule));
                            });
                        byzantiumModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, byzantiumModule));
                    });
                identityModule.AddOption("Tell me about your challenges.",
                    p => true,
                    p =>
                    {
                        DialogueModule challengeModule = new DialogueModule("Challenges? HA! Every single day is a challenge! The nobles want power, the generals want war, the peasants want bread, and I—well, I want just a moment's peace! Do you know what it's like to balance the weight of an empire on your shoulders? It's like juggling flaming swords while standing on a barrel of gunpowder!");
                        challengeModule.AddOption("That sounds impossible.",
                            pu => true,
                            pu =>
                            {
                                DialogueModule impossibleModule = new DialogueModule("Impossible, yes! Yet here I stand, alive, breathing, and not yet completely mad! Though some days I feel I am on the edge, teetering, ready to fall into the abyss of madness! It is a miracle I haven't lost my mind already, with all the demands, the constant noise, the cries for help, the cries for war!");
                                impossibleModule.AddOption("You must be very resilient.",
                                    pl => true,
                                    pl =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, impossibleModule));
                            });
                        challengeModule.AddOption("What keeps you going?",
                            pi => true,
                            pi =>
                            {
                                DialogueModule motivationModule = new DialogueModule("What keeps me going? Duty, perhaps. Or maybe sheer stubbornness. There is a fire within me, one that refuses to be snuffed out. Byzantium must endure. I cannot let it fall—not to enemies, not to traitors, and not to my own madness. Even if I crumble, Byzantium will stand tall!");
                                motivationModule.AddOption("You are an inspiration.",
                                    pl => true,
                                    pl =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, motivationModule));
                            });
                        player.SendGump(new DialogueGump(player, challengeModule));
                    });
                identityModule.AddOption("I have no further questions.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Tell me about the virtue you follow.",
            player => true,
            player =>
            {
                DialogueModule virtueModule = new DialogueModule("The virtue of Humility teaches us to be humble in victory and gracious in defeat. Do you follow the path of virtue?");
                virtueModule.AddOption("Yes, I follow the path of virtue.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("It is a noble path, traveler. May your deeds reflect the virtues you hold dear.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                virtueModule.AddOption("No, I do not.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("The path of virtue is a treacherous one, filled with trials and tribulations. Yet, it is also the most rewarding. Every choice we make defines our character. Choose wisely, traveler.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("Tell me about the spirits in the catacombs.",
            player => true,
            player =>
            {
                DialogueModule spiritsModule = new DialogueModule("The spirits of the catacombs are the souls of Byzantium's most devout protectors. They watch over the relic, ensuring it does not fall into the wrong hands. Treat them with respect, and they may aid you in your quest.");
                spiritsModule.AddOption("Thank you for the information.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, spiritsModule));
            });

        greeting.AddOption("Goodbye, Basil II.",
            player => true,
            player =>
            {
                player.SendMessage("Basil II nods respectfully as you depart.");
            });

        return greeting;
    }

    public BasilII(Serial serial) : base(serial) { }

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