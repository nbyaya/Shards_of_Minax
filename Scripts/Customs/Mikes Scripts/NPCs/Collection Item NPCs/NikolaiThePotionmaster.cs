using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class NikolaiThePotionmaster : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NikolaiThePotionmaster() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nikolai the Potionmaster";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1161)); // Dark green robe
        AddItem(new Boots(1109)); // Brown boots
        AddItem(new WizardsHat(1150)); // Green wizard hat
        AddItem(new GoldBracelet()); // A gold bracelet as an accessory

        VirtualArmor = 15;
    }

    public NikolaiThePotionmaster(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Nikolai, a master of potions and strange concoctions. Do you dabble in the art of alchemy, by chance?");

        // Dialogue options
        greeting.AddOption("Tell me about your potions.",
            p => true,
            p =>
            {
                DialogueModule potionsModule = new DialogueModule("Potions are the bridge between science and magic. I have spent my life refining recipes that heal, enhance, and sometimes explode. But I am always on the lookout for unique ingredients. You know, I was once a scientist, a leading researcher, if you can believe it. Now, well... things change.");

                // Nested detail about past life as a scientist
                potionsModule.AddOption("You were a scientist?", 
                    pl => true,
                    pl =>
                    {
                        DialogueModule scientistModule = new DialogueModule("Yes, a scientist. I worked in a prestigious laboratory, one that sought to push the boundaries of human knowledge. We dabbled in things that should perhaps never have been touched. The mutations, the changes... I was once obsessed with understanding radiation and the changes it wrought on life. We wanted to reverse it, make things as they once were.");

                        scientistModule.AddOption("What happened?", 
                            pl2 => true, 
                            pl2 =>
                            {
                                DialogueModule whatHappenedModule = new DialogueModule("Our hubris got the better of us. We thought we could control the forces we barely understood. One day, an experiment went awry. I managed to escape, but the lab... and my colleagues... were not so fortunate. The incident left me cynical, questioning everything I once believed in. It's why I live here now, away from society, surrounded by the relics of my failed past.");

                                whatHappenedModule.AddOption("That sounds tragic.",
                                    pl3 => true,
                                    pl3 =>
                                    {
                                        DialogueModule tragicModule = new DialogueModule("Tragic, yes, but enlightening. It taught me that nature cannot be so easily controlled. Potions, on the other hand, are small, manageable. They are an attempt to distill the chaos of nature into something we can use, something we can understand.");

                                        tragicModule.AddOption("So you still pursue your research?",
                                            pl4 => true,
                                            pl4 =>
                                            {
                                                DialogueModule researchModule = new DialogueModule("In a way, yes. My potions are a continuation of my work, but on a smaller, more personal scale. I no longer seek to reverse the mutations; now, I seek to understand them. Every potion, every concoction is a step towards understanding the chaos that we once unleashed.");

                                                researchModule.AddOption("Do you think the damage can be reversed?",
                                                    pl5 => true,
                                                    pl5 =>
                                                    {
                                                        DialogueModule reverseDamageModule = new DialogueModule("I don't know. Sometimes I think it's possible, and other times I feel that trying to undo the past is a fool's errand. Perhaps we must adapt instead of trying to return to what was. Evolution, after all, is about change. But enough about that—do you have a WeaponBottle for me? I may have something that will aid you in your travels.");

                                                        // Trade option
                                                        reverseDamageModule.AddOption("Do you need any ingredients?",
                                                            pla => true,
                                                            pla =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("Yes indeed! If you happen to have a WeaponBottle, I will offer you something in return. You may choose between an OldEmbroideryTool or DistilledEssence.");
                                                                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                                                    pla2 => CanTradeWithPlayer(pla2),
                                                                    pla2 =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a WeaponBottle for me?");
                                                                        tradeModule.AddOption("Yes, I have a WeaponBottle.",
                                                                            plaa => HasWeaponBottle(plaa) && CanTradeWithPlayer(plaa),
                                                                            plaa =>
                                                                            {
                                                                                CompleteTrade(plaa);
                                                                            });
                                                                        tradeModule.AddOption("No, I don't have one right now.",
                                                                            plaa => !HasWeaponBottle(plaa),
                                                                            plaa =>
                                                                            {
                                                                                plaa.SendMessage("Come back when you have a WeaponBottle.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                                                            plaa => !CanTradeWithPlayer(plaa),
                                                                            plaa =>
                                                                            {
                                                                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        pla2.SendGump(new DialogueGump(pla2, tradeModule));
                                                                    });
                                                                tradeIntroductionModule.AddOption("Maybe another time.",
                                                                    pla2 => true,
                                                                    pla2 =>
                                                                    {
                                                                        pla2.SendGump(new DialogueGump(pla2, CreateGreetingModule(pla2)));
                                                                    });
                                                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                                                            });

                                                        pl5.SendGump(new DialogueGump(pl5, reverseDamageModule));
                                                    });

                                                pl4.SendGump(new DialogueGump(pl4, researchModule));
                                            });

                                        pl3.SendGump(new DialogueGump(pl3, tragicModule));
                                    });

                                pl2.SendGump(new DialogueGump(pl2, whatHappenedModule));
                            });

                        pl.SendGump(new DialogueGump(pl, scientistModule));
                    });

                p.SendGump(new DialogueGump(p, potionsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Nikolai nods knowingly, as if lost in thought.");
            });

        return greeting;
    }

    private bool HasWeaponBottle(PlayerMobile player)
    {
        // Check the player's inventory for WeaponBottle
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WeaponBottle)) != null;
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
        // Remove the WeaponBottle and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item weaponBottle = player.Backpack.FindItemByType(typeof(WeaponBottle));
        if (weaponBottle != null)
        {
            weaponBottle.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for OldEmbroideryTool and DistilledEssence
            rewardChoiceModule.AddOption("OldEmbroideryTool", pl => true, pl =>
            {
                pl.AddToBackpack(new OldEmbroideryTool());
                pl.SendMessage("You receive an OldEmbroideryTool!");
            });

            rewardChoiceModule.AddOption("DistilledEssence", pl => true, pl =>
            {
                pl.AddToBackpack(new DistilledEssence());
                pl.SendMessage("You receive a DistilledEssence!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a WeaponBottle.");
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