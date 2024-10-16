using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MiraTheShardSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MiraTheShardSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mira the Shard Seeker";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(180);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows(1109)); // A dark hooded shroud
        AddItem(new Sandals(1175)); // Ethereal blue sandals
        AddItem(new Skirt(2124)); // A swirling silver skirt
        AddItem(new Spellbook() { Name = "Mira's Tome of Forgotten Shards", Hue = 1153 }); // Unique spellbook

        VirtualArmor = 15;
    }

    public MiraTheShardSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Mira, seeker of the elusive ShardCrests. Have you come across one in your adventures?");
        
        // Dialogue options
        greeting.AddOption("Who are you, Mira?", 
            p => true, 
            p =>
            {
                DialogueModule aboutModule = new DialogueModule("I was once a renowned scholar, delving into the mysteries of ancient magic and forgotten realms. But now... now I am cursed. A curse that has shackled my mind with endless visions of an impending apocalypse.");
                aboutModule.AddOption("A curse? What happened to you?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule curseModule = new DialogueModule("It was a shard I found during my studies—a shard that should have stayed buried. It whispered to me, showed me things... things I cannot unsee. I saw the end of everything, the world collapsing, fire and shadows consuming all. Now, I am but a shell of my former self, tormented by these visions.");
                        curseModule.AddOption("Is there a way to break the curse?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule curseBreakModule = new DialogueModule("Perhaps... or perhaps not. The shards are fickle, powerful relics of old, imbued with both wonder and doom. I have sought answers, but every revelation brings only more questions and more agony. It is why I seek the ShardCrests, for they might be the key to either my salvation or our world's doom.");
                                curseBreakModule.AddOption("What do the visions show you?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule visionsModule = new DialogueModule("I see shadows... a darkness that stretches across the lands. A great beast rises from the abyss, with eyes like burning coals, and wings that blot out the sun. People running, their screams echoing through my mind. It is not just a vision of the future—it feels like a warning, a prophecy.");
                                        visionsModule.AddOption("Can we stop this apocalypse?", 
                                            plaab => true, 
                                            plaab =>
                                            {
                                                DialogueModule stopApocalypseModule = new DialogueModule("Perhaps, but I do not know how. The shards have a will of their own. They choose those who wield them, and sometimes, they consume their bearers. I believe that with enough ShardCrests, we might be able to change the course of fate—perhaps find a way to avert the darkness I have seen.");
                                                stopApocalypseModule.AddOption("What can I do to help?", 
                                                    plaabc => true, 
                                                    plaabc =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("If you come across a ShardCrest, bring it to me. I can offer you something in return—something that might help you on your journey. But beware, the power of the shards is not to be taken lightly.");
                                                        helpModule.AddOption("What can you offer in exchange?", 
                                                            plq => true, 
                                                            plq =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("If you have a ShardCrest, I can offer you a choice: either an EvilCandle, glowing with dark energy, or a HotFlamingScarecrow. Additionally, you shall always receive a MaxxiaScroll as a token of my appreciation.");
                                                                tradeIntroductionModule.AddOption("I'd like to make a trade.", 
                                                                    plaw => CanTradeWithPlayer(plaw), 
                                                                    plaw =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a ShardCrest for me?");
                                                                        tradeModule.AddOption("Yes, I have a ShardCrest.", 
                                                                            plaae => HasShardCrest(plaae) && CanTradeWithPlayer(plaae), 
                                                                            plaae =>
                                                                            {
                                                                                CompleteTrade(plaa);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.", 
                                                                            plaar => !HasShardCrest(plaar), 
                                                                            plaar =>
                                                                            {
                                                                                plaa.SendMessage("Come back when you have a ShardCrest.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                            plaat => !CanTradeWithPlayer(plaat), 
                                                                            plaat =>
                                                                            {
                                                                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        pla.SendGump(new DialogueGump(pla, tradeModule));
                                                                    });
                                                                tradeIntroductionModule.AddOption("Maybe another time.", 
                                                                    play => true, 
                                                                    play =>
                                                                    {
                                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                                                    });
                                                                pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                                                            });
                                                        plaabc.SendGump(new DialogueGump(plaabc, helpModule));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, stopApocalypseModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, visionsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, curseBreakModule));
                            });
                        pl.SendGump(new DialogueGump(pl, curseModule));
                    });
                p.SendGump(new DialogueGump(p, aboutModule));
            });

        greeting.AddOption("What do you mean by ShardCrests?", 
            p => true, 
            p =>
            {
                DialogueModule shardExplanationModule = new DialogueModule("ShardCrests are pieces of a shattered world—artifacts left behind from a time when magic was untamed and raw. They are powerful, yes, but also dangerous. They hold secrets, visions, and sometimes... madness.");
                shardExplanationModule.AddOption("Are you not afraid of them?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule fearModule = new DialogueModule("Afraid? Oh, I am terrified of them. But they call to me, like an insistent whisper that never stops. I cannot ignore their pull, even if it leads me further into despair. I must uncover their truth, no matter the cost.");
                        fearModule.AddOption("What if they destroy you?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule destructionModule = new DialogueModule("Then so be it. I would rather be destroyed seeking knowledge than live in ignorance. Besides, if my fate is tied to these shards, perhaps my end will serve as a warning to others, or a beacon to those brave enough to continue my work.");
                                destructionModule.AddOption("You are braver than most.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Mira gives you a sad smile, her eyes haunted by the visions she cannot escape.");
                                    });
                                pla.SendGump(new DialogueGump(pla, destructionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, fearModule));
                    });
                p.SendGump(new DialogueGump(p, shardExplanationModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Mira nods, her eyes glinting with hidden knowledge.");
            });

        return greeting;
    }

    private bool HasShardCrest(PlayerMobile player)
    {
        // Check the player's inventory for ShardCrest
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ShardCrest)) != null;
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
        // Remove the ShardCrest and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item shardCrest = player.Backpack.FindItemByType(typeof(ShardCrest));
        if (shardCrest != null)
        {
            shardCrest.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for EvilCandle and HotFlamingScarecrow
            rewardChoiceModule.AddOption("EvilCandle", pl => true, pl => 
            {
                pl.AddToBackpack(new EvilCandle());
                pl.SendMessage("You receive an EvilCandle, flickering with dark energy!");
            });
            
            rewardChoiceModule.AddOption("HotFlamingScarecrow", pl => true, pl =>
            {
                pl.AddToBackpack(new HotFlamingScarecrow());
                pl.SendMessage("You receive a HotFlamingScarecrow, blazing with fierce flames!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a ShardCrest.");
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