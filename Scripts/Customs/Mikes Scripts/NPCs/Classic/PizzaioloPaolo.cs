using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pizzaiolo Paolo")]
    public class PizzaioloPaolo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PizzaioloPaolo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pizzaiolo Paolo";
            Body = 0x190; // Human male body

            // Stats
            SetStr(88);
            SetDex(50);
            SetInt(62);
            SetHits(68);

            // Appearance
            AddItem(new ShortPants() { Hue = 38 });
            AddItem(new FancyShirt() { Hue = 295 });
            AddItem(new Shoes() { Hue = 38 });
            AddItem(new LeatherGloves() { Name = "Paolo's Pizza Mitts" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
        }

        public PizzaioloPaolo(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Welcome, traveler! I am Pizzaiolo Paolo, the master of pizzas! How may I entice your taste buds today?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player => {
                    DialogueModule jobModule = new DialogueModule("Feast your eyes upon my culinary creations! They are a feast for the senses! Would you like to know about my special recipes?");
                    jobModule.AddOption("Yes, tell me about your special recipes.",
                        pl => true,
                        pl => {
                            DialogueModule specialRecipesModule = new DialogueModule("I have many special recipes! Each one tells a story. For example, my 'Lunar Tomato Delight' is a classic! Would you like to hear about my opinions on pizza toppings?");
                            specialRecipesModule.AddOption("What toppings do you recommend?",
                                p => true,
                                p => {
                                    DialogueModule toppingsModule = new DialogueModule("Ah, toppings! The essence of any pizza! The right combination can elevate a pizza to legendary status! What do you want to know about them?");
                                    toppingsModule.AddOption("How about pineapple?",
                                        plq => true,
                                        plq => {
                                            DialogueModule pineappleModule = new DialogueModule("Pineapple! Ah, a topic of great debate! Some adore it for its sweet juiciness, while others believe it has no place on a pizza. I personally enjoy the contrast it brings to the savory flavors. What do you think?");
                                            pineappleModule.AddOption("I love pineapple on pizza!",
                                                pw => true,
                                                pw => {
                                                    pineappleModule = new DialogueModule("Wonderful! Pineapple adds a tropical twist! It pairs exceptionally well with ham and a touch of jalapeño for a sweet and spicy combination. Have you tried it that way?");
                                                    pineappleModule.AddOption("Not yet! I must try that combination.",
                                                        ple => true,
                                                        ple => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    pineappleModule.AddOption("I still think it ruins pizza.",
                                                        plr => true,
                                                        plr => {
                                                            pineappleModule = new DialogueModule("Ah, a purist! And I respect that! A classic Margherita with fresh basil, mozzarella, and a drizzle of olive oil is hard to beat. Tell me, what toppings do you enjoy?");
                                                            pineappleModule.AddOption("I like pepperoni.",
                                                                pt => true,
                                                                pt => {
                                                                    pineappleModule = new DialogueModule("Ah, pepperoni! A timeless classic! The spicy, smoky flavor complements the cheese beautifully. Would you like to hear how I prepare my pepperoni pizza?");
                                                                    pineappleModule.AddOption("Yes, please!",
                                                                        ply => true,
                                                                        ply => {
                                                                            pl.SendGump(new DialogueGump(pl, CreatePepperoniModule()));
                                                                        });
                                                                    pineappleModule.AddOption("Maybe another time.",
                                                                        plu => true,
                                                                        plu => {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    player.SendGump(new DialogueGump(player, pineappleModule));
                                                                });
                                                            pineappleModule.AddOption("What about mushrooms?",
                                                                pi => true,
                                                                pi => {
                                                                    pineappleModule = new DialogueModule("Mushrooms! A delightful choice! They add earthiness and depth to a pizza. I often use a mix of shiitake and button mushrooms for variety. Do you have a favorite mushroom?");
                                                                    pineappleModule.AddOption("I like portobello mushrooms.",
                                                                        plo => true,
                                                                        plo => {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    pineappleModule.AddOption("Just regular mushrooms for me.",
                                                                        plp => true,
                                                                        plp => {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    player.SendGump(new DialogueGump(player, pineappleModule));
                                                                });
                                                            player.SendGump(new DialogueGump(player, pineappleModule));
                                                        });
                                                    player.SendGump(new DialogueGump(player, pineappleModule));
                                                });
                                            pineappleModule.AddOption("I think pineapple is okay in desserts, not on pizza.",
                                                pa => true,
                                                pa => {
                                                    pineappleModule = new DialogueModule("Ah, a dessert enthusiast! I can understand that. A pineapple tart is delightful! Yet, the unexpected pairing of sweet and savory can surprise you. Perhaps give it a chance on a pizza one day?");
                                                    pineappleModule.AddOption("Maybe I'll try it someday.",
                                                        pls => true,
                                                        pls => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    pineappleModule.AddOption("I doubt it.",
                                                        pld => true,
                                                        pld => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    player.SendGump(new DialogueGump(player, pineappleModule));
                                                });
                                            player.SendGump(new DialogueGump(player, pineappleModule));
                                        });
                                    toppingsModule.AddOption("What do you think of olives?",
                                        plf => true,
                                        plf => {
                                            DialogueModule olivesModule = new DialogueModule("Olives! A bold choice! Their briny flavor can be a hit or miss. I love using kalamata olives for their richness. What’s your opinion?");
                                            olivesModule.AddOption("I adore olives!",
                                                pg => true,
                                                pg => {
                                                    olivesModule = new DialogueModule("Fantastic! Olives add a nice punch! Pair them with feta cheese for a Mediterranean twist. Have you tried that?");
                                                    olivesModule.AddOption("Not yet! Sounds delicious!",
                                                        plh => true,
                                                        plh => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    olivesModule.AddOption("I prefer plain cheese pizza.",
                                                        plj => true,
                                                        plj => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    player.SendGump(new DialogueGump(player, olivesModule));
                                                });
                                            olivesModule.AddOption("I can’t stand olives.",
                                                pk => true,
                                                pk => {
                                                olivesModule = new DialogueModule("Understandable! They’re certainly an acquired taste. What about other toppings? Do you enjoy vegetables?");
                                                    olivesModule.AddOption("I like bell peppers.",
                                                        pll => true,
                                                        pll => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    olivesModule.AddOption("I prefer meat toppings.",
                                                        plz => true,
                                                        plz => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    player.SendGump(new DialogueGump(player, olivesModule));
                                                });
                                            player.SendGump(new DialogueGump(player, olivesModule));
                                        });
                                    toppingsModule.AddOption("What about anchovies?",
                                        plx => true,
                                        plx => {
                                            DialogueModule anchoviesModule = new DialogueModule("Anchovies! A classic yet polarizing topping! Their saltiness can elevate a pizza but is definitely not for everyone. What do you think?");
                                            anchoviesModule.AddOption("I love anchovies!",
                                                pc => true,
                                                pc => {
                                                    anchoviesModule = new DialogueModule("Ah, a bold choice! Anchovies add a savory depth to pizzas. Have you tried them with capers and olives? It's divine!");
                                                    anchoviesModule.AddOption("I haven't tried that, but I will!",
                                                        plv => true,
                                                        plv => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    anchoviesModule.AddOption("I still think they belong in the sea.",
                                                        plb => true,
                                                        plb => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    player.SendGump(new DialogueGump(player, anchoviesModule));
                                                });
                                            anchoviesModule.AddOption("No, never!",
                                                pn => true,
                                                pn => {
                                                    anchoviesModule = new DialogueModule("Fair enough! They’re certainly a strong flavor. Let’s find a topping that suits you better. How about fresh basil?");
                                                    anchoviesModule.AddOption("Basil sounds good!",
                                                        plm => true,
                                                        plm => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    anchoviesModule.AddOption("Not a fan of herbs.",
                                                        plqq => true,
                                                        plqq => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    player.SendGump(new DialogueGump(player, anchoviesModule));
                                                });
                                            player.SendGump(new DialogueGump(player, anchoviesModule));
                                        });
                                    player.SendGump(new DialogueGump(player, toppingsModule));
                                });
                            specialRecipesModule.AddOption("What are your thoughts on cheese?",
                                plww => true,
                                plww => {
                                    DialogueModule cheeseModule = new DialogueModule("Cheese is the heart of every pizza! The gooeyness, the flavor—it binds everything together! What's your favorite type of cheese?");
                                    cheeseModule.AddOption("I love mozzarella!",
                                        p => true,
                                        p => {
                                            cheeseModule = new DialogueModule("Mozzarella! A classic choice! It melts beautifully and has a creamy texture. Have you tried it fresh from the deli?");
                                            cheeseModule.AddOption("Not yet, but I want to!",
                                                plee => true,
                                                plee => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            cheeseModule.AddOption("I prefer shredded mozzarella.",
                                                plrr => true,
                                                plrr => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, cheeseModule));
                                        });
                                    cheeseModule.AddOption("I prefer cheddar.",
                                        p => true,
                                        p => {
                                            cheeseModule = new DialogueModule("Cheddar? A bold choice! It adds a sharpness that can elevate certain pizzas! Have you tried it with barbecue chicken?");
                                            cheeseModule.AddOption("No, but that sounds interesting!",
                                                pltt => true,
                                                pltt => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            cheeseModule.AddOption("I don't think I would try that.",
                                                plyy => true,
                                                plyy => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, cheeseModule));
                                        });
                                    cheeseModule.AddOption("What about feta?",
                                        p => true,
                                        p => {
                                            cheeseModule = new DialogueModule("Feta is a delightful choice! It adds a tangy flavor and pairs beautifully with olives and spinach. Are you a fan of Mediterranean pizzas?");
                                            cheeseModule.AddOption("Absolutely! I love Mediterranean flavors.",
                                                pluu => true,
                                                pluu => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            cheeseModule.AddOption("Not really, I prefer classic styles.",
                                                plii => true,
                                                plii => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, cheeseModule));
                                        });
                                    player.SendGump(new DialogueGump(player, cheeseModule));
                                });
                            player.SendGump(new DialogueGump(player, specialRecipesModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Do you sell pizzas?",
                player => true,
                player => {
                    DialogueModule sellModule = new DialogueModule("I can’t sell you my pizzas, but I can share the recipe! Would you like to hear about it?");
                    sellModule.AddOption("Yes, please!",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    sellModule.AddOption("Maybe later.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, sellModule));
                });

            greeting.AddOption("Tell me about flavors.",
                player => true,
                player => {
                    DialogueModule flavorsModule = new DialogueModule("Flavors are the soul of a pizza. From the spicy tang of a pepperoni to the rich creaminess of mozzarella, each ingredient adds its voice to the symphony.");
                    flavorsModule.AddOption("That sounds delicious.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, flavorsModule));
                });

            return greeting;
        }

        private DialogueModule CreatePepperoniModule()
        {
            DialogueModule pepperoniModule = new DialogueModule("To make a great pepperoni pizza, I use a blend of spices to enhance the pepperoni's flavor. It’s crucial to balance the spices with the cheese. Do you like your pepperoni crispy or chewy?");
            pepperoniModule.AddOption("I prefer it crispy!",
                pl => true,
                pl => {
                    pepperoniModule = new DialogueModule("Crispy pepperoni adds a delightful crunch! It's like a flavor explosion in every bite! Have you tried it with a sprinkle of crushed red pepper?");
                    pepperoniModule.AddOption("Not yet, but I will!",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pepperoniModule.AddOption("That sounds a bit much for me.",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pl.SendGump(new DialogueGump(pl, pepperoniModule));
                });
            pepperoniModule.AddOption("I like it chewy.",
                pl => true,
                pl => {
                    pepperoniModule = new DialogueModule("Chewy pepperoni can be delightful too! It has a softer texture that blends nicely with the melted cheese. How do you feel about extra cheese on your pepperoni pizza?");
                    pepperoniModule.AddOption("Extra cheese is a must!",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pepperoniModule.AddOption("I prefer it with just the standard amount.",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pl.SendGump(new DialogueGump(pl, pepperoniModule));
                });
            return pepperoniModule;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
