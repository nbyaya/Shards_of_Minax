using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Mirielle : BaseCreature
    {
        [Constructable]
        public Mirielle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mirielle";
            Body = 0x191; // Human-like body

            // Set basic stats
            SetStr(80);
            SetDex(90);
            SetInt(100);
            SetHits(100);

            // Appearance and gear
            AddItem(new FancyDress() { Hue = 1150, Name = "Gossamer Robe of the Wild" });
            AddItem(new TricorneHat() { Hue = 1150, Name = "Mushroom Cap" });
            AddItem(new Sandals() { Hue = 0 });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Mirielle(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Greetings, dear traveler! I am Mirielle—a wanderer of enchanted woods, a connoisseur of the rarest mushrooms from the treacherous depths of Catastrophe, " +
                "and, beneath this humble guise, a scion of a noble bloodline destined for greatness. My refined heritage, long hidden from prying eyes, " +
                "fuels my ambition to reclaim a throne that is rightfully mine by birth. Tell me, what tale intrigues you today?"
            );
            
            // Option 1: Mushroom Adventures in Catastrophe
            greeting.AddOption("Tell me about your adventures in Catastrophe.", 
                player => true,
                player =>
                {
                    DialogueModule mushroomModule = new DialogueModule(
                        "Ah, Catastrophe—a labyrinth of bioluminescent corridors and whispered legends. I venture into its depths to harvest mushrooms that glow " +
                        "with the essence of forgotten magic. Recently, I encountered a ferocious spore-beast guarding a hidden cavern. Would you like to " +
                        "hear the thrilling details of that expedition, or learn about the mystical properties these fungi possess?"
                    );
                    
                    mushroomModule.AddOption("Describe the perilous expedition in detail.",
                        p => true,
                        p =>
                        {
                            DialogueModule expeditionModule = new DialogueModule(
                                "The expedition was fraught with peril. The dank air was heavy with spores, and each step echoed against stone walls " +
                                "bathed in eerie phosphorescence. As I navigated the winding tunnels, I sensed not only monstrous guardians but also " +
                                "the stirring call of destiny in my veins—a reminder of my noble heritage and the crown I secretly covet."
                            );
                            
                            expeditionModule.AddOption("What do you mean by your 'noble heritage'?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule nobleModule = new DialogueModule(
                                        "I was born of a long-forgotten line of aristocrats—once rulers of vast lands, celebrated for both art and strategy. " +
                                        "Though forced into obscurity by treacherous rivals, the blood of kings still flows in my veins, urging me toward " +
                                        "a destiny of power and grandeur. It is a secret I share with few."
                                    );
                                    
                                    nobleModule.AddOption("Are you saying you plan to claim a throne?",
                                        pl2 => true,
                                        pl2 =>
                                        {
                                            DialogueModule throneModule = new DialogueModule(
                                                "Indeed, though it must remain our little secret. The current liege has usurped what is rightfully mine, and my ambition " +
                                                "burns like a forge within me. I have devised a careful plan—one that uses both charm and ruthless strategy—to reclaim my birthright."
                                            );
                                            
                                            throneModule.AddOption("Tell me more about your plan.",
                                                pl3 => true,
                                                pl3 =>
                                                {
                                                    DialogueModule planModule = new DialogueModule(
                                                        "Every great revolution begins with whispers in the dark. I have been quietly rallying like-minded allies, " +
                                                        "exploiting the weaknesses of our oppressors, and preparing to strike when the moment is ripe. It is a delicate " +
                                                        "dance between elegance and brutality—a true testament to the duality of power."
                                                    );
                                                    pl3.SendGump(new DialogueGump(pl3, planModule));
                                                });
                                            
                                            throneModule.AddOption("That sounds dangerous.",
                                                pl3 => true,
                                                pl3 =>
                                                {
                                                    DialogueModule dangerModule = new DialogueModule(
                                                        "Danger is the very spice of ambition. Without the willingness to risk all, the crown remains a distant dream. " +
                                                        "I am prepared to face both the literal monsters in Catastrophe and the metaphorical ones in the halls of power."
                                                    );
                                                    pl3.SendGump(new DialogueGump(pl3, dangerModule));
                                                });
                                            
                                            pl2.SendGump(new DialogueGump(pl2, throneModule));
                                        });
                                    
                                    nobleModule.AddOption("What sacrifices have you made for this ambition?",
                                        pl2 => true,
                                        pl2 =>
                                        {
                                            DialogueModule sacrificeModule = new DialogueModule(
                                                "Sacrifice is the language of power. I have forfeited comfort, hidden my true identity, and even betrayed those I once called friends " +
                                                "to further my secret designs. Every scar on my soul is a testament to the price of greatness."
                                            );
                                            pl2.SendGump(new DialogueGump(pl2, sacrificeModule));
                                        });
                                    
                                    pl.SendGump(new DialogueGump(pl, nobleModule));
                                });
                            
                            expeditionModule.AddOption("How did you escape the spore-beast?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule escapeModule = new DialogueModule(
                                        "With a mix of agility, cunning, and a touch of that noble arrogance, I evaded its grasp by slipping through a narrow crevice " +
                                        "that only one of refined lineage might dare to traverse. The thrill of the escape only deepened my resolve."
                                    );
                                    pl.SendGump(new DialogueGump(pl, escapeModule));
                                });
                            
                            expeditionModule.AddOption("Your words paint a vivid picture—what power do these mushrooms hold?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule magicModule = new DialogueModule(
                                        "These fungi are no ordinary ingredients; they pulse with a mystical energy that can awaken hidden potentials and stir " +
                                        "long-forgotten memories of grandeur. In the right concoction, they can transform a meal into a sensory experience fit for royalty."
                                    );
                                    pl.SendGump(new DialogueGump(pl, magicModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, expeditionModule));
                        });
                    
                    mushroomModule.AddOption("Tell me more about the mystical properties of the fungi.",
                        p => true,
                        p =>
                        {
                            DialogueModule secretsModule = new DialogueModule(
                                "The fungi of Catastrophe are imbued with arcane energies, capable of unlocking visions and stirring the soul. " +
                                "Some say that a taste of these mushrooms can offer a glimpse of one's destiny—a crown, a throne, a legacy reawakened."
                            );
                            
                            secretsModule.AddOption("Can these visions guide one to power?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule visionsModule = new DialogueModule(
                                        "Perhaps, if one possesses the heart of an aristocrat and the unwavering ambition to seize what is rightfully theirs. " +
                                        "I have seen flashes of a grand crown and a realm ruled by strength and wisdom—visions that fuel my every secret step."
                                    );
                                    pl.SendGump(new DialogueGump(pl, visionsModule));
                                });
                            
                            secretsModule.AddOption("How do you safely harvest these fungi?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule safeModule = new DialogueModule(
                                        "Every venture into Catastrophe is planned with meticulous care. I use enchanted markers—artifacts of my hidden lineage—to " +
                                        "chart my course and ward off lurking dangers. Such precision is the mark of one born to rule."
                                    );
                                    pl.SendGump(new DialogueGump(pl, safeModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, secretsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, mushroomModule));
                });
            
            // Option 2: Unique Recipes and Culinary Collaboration with Kess
            greeting.AddOption("I’d love to hear about your unique recipes and your work with Kess.", 
                player => true,
                player =>
                {
                    DialogueModule recipesModule = new DialogueModule(
                        "My culinary art is a symphony of nature and nobility. One of my prized creations is the 'Moonlit Mushroom Medley,' " +
                        "a dish that blends rare, enchanted fungi with herbs imbued with ancient magic. Kess, my esteemed collaborator in Dawn, " +
                        "brings a discerning palate to our secret kitchen, where every recipe is a step toward reclaiming a legacy."
                    );
                    
                    recipesModule.AddOption("Describe the 'Moonlit Mushroom Medley' in detail.",
                        p => true,
                        p =>
                        {
                            DialogueModule signatureModule = new DialogueModule(
                                "The 'Moonlit Mushroom Medley' is a dish of sublime complexity. A rich, simmering broth infused with wild garlic, " +
                                "a secret spice blend passed down through generations of noble chefs, and mushrooms that seem to capture the very " +
                                "essence of the night sky. Each spoonful is a reminder of the crown I am destined to reclaim."
                            );
                            
                            signatureModule.AddOption("How did you perfect this recipe?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule perfectModule = new DialogueModule(
                                        "Countless secret meetings in shadowed kitchens, whispered exchanges with Kess at the break of dawn, " +
                                        "and relentless experiments have refined this masterpiece. Our dialogue is as much about flavor as it is about " +
                                        "our shared, ambitious destiny."
                                    );
                                    pl.SendGump(new DialogueGump(pl, perfectModule));
                                });
                            
                            signatureModule.AddOption("What makes this dish so unique?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule uniqueModule = new DialogueModule(
                                        "It is the union of raw, untamed nature with the refined touch of noble tradition. The rare mushrooms " +
                                        "imbue the dish with a hint of otherworldly magic, while our secret spices elevate it to a feast befitting a sovereign."
                                    );
                                    pl.SendGump(new DialogueGump(pl, uniqueModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, signatureModule));
                        });
                    
                    recipesModule.AddOption("Tell me about your collaboration with Kess.",
                        p => true,
                        p =>
                        {
                            DialogueModule collabModule = new DialogueModule(
                                "Our alliance began with a simple exchange—a basket of my foraged treasures delivered to Kess’s bakery in Dawn. " +
                                "Her discerning taste recognized the magic within my finds, and soon our fates intertwined. In our clandestine meetings, " +
                                "we share not only recipes but dreams of reclaiming lost glory and overturning a corrupt order."
                            );
                            
                            collabModule.AddOption("How do you work together?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule workModule = new DialogueModule(
                                        "We convene in secret, far from prying eyes, at the break of day. Together we test rare ingredients, exchange " +
                                        "whispers of forbidden recipes, and strategize about the future—a future where our combined talents may one day " +
                                        "restore our rightful heritage."
                                    );
                                    pl.SendGump(new DialogueGump(pl, workModule));
                                });
                            
                            collabModule.AddOption("What do you admire most about Kess?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule admireModule = new DialogueModule(
                                        "Kess has an almost mystical intuition for blending flavors, an artistry that transcends mere cooking. " +
                                        "Her steadfast dedication mirrors my own ambition, and in her I see the fierce spirit necessary to reclaim what " +
                                        "is rightfully ours."
                                    );
                                    pl.SendGump(new DialogueGump(pl, admireModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, collabModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, recipesModule));
                });
            
            // Option 3: Discuss Artistic Inspirations and Secret Conversations with Toma
            greeting.AddOption("What inspires your art and creativity? Tell me about your talks with Toma.", 
                player => true,
                player =>
                {
                    DialogueModule artModule = new DialogueModule(
                        "Art is my sanctuary and my weapon. In the hushed glows of twilight, I share my inner visions with Toma of Fawn—a kindred spirit " +
                        "whose philosophical musings echo my secret ambition. Our conversations traverse the delicate balance between beauty and ruthless " +
                        "power. Would you like to hear of a recent, revealing dialogue or our broader philosophies on art and dominion?"
                    );
                    
                    artModule.AddOption("Share a recent conversation with Toma.",
                        p => true,
                        p =>
                        {
                            DialogueModule talkModule = new DialogueModule(
                                "Just last moon, under a sky lit by silver luminescence, Toma remarked, 'Beauty is transient, but ambition endures.' " +
                                "In that moment, I confessed the burden of my noble lineage and the destiny that burns within me—to reclaim a throne " +
                                "lost to treachery. Our words wove together a tapestry of art, ambition, and a shared longing for justice."
                            );
                            
                            talkModule.AddOption("What did Toma say in response?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule responseModule = new DialogueModule(
                                        "With quiet intensity, Toma replied, 'True power is born of the will to seize it, even in the face of betrayal and despair.' " +
                                        "His words affirmed my resolve and reminded me that our paths, though dangerous, are lit by a fire that cannot be quenched."
                                    );
                                    pl.SendGump(new DialogueGump(pl, responseModule));
                                });
                            
                            talkModule.AddOption("How do these talks shape your art?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule shapeModule = new DialogueModule(
                                        "Every word exchanged with Toma etches itself onto my soul. Our debates inspire canvases painted with both delicate hues and " +
                                        "bold strokes—a vivid portrayal of a future where beauty and ruthlessness coalesce into the power of rightful dominion."
                                    );
                                    pl.SendGump(new DialogueGump(pl, shapeModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, talkModule));
                        });
                    
                    artModule.AddOption("Tell me about your shared philosophies on power and art.",
                        p => true,
                        p =>
                        {
                            DialogueModule philosophyModule = new DialogueModule(
                                "Toma and I believe that art is not merely an expression of the soul, but a crucible for forging empires. " +
                                "In our secret discussions, we explore how aesthetics can disguise the raw force of ambition. The beauty of a sunset " +
                                "masks the ruthless drive to overturn the order of things—a duality that defines my very existence."
                            );
                            
                            philosophyModule.AddOption("How do you reconcile beauty with ruthlessness?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule reconcileModule = new DialogueModule(
                                        "I have learned that beauty and ruthlessness are two sides of the same coin. My art soothes the heart even as " +
                                        "my ambition sharpens my resolve. Every masterpiece is a silent promise that the meek shall one day bow before true power."
                                    );
                                    pl.SendGump(new DialogueGump(pl, reconcileModule));
                                });
                            
                            philosophyModule.AddOption("What role does pride play in your vision?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule prideModule = new DialogueModule(
                                        "Pride is the flame that burns within me—a constant reminder of the noble birth that I refuse to let fade into obscurity. " +
                                        "It is both a burden and a beacon, driving me to reclaim a throne that history denied me."
                                    );
                                    pl.SendGump(new DialogueGump(pl, prideModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, philosophyModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, artModule));
                });
            
            // Option 4: Delve into the secrets of your noble heritage
            greeting.AddOption("Reveal more about your noble heritage.", 
                player => true,
                player =>
                {
                    DialogueModule heritageModule = new DialogueModule(
                        "Ah, you wish to glimpse behind the mask? My heritage is a tapestry of grandeur and betrayal—a lineage once steeped in power " +
                        "and honor, now forced into the shadows. Would you prefer to hear of the ancient legacy of my family, or the treacherous plots " +
                        "that still haunt our name?"
                    );
                    
                    heritageModule.AddOption("Tell me about your ancient legacy.",
                        p => true,
                        p =>
                        {
                            DialogueModule legacyModule = new DialogueModule(
                                "My ancestors were celebrated rulers, patrons of the arts and architects of empires. We ruled with a refined " +
                                "hand and a visionary spirit. Though our reign was cut short by betrayal, the spark of our nobility still burns within me—" +
                                "a secret flame destined to ignite a new era."
                            );
                            
                            legacyModule.AddOption("How did your family lose its power?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule lossModule = new DialogueModule(
                                        "Betrayal, like a slow poison, seeped into our court. A trusted advisor—driven by his own ambition—conspired against us, " +
                                        "ensuring our downfall with ruthless efficiency. It was a bitter lesson that power, once lost, must be reclaimed at all costs."
                                    );
                                    pl.SendGump(new DialogueGump(pl, lossModule));
                                });
                            
                            legacyModule.AddOption("Do you ever regret your past?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule regretModule = new DialogueModule(
                                        "Regret is a luxury for those who have not tasted the true fire of ambition. I embrace my past—its triumphs and betrayals alike—" +
                                        "for they have forged the unyielding resolve I carry today."
                                    );
                                    pl.SendGump(new DialogueGump(pl, regretModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, legacyModule));
                        });
                    
                    heritageModule.AddOption("What treacherous plots surround your name?",
                        p => true,
                        p =>
                        {
                            DialogueModule plotsModule = new DialogueModule(
                                "In the corridors of power, whispers speak of broken alliances and shattered promises. I have navigated a web of deceit, " +
                                "where even the closest of confidants may hide a dagger behind their smiles. Yet, every betrayal only sharpens my resolve " +
                                "to reclaim what is mine."
                            );
                            
                            plotsModule.AddOption("How do you handle such betrayal?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule handleModule = new DialogueModule(
                                        "With cold calculation and an unwavering ruthlessness. I trust no one completely, and every act of betrayal is turned " +
                                        "into a lesson in power. The path to my destiny is paved with the shards of broken trust."
                                    );
                                    pl.SendGump(new DialogueGump(pl, handleModule));
                                });
                            
                            plotsModule.AddOption("What is your ultimate end goal?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule goalModule = new DialogueModule(
                                        "My ultimate goal is audacious yet clear: to reclaim the throne that is rightfully mine by blood and to usher in an era " +
                                        "where true nobility reigns supreme. I envision a realm where the virtues of honor and ruthless ambition are perfectly balanced."
                                    );
                                    pl.SendGump(new DialogueGump(pl, goalModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, plotsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, heritageModule));
                });
            
            // Option to close or restart the conversation
            greeting.AddOption("Thank you for sharing your secrets.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
