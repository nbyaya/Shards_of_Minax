using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Heidi : BaseCreature
    {
        [Constructable]
        public Heidi() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Heidi the Herbalist";
            Body = 0x191; // Human female body

            // Basic stats
            SetStr(80);
            SetDex(90);
            SetInt(100);
            SetHits(120);

            // Appearance: a graceful herbalist with a hidden spark in her eyes
            AddItem(new FancyDress() { Hue = 1370 });
            AddItem(new Sandals() { Hue = 1170 });
            AddItem(new DeerMask()); // A decorative touch hinting at her artistic past

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Heidi(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // The opening text shows her warm but mysterious nature.
            DialogueModule greeting = new DialogueModule("Greetings, dear traveler. I am Heidi, a devoted herbalist and healer of Sosaria. My days are spent gathering rare herbs from hidden glades, learning mystical remedies from Mother Edda, and exchanging healing traditions with Yorn at Devil Guard. Yet, there is a secret flame that burns within me—a passion from a past life that few ever glimpse. What would you like to discuss?");
            
            // Option 1: Discussion on rare herbs and remedies.
            greeting.AddOption("Tell me about your rare herbs and remedies.", 
                player => true, 
                player =>
                {
                    DialogueModule herbsModule = new DialogueModule("Ah, the art of healing through nature is my passion! I forage for rare herbs like Moonleaf, which glows under a full moon, Starflower with its celestial blue petals, and Silverthorn—whose ground form, lovingly called Silverdust, holds a mystic sparkle. Would you like details on one of these, or learn about my delicate method of preparation?");
                    
                    herbsModule.AddOption("What is Moonleaf?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule moonleafModule = new DialogueModule("Moonleaf is otherworldly. It soothes troubled minds and eases restless nights, making it ideal for a warming tea. I harvest it at precisely midnight, when its glow is at its peak.");
                            pl.SendGump(new DialogueGump(pl, moonleafModule));
                        });
                    
                    herbsModule.AddOption("Tell me more about Starflower.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule starflowerModule = new DialogueModule("The Starflower’s petals shimmer like droplets of starlight. Its essence can renew one’s spirit, especially after long hardships. I learned of its virtues from ancient scrolls and long evenings spent in Yew’s enchanted groves.");
                            pl.SendGump(new DialogueGump(pl, starflowerModule));
                        });

                    herbsModule.AddOption("How do you prepare your remedies?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule preparationModule = new DialogueModule("Preparation is a ritual—a blend of science and art. I grind my herbs with delicate care, mix them with pure spring water tapped near Moon Portals, and let the elixir rest under the soft glow of twilight. This slow infusion preserves the natural energies of every ingredient.");
                            pl.SendGump(new DialogueGump(pl, preparationModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, herbsModule));
                });
            
            // Option 2: The bond with Nellie from Yew.
            greeting.AddOption("Tell me about your bond with Nellie from Yew.", 
                player => true, 
                player =>
                {
                    DialogueModule nellieModule = new DialogueModule("Nellie is a dear friend and mentor from Yew. We met in a sunlit clearing among ancient orchards, where the whispers of old trees shared secrets of nature. Our shared wisdom has deepened my understanding of both healing and the ancient lore of the land.");
                    
                    nellieModule.AddOption("How did you meet Nellie?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule meetModule = new DialogueModule("It was a day of unexpected rain—a time when nature sought to cleanse itself. I was sheltering under a sprawling oak when Nellie, with a smile as warm as spring, offered shelter with her words. That meeting blossomed into a lifelong friendship rooted in the lore of ancient groves.");
                            pl.SendGump(new DialogueGump(pl, meetModule));
                        });

                    nellieModule.AddOption("What remedies do you share with her?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule remediesModule = new DialogueModule("Our remedies blend the nurturing essence of orchard fruit with the potent magic of wild herbs. Together, we craft elixirs that mend both body and soul—a true union of nature’s bounty and ancient wisdom.");
                            pl.SendGump(new DialogueGump(pl, remediesModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, nellieModule));
                });
            
            // Option 3: Insights from Mother Edda.
            greeting.AddOption("What mystical insights do you gain from Mother Edda?", 
                player => true, 
                player =>
                {
                    DialogueModule eddaModule = new DialogueModule("Mother Edda, the venerable mystic of Yew, sees beyond the mortal veil. Her prophetic visions speak of nature’s hidden balance and the subtle interplay between healing and magic. I treasure her words as they guide my hands and heart.");
                    
                    eddaModule.AddOption("Share one of her visions with me.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule visionModule = new DialogueModule("In one poignant vision, she described a moment when the Silverthorn stirred beneath a shrouded moon—a sign that the land itself was calling for renewal. 'When the earth sings, even the most weary soul finds its healing song,' she whispered in a voice like wind through pines.");
                            pl.SendGump(new DialogueGump(pl, visionModule));
                        });
                    
                    eddaModule.AddOption("How do her visions affect your work?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule influenceModule = new DialogueModule("Her visions are the compass of my craft. They reveal the precise moments for harvest and warn me when nature’s pulse falters. This ethereal guidance ensures that every remedy is in perfect harmony with the heartbeat of Sosaria.");
                            pl.SendGump(new DialogueGump(pl, influenceModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, eddaModule));
                });
            
            // Option 4: Healing traditions shared with Yorn.
            greeting.AddOption("What role does Yorn of Devil Guard play in your healing practices?", 
                player => true, 
                player =>
                {
                    DialogueModule yornModule = new DialogueModule("Yorn is not merely a healer in the mountains, but a sage of ancient rites. His strength and practical wisdom—honed in the rugged lands of Devil Guard—balance my more delicate approach. Our exchanges have birthed remedies that resonate with both raw power and subtle magic.");
                    
                    yornModule.AddOption("How do you collaborate with him?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule collabModule = new DialogueModule("Our collaboration is a melding of opposites. While I tend to the nurturing energies of the wild herbs, Yorn teaches me the strength found in mountain traditions: the art of bone setting, the secrets of energy flow, and the timeless rituals of restoration. Together, our cures are as robust as they are gentle.");
                            pl.SendGump(new DialogueGump(pl, collabModule));
                        });
                    
                    yornModule.AddOption("What has he taught you about healing?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule taughtModule = new DialogueModule("Yorn once told me, 'Healing is not just about mending wounds, but also about mending hearts.' His lessons remind me that every remedy should nourish both body and spirit. His teachings have made me a better healer—and a more compassionate soul.");
                            pl.SendGump(new DialogueGump(pl, taughtModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, yornModule));
                });
            
            // Option 5: Learning a healing remedy recipe.
            greeting.AddOption("I would like to learn one of your healing remedies.", 
                player => true, 
                player =>
                {
                    DialogueModule remedyModule = new DialogueModule("Certainly, dear traveler. Allow me to share with you the recipe for a restorative tonic—a brew that soothes minor wounds and rekindles a weary spirit. Are you ready to embrace the art of healing?");
                    
                    remedyModule.AddOption("Yes, please share the recipe.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule recipeModule = new DialogueModule("Very well. You will need:\n\n• A handful of Moonleaf, gathered under the full moon’s tender light.\n• A sprig of Starflower, plucked at the break of dawn.\n• A pinch of finely ground Silverdust from the Silverthorn.\n\nMix these in a clay bowl with water drawn from enchanted springs near the Moon Portals. Stir slowly, letting the natural energies awaken, and let the mixture rest until sunset. Drink slowly, and let the healing warmth seep into your bones.");
                            pl.SendGump(new DialogueGump(pl, recipeModule));
                        });
                    
                    remedyModule.AddOption("Maybe another time.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule declineModule = new DialogueModule("As you wish. The path of healing is always open to those with a sincere heart. Remember, each remedy is a quiet whisper from the soul of nature.");
                            pl.SendGump(new DialogueGump(pl, declineModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, remedyModule));
                });
            
            // Option 6: The secret rebellious past.
            greeting.AddOption("I sense a hidden passion in your eyes—were you once a bard of forbidden lands?", 
                player => true, 
                player =>
                {
                    DialogueModule bardModule = new DialogueModule("Ah... You have an eye for truth. There is indeed a secret I rarely reveal. Long ago, before I devoted myself to the healing arts, I was a bard—a fearless, rebellious soul wandering lands forbidden by tyrants. My songs were not mere notes, but calls for revolution, whispered in secret taverns and moonlit clearings. What part of that past intrigues you?");
                    
                    bardModule.AddOption("Tell me about your daring performances.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule performancesModule = new DialogueModule("My performances were legendary and perilous. Under stormy skies, in hidden glades far from prying eyes, I would raise my voice in ballads that stirred the oppressed. I played on a lute carved with secret sigils, each strum a silent act of defiance. Would you like to hear about one unforgettable night?");
                            
                            performancesModule.AddOption("Yes, recount one memorable performance.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule memorableModule = new DialogueModule("In the ruins of an ancient fortress on the edge of forbidden territory, I sang a song of liberation. The crumbling walls bore witness as an assembly of rebels gathered in shadows. Every note became a spark that set hearts ablaze. Amid the echoes of our unity, I recited a verse that still haunts my dreams.");
                                    
                                    memorableModule.AddOption("Yes, please share the verse.", 
                                        pl3 => true, 
                                        pl3 =>
                                        {
                                            DialogueModule verseModule = new DialogueModule("I sang:\n\n'The night is our cloak, the stars our guide,\nBreak these chains, let freedom reside.\nIn shadows, our hearts beat as one,\nUntil the tyrant’s reign is undone.'\n\nEach word was a beacon in the dark.");
                                            pl3.SendGump(new DialogueGump(pl3, verseModule));
                                        });
                                    
                                    memorableModule.AddOption("No, continue with your story.", 
                                        pl3 => true, 
                                        pl3 =>
                                        {
                                            DialogueModule continueModule = new DialogueModule("Then know that that night changed everything—a spark that ignited a quiet storm of rebellion. The memory of that performance still fuels my passion and reminds me why I turned to healing: to mend the wounds inflicted by tyranny.");
                                            pl3.SendGump(new DialogueGump(pl3, continueModule));
                                        });
                                    
                                    pl2.SendGump(new DialogueGump(pl2, memorableModule));
                                });
                            
                            performancesModule.AddOption("What instruments did you play?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule instrumentsModule = new DialogueModule("I played a lute forged by fellow rebels—a masterpiece adorned with symbols of freedom. Its strings resonated with the heartbeat of the wild, and every chord struck was an act of defiance. It was more than an instrument—it was my voice against oppression.");
                                    pl2.SendGump(new DialogueGump(pl2, instrumentsModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, performancesModule));
                        });
                    
                    bardModule.AddOption("What forbidden lands did you traverse?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule landsModule = new DialogueModule("I journeyed to lands shrouded in mystery and barred by those in power. I roamed the dark labyrinth of Blackwood, where trees whispered ancient secrets; crossed the Burning Fields, where rebel fires burned unabated; and trekked through the ethereal mists of the Highlands, where every step was a defiant act against tyranny. Which of these intrigues you?");
                            
                            landsModule.AddOption("Tell me about the Blackwood.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule blackwoodModule = new DialogueModule("The Blackwood is a realm of shadow and lore. Beneath its gnarled boughs, I discovered a sanctuary for the downtrodden. Each rustle of leaves told tales of hidden rebellions and ancient rites—a place where the spirit of the land defied oppression.");
                                    pl2.SendGump(new DialogueGump(pl2, blackwoodModule));
                                });
                            
                            landsModule.AddOption("What were the Burning Fields like?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule burningFieldsModule = new DialogueModule("The Burning Fields were paradoxical—a scorched earth where hope still flickered like a stubborn ember. There, amid the ruin and ash, I witnessed the raw power of rebellion, a testament to the resilience of those yearning for freedom.");
                                    pl2.SendGump(new DialogueGump(pl2, burningFieldsModule));
                                });
                            
                            landsModule.AddOption("Describe the Highlands for me.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule highlandsModule = new DialogueModule("The Highlands, cloaked in perpetual mist, were both beautiful and treacherous. Under their ghostly veil, secret meetings were held and songs of uprising whispered through the fog. I navigated those paths with nothing but my inner fire and the distant echo of freedom.");
                                    pl2.SendGump(new DialogueGump(pl2, highlandsModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, landsModule));
                        });
                    
                    bardModule.AddOption("What sacrifices did you endure as a rebel bard?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule sacrificeModule = new DialogueModule("The road of rebellion demands great sacrifice. I abandoned comfort and safety—facing danger at every turn, losing dear comrades, and bearing scars both seen and unseen. Each loss forged my resolve, and every sacrifice became a verse in the ballad of my defiant past.");
                            
                            sacrificeModule.AddOption("Did you ever regret these sacrifices?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule regretModule = new DialogueModule("Regret? No—I wear my scars as emblems of my fight. They remind me that even in darkness, there is a relentless spark of hope that must be kindled.");
                                    pl2.SendGump(new DialogueGump(pl2, regretModule));
                                });
                            
                            sacrificeModule.AddOption("How do these experiences shape your healing?", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule influenceModule = new DialogueModule("Every remedy I craft now is infused with the strength and passion of my past. I channel that rebellious fire to heal not just the body, but to rejuvenate the spirit of those crushed by oppression.");
                                    pl2.SendGump(new DialogueGump(pl2, influenceModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, sacrificeModule));
                        });
                    
                    bardModule.AddOption("Why did you leave the life of a bard for healing?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule abandonModule = new DialogueModule("One fateful night, after a clandestine performance in a hidden glen that nearly cost me my life, I realized that my true calling was to mend not just wounded bodies, but broken souls. The path of healing beckoned me—a quieter rebellion, one of renewal rather than defiance. Would you like to know more about that night?");
                            
                            abandonModule.AddOption("Yes, please recount that night.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule fatefulModule = new DialogueModule("The night was thick with danger. Beneath a starless sky, I performed for a secret assembly of rebels in a desolate clearing. My song, a bold proclamation of freedom, attracted the ire of those who feared change. As enemy patrols descended, chaos erupted. In that crucible of fire and fear, I vowed to transform my pain into healing—to use my art to mend the deep divisions wrought by tyranny.");
                                    
                                    fatefulModule.AddOption("That sounds incredibly brave.", 
                                        pl3 => true, 
                                        pl3 =>
                                        {
                                            DialogueModule braveModule = new DialogueModule("Bravery is the very essence of rebellion. Even in the face of mortal peril, I chose to stand tall and sing the song of freedom. That night remains a constant reminder of the cost of liberty and the power of a passionate heart.");
                                            pl3.SendGump(new DialogueGump(pl3, braveModule));
                                        });
                                    
                                    fatefulModule.AddOption("What became of your comrades?", 
                                        pl3 => true, 
                                        pl3 =>
                                        {
                                            DialogueModule comradesModule = new DialogueModule("Some of my comrades were lost to the darkness that night, their voices silenced forever. Yet, their memory fuels my every note. I honor them by healing others and ensuring their sacrifice was not in vain.");
                                            pl3.SendGump(new DialogueGump(pl3, comradesModule));
                                        });
                                    
                                    pl2.SendGump(new DialogueGump(pl2, fatefulModule));
                                });
                            
                            abandonModule.AddOption("No, I prefer to focus on your healing now.", 
                                pl2 => true, 
                                pl2 =>
                                {
                                    DialogueModule healModule = new DialogueModule("Then know that every remedy I prepare is a tribute to that fateful choice—a melding of rebellious fire and healing grace. I continue to honor my past through every potion and salve.");
                                    pl2.SendGump(new DialogueGump(pl2, healModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, abandonModule));
                        });
                    
                    bardModule.AddOption("I will keep your secret safe. Thank you for sharing.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule secretModule = new DialogueModule("Your trust honors me, dear traveler. Remember, the flame of rebellion burns quietly within me still—a reminder of days when my voice was a weapon against tyranny. May your own heart remain fearless and passionate.");
                            pl.SendGump(new DialogueGump(pl, secretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, bardModule));
                });
            
            // Option 7: Ending the conversation.
            greeting.AddOption("I must take my leave for now.", 
                player => true, 
                player =>
                {
                    DialogueModule leaveModule = new DialogueModule("Safe travels, dear one. May the healing winds of Sosaria and the quiet strength of rebellion guide your path.");
                    player.SendGump(new DialogueGump(player, leaveModule));
                });
            
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
