using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class BrynjaTheSnowSculptor : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public BrynjaTheSnowSculptor() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Brynja the Snow Sculptor";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FurCape(2154)); // A white fur cloak
        AddItem(new Boots(141)); // Dark gray boots
        AddItem(new LongPants(753)); // Snow-white pants
        AddItem(new Bonnet(2552)); // A snow-colored bonnet
        AddItem(new FluxCompound()); // The reward in her invantory (could be stolen)

        VirtualArmor = 10;
    }

    public BrynjaTheSnowSculptor(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler in this cold land! I am Brynja, a sculptor of ice and snow. Do you share an appreciation for frozen art?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your sculptures.", 
            p => true, 
            p =>
            {
                DialogueModule sculpturesModule = new DialogueModule("The snow here has a life of its own, and with the right tools and skill, I create grand statues, each a tribute to winter’s beauty. Alas, I am always in need of materials for my art.");
                
                // Trade option after story
                sculpturesModule.AddOption("Do you need any materials?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a SnowSculpture, I would gladly trade you something special. You may choose between a TinkeringTalisman or a FluxCompound.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SnowSculpture for me?");
                                tradeModule.AddOption("Yes, I have a SnowSculpture.", 
                                    plaa => HasSnowSculpture(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasSnowSculpture(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SnowSculpture.");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, sculpturesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Brynja nods warmly despite the cold.");
            });

        return greeting;
    }

    private bool HasSnowSculpture(PlayerMobile player)
    {
        // Check the player's inventory for SnowSculpture
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SnowSculpture)) != null;
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
		// Remove the SnowSculpture and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
		Item snowSculpture = player.Backpack.FindItemByType(typeof(SnowSculpture));
		if (snowSculpture != null)
		{
			snowSculpture.Delete();
			
			player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

			// Create a module for the reward choice
			DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
			
			// Add options for TinkeringTalisman and FluxCompound
			rewardChoiceModule.AddOption("TinkeringTalisman", pl => true, pl => 
			{
				pl.AddToBackpack(new TinkeringTalisman());
				pl.SendMessage("You receive a TinkeringTalisman!");
			});
			
			rewardChoiceModule.AddOption("FluxCompound", pl => true, pl =>
			{
				pl.AddToBackpack(new FluxCompound());
				pl.SendMessage("You receive a FluxCompound!");
			});

			// Send the gump with the reward choice
			player.SendGump(new DialogueGump(player, rewardChoiceModule));

			LastTradeTime[player] = DateTime.UtcNow;
		}
		else
		{
			player.SendMessage("It seems you no longer have a SnowSculpture.");
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
