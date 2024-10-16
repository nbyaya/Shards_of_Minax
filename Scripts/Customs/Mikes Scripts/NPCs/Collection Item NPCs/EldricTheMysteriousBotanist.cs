using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EldricTheMysteriousBotanist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EldricTheMysteriousBotanist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Eldric the Mysterious Botanist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(60);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new Robe(1157)); // Dark green robe
        AddItem(new Sandals(1175)); // Deep brown sandals
        AddItem(new WizardsHat(1366)); // A dark leafy-colored hat
        AddItem(new HalfApron(2213)); // A faded yellow apron

        VirtualArmor = 15;
    }

    public EldricTheMysteriousBotanist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a curious soul! I am Eldric, a botanist fascinated by rare flora and forgotten magic. Do you wish to hear more about my discoveries?");

        // Dialogue options
        greeting.AddOption("Tell me about your discoveries.",
            p => true,
            p =>
            {
                DialogueModule discoveriesModule = new DialogueModule("The wilds are filled with wondrous plants, each with its secrets. However, I am in search of something very peculiar. Do you perhaps have a BabyLavos item?");

                discoveriesModule.AddOption("What kind of secrets do these plants hold?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule plantSecretsModule = new DialogueModule("Ah, the secrets are many! Some plants can heal, while others can curse. But it is the rarest of them all that intrigues me the most—the plants that defy the very nature of life and death. My pursuit has taken me down dark paths, but such is the price of knowledge.");
                        
                        plantSecretsModule.AddOption("Dark paths? What do you mean?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule darkPathsModule = new DialogueModule("I once sought the secret of immortality. It was an obsession, an all-consuming drive. I experimented with alchemical concoctions, rare herbs, and even... forbidden rituals. My failures left me disfigured, but they also opened my eyes to the true cost of tampering with nature.");

                                darkPathsModule.AddOption("Immortality? That sounds dangerous.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule immortalityModule = new DialogueModule("Dangerous indeed. The line between life and death is thin, and those who seek to cross it often find themselves lost in the shadows. I still bear the scars—both physical and spiritual. But the knowledge I gained is invaluable, even if it cost me my humanity.");

                                        immortalityModule.AddOption("Is that why you hide here, among the plants?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule hideModule = new DialogueModule("Yes, the plants do not judge me. They listen, they grow, and they reveal their secrets to those who are willing to listen. In these woods, I find solace, away from the prying eyes of those who would call me a monster.");

                                                hideModule.AddOption("You don't seem like a monster to me.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Eldric smiles faintly, his eyes softening for a brief moment.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });

                                                hideModule.AddOption("I think I'll be on my way.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Eldric nods and turns back to his plants, muttering softly.");
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, hideModule));
                                            });

                                        immortalityModule.AddOption("I think I'll take my leave.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Eldric nods knowingly and returns to his studies.");
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, immortalityModule));
                                    });

                                darkPathsModule.AddOption("That sounds too dangerous for me.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Wise choice. Some secrets are better left forgotten.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });

                                pla.SendGump(new DialogueGump(pla, darkPathsModule));
                            });

                        plantSecretsModule.AddOption("I'd rather not hear about dark paths.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Very well, some things are better left unsaid.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, plantSecretsModule));
                    });

                // Trade option after story
                discoveriesModule.AddOption("I have a BabyLavos item.",
                    pl => HasBabyLavos(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                discoveriesModule.AddOption("I don't have a BabyLavos item.",
                    pl => !HasBabyLavos(pl),
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a BabyLavos item, and I shall reward you handsomely.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                discoveriesModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, discoveriesModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Eldric nods knowingly and returns to his studies.");
            });

        return greeting;
    }

    private bool HasBabyLavos(PlayerMobile player)
    {
        // Check the player's inventory for BabyLavos item
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BabyLavos)) != null;
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
        // Remove the BabyLavos item and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item babyLavos = player.Backpack.FindItemByType(typeof(BabyLavos));
        if (babyLavos != null)
        {
            babyLavos.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for CowToken and WallFlowers
            rewardChoiceModule.AddOption("CowToken", pl => true, pl =>
            {
                pl.AddToBackpack(new CowToken());
                pl.SendMessage("You receive a CowToken!");
            });

            rewardChoiceModule.AddOption("WallFlowers", pl => true, pl =>
            {
                pl.AddToBackpack(new WallFlowers());
                pl.SendMessage("You receive WallFlowers!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a BabyLavos item.");
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