using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of June")]
    public class June : BaseCreature
    {
        [Constructable]
        public June() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "June";
            Body = 0x191; // Human body

            // Core stats
            SetStr(90);
            SetDex(80);
            SetInt(100);
            SetHits(100);

            // Appearance customization
            AddItem(new PlainDress() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1150 });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public June(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // The opening message hints at June's varied interests—and even suggests that there is more beneath the surface.
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I’m June, a humble soul of Dawn. My days are filled with studying the uncanny behaviors of animals near ancient ruins, collaborating on innovative farming techniques with Bryn, and learning the secrets of natural remedies from Heidi. Yet, there lies another chapter—a hidden burden from my past. What would you care to discuss?");
            
            // Option 1: Unusual Animal Behaviors
            greeting.AddOption("Tell me about the unusual animal behaviors near the ancient ruins.",
                player => true,
                player =>
                {
                    DialogueModule animalModule = new DialogueModule("The ancient ruins radiate a mysterious energy that affects our local wildlife. I’ve seen foxes moving as if in a ritual, owls calling out in eerie harmonies, and deer pausing in silent reverence. I discussed these anomalies with Mino, who thinks the ruins harbor residual alchemical forces, and with Silas, who whispers of awakened ancient spirits. What intrigues you most?");
                    
                    animalModule.AddOption("What specific behaviors have you observed?",
                        p => true,
                        p =>
                        {
                            DialogueModule behaviorsModule = new DialogueModule("On moonlit nights, I see packs of foxes moving in perfect synchrony, and the hoots of the owls seem to carry secret messages across the valley. The deer, with eyes reflecting sorrow, almost appear to guard the ruined stones. It is as if nature itself remembers an age lost to time.");
                            p.SendGump(new DialogueGump(p, behaviorsModule));
                        });

                    animalModule.AddOption("Tell me more about Mino’s and Silas’s theories.",
                        p => true,
                        p =>
                        {
                            DialogueModule opinionsModule = new DialogueModule("Mino is convinced that chemical residues or ancient alchemical spells from the ruins affect the creatures. In contrast, Silas—a mystic at heart—argues that it's the work of spirits born of the ruins’ tragic past. Their differing insights only add layers to the mystery.");
                            p.SendGump(new DialogueGump(p, opinionsModule));
                        });

                    animalModule.AddOption("Do you have your own theory?",
                        p => true,
                        p =>
                        {
                            DialogueModule theoryModule = new DialogueModule("I believe the phenomenon is a blend of nature’s resilience and the echoes of ancient rites. The animals, in their mysterious ways, might be the living expressions of history trying to reconcile with the present.");
                            p.SendGump(new DialogueGump(p, theoryModule));
                        });

                    player.SendGump(new DialogueGump(player, animalModule));
                });

            // Option 2: Farm Innovations with Bryn
            greeting.AddOption("Tell me about your work with Bryn on farm innovations.",
                player => true,
                player =>
                {
                    DialogueModule farmModule = new DialogueModule("Bryn and I are on a mission: to blend tradition and magic with modern farming. We’re experimenting with enchanted irrigation channels, crop rotations aligned with lunar phases, and bio-fertilizers that seem to awaken the land’s hidden energy. What details interest you?");
                    
                    farmModule.AddOption("What new techniques have you implemented?",
                        p => true,
                        p =>
                        {
                            DialogueModule techniqueModule = new DialogueModule("We’ve designed systems that tap directly into the natural flow of magic in the soil. For example, synchronizing planting with the moon’s cycles has yielded astonishing results. It’s as if the very earth rejoices when treated with respect and ancient wisdom.");
                            p.SendGump(new DialogueGump(p, techniqueModule));
                        });

                    farmModule.AddOption("How do these innovations benefit the community?",
                        p => true,
                        p =>
                        {
                            DialogueModule impactModule = new DialogueModule("Our breakthroughs do more than produce abundant harvests—they foster hope and economic resilience. With surplus crops, trade flourishes between Dawn and neighboring settlements, strengthening bonds and ensuring that everyone prospers.");
                            p.SendGump(new DialogueGump(p, impactModule));
                        });

                    farmModule.AddOption("What challenges have you faced in implementing these techniques?",
                        p => true,
                        p =>
                        {
                            DialogueModule challengeModule = new DialogueModule("Integrating modern ideas with ancient customs is a challenge. The unpredictability of nature, fluctuating weather, and stubborn pests test our mettle daily. Yet every obstacle teaches us valuable lessons, nudging us ever closer to sustainable success.");
                            p.SendGump(new DialogueGump(p, challengeModule));
                        });

                    player.SendGump(new DialogueGump(player, farmModule));
                });

            // Option 3: Natural Remedies with Heidi
            greeting.AddOption("Discuss the natural remedies you've discovered with Heidi.",
                player => true,
                player =>
                {
                    DialogueModule remediesModule = new DialogueModule("Heidi, our gifted herbalist, has shown me that every herb holds a hidden virtue. Together, we’ve crafted potions and salves using ingredients sourced from the wild. Each remedy is steeped in lore and nature’s own magic. What would you like to explore?");
                    
                    remediesModule.AddOption("Describe a recent remedy that succeeded.",
                        p => true,
                        p =>
                        {
                            DialogueModule remedySuccessModule = new DialogueModule("Not long ago, we brewed a salve from the rare Moonleaf flower—plucked under a full moon—and blended it with forest honey and wild lavender. This remedy not only accelerates healing but also soothes the spirit after long days of toil.");
                            p.SendGump(new DialogueGump(p, remedySuccessModule));
                        });

                    remediesModule.AddOption("How does Heidi choose the perfect herbs?",
                        p => true,
                        p =>
                        {
                            DialogueModule selectionModule = new DialogueModule("Heidi’s process is a marvel of intuition and science. She studies the land, listens to the rustling leaves, and even notes the phase of the moon before foraging. Her choices are guided by both ancient recipes and the subtle rhythms of nature.");
                            p.SendGump(new DialogueGump(p, selectionModule));
                        });

                    remediesModule.AddOption("Explain the ritual of creating a remedy.",
                        p => true,
                        p =>
                        {
                            DialogueModule processModule = new DialogueModule("Each remedy begins with foraging only the freshest herbs, followed by careful cleansing and drying in the early morning sun. Then, in a quiet ritual often accompanied by recited incantations taught by Heidi herself, the ingredients are ground, mixed with natural oils, and allowed to meld into a healing salve. The process is both scientific and spiritual.");
                            p.SendGump(new DialogueGump(p, processModule));
                        });

                    player.SendGump(new DialogueGump(player, remediesModule));
                });

            // Option 4: Personal Journey and Life in Dawn – Including the Secret Past
            greeting.AddOption("Tell me about your personal journey and life in Dawn.",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule("My life in Dawn is a mosaic of light and shadow. I grew up amidst the whispering fields and ancient ruins, fascinated by the wonders of nature and magic. Over time, I forged bonds with souls like Bryn, Heidi, Mino, and Silas. Yet, there lies a secret—a weight I bear in silence. Which part of my journey would you like to hear?");
                    
                    personalModule.AddOption("Tell me about your early days.",
                        p => true,
                        p =>
                        {
                            DialogueModule earlyModule = new DialogueModule("In my youth, I wandered vast meadows and sat by babbling streams. The world was a canvas of vibrant colors, and I was captivated by every subtle sound and movement. Those innocent days imbued me with a passion for understanding every secret the land held.");
                            p.SendGump(new DialogueGump(p, earlyModule));
                        });

                    personalModule.AddOption("What are you pursuing now?",
                        p => true,
                        p =>
                        {
                            DialogueModule currentModule = new DialogueModule("Today, I strive to merge the wisdom of old with the innovations of the present. Whether by refining new remedies or testing revolutionary farming techniques, every action is a step toward nurturing our land—and by extension, healing our community’s wounds.");
                            p.SendGump(new DialogueGump(p, currentModule));
                        });

                    // Secret Branch: June's Hidden Noble Past
                    personalModule.AddOption("I sense a secret within you—care to share more about your past?",
                        p => true,
                        p =>
                        {
                            DialogueModule secretModule = new DialogueModule("Your insight is impressive, and indeed, there is a truth I have long concealed. I was born into nobility—a lineage that once wielded immense power. My family, in their quest for control, helped forge the very regime that now subjugates our people. The guilt of this legacy has haunted me for years.");
                            
                            secretModule.AddOption("That is a heavy burden. How do you cope with it?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule guiltModule = new DialogueModule("Every night, I wrestle with guilt for the part my heritage played in inflicting suffering. In quiet acts of defiance, I anonymously fund rebellions against the oppressive forces my family helped unleash. These secret acts of generosity are my way to mend the damage, however small it may seem.");
                                    pl.SendGump(new DialogueGump(pl, guiltModule));
                                });

                            secretModule.AddOption("Why remain so secretive about this part of your past?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule secretiveModule = new DialogueModule("Revealing my true origins would not only endanger the brave souls fighting the regime but would also dismantle the fragile trust I've built with my allies. Secrecy is my armor—a necessity in a world where a noble name can bring ruin. I must protect the rebels and myself at all costs.");
                                    pl.SendGump(new DialogueGump(pl, secretiveModule));
                                });

                            secretModule.AddOption("Your generosity in funding rebellions is admirable. What drives you?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule generousModule = new DialogueModule("My generosity springs from deep remorse and the desire to atone for past sins. Every coin I discreetly offer to the rebels is a silent promise—a promise that even in darkness, the light of justice can be kindled. It is my penance, my hope for redemption.");
                                    pl.SendGump(new DialogueGump(pl, generousModule));
                                });

                            // Further details about the noble lineage and inner conflict
                            secretModule.AddOption("Tell me more about your noble lineage.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule lineageModule = new DialogueModule("I was raised in opulence, amidst gilded halls and strict expectations. Yet, behind the grandeur lay dark ambition and ruthless policies that oppressed many. As I grew, I found myself questioning a legacy steeped in injustice. The moment of clarity came when I realized that true honor meant opposing that legacy, even if it meant turning my back on my own blood.");
                                    
                                    lineageModule.AddOption("How did you decide to oppose your family?",
                                        plc => true,
                                        plc =>
                                        {
                                            DialogueModule decisionModule = new DialogueModule("In a moment of painful realization, I understood that loyalty to one’s conscience must surpass that to one’s lineage. Watching the suffering wrought by our actions, I made a silent vow to redirect the wealth I inherited towards acts of rebellion—funding those who fought for freedom and justice. It was a choice born of desperation and hope.");
                                            plc.SendGump(new DialogueGump(plc, decisionModule));
                                        });
                                    
                                    lineageModule.AddOption("Do you ever regret your decisions?",
                                        plc => true,
                                        plc =>
                                        {
                                            DialogueModule regretModule = new DialogueModule("Regret is a constant companion on my path—a reminder of every life affected by my family's misdeeds. Yet, every act of secret generosity and support for the downtrodden affirms my belief that redemption is a journey, not a destination. I cling to that hope, even when the weight of guilt is almost unbearable.");
                                            plc.SendGump(new DialogueGump(plc, regretModule));
                                        });
                                    
                                    pl.SendGump(new DialogueGump(pl, lineageModule));
                                });

                            p.SendGump(new DialogueGump(p, secretModule));
                        });

                    player.SendGump(new DialogueGump(player, personalModule));
                });

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
