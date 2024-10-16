using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BrewmasterBarlin : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BrewmasterBarlin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Brewmaster Barlin";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(35);
        SetInt(125);
        SetHits(75);

        // Appearance
        AddItem(new ShortPants() { Hue = 2126 });
        AddItem(new Surcoat() { Hue = 1446 });
        AddItem(new Sandals() { Hue = 1904 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BrewmasterBarlin(Serial serial) : base(serial)
    {
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
        DialogueModule greeting = new DialogueModule("Welcome to my alchemy shop, traveler! I am Brewmaster Barlin. What brings you here today?");
        
        greeting.AddOption("Tell me about alchemy.",
            player => true,
            player =>
            {
                DialogueModule alchemyModule = new DialogueModule("Alchemy requires patience and knowledge of the ingredients' properties. Do you have an interest in alchemy?");
                alchemyModule.AddOption("Yes, I'm interested!",
                    p => true,
                    p =>
                    {
                        DialogueModule interestModule = new DialogueModule("That's wonderful! Alchemy can reveal the hidden virtues of ingredients and create powerful elixirs.");
                        interestModule.AddOption("Can you teach me more?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule teachModule = new DialogueModule("Many are intrigued by alchemy, but only a few truly dedicate themselves to its study. I sometimes offer apprenticeships to the worthy.");
                                teachModule.AddOption("How can I prove myself worthy?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule worthyModule = new DialogueModule("Determining who is worthy is a challenge. However, if you bring me a rare ingredient, I might consider you for training. The ingredient I seek is the 'Mystic Moonflower'. It grows only under the full moon in the enchanted forest to the north.");
                                        worthyModule.AddOption("I'll bring you the Mystic Moonflower.",
                                            plr => true,
                                            plr =>
                                            {
                                                plr.SendMessage("You set off on a quest to find the Mystic Moonflower.");
                                            });
                                        worthyModule.AddOption("That sounds too difficult for me.",
                                            plr => true,
                                            plr =>
                                            {
                                                plr.SendGump(new DialogueGump(plr, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, worthyModule));
                                    });
                                teachModule.AddOption("Maybe another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, teachModule));
                            });
                        interestModule.AddOption("Tell me about your favorite brews.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule brewsModule = new DialogueModule("Ah, you've touched upon my passion! I have several favorite brews, each with its own story and unique flavor.");
                                brewsModule.AddOption("Tell me about the Heartwarming Mead.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule meadModule = new DialogueModule("The Heartwarming Mead is a special recipe handed down through my family. It's made with honey collected from bees that gather nectar only from moonlit flowers. The result is a sweet, soothing brew that brings warmth to even the coldest nights.");
                                        meadModule.AddOption("What makes it so special?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule specialMeadModule = new DialogueModule("The secret lies in the moonlit flowers. They bloom only during full moons, and their essence is said to contain traces of lunar magic. This gives the mead its distinctive flavor and a hint of mystical properties. It's perfect for cold evenings and for those seeking a gentle sense of peace.");
                                                specialMeadModule.AddOption("I'd love to try it sometime.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Brewmaster Barlin smiles warmly. 'Perhaps one day, if you prove yourself worthy, I'll brew a batch just for you.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                specialMeadModule.AddOption("What other brews do you have?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, brewsModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, specialMeadModule));
                                            });
                                        meadModule.AddOption("Tell me about another brew.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, brewsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, meadModule));
                                    });
                                brewsModule.AddOption("Tell me about the Dragon's Breath Ale.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule aleModule = new DialogueModule("The Dragon's Breath Ale is not for the faint of heart. It's brewed with a dash of firebloom petals, which give it a spicy kick. The petals are harvested from a plant that grows only near dragon lairs, hence the name.");
                                        aleModule.AddOption("How do you get firebloom petals?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule petalsModule = new DialogueModule("Harvesting firebloom petals is a dangerous endeavor. You must approach dragon lairs with utmost caution, as the dragons guard these plants fiercely. The petals have absorbed the heat of their surroundings, giving the ale its fiery characteristic.");
                                                petalsModule.AddOption("Sounds risky but rewarding.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Barlin nods approvingly. 'Indeed, only the brave can taste the true essence of the Dragon's Breath Ale.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                petalsModule.AddOption("Tell me more about your other brews.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, brewsModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, petalsModule));
                                            });
                                        aleModule.AddOption("Tell me about another brew.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, brewsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, aleModule));
                                    });
                                brewsModule.AddOption("Tell me about the Stardust Elixir.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule elixirModule = new DialogueModule("The Stardust Elixir is a rare concoction made with powdered stardust crystals. It's said to enhance one's magical abilities temporarily. The crystals are incredibly rare, found only in the highest mountain peaks where the stars seem to touch the earth.");
                                        elixirModule.AddOption("What are the effects of the elixir?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule effectsModule = new DialogueModule("The elixir enhances one's focus and magical potency, allowing the drinker to cast spells with greater power and efficiency for a short period. However, it comes at a cost—after its effects wear off, the drinker feels a deep exhaustion.");
                                                effectsModule.AddOption("That sounds powerful.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Barlin nods. 'Indeed, it's a double-edged sword. Power always comes with a price.'");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                effectsModule.AddOption("Tell me about your other brews.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, brewsModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, effectsModule));
                                            });
                                        elixirModule.AddOption("Tell me about another brew.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, brewsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, elixirModule));
                                    });
                                brewsModule.AddOption("Maybe another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, brewsModule));
                            });
                        p.SendGump(new DialogueGump(p, interestModule));
                    });
                alchemyModule.AddOption("Not really.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, alchemyModule));
            });

        greeting.AddOption("Do you need any rare ingredients?",
            player => true,
            player =>
            {
                DialogueModule ingredientModule = new DialogueModule("The ingredient I seek is the 'Mystic Moonflower'. It grows only under the full moon in the enchanted forest to the north.");
                ingredientModule.AddOption("I have the Mystic Moonflower.",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        DialogueModule rewardModule = new DialogueModule("Ah, you've found it! As a token of my appreciation, here's something special for you. And if you're ever interested, my doors are open for apprenticeship.");
                        p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow;
                        rewardModule.AddOption("Thank you!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rewardModule));
                    });
                ingredientModule.AddOption("I'll come back later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, ingredientModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Brewmaster Barlin nods as you take your leave.");
            });

        return greeting;
    }

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