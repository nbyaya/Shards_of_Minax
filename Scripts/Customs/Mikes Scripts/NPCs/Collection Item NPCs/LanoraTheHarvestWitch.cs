using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class LanoraTheHarvestWitch : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public LanoraTheHarvestWitch() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lanora the Harvest Witch";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new Robe(1260)); // Dark orange robe
        AddItem(new Sandals(2213)); // Burgundy sandals
        AddItem(new WideBrimHat(1123)); // Witch hat, dark hue
        AddItem(new GnarledStaff()); // A twisted staff, fitting for a witch

        VirtualArmor = 15;
    }

    public LanoraTheHarvestWitch(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler... I am Lanora, keeper of the harvest's secrets. The veil between realms thins, and strange things are afoot. Do you seek a trade of harvest magic?");
        
        // Dialogue options
        greeting.AddOption("What kind of trade?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("I am in need of a special ingredient: FillerPowder. Bring it to me, and I shall grant you a choice between a HorrorPumpkin or a HumanCarvingKit, along with a MaxxiaScroll as a token of my appreciation.");
                
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the FillerPowder for me?");
                        
                        tradeModule.AddOption("Yes, I have FillerPowder.", 
                            plaa => HasFillerPowder(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        
                        tradeModule.AddOption("No, I don't have it right now.", 
                            plaa => !HasFillerPowder(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Return to me when you have the FillerPowder.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        
                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        
                        pla.SendGump(new DialogueGump(pla, tradeModule));
                    });
                
                tradeIntroductionModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Tell me about yourself.", 
            p => true, 
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I am but an old woman, touched by the whispers of time. They call me a witch, but I am merely a listener to the voices of the harvest, of the spirits in the fields. They tell me secrets, traveler, secrets of what was, and what shall be.");
                
                aboutModule.AddOption("What do you mean by secrets?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, the secrets are not mine to give freely. They are cryptic, like a shadow at twilight. I have seen things... the end of days, perhaps. The winds have whispered to me of great calamities, but also of hope, for those who dare listen.");
                        
                        secretsModule.AddOption("Tell me of the end of days.", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule endDaysModule = new DialogueModule("The end comes not with fire, but with silence... when the laughter of children fades, when the harvest yields nothing but dust. I see the shadows grow long, and the sun dim. But there is hope, even in darkness. Will you be one to find that hope, or shall you succumb to the void?");
                                
                                endDaysModule.AddOption("How can I find hope?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule hopeModule = new DialogueModule("Hope lies in understanding, dear traveler. The old ways must be remembered—the bonds between people, the land, and the spirits. When one loses their way, they must look to those connections, for in them lies salvation. Perhaps you might aid others, bring them solace, and in doing so, weave a thread of hope.");
                                        
                                        hopeModule.AddOption("I will try to help others.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Lanora smiles, a weary yet genuine expression. 'Good... good. The world needs more like you.'");
                                            });
                                        
                                        hopeModule.AddOption("I don't think I can help anyone.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Lanora sighs, her eyes distant. 'That is the burden we all carry. Perhaps one day, you will find the strength.'");
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, hopeModule));
                                    });
                                
                                endDaysModule.AddOption("That sounds too grim.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Lanora nods slowly, her gaze fixed on something far away. 'Grim, yes. But sometimes, the darkest tales hold the greatest truths.'");
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, endDaysModule));
                            });
                        
                        secretsModule.AddOption("I don't want to hear more.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Lanora shrugs, her eyes twinkling. 'Not all are ready for the burden of knowledge.'");
                            });
                        
                        pl.SendGump(new DialogueGump(pl, secretsModule));
                    });
                
                aboutModule.AddOption("You speak in riddles.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendMessage("Lanora cackles softly. 'Riddles? Perhaps. Or perhaps the truth is hidden in plain sight, and it is your eyes that cannot see.'");
                    });
                
                p.SendGump(new DialogueGump(p, aboutModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Lanora gives you a mysterious smile as you walk away.");
            });

        return greeting;
    }

    private bool HasFillerPowder(PlayerMobile player)
    {
        // Check the player's inventory for FillerPowder
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FillerPowder)) != null;
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
        // Remove the FillerPowder and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fillerPowder = player.Backpack.FindItemByType(typeof(FillerPowder));
        if (fillerPowder != null)
        {
            fillerPowder.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for HorrorPumpkin and HumanCarvingKit
            rewardChoiceModule.AddOption("HorrorPumpkin", pl => true, pl =>
            {
                pl.AddToBackpack(new HorrorPumpkin());
                pl.SendMessage("You receive a HorrorPumpkin, infused with dark harvest energy!");
            });

            rewardChoiceModule.AddOption("HumanCarvingKit", pl => true, pl =>
            {
                pl.AddToBackpack(new HumanCarvingKit());
                pl.SendMessage("You receive a HumanCarvingKit, a curious tool indeed...");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have FillerPowder.");
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