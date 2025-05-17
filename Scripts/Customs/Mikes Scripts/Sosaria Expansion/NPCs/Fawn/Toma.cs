using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the shattered remains of exquisite glass")]
    public class Toma : BaseCreature
    {
        [Constructable]
		public Toma() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Toma, the Artisan of Glass";
            Body = 0x190; // Human male body

            // Set basic stats—with heightened intellect for his scholarly side
            SetStr(100);
            SetDex(80);
            SetInt(110);
            SetHits(100);

            // Appearance & Items – practical attire for an artisan and a nod to his trade
            AddItem(new HalfApron() { Hue = 200 });
            AddItem(new Cap() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            // Custom item representing his glassblower's pipe
            AddItem(new QuarterStaff() { Name = "Glassblower's Pipe" });

            Hue = 1150;
        }

        public Toma(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Toma, the Artisan of Glass. In the searing glow of my forge, I shape molten sand into exquisite art—and, quietly, I pursue a far more secret passion. Beyond the flame and crystal lies a mystery: a lost civilization whose forbidden knowledge I am determined to uncover. My craft is a union of art, science, and hidden history. How might I share my lore with you today?");

            // Option 1: Glassblowing Techniques
            greeting.AddOption("Tell me about your glassblowing techniques.", player => true, player =>
            {
                DialogueModule techniquesModule = new DialogueModule("Ah, glassblowing is an intricate dance of heat, breath, and timing. Under the tutelage of Iris at Devil Guard, I learned to blend pure molten glass with mystical crystals. This fusion creates objects that shimmer with uncanny light. Would you like to explore the crystal infusion process, the mechanics of shaping glass, or both in full detail?");
                
                techniquesModule.AddOption("Explain the crystal infusion process.", p => true, p =>
                {
                    DialogueModule crystalsModule = new DialogueModule("Crystals, to me, are living relics of ancient power. Iris taught me that every crystal carries a unique resonance—almost like a fragment of a lost language. When embedded at just the right moment, their inner glow transforms my glasswork into something transcendent. Yet, some whisper that these crystals may harbor remnants of dark magic...");
                    
                    crystalsModule.AddOption("Do these crystals hide any dangers?", p2 => true, p2 =>
                    {
                        DialogueModule dangerModule = new DialogueModule("An excellent question. While many caution that such crystals may be cursed relics of forbidden rituals, I remain skeptical—though my research compels me to investigate these claims further. The balance between art and peril is delicate, and I often wonder if the magic within them might be a legacy of that lost civilization.");
                        p2.SendGump(new DialogueGump(p2, dangerModule));
                    });
                    
                    p.SendGump(new DialogueGump(p, crystalsModule));
                });
                
                techniquesModule.AddOption("Describe the shaping process.", p => true, p =>
                {
                    DialogueModule processModule = new DialogueModule("The shaping process is as precise as it is passionate. I begin with the finest sand, heating it until it liquefies into a glowing mass. With rhythmic breaths and deft movements honed by years of practice, I mold the glass into intricate forms. Every swirl, every curve—it's a moment captured in time, much like the elusive fragments of history I chase in secret.");
                    p.SendGump(new DialogueGump(p, processModule));
                });
                
                techniquesModule.AddOption("Tell me about both processes together.", p => true, p =>
                {
                    DialogueModule bothModule = new DialogueModule("Certainly. As I blow the glass, I carefully time the introduction of crystals to capture their fleeting energy. This synchronous dance of heat and light creates objects that are not only visually stunning but also echo with the mysteries of ancient lore. It is said that such techniques were once perfected by a long-forgotten civilization—a secret that drives my every inquiry.");
                    
                    bothModule.AddOption("A lost civilization? Tell me more!", p2 => true, p2 =>
                    {
                        DialogueModule ancientModule = new DialogueModule("Yes... legends speak of a civilization that mastered both the arts and arcane forces. In my countless nights poring over crumbling manuscripts and exploring abandoned ruins, I have pieced together clues of their existence. Their methods were rumored to merge elemental fire with dark magic—a practice I pursue with obsessive zeal, even as I ignore dire warnings from cautious scholars.");
                        
                        ancientModule.AddOption("How did you become so obsessed?", p3 => true, p3 =>
                        {
                            DialogueModule obsessionModule = new DialogueModule("My fascination began as a mere curiosity in my youth. While others saw glass as a medium for beauty, I perceived it as a key to unraveling the past. Every shard, every glimmer hints at stories lost to time. Despite warnings and the ominous aura of dark magic, I plunged into research, driven by an unyielding desire to understand and document these secrets.");
                            p3.SendGump(new DialogueGump(p3, obsessionModule));
                        });
                        
                        ancientModule.AddOption("Aren't you afraid of what dark magic might bring?", p3 => true, p3 =>
                        {
                            DialogueModule magicModule = new DialogueModule("Fear is a natural response, yet my scholarly mind demands proof over superstition. I question each legend with a skeptical eye, examining evidence and relics. While dark magic is a subject of dire warnings, I choose to challenge its mysteries—determined to separate myth from verifiable truth, even if it means risking my own well-being.");
                            p3.SendGump(new DialogueGump(p3, magicModule));
                        });
                        
                        p2.SendGump(new DialogueGump(p2, ancientModule));
                    });
                    
                    p.SendGump(new DialogueGump(p, bothModule));
                });
                
                player.SendGump(new DialogueGump(player, techniquesModule));
            });

            // Option 2: Learning from Iris
            greeting.AddOption("How did you come to learn your techniques from Iris?", player => true, player =>
            {
                DialogueModule irisModule = new DialogueModule("My encounter with Iris occurred in the rugged terrains of Devil Guard, where danger and mysticism intertwine. Under her stern guidance, I learned not only the practicalities of crystal infusion but also the subtle lore hidden within each gem. Would you like to know about her teachings, our ongoing collaboration, or the hardships we endured together?");
                
                irisModule.AddOption("What exactly did she teach you?", p => true, p =>
                {
                    DialogueModule teachModule = new DialogueModule("Iris imparted wisdom far beyond mere technique. She taught me that every crystal vibrates with an echo of ancient history—a secret code from a bygone era. Our lessons were as much about unlocking the mysteries of nature as they were about mastering the art of glass. In her eyes, the fusion of art and lore was the highest form of craftsmanship.");
                    p.SendGump(new DialogueGump(p, teachModule));
                });
                
                irisModule.AddOption("Do you still work with her?", p => true, p =>
                {
                    DialogueModule stillModule = new DialogueModule("Indeed, though our paths are fraught with peril and mystery, Iris and I continue to share discoveries. Our meetings are rare but potent, filled with discussions of recent anomalies found in ancient ruins and cryptic signals hidden in crystal formations. Her pragmatism perfectly counters my obsessive pursuit of lost truths.");
                    p.SendGump(new DialogueGump(p, stillModule));
                });
                
                irisModule.AddOption("What challenges did you face together?", p => true, p =>
                {
                    DialogueModule challengesModule = new DialogueModule("In the shadowy corridors of Devil Guard, we encountered strange phenomena—glowing minerals, eerie echoes, and inscriptions in forgotten tongues. Each challenge deepened my resolve to unlock the secrets of a civilization long erased from history. Despite ominous warnings and unsettling encounters, our journey only fueled my scholarly obsession.");
                    p.SendGump(new DialogueGump(p, challengesModule));
                });
                
                player.SendGump(new DialogueGump(player, irisModule));
            });

            // Option 3: Collaborations with Mirielle
            greeting.AddOption("What can you tell me about your artistic exchanges with Mirielle?", player => true, player =>
            {
                DialogueModule mirielleModule = new DialogueModule("Mirielle is a visionary whose artistry mirrors the untamed forces of nature. Our creative exchanges are spirited debates—where my structured glass forms meet her wild, organic expressions. Would you like details on one of our collaborative projects, insights into her creative influences, or the philosophical discussions we share?");
                
                mirielleModule.AddOption("Describe one of your collaborative projects.", p => true, p =>
                {
                    DialogueModule projectModule = new DialogueModule("One memorable project was a moonlit sculpture that captured the ephemeral dance of light and shadow. I sculpted precise, crystalline forms while Mirielle splashed in vibrant hues drawn from nature’s own palette. Some even say that the piece hides a coded map—perhaps a relic of that ancient civilization I so fervently study.");
                    
                    projectModule.AddOption("What do you mean by a coded map?", p2 => true, p2 =>
                    {
                        DialogueModule codedModule = new DialogueModule("There are murmurs among a select few scholars that the arrangement of glass and crystal in our work may mirror ancient sigils. I examine these patterns obsessively, convinced that beneath the artistry lies a message—a guide to long-lost knowledge. This notion fuels my determination, even as others dismiss it as mere fancy.");
                        p2.SendGump(new DialogueGump(p2, codedModule));
                    });
                    
                    projectModule.AddOption("How did this project affect your art?", p2 => true, p2 =>
                    {
                        DialogueModule effectModule = new DialogueModule("That project was transformative. It forced me to integrate a new level of historical inquiry into my art. Mirielle’s raw expression pushed me to reconsider every technique, merging meticulous craftsmanship with my passion for uncovering ancient secrets. My pieces now carry an undercurrent of mystery and scholarly pursuit.");
                        p2.SendGump(new DialogueGump(p2, effectModule));
                    });
                    
                    p.SendGump(new DialogueGump(p, projectModule));
                });
                
                mirielleModule.AddOption("What inspires her art?", p => true, p =>
                {
                    DialogueModule inspireModule = new DialogueModule("Mirielle finds her muse in nature's unbridled beauty—ancient groves, luminous fungi, and the silent echoes of forgotten lore. Her passion for the wild and mysterious often challenges my own structured view, sparking debates that lead us both into deeper philosophical territory.");
                    p.SendGump(new DialogueGump(p, inspireModule));
                });
                
                player.SendGump(new DialogueGump(player, mirielleModule));
            });

            // Option 4: Consultations with Serra
            greeting.AddOption("Tell me about your consultations with Serra.", player => true, player =>
            {
                DialogueModule serraModule = new DialogueModule("Serra, hailing from the radiant shores of Renika, is both a muse and a mentor. Her insights into capturing the fluidity of the sea through glass have transformed my work. Would you like to discuss the techniques she introduced, the philosophical debates we share, or how her guidance has influenced my research into lost civilizations?");
                
                serraModule.AddOption("What techniques did she introduce?", p => true, p =>
                {
                    DialogueModule techniquesSerra = new DialogueModule("Serra revealed the art of layering translucent glass to mimic the ebb and flow of ocean waves. This method not only enhances the visual beauty of my work but also serves as a metaphor for the hidden depths of history. Each layer may conceal clues from an ancient past.");
                    p.SendGump(new DialogueGump(p, techniquesSerra));
                });
                
                serraModule.AddOption("What philosophical insights do you share?", p => true, p =>
                {
                    DialogueModule philosophySerra = new DialogueModule("Our discussions wander through the realms of metaphysics and history. Serra posits that art is a bridge connecting the seen and unseen—a narrative written in color and light. We often debate the role of dark magic in the rituals of a lost civilization, a subject that both terrifies and fascinates me.");
                    
                    philosophySerra.AddOption("Tell me more about dark magic and ancient rituals.", p2 => true, p2 =>
                    {
                        DialogueModule darkMagicModule = new DialogueModule("Dark magic is whispered to have been a tool of the ancient masters—a means to manipulate forces beyond mortal ken. While many dismiss these tales as myth, my own research into forbidden texts suggests there is more truth hidden within these legends than one might believe. I tread carefully, ever the skeptic, yet driven by an obsessive need for knowledge.");
                        p2.SendGump(new DialogueGump(p2, darkMagicModule));
                    });
                    
                    p.SendGump(new DialogueGump(p, philosophySerra));
                });
                
                serraModule.AddOption("How has her guidance shaped your research?", p => true, p =>
                {
                    DialogueModule impactModule = new DialogueModule("Serra's insights have been invaluable. With her encouragement, I now see my art as a vessel for historical revelation. Her ideas have led me to theorize that the ancient art of glassblowing might have been a sacred ritual—an alchemical practice designed to capture and preserve lost knowledge. This theory drives my every experiment.");
                    p.SendGump(new DialogueGump(p, impactModule));
                });
                
                player.SendGump(new DialogueGump(player, serraModule));
            });

            // Option 5: Toma’s Life, Secret Past & Scholarly Pursuits
            greeting.AddOption("Tell me more about your life, past, and secret pursuits.", player => true, player =>
            {
                DialogueModule lifeModule = new DialogueModule("My existence is a tapestry woven with art, science, and a forbidden quest. Beyond my reputation as an artisan, I am a scholar—a historian obsessed with unearthing the truth about a lost civilization. Despite dire warnings of dark magic and the costs of obsession, I have delved into dusty tomes, crumbling ruins, and dangerous archives to piece together their story.");
                
                lifeModule.AddOption("What drew you to this lost civilization?", p => true, p =>
                {
                    DialogueModule drawModule = new DialogueModule("From my earliest days, I was enchanted by legends of a society that mastered both art and the arcane. I discovered fragments of ancient inscriptions and relics that hinted at a civilization with knowledge far beyond our own. Even as conventional wisdom cautioned me, I embraced my obsession—resolute in uncovering a truth that many believed should remain forever buried.");
                    
                    drawModule.AddOption("How do you balance your skepticism with your obsession?", p2 => true, p2 =>
                    {
                        DialogueModule balanceModule = new DialogueModule("It is a constant internal debate. I scrutinize every myth with a rigorous, skeptical mind—demanding evidence and logical reasoning. Yet, my passion and obsessive curiosity drive me to explore even the most forbidden of ideas. I see my work as a quest to reconcile rational inquiry with the mysteries that defy explanation.");
                        p2.SendGump(new DialogueGump(p2, balanceModule));
                    });
                    
                    drawModule.AddOption("Have there been consequences for your research?", p2 => true, p2 =>
                    {
                        DialogueModule consequencesModule = new DialogueModule("Alas, yes. My relentless pursuit has isolated me from peers and invited ridicule from cautious scholars. I have faced warnings from mystics and even threats from those who guard these secrets. But every setback only deepens my resolve—each sacrifice a testament to my commitment to unveil a civilization lost to time.");
                        p2.SendGump(new DialogueGump(p2, consequencesModule));
                    });
                    
                    p.SendGump(new DialogueGump(p, drawModule));
                });
                
                lifeModule.AddOption("What personal sacrifices have you made?", p => true, p =>
                {
                    DialogueModule sacrificeModule = new DialogueModule("I have sacrificed many a night to feverish research, forgoing comfort and companionship in favor of dusty scrolls and treacherous explorations. Friends have distanced themselves, and my health has suffered under the strain of relentless inquiry. Yet, every hardship brings me one step closer to the elusive truth.");
                    p.SendGump(new DialogueGump(p, sacrificeModule));
                });
                
                lifeModule.AddOption("Do you ever regret ignoring warnings of dark magic?", p => true, p =>
                {
                    DialogueModule regretModule = new DialogueModule("Regret is an ever-present shadow. There are moments when the weight of forbidden knowledge and the ominous warnings echo in my mind. Still, my scholarly nature refuses to yield to fear. I persist, driven by a desire to differentiate myth from reality—even if that path is fraught with peril.");
                    p.SendGump(new DialogueGump(p, regretModule));
                });
                
                lifeModule.AddOption("How has your secret past influenced your art?", p => true, p =>
                {
                    DialogueModule artModule = new DialogueModule("Every piece of glass I craft is a fragment of my soul—a mosaic of light, history, and longing. The lost civilization I pursue whispers through every curve and facet. My art is my testimony, an eternal dialogue between the brilliance of the past and the uncertain promise of the future.");
                    p.SendGump(new DialogueGump(p, artModule));
                });
                
                player.SendGump(new DialogueGump(player, lifeModule));
            });

            // Option 6: Farewell
            greeting.AddOption("Farewell.", player => true, player =>
            {
                DialogueModule farewellModule = new DialogueModule("May the lost light of forgotten ages and the spark of your own curiosity forever guide your journey. Farewell, traveler.");
                player.SendGump(new DialogueGump(player, farewellModule));
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
