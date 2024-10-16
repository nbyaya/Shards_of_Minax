using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ChefSamantha : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ChefSamantha() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Chef Samantha";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(50);
        SetInt(70);
        SetHits(60);

        // Appearance
        AddItem(new Skirt() { Hue = 443 });
        AddItem(new FancyShirt() { Hue = 490 });
        AddItem(new Boots() { Hue = 30 });
        AddItem(new LeatherGloves() { Name = "Samantha's Oven Mitts" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ChefSamantha(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Welcome, traveler! I am Chef Samantha, the finest cook in these lands. How may I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Cooking is my passion, my livelihood. It's not just a job; it's an art. I take great pride in crafting dishes that bring people joy and nourishment.");
                aboutModule.AddOption("Why do you consider cooking an art?",
                    p => true,
                    p =>
                    {
                        DialogueModule artModule = new DialogueModule("The art of cooking is not just about flavors, it's about creating an experience. Every dish tells a story, and each ingredient adds its voice to the symphony. Have you ever dined in a place that left an unforgettable memory?");
                        artModule.AddOption("Yes, I have.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Chef Samantha smiles warmly, clearly delighted by your response.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        artModule.AddOption("Not yet, but I'd love to.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Chef Samantha nods, \"I hope one day you do. Food has a way of touching the soul.\"");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, artModule));
                    });
                aboutModule.AddOption("That's impressive.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Chef Samantha nods gratefully.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have any special ingredients?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no special ingredients for you right now. Please return later.");
                }
                else
                {
                    DialogueModule ingredientModule = new DialogueModule("Ah, herbs can truly elevate a dish. Here, take this rare herb as a token of appreciation. Remember to always cook with love.");
                    ingredientModule.AddOption("Thank you!",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, ingredientModule));
                }
            });

        greeting.AddOption("Can you teach me about cooking?",
            player => true,
            player =>
            {
                DialogueModule teachModule = new DialogueModule("True virtue lies in the art of cooking. I could teach you a few basics, but to truly master cooking, you must experiment and have passion. What would you like to know?");
                teachModule.AddOption("How do I create a perfect dish?",
                    p => true,
                    p =>
                    {
                        DialogueModule dishModule = new DialogueModule("The perfect dish starts with fresh ingredients, a clear mind, and patience. Never rush a meal, let the flavors blend naturally. Cooking is not just about feeding the body, but also about feeding the soul.");
                        dishModule.AddOption("Thank you for the advice.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, dishModule));
                    });
                teachModule.AddOption("What are the rarest ingredients?",
                    p => true,
                    p =>
                    {
                        DialogueModule rareModule = new DialogueModule("Some of the rarest ingredients include Moonlit Truffles, Phoenix Eggs, and Stardust Nectar. They're challenging to find but can create flavors that transcend the ordinary.");
                        rareModule.AddOption("Sounds intriguing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rareModule));
                    });
                teachModule.AddOption("Can you tell me about enchanted apples?",
                    p => true,
                    p =>
                    {
                        DialogueModule enchantedAppleModule = new DialogueModule("Enchanted apples are a secret ingredient that can bring extraordinary magic to any dish. They're not easy to come by, and their effects are unpredictable yet wonderful.");
                        enchantedAppleModule.AddOption("What kind of dishes can you make with enchanted apples?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule dishesModule = new DialogueModule("Oh, enchanted apples can be used in a variety of dishes. My favorite is the 'Mystic Apple Pie', which has the power to temporarily boost one's physical strength and even bring about feelings of pure joy. However, it must be prepared with utmost care.");
                                dishesModule.AddOption("What happens if it's not prepared carefully?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule carefulPreparationModule = new DialogueModule("If not prepared properly, the magic within the apple could backfire. Instead of enhancing strength, it could make someone as weak as a kitten for an entire day. Cooking with enchanted ingredients requires a steady hand and a calm mind.");
                                        carefulPreparationModule.AddOption("How do you ensure it's prepared properly?",
                                            plla => true,
                                            plla =>
                                            {
                                                DialogueModule preparationModule = new DialogueModule("The key is balance. The apple must be cut precisely into seven slices, and each slice needs to be caramelized slowly over low heat while being stirred clockwise exactly thirteen times. Any deviation could spoil the magic.");
                                                preparationModule.AddOption("That sounds incredibly precise.",
                                                    pllaa => true,
                                                    pllaa =>
                                                    {
                                                        pllaa.SendMessage("Chef Samantha nods. \"Indeed, cooking with magic is an art of precision. But when done correctly, the results are truly marvelous.\"");
                                                        pllaa.SendGump(new DialogueGump(pllaa, CreateGreetingModule()));
                                                    });
                                                plla.SendGump(new DialogueGump(plla, preparationModule));
                                            });
                                        pll.SendGump(new DialogueGump(pll, carefulPreparationModule));
                                    });
                                dishesModule.AddOption("Can you teach me how to make the Mystic Apple Pie?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule pieRecipeModule = new DialogueModule("The recipe for Mystic Apple Pie is not something I can simply give away. It requires dedication and a promise to use it wisely. Are you willing to take an oath to use this recipe only for good?");
                                        pieRecipeModule.AddOption("I swear to use it for good.",
                                            plla => true,
                                            plla =>
                                            {
                                                plla.SendMessage("Chef Samantha smiles, \"Very well. Gather enchanted apples, stardust sugar, and moonlit butter. Mix them in harmony, and always remember: magic must be respected.\"");
                                                plla.SendGump(new DialogueGump(plla, CreateGreetingModule()));
                                            });
                                        pieRecipeModule.AddOption("I'm not sure I can take that responsibility.",
                                            plla => true,
                                            plla =>
                                            {
                                                plla.SendMessage("Chef Samantha nods understandingly. \"I appreciate your honesty. Magic should not be taken lightly.\"");
                                                plla.SendGump(new DialogueGump(plla, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, pieRecipeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, dishesModule));
                            });
                        enchantedAppleModule.AddOption("Where can I find enchanted apples?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule locationModule = new DialogueModule("Enchanted apples are found deep within the Whispering Woods, where the magic of the forest is strongest. They grow only under the light of a full moon, and picking them requires a gentle touch, or they will vanish into thin air.");
                                locationModule.AddOption("That sounds challenging, but I'm willing to try.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendMessage("Chef Samantha smiles. \"Bravery and determination are crucial for a cook who wishes to work with enchanted ingredients. I wish you luck, traveler.\"");
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                locationModule.AddOption("Maybe it's too dangerous for me.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendMessage("Chef Samantha nods. \"There's no shame in knowing your limits. Magic can be unpredictable, and safety should always come first.\"");
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, locationModule));
                            });
                        p.SendGump(new DialogueGump(p, enchantedAppleModule));
                    });
                player.SendGump(new DialogueGump(player, teachModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Chef Samantha waves you off with a warm smile.");
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