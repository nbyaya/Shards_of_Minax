using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lily the Lovely Cook")]
public class LilyTheLovelyCook : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LilyTheLovelyCook() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lily the Lovely Cook";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(70);
        SetHits(45);

        // Appearance
        AddItem(new LongPants() { Hue = 2128 });
        AddItem(new FancyShirt() { Hue = 2128 });
        AddItem(new Sandals() { Hue = 2128 });
        AddItem(new HalfApron() { Hue = 38 });
        AddItem(new SkullCap() { Name = "Lily's Cooking Cap" });
        AddItem(new Spoon() { Name = "Lily's Stirring Spoon" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, dear traveler. I am Lily the Lovely Cook. How can I assist you today?");
        
        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I am the cook of this fine establishment, serving up delectable dishes.");
                jobModule.AddOption("What is your favorite dish?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule favoriteDishModule = new DialogueModule("My favorite dish is the hearty stew, made with fresh herbs and tender meat from the northern valleys.");
                        favoriteDishModule.AddOption("What ingredients do you use?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule ingredientsModule = new DialogueModule("I use a variety of ingredients: carrots, potatoes, onions, and of course, a secret blend of herbs.");
                                ingredientsModule.AddOption("What herbs are in the blend?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule herbsModule = new DialogueModule("Ah, the herbs! I use rosemary, thyme, and a pinch of sage to bring out the flavor. But the secret is in the timing of when you add them!");
                                        plll.SendGump(new DialogueGump(plll, herbsModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, ingredientsModule));
                            });
                        favoriteDishModule.AddOption("Can you teach me how to make it?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule teachModule = new DialogueModule("Of course! It's all about the right ingredients and patience. Would you like to learn how to gather them or the cooking techniques?");
                                teachModule.AddOption("Teach me about gathering ingredients.",
                                    pllll => true,
                                    pllll =>
                                    {
                                        DialogueModule gatheringModule = new DialogueModule("Gathering ingredients can be quite the adventure! You’ll find fresh vegetables in the Whispering Forest, and the best herbs grow on the hills.");
                                        gatheringModule.AddOption("What about meat?",
                                            plllll => true,
                                            plllll =>
                                            {
                                                DialogueModule meatModule = new DialogueModule("For meat, I often hunt in the northern valleys. Just be cautious of the wildlife! It's both beautiful and dangerous.");
                                                plllll.SendGump(new DialogueGump(plllll, meatModule));
                                            });
                                        pllll.SendGump(new DialogueGump(pllll, gatheringModule));
                                    });
                                teachModule.AddOption("Teach me the cooking techniques.",
                                    plllll => true,
                                    plllll =>
                                    {
                                        DialogueModule cookingTechniquesModule = new DialogueModule("Cooking is an art! It's important to chop the vegetables finely and to simmer the stew slowly. The longer, the better!");
                                        plllll.SendGump(new DialogueGump(plllll, cookingTechniquesModule));
                                    });
                                player.SendGump(new DialogueGump(player, teachModule));
                            });
                        player.SendGump(new DialogueGump(player, favoriteDishModule));
                    });
                greeting.AddOption("What do you think about battles?",
                    playerq => true,
                    playerq =>
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("The true test of character isn't in the kitchen, but in the world beyond. Have you faced your battles?")));
                    });
                greeting.AddOption("What treasures do you know of?",
                    playerw => true,
                    playerw =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            lastRewardTime = DateTime.UtcNow;
                            player.AddToBackpack(new GrandmastersRobe()); // Give the reward
                            player.SendGump(new DialogueGump(player, new DialogueModule("Treasures aren't just gold and jewels. Sometimes, the greatest treasures are recipes passed down through generations. Here's a little reward from my collection.")));
                        }
                    });
                greeting.AddOption("Tell me about your recipes.",
                    playere => true,
                    playere =>
                    {
                        DialogueModule recipesModule = new DialogueModule("Each ingredient has its own tale, and every dish tells a story of its own. What would you like to know about my recipes?");
                        recipesModule.AddOption("Do you have any secret recipes?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule secretRecipesModule = new DialogueModule("Ah, secret recipes! I can only share my grandmother's secret pie recipe. It's a treasure in our family!");
                                secretRecipesModule.AddOption("What is the pie made of?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule pieDetailsModule = new DialogueModule("It's a berry pie made with the freshest berries from the forest. The crust is buttery and flaky, a perfect balance!");
                                        pll.SendGump(new DialogueGump(pll, pieDetailsModule));
                                    });
                                player.SendGump(new DialogueGump(player, secretRecipesModule));
                            });
                        recipesModule.AddOption("Can you give me a recipe?",
                            pl => true,
                            pl =>
                            {
                                player.AddToBackpack(new GrandmastersRobe()); // Give a sample recipe item
                                player.SendGump(new DialogueGump(player, new DialogueModule("Certainly! Here’s a simple recipe for vegetable soup. Enjoy your cooking!")));
                            });
                        player.SendGump(new DialogueGump(player, recipesModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
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

    public LilyTheLovelyCook(Serial serial) : base(serial) { }
}
