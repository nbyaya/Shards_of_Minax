using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZoggarTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZoggarTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zoggar the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(150);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // A bright green fancy shirt
        AddItem(new Kilt(1265)); // A brown kilt
        AddItem(new Sandals(1153)); // Light blue sandals
        AddItem(new FloppyHat(1175)); // A colorful floppy hat
        AddItem(new Cloak(1109)); // A dark cloak

        VirtualArmor = 15;
    }

    public ZoggarTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Zoggar, collector of the rare and unusual. Tell me, have you perhaps come across a PetRock in your adventures?");

        // Dialogue options
        greeting.AddOption("What is a PetRock?", 
            p => true, 
            p =>
            {
                DialogueModule petRockInfoModule = new DialogueModule("Ah, a PetRock is a peculiar item indeed! A simple stone, but full of charm. I collect them, and I'd be willing to trade something special if you bring me one.");
                petRockInfoModule.AddOption("I think I have one. Can we trade?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Wonderful! If you have a PetRock, I can offer you a choice between MeatHooks or GamerJelly, and I'll throw in a MaxxiaScroll as well. Are you interested?");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a PetRock for me?");
                                tradeModule.AddOption("Yes, I have a PetRock.", 
                                    plaa => HasPetRock(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasPetRock(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a PetRock.");
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

                petRockInfoModule.AddOption("Tell me more about yourself, Zoggar.", 
                    pq => true, 
                    pq =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, my story is a tangled web of knowledge and whispers. Once, I was a librarian, a mere servant to the written word. But I uncovered texts that should never have been touched, forbidden tomes filled with whispers—entities unseen, keepers of lost knowledge. The world is filled with secrets, traveler, and I have dedicated my life to unraveling them.");
                        backstoryModule.AddOption("What kind of entities do you speak of?", 
                            pl => true, 
                            pl =>
                            {
                                DialogueModule entityModule = new DialogueModule("The entities... they are not like you or I. They are ancient voices, echoing from the void, from places beyond comprehension. They whisper secrets to me, secrets of forgotten empires, lost artifacts, and strange magics. Some would call me mad for listening to them, but they do not understand the beauty of true knowledge.");
                                entityModule.AddOption("Aren't you afraid of them?", 
                                    pla => true, 
                                    pla =>
                                    {
                                        DialogueModule fearModule = new DialogueModule("Afraid? Ha! Fear is the first barrier to knowledge. I am meticulous, I record everything they say, every symbol they describe. I trust no one else with my notes—paranoia is a small price to pay for wisdom, don't you think? The symbols are a key, a cipher, and only I can unlock their mysteries.");
                                        fearModule.AddOption("What symbols are these?", 
                                            plaa => true, 
                                            plaa =>
                                            {
                                                DialogueModule symbolsModule = new DialogueModule("They are symbols of power, scrawled in the margins of my books. They guide me, showing me where to look, what to collect. The symbols speak of lost places, of secrets buried beneath the sands, beneath the ice, within the very fabric of this world. I scribble them constantly, as the voices command.");
                                                symbolsModule.AddOption("What do they tell you to collect?", 
                                                    plaaa => true, 
                                                    plaaa =>
                                                    {
                                                        DialogueModule collectionModule = new DialogueModule("Ah, yes. The voices tell me of objects—trivial to some, but priceless to those who know. PetRocks, for instance. They tell me that these stones are not just stones. They are anchors, holding pieces of the forgotten within them. I must collect them, I must keep them safe. The world is not ready for what they contain.");
                                                        collectionModule.AddOption("This all sounds... dangerous.", 
                                                            plaaaa => true, 
                                                            plaaaa =>
                                                            {
                                                                DialogueModule dangerModule = new DialogueModule("Danger is the twin of discovery, traveler. You cannot have one without the other. Those who shy away from danger will never see beyond the veil, never glimpse the true essence of reality. But you... you seem different. Perhaps you understand. Perhaps you see the value in these secrets.");
                                                                dangerModule.AddOption("I think I do understand.", 
                                                                    plaaaaa => true, 
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("Zoggar smiles, his eyes glinting with a manic, yet appreciative light.");
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                                    });
                                                                dangerModule.AddOption("No, I think I should leave.", 
                                                                    plaaaaa => true, 
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendMessage("Zoggar nods, his gaze returning to his scribbled notes, lost once again in his whispers.");
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, dangerModule));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, collectionModule));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, symbolsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, fearModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, entityModule));
                            });
                        p.SendGump(new DialogueGump(p, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, petRockInfoModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zoggar waves cheerfully, his collection of oddities glittering behind him.");
            });

        return greeting;
    }

    private bool HasPetRock(PlayerMobile player)
    {
        // Check the player's inventory for PetRock
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(PetRock)) != null;
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
        // Remove the PetRock and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item petRock = player.Backpack.FindItemByType(typeof(PetRock));
        if (petRock != null)
        {
            petRock.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for MeatHooks and GamerJelly
            rewardChoiceModule.AddOption("MeatHooks", pl => true, pl =>
            {
                pl.AddToBackpack(new MeatHooks());
                pl.SendMessage("You receive MeatHooks!");
            });

            rewardChoiceModule.AddOption("GamerJelly", pl => true, pl =>
            {
                pl.AddToBackpack(new GamerJelly());
                pl.SendMessage("You receive GamerJelly!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a PetRock.");
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