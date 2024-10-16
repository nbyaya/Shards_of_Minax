using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class RumTheSnowSculptor : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RumTheSnowSculptor() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rum the Snow Sculptor";
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
        AddItem(new FurCape(1154)); // A blue-white fur cloak
        AddItem(new ThighBoots(2413)); // Dark blue boots
        AddItem(new LongPants(1153)); // Frost-white pants
        AddItem(new FeatheredHat(1152)); // A snow-colored feathered hat
        AddItem(new Lantern()); // Holding a lantern, adding to her mystic appearance

        VirtualArmor = 10;
    }

    public RumTheSnowSculptor(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler in this cold land! I am Rum, a sculptor of ice and snow. Do you share an appreciation for frozen art?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your sculptures.", 
            p => true, 
            p =>
            {
                DialogueModule sculpturesModule = new DialogueModule("The snow here has a life of its own, and with the right tools and skill, I create grand statues, each a tribute to winter’s beauty. Alas, I am always in need of rare materials to continue my work.");
                
                // Nested dialogue options
                sculpturesModule.AddOption("What kind of rare materials do you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule materialsModule = new DialogueModule("Ah, the FrostToken! A rare and mystical artifact imbued with the essence of the cold itself. Without it, my visions of grandeur are but fleeting dreams, and my sculptures remain incomplete.");
                        materialsModule.AddOption("Where can I find a FrostToken?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule findTokenModule = new DialogueModule("The FrostToken is not easy to come by. It lies hidden within the hearts of the frozen beasts that wander the tundra. Many have tried to claim it, but only the worthy shall succeed. Beware, for the path is perilous, and the cold whispers warnings of doom.");
                                findTokenModule.AddOption("I will find it.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Rum's eyes gleam with a strange light. 'May the cold guide your steps, traveler.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                findTokenModule.AddOption("That sounds dangerous. Maybe another time.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Rum sighs, her eyes growing distant. 'The cold waits for no one, but perhaps you will change your mind.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, findTokenModule));
                            });
                        materialsModule.AddOption("I have a FrostToken.", 
                            pla => HasFrostToken(pla), 
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("You have it?! Incredible! You must have faced the biting winds and the growls of the frozen beasts. Now, let us discuss a trade.");
                                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                    plaa => CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeIntroductionModule.AddOption("Maybe another time.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, materialsModule));
                    });

                sculpturesModule.AddOption("Why do you sculpt in the cold?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule whyColdModule = new DialogueModule("Ah, the cold... it speaks to me in ways that the warmth cannot. The chill carries whispers, visions of the future, prophecies yet to be fulfilled. Some call me mad, but I know the truth. In the cold, I see what others cannot - doom, beauty, and everything in between.");
                        whyColdModule.AddOption("What kind of visions?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule visionsModule = new DialogueModule("Visions of the future! A future where the ice engulfs the lands, and only those prepared will survive. I see the shadows of the coming storms, the frost creeping over the hearts of men. But there is hope too, for those who dare to see it. The FrostToken is a key, a glimmer of salvation.");
                                visionsModule.AddOption("You seem... different. Are you alright?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule madnessModule = new DialogueModule("Different? Yes, yes, of course! The cold does things to a mind, makes it sharper, clearer... or perhaps, blurred and confused. Who can say? Madness, they call it, but I call it insight! I see what others cannot - and that, dear traveler, is a blessing and a curse.");
                                        madnessModule.AddOption("I understand. You're a visionary.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rum cackles, her eyes alight with a strange gleam. 'Yes, yes! A visionary! One day, they will all see!'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        madnessModule.AddOption("This is too much for me.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rum's eyes darken, and she nods slowly. 'Perhaps the cold is not for everyone, traveler. Go, before it claims you.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, madnessModule));
                                    });
                                visionsModule.AddOption("How can I prepare for this future?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule preparationModule = new DialogueModule("Prepare? Hah! Few can prepare for what is to come. Gather warmth, gather strength, gather allies. The FrostToken is but one piece of the puzzle. There will be trials, battles, and sacrifices. But perhaps... perhaps you are the one who can make it.");
                                        preparationModule.AddOption("I will try.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rum nods, her eyes softening. 'Good. Trying is all one can do in the face of such inevitabilities.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        preparationModule.AddOption("I don't think I'm ready.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rum sighs deeply, her voice barely a whisper. 'Few are. But readiness is often thrust upon us, whether we want it or not.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, preparationModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, visionsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, whyColdModule));
                    });

                p.SendGump(new DialogueGump(p, sculpturesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Rum nods warmly despite the cold.");
            });

        return greeting;
    }

    private bool HasFrostToken(PlayerMobile player)
    {
        // Check the player's inventory for FrostToken
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FrostToken)) != null;
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
        // Remove the FrostToken and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item frostToken = player.Backpack.FindItemByType(typeof(FrostToken));
        if (frostToken != null)
        {
            frostToken.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for SoftTowel and WeddingDayCake
            rewardChoiceModule.AddOption("SoftTowel", pl => true, pl => 
            {
                pl.AddToBackpack(new SoftTowel());
                pl.SendMessage("You receive a SoftTowel!");
            });
            
            rewardChoiceModule.AddOption("WeddingDayCake", pl => true, pl =>
            {
                pl.AddToBackpack(new WeddingDayCake());
                pl.SendMessage("You receive a WeddingDayCake!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a FrostToken.");
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