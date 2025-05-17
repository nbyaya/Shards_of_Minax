using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Bryn : BaseCreature
    {
        [Constructable]
        public Bryn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bryn the Baker";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance & Items: A simple baker's outfit with hints of a farmer's practicality.
            AddItem(new Server.Items.HalfApron() { Hue = 1150, Name = "Sturdy Apron" });
            AddItem(new Server.Items.Shirt() { Hue = 950 });
            AddItem(new Server.Items.ShortPants() { Hue = 850 });
            AddItem(new Server.Items.Sandals() { Hue = 750 });
            // Additional items or hints of his past may be added as needed.
        }

        public Bryn(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // The root dialogue greeting with multiple main options.
            DialogueModule greeting = new DialogueModule("Greetings, friend. I’m Bryn—the baker of Dawn and a humble servant of our land. I shape bread as I once tilled the soil, with patience and care. What would you like to discuss?");
            
            // Option 1: Life as a Baker (with additional nested details)
            greeting.AddOption("Tell me about your life as a baker.", 
                player => true, 
                player =>
                {
                    DialogueModule bakerLife = new DialogueModule("My day begins before the break of dawn, when the fields are still cloaked in dew. I mix the grains, water, and sometimes a secret herb blend—reminders of a bygone era—to create bread that nourishes both body and soul.");
                    
                    bakerLife.AddOption("What secret ingredients do you use?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule ingredientsModule = new DialogueModule("I source the finest local wheat and trade insights with Darvin in West Montor, whose harvest reports are as dependable as the sunrise. I also gather herbs from Old Bramble’s orchard in Yew. Every ingredient carries the memory of the land and whispers of home.");
                            
                            ingredientsModule.AddOption("That sounds enchanting.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, ingredientsModule));
                        });
                    
                    bakerLife.AddOption("How do you manage these early mornings?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule morningRoutine = new DialogueModule("I rise with the roosters. Before the ovens roar, I share quiet moments with Silas—a trusted friend who gathers rare ingredients from the wilderness. In these early hours, while the world sleeps, I prepare both my bread and the seeds of hope.");
                            
                            morningRoutine.AddOption("Your dedication is inspiring.", 
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, morningRoutine));
                        });
                    
                    bakerLife.AddOption("Does your baking hide deeper secrets?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule deeperSecrets = new DialogueModule("Ah, you notice. Every loaf is a testament to survival. Beneath these simple ingredients lies a past filled with sorrow and resolve. I once tilled these fields as a humble farmer—a life shattered by tyranny and loss.");
                            
                            deeperSecrets.AddOption("What happened to you?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule pastPain = new DialogueModule("I witnessed injustice firsthand. Those in power took away my loved ones, leaving scars in both my heart and the land. In response, I vowed never to let cruelty win. Thus, I opened my home to those escaping oppression—even now, in secret, I shelter desperate souls who seek refuge from tyranny.");
                                    
                                    pastPain.AddOption("I’m sorry you had to suffer so much.", 
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule acceptanceModule = new DialogueModule("Thank you for your compassion. Though I remain practical and patient, the memory of that day fuels my every action. I tend to wounded hearts as I do my fields—with care, secrecy, and a steadfast resolve to protect the vulnerable.");
                                            
                                            acceptanceModule.AddOption("Your strength is admirable.", 
                                                plr => true,
                                                plr => plr.SendGump(new DialogueGump(plr, CreateGreetingModule())));
                                            
                                            plq.SendGump(new DialogueGump(plq, acceptanceModule));
                                        });
                                    
                                    pastPain.AddOption("How did you come to shelter these people?", 
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule sheltering = new DialogueModule("It began quietly. I noticed a scarred traveler hiding among the barley one chilly night. Over time, more sought my door—each with their own painful past. In the solitude of my barn and secret cellar, I offered warmth, food, and a safe haven. It is not a choice made lightly, but one born of deep empathy and practicality.");
                                            
                                            sheltering.AddOption("Do you fear retribution for this?", 
                                                pls => true,
                                                pls =>
                                                {
                                                    DialogueModule fearModule = new DialogueModule("Always. But fear is tempered by the need to do what is right. I maintain utmost discretion—none outside the trusted circle know of my actions. This quiet rebellion is my way of righting ancient wrongs.");
                                                    
                                                    fearModule.AddOption("I understand. Your resolve is unwavering.", 
                                                        plt => true,
                                                        plt => plt.SendGump(new DialogueGump(plt, CreateGreetingModule())));
                                                    
                                                    pls.SendGump(new DialogueGump(pls, fearModule));
                                                });
                                            
                                            sheltering.AddOption("And what of those you shelter?", 
                                                pls => true,
                                                pls =>
                                                {
                                                    DialogueModule thoseSheltered = new DialogueModule("They are mostly farmers, laborers, and even small families who have suffered the cruelty of tyrants. I see them not as strangers but as kin—the seeds of a better tomorrow. While they live in the shadows, I give them hope and a chance to rebuild.");
                                                    
                                                    thoseSheltered.AddOption("A noble deed indeed.", 
                                                        plt => true,
                                                        plt => plt.SendGump(new DialogueGump(plt, CreateGreetingModule())));
                                                    
                                                    pls.SendGump(new DialogueGump(pls, thoseSheltered));
                                                });
                                            
                                            plq.SendGump(new DialogueGump(plq, sheltering));
                                        });
                                    
                                    pastPain.AddOption("Have you ever thought of revealing your secret?", 
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule revealModule = new DialogueModule("Some nights, under the silent gaze of the moon, I wonder if the world would embrace the truth. Yet the terror of tyranny still looms, and secrecy remains a shield. I choose silence—for the safety of those I protect, and for my own peace.");
                                            
                                            revealModule.AddOption("Silence can be powerful.", 
                                                plt => true,
                                                plt => plt.SendGump(new DialogueGump(plt, CreateGreetingModule())));
                                            
                                            revealModule.AddOption("I respect your need for discretion.", 
                                                plt => true,
                                                plt => plt.SendGump(new DialogueGump(plt, CreateGreetingModule())));
                                            
                                            plq.SendGump(new DialogueGump(plq, revealModule));
                                        });
                                    
                                    plc.SendGump(new DialogueGump(plc, pastPain));
                                });
                            
                            deeperSecrets.AddOption("How do you reconcile your past with your baking?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule reconcileModule = new DialogueModule("Each loaf is an act of healing. Baking is both a craft and a prayer—a way to transform sorrow into sustenance. I use my hands to mold pain into hope, nurturing not just the body but also the soul of our community.");
                                    
                                    reconcileModule.AddOption("Your artistry is inspiring.", 
                                        plr => true,
                                        plr => plr.SendGump(new DialogueGump(plr, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, reconcileModule));
                                });
                            
                            deeperSecrets.AddOption("I’d like to hear more about your past another time.", 
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, deeperSecrets));
                        });
                    
                    bakerLife.AddOption("Back to the beginning.", 
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, bakerLife));
                });
            
            // Option 2: News from the Harvest
            greeting.AddOption("Any news from the harvest?", 
                player => true, 
                player =>
                {
                    DialogueModule harvestNews = new DialogueModule("This season’s harvest is both bountiful and bittersweet. Darvin in West Montor reports that the wheat ripens gloriously—but there are hints of an unusual chill that stirs unease among the fields.");
                    
                    harvestNews.AddOption("What did Darvin say about the crops?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule cropsUpdate = new DialogueModule("Darvin mentioned that while the wheat dances in the sun, a strange mist creeps along the barley. Some say it is nature's omen; I suspect deeper forces at work. But practical minds focus on the bounty and weather alike.");
                            
                            cropsUpdate.AddOption("I hope the harvest holds strong.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, cropsUpdate));
                        });
                    
                    harvestNews.AddOption("Tell me more about Darvin’s insights.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule darvinModule = new DialogueModule("Darvin is as steady as the ancient oaks in his fields. His precise observations and warm counsel remind me of better days when land and people were unbroken by tyranny. His optimism is a quiet rebellion against despair.");
                            
                            darvinModule.AddOption("He seems like a truly wise farmer.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, darvinModule));
                        });
                    
                    harvestNews.AddOption("Back to main menu.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, harvestNews));
                });
            
            // Option 3: Partnerships and Trade
            greeting.AddOption("Tell me about your partners in trade.", 
                player => true, 
                player =>
                {
                    DialogueModule partnersTalk = new DialogueModule("In Dawn, every relationship is a covenant. I work hand-in-hand with Old Bramble of Yew, whose orchard gifts are steeped in lore, and with Silas, whose journeys into the wild yield both nourishment and wisdom.");
                    
                    partnersTalk.AddOption("Who is Old Bramble?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule brambleTalk = new DialogueModule("Old Bramble is the keeper of nature’s memories. His orchard is not merely a source of fruits and herbs—it’s a living chronicle of our land’s past. Every apple and every sprig speaks of seasons survived and histories cherished.");
                            
                            brambleTalk.AddOption("I love the storytelling of nature.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, brambleTalk));
                        });
                    
                    partnersTalk.AddOption("And Silas?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule silasTalk = new DialogueModule("Silas is a wanderer of the wild, a patient soul whose skills in foraging and tracking are unmatched. His contributions go far beyond simple trade—he’s a confidant who understands the unspoken language of nature and survival.");
                            
                            silasTalk.AddOption("He truly is a kindred spirit.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, silasTalk));
                        });
                    
                    partnersTalk.AddOption("Return to main menu.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, partnersTalk));
                });
            
            // Option 4: Secret Bread Recipes with Hints of His Past
            greeting.AddOption("Do you have any secret bread recipes?", 
                player => true, 
                player =>
                {
                    DialogueModule secretRecipes = new DialogueModule("Ah, the 'Golden Harvest Bread'—a recipe as guarded as my own story. It blends freshly milled grain, wild honey, and a handful of herbs from Old Bramble’s grove. Many believe it’s enchanted; few know it carries the flavor of resilience born from sorrow.");
                    
                    secretRecipes.AddOption("What makes this bread so unique?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule specialIngredients = new DialogueModule("Its uniqueness lies in the careful balance—a whisper of sweetness from honey, the earthy tang of the grain, and a secret herb that I discovered when my past was still raw with pain. Every loaf is an act of nurture—a small rebellion against a cruel world.");
                            
                            specialIngredients.AddOption("Such craftsmanship is remarkable.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, specialIngredients));
                        });
                    
                    secretRecipes.AddOption("How do you perfect the baking process?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule bakingProcess = new DialogueModule("The magic is in patience—letting the dough rest under gentle warmth until it transforms, just as I once nurtured wounded souls in secret. Each step, from kneading to baking in our ancient ovens, is a quiet ritual, a merging of the past and hopes for the future.");
                            
                            bakingProcess.AddOption("I’d love to learn that art someday.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            pl.SendGump(new DialogueGump(pl, bakingProcess));
                        });
                    
                    secretRecipes.AddOption("Back to main menu.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, secretRecipes));
                });
            
            // Option 5: Ask About His Secret Past and Inner Self
            greeting.AddOption("There’s something in your eyes… Tell me about your past.", 
                player => true, 
                player =>
                {
                    DialogueModule secretPast = new DialogueModule("You have a keen eye. Few have seen the truth behind these gentle eyes. Long ago, I was a simple farmer whose life was upended by a cruel regime. I lost dear ones to tyranny and the scars of that time have never fully healed.");
                    
                    secretPast.AddOption("How did this tragedy change you?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule tragedyModule = new DialogueModule("It taught me that survival requires more than strength—it requires compassion. I learned that nurturing life, whether through tending to the land or sheltering the desperate, is the truest act of defiance. I have since secretly sheltered escaped prisoners, giving them refuge when hope was near lost.");
                            
                            tragedyModule.AddOption("That is both heartbreaking and heroic.", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule heroismModule = new DialogueModule("I do what I must, with practical care and patient resolve. Every action in my bakery and on my farm is a tribute to those I lost—a silent promise that injustice shall never prevail while I have breath in me.");
                                    
                                    heroismModule.AddOption("I admire your quiet strength.", 
                                        plr => true,
                                        plr => plr.SendGump(new DialogueGump(plr, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, heroismModule));
                                });
                            
                            tragedyModule.AddOption("Do you ever regret this heavy burden?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule regretModule = new DialogueModule("Regret is a luxury I cannot afford. Instead, I embrace the duty that fate has cast upon me. Every life I protect gives me strength—and every loaf of bread I bake is a small beacon against the darkness that once tore my world apart.");
                                    
                                    regretModule.AddOption("Your resolve is inspiring.", 
                                        plr => true,
                                        plr => plr.SendGump(new DialogueGump(plr, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, regretModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, tragedyModule));
                        });
                    
                    secretPast.AddOption("How do you manage to shelter these escaped souls?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule shelterModule = new DialogueModule("Under the cloak of twilight, in the hidden corners of my barn and a secret cellar beneath the bakery, I provide shelter, nourishment, and a listening ear. Every rescued soul is given anonymity, and I work quietly behind the scenes—ever practical, ever patient.");
                            
                            shelterModule.AddOption("That takes incredible courage.", 
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            
                            shelterModule.AddOption("Is there anyone you hold dear among them?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule dearModule = new DialogueModule("They are like a new family—a patchwork of broken lives that I mend with care. I offer them not only shelter but also a chance to heal. In every smile and every small act of kindness, I find a reason to keep moving forward.");
                                    
                                    dearModule.AddOption("Their courage mirrors your own.", 
                                        plr => true,
                                        plr => plr.SendGump(new DialogueGump(plr, CreateGreetingModule())));
                                    
                                    plc.SendGump(new DialogueGump(plc, dearModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, shelterModule));
                        });
                    
                    secretPast.AddOption("Let’s return to lighter topics.", 
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, secretPast));
                });
            
            // Option 6: End Conversation
            greeting.AddOption("Goodbye.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, new DialogueModule("Farewell, and may your days be filled with warm bread, gentle hope, and the promise of a fairer world."))));
            
            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
