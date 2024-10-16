using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grillmaster Gary")]
    public class GrillmasterGary : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrillmasterGary() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grillmaster Gary";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 55;
            Hits = 70;

            // Appearance
            AddItem(new LongPants() { Hue = 58 }); // Clothing item with hue 58
            AddItem(new Tunic() { Hue = 295 });    // Clothing item with hue 295
            AddItem(new Boots() { Hue = 2426 });   // Boots with hue 2426
            AddItem(new LeatherGloves() { Name = "Gary's Grilling Gloves" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public GrillmasterGary(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Welcome to my grill! I am Grillmaster Gary, the master of the flames. I’m thrilled to share my love for grilling exotic monster meats from this wonderful world.");

            greeting.AddOption("Tell me about your love for grilling exotic meats.",
                player => true,
                player =>
                {
                    DialogueModule loveModule = new DialogueModule("Ah, grilling is not just cooking—it's an experience! Each type of monster meat offers unique flavors and textures. It's like a new adventure for my taste buds!");
                    loveModule.AddOption("What exotic meats do you enjoy grilling?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule meatsModule = new DialogueModule("I have quite the collection! From the succulent flesh of Behemoths to the tender steaks of Chimeras, each brings its own flair. Let me tell you about some of my favorites!");
                            meatsModule.AddOption("Tell me about Behemoth meat.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule behemothModule = new DialogueModule("Behemoth meat is rich and flavorful. I love to marinate it in a blend of garlic, herbs, and a hint of chili before slow-grilling it. The result? Tender, juicy ribs that fall off the bone!");
                                    behemothModule.AddOption("How do I catch a Behemoth?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule catchBehemothModule = new DialogueModule("Catching a Behemoth requires patience. They roam the plains, but be cautious; they can be quite aggressive. Using traps or luring them with food can help!");
                                            pl.SendGump(new DialogueGump(pl, catchBehemothModule));
                                        });
                                    behemothModule.AddOption("What other preparations do you suggest?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule behemothPrepModule = new DialogueModule("For a different take, try smoking the meat with applewood. It infuses a sweet flavor that complements the meat beautifully. Pair it with a tangy BBQ sauce for a knockout meal!");
                                            pl.SendGump(new DialogueGump(pl, behemothPrepModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, behemothModule));
                                });
                            meatsModule.AddOption("What about Chimera meat?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule chimeraModule = new DialogueModule("Chimera meat is truly fascinating! It has a gamey taste with rich textures. I love making chimera kebabs, marinated in a mix of yogurt, spices, and herbs before grilling!");
                                    chimeraModule.AddOption("What spices do you use for the marinade?",
                                        ple => true,
                                        ple =>
                                        {
                                            DialogueModule chimeraSpicesModule = new DialogueModule("I typically use cumin, coriander, and a bit of turmeric. The yogurt tenderizes the meat, while the spices create a flavor explosion!");
                                            pl.SendGump(new DialogueGump(pl, chimeraSpicesModule));
                                        });
                                    chimeraModule.AddOption("Do you grill any special side dishes?",
                                        plr => true,
                                        plr =>
                                        {
                                            DialogueModule chimeraSidesModule = new DialogueModule("Absolutely! Grilled vegetables like bell peppers and zucchini pair wonderfully with chimera kebabs. Toss them in olive oil and seasoning before grilling for the perfect side!");
                                            pl.SendGump(new DialogueGump(pl, chimeraSidesModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, chimeraModule));
                                });
                            meatsModule.AddOption("What about Troll meat?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule trollModule = new DialogueModule("Troll meat is hearty and substantial. It has a rich flavor that reminds me of beef. I like to use it in stews, but grilling it can yield excellent results too!");
                                    trollModule.AddOption("What’s your favorite Troll recipe?",
                                        plt => true,
                                        plt =>
                                        {
                                            DialogueModule trollRecipeModule = new DialogueModule("I like to cube it and skewer it with mushrooms and onions. A nice marinade of soy sauce, garlic, and ginger really brings out its flavor when grilled!");
                                            pl.SendGump(new DialogueGump(pl, trollRecipeModule));
                                        });
                                    trollModule.AddOption("Where can I find Trolls?",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule trollLocationModule = new DialogueModule("Trolls usually dwell in caves and dark forests. They can be tricky to find, but they drop great meat when defeated!");
                                            pl.SendGump(new DialogueGump(pl, trollLocationModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, trollModule));
                                });
                            meatsModule.AddOption("What about the meat from a Hydra?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule hydraModule = new DialogueModule("Hydra meat is a delicacy! It’s incredibly tender if cooked correctly. I love grilling it slowly over a low flame and basting it with a citrus glaze!");
                                    hydraModule.AddOption("How do you prepare the glaze?",
                                        plu => true,
                                        plu =>
                                        {
                                            DialogueModule hydraGlazeModule = new DialogueModule("The glaze is simple: mix orange juice, honey, and a hint of chili flakes. It creates a perfect balance of sweet and spicy that enhances the hydra’s natural flavors!");
                                            pl.SendGump(new DialogueGump(pl, hydraGlazeModule));
                                        });
                                    hydraModule.AddOption("What’s the secret to grilling Hydra meat?",
                                        pli => true,
                                        pli =>
                                        {
                                            DialogueModule hydraSecretModule = new DialogueModule("The key is low and slow. If you rush it, the meat becomes tough. A good rule of thumb is to grill it for about 20 minutes per pound, turning occasionally!");
                                            pl.SendGump(new DialogueGump(pl, hydraSecretModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, hydraModule));
                                });
                            meatsModule.AddOption("What do you think about Basilisk meat?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule basiliskModule = new DialogueModule("Basilisk meat has a unique texture, and I find it delightful! It’s best grilled over an open flame, and I often serve it with a spicy dipping sauce.");
                                    basiliskModule.AddOption("What dipping sauce do you recommend?",
                                        plo => true,
                                        plo =>
                                        {
                                            DialogueModule basiliskSauceModule = new DialogueModule("I love a simple mix of soy sauce, wasabi, and sesame oil. It adds an incredible kick to the tender basilisk meat!");
                                            pl.SendGump(new DialogueGump(pl, basiliskSauceModule));
                                        });
                                    basiliskModule.AddOption("How do I catch a Basilisk?",
                                        plp => true,
                                        plp =>
                                        {
                                            DialogueModule basiliskCatchModule = new DialogueModule("Basilisks can be tricky to catch! They have a petrifying gaze, so bring along some protective gear. They often hide in caves or near rocky terrain.");
                                            pl.SendGump(new DialogueGump(pl, basiliskCatchModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, basiliskModule));
                                });
                            meatsModule.AddOption("Tell me more about other exotic meats!",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule exoticMeatsModule = new DialogueModule("Of course! The meat from a Dire Wolf is another favorite of mine. It has a robust flavor and works well in hearty stews or grilled over an open flame!");
                                    exoticMeatsModule.AddOption("How do you prepare Dire Wolf meat?",
                                        plaz => true,
                                        plaz =>
                                        {
                                            DialogueModule direWolfPrepModule = new DialogueModule("I prefer to marinate it in a mix of red wine, rosemary, and garlic. Then, grill it to medium-rare for the best flavor!");
                                            pl.SendGump(new DialogueGump(pl, direWolfPrepModule));
                                        });
                                    exoticMeatsModule.AddOption("What other monster meats should I try?",
                                        pls => true,
                                        pls =>
                                        {
                                            DialogueModule otherMeatsListModule = new DialogueModule("You should try the meat of a Griffin! It's lean and has a gamey taste, perfect for grilling. Or the meat of a Phoenix, which is said to have a spicy kick!");
                                            pl.SendGump(new DialogueGump(pl, otherMeatsListModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, exoticMeatsModule));
                                });
                            player.SendGump(new DialogueGump(player, meatsModule));
                        });
                    player.SendGump(new DialogueGump(player, loveModule));
                });

            greeting.AddOption("What do you think about new monster meats?",
                player => true,
                player =>
                {
                    DialogueModule monsterMeatsModule = new DialogueModule("Oh, the new monster meats in this world are a revelation! Each one brings unique flavors and textures that inspire my creativity at the grill.");
                    monsterMeatsModule.AddOption("Tell me more about the flavors!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule flavorsModule = new DialogueModule("Each meat tells a story through its flavor. For example, Troll meat has a hearty richness, while Basilisk meat offers a delightful chewiness. It's fascinating!");
                            flavorsModule.AddOption("Which meat do you find most challenging to grill?",
                                p => true,
                                p =>
                                {
                                    DialogueModule challengeModule = new DialogueModule("I find grilling Phoenix meat to be the most challenging. Its unique properties require careful attention to avoid overcooking. But when done right, it's exquisite!");
                                    p.SendGump(new DialogueGump(p, challengeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, flavorsModule));
                        });
                    monsterMeatsModule.AddOption("What do you consider the best monster meat?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule bestMeatModule = new DialogueModule("For my taste, Behemoth meat is the best! Its robust flavor and versatility in cooking make it a top choice. I never tire of experimenting with it!");
                            pl.SendGump(new DialogueGump(pl, bestMeatModule));
                        });
                    player.SendGump(new DialogueGump(player, monsterMeatsModule));
                });

            greeting.AddOption("Do you have any BBQ competitions coming up?",
                player => true,
                player =>
                {
                    DialogueModule competitionsModule = new DialogueModule("Yes! I'm preparing for the Grand BBQ Showdown next month! It’s a fierce competition, but I believe my secret weapon—Grilled Hydra Steak—will take me to victory!");
                    competitionsModule.AddOption("How do you prepare Hydra Steak?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule hydraSteakModule = new DialogueModule("Grilling Hydra Steak is all about precision. You need to cook it slowly over low heat to ensure it’s tender and juicy. A touch of citrus marinade works wonders!");
                            hydraSteakModule.AddOption("What’s your strategy for the competition?",
                                p => true,
                                p =>
                                {
                                    DialogueModule strategyModule = new DialogueModule("My strategy is to showcase a variety of meats, each prepared uniquely. I want to surprise the judges with unexpected flavor combinations and presentations!");
                                    p.SendGump(new DialogueGump(p, strategyModule));
                                });
                            hydraSteakModule.AddOption("What do you think your biggest challenge will be?",
                                p => true,
                                p =>
                                {
                                    DialogueModule challengeInCompetitionModule = new DialogueModule("The biggest challenge is always the competition! Other grillmasters have amazing skills, and I must be on my A-game to impress the judges.");
                                    p.SendGump(new DialogueGump(p, challengeInCompetitionModule));
                                });
                            pl.SendGump(new DialogueGump(pl, hydraSteakModule));
                        });
                    competitionsModule.AddOption("I wish you luck in the competition!",
                        p => true,
                        p =>
                        {
                            DialogueModule luckModule = new DialogueModule("Thank you! Your support means a lot. Maybe one day I can teach you the secrets of grilling! Just remember: the grill is an extension of yourself!");
                            p.SendGump(new DialogueGump(p, luckModule));
                        });
                    player.SendGump(new DialogueGump(player, competitionsModule));
                });

            return greeting;
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
