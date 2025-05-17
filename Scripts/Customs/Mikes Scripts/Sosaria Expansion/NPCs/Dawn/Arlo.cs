using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Arlo : BaseCreature
    {
        [Constructable]
        public Arlo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Arlo";
            Body = 0x190; // Human male body

            // Basic Stats
            SetStr(100);
            SetDex(80);
            SetInt(90);
            SetHits(110);

            // Appearance & Gear – well-worn clothes that tell a tale of hardship and secrets of the sea
            AddItem(new Shirt() { Hue = 1450 });
            AddItem(new LongPants() { Hue = 450 });
            AddItem(new Boots() { Hue = 2250 });
            AddItem(new Cloak() { Hue = 2850 });

            // Random appearance details
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Arlo(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Opening dialogue: Arlo introduces himself, his losses, and his multifaceted journey.
            DialogueModule greeting = new DialogueModule("Hi, I'm Arlo. Life in Dawn is a bittersweet blend of memory, loss, and rebuilding hope. My twin vanished under mysterious circumstances—a tragedy that still haunts me. I've sought clues with Captain Waylon and pored over cryptic maps with Miko, all while learning to rebuild this town with guidance from Torren. There are even whispers of a past I rarely share—a time when I was a fisherman sailing treacherous, enchanted seas. What would you like to know?");
            
            // Option 1: Focus on the twin's disappearance
            greeting.AddOption("Tell me about your twin brother's disappearance.",
                player => true,
                player =>
                {
                    DialogueModule twinModule = new DialogueModule("My twin and I were inseparable, sharing every triumph and sorrow. When he vanished without a trace, it wasn’t just a family loss—it felt like a harbinger of deeper, darker forces at play. Captain Waylon’s eerie maritime legends and Miko's cryptic maps only deepened my suspicions. Which part of this mystery calls to you?");
                    
                    twinModule.AddOption("Tell me about Captain Waylon's insights.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule waylonModule = new DialogueModule("Captain Waylon, the rugged veteran of Pirate Isle, carries scars and lore alike. He once recounted how a spectral storm revealed cursed relics along the coast. His tales, full of grim warnings and raw emotion, make him both revered and feared. Would you like to hear one of his most unforgettable adventures or know how his guidance shapes my quest?");
                            waylonModule.AddOption("Share one of his unforgettable adventures.",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule adventuresModule = new DialogueModule("On a night when the moon hid and storms raged, Captain Waylon led us to a desolate inlet where ghostly ships seemed to sail through the fog. Each crashing wave echoed the lost cries of sailors long past. His account still sends chills down my spine.");
                                    pls.SendGump(new DialogueGump(pls, adventuresModule));
                                });
                            waylonModule.AddOption("How does his advice impact your search?",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule wisdomModule = new DialogueModule("His pragmatic yet mystical counsel urges me to never overlook the smallest clue—every whisper of the wind or shift in the tides might hint at hidden truths. Without his guidance, I'd be adrift in a sea of uncertainty.");
                                    pls.SendGump(new DialogueGump(pls, wisdomModule));
                                });
                            pl.SendGump(new DialogueGump(pl, waylonModule));
                        });

                    twinModule.AddOption("Tell me more about Miko's mysterious maps.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule mikoModule = new DialogueModule("Miko, the enigmatic mapmaker of Pirate Isle, crafts charts that border on magic. His maps are riddled with symbols and secret markers—a language of the deep that hints at underwater curses and long-forgotten trails. Would you like to learn how one such map may hide clues to my brother’s fate?");
                            mikoModule.AddOption("Show me an example of his cryptic clues.",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule cluesModule = new DialogueModule("One map reveals a coastline etched with bizarre runes, culminating at a cursed cove whispered to be the haunt of ancient spirits. Miko insists that these markings are no coincidence. They beckon us to pursue the hidden history beneath the waves.");
                                    pls.SendGump(new DialogueGump(pls, cluesModule));
                                });
                            mikoModule.AddOption("How do his maps guide your investigation?",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule guidanceModule = new DialogueModule("Every mark and faded line on his maps presents a puzzle—one that demands exploration. His work challenges me to follow a trail that might lead to my brother, even if it weaves through realms of maritime myth.");
                                    pls.SendGump(new DialogueGump(pls, guidanceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, mikoModule));
                        });

                    twinModule.AddOption("Do you suspect dark forces are at work?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule forcesModule = new DialogueModule("There are whispers of curses linked to lost relics, and the sea itself seems to hold grudges. I believe that forces beyond mere chance may be entwined with my brother’s fate—a dissonance of ancient magic and maritime woe.");
                            pl.SendGump(new DialogueGump(pl, forcesModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, twinModule));
                });
            
            // Option 2: Rebuilding Dawn and learning from Torren
            greeting.AddOption("I need advice on rebuilding Dawn.",
                player => true,
                player =>
                {
                    DialogueModule rebuildModule = new DialogueModule("Rebuilding Dawn is as much about patching broken walls as it is about healing wounded hearts. Torren, a master builder from East Montor, guides us with a blend of time-honored techniques and innovative methods. Would you like to hear about his construction secrets or the spirit that drives our restoration?");
                    
                    rebuildModule.AddOption("Tell me about Torren's construction techniques.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule torrenModule = new DialogueModule("Torren employs both old-world joinery and even experiments with runic inscriptions on building materials. He believes that integrating subtle enchantments within our structures can protect us from calamities. His hands, though rough and worn, work as if guided by ancient wisdom.");
                            torrenModule.AddOption("What kind of runes does he use?",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule runesModule = new DialogueModule("He carves simple symbols of protection—a crescent moon, a looping spiral—each a relic of ancestral magic believed to ward off bad fortune. These runes, modest in form, are potent in their effect.");
                                    pls.SendGump(new DialogueGump(pls, runesModule));
                                });
                            torrenModule.AddOption("How has his guidance inspired the community?",
                                pls => true,
                                pls =>
                                {
                                    DialogueModule communityModule = new DialogueModule("Under his watchful eyes, people of Dawn are learning not only to rebuild their homes but also their spirits. Every repaired roof and mended path is a testament to our resilience and unwavering hope.");
                                    pls.SendGump(new DialogueGump(pls, communityModule));
                                });
                            pl.SendGump(new DialogueGump(pl, torrenModule));
                        });
                    
                    rebuildModule.AddOption("What does rebuilding Dawn mean to you personally?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule feelingsModule = new DialogueModule("For me, every brick laid is both a tribute to my lost brother and a promise to the future. Rebuilding is not just about structures—it is an act of defiance against despair. It keeps me grounded, even when shadows of the past loom large.");
                            pl.SendGump(new DialogueGump(pl, feelingsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, rebuildModule));
                });
            
            // Option 3: Learn more about Arlo’s personal journey and relationships.
            greeting.AddOption("Can you tell me more about your personal journey?",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule("My life is a mosaic of tender memories and hard-won lessons. Growing up with my twin forged an inseparable bond—a bond now fractured by loss. Yet, mentors like Captain Waylon, Miko, and Torren have all, in their own ways, helped piece my identity back together. Which part of my journey interests you?");
                    
                    personalModule.AddOption("Tell me about Captain Waylon.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule personalWaylonModule = new DialogueModule("Captain Waylon is more than a sailor; he’s a living repository of maritime legends. His weathered face and calloused hands tell tales of battles against both man and myth. I often marvel at how his life bridges the gap between the earthly and the enchanted.");
                            pl.SendGump(new DialogueGump(pl, personalWaylonModule));
                        });
                    
                    personalModule.AddOption("Tell me about Miko.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule personalMikoModule = new DialogueModule("Miko weaves reality and mystery into his maps. Each parchment he crafts is a journey into hidden realms—a blend of art and arcana that challenges what we know about our world. His silent confidence speaks volumes about secrets best kept close.");
                            pl.SendGump(new DialogueGump(pl, personalMikoModule));
                        });
                    
                    personalModule.AddOption("Tell me about Torren.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule personalTorrenModule = new DialogueModule("Torren stands as a pillar of steadfast pragmatism and hope. A master builder whose expert hands restore not only physical structures but also community spirit, his guidance has helped many find resilience in the face of overwhelming odds.");
                            pl.SendGump(new DialogueGump(pl, personalTorrenModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, personalModule));
                });
            
            // Option 4: A secret branch—Arlo reveals his hidden past as a fisherman.
            greeting.AddOption("Tell me about your secret past as a fisherman.",
                player => true,
                player =>
                {
                    DialogueModule fishermanModule = new DialogueModule("Before I found solace in rebuilding Dawn, I was once a humble fisherman—hardy and superstitious—who braved the mighty, mysterious seas. Those days are filled with memories of monstrous creatures, enchanted fogs, and hidden islands that many dismiss as sailor's folly. What part of that life interests you?");
                    
                    fishermanModule.AddOption("Tell me of the sea monsters you encountered.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule monstersModule = new DialogueModule("Deep in the inky black of a moonless night, I witnessed a beast like no other—a titanic creature with rippling, dark scales and tendrils that snaked through the waves. Its eyes, glowing a fierce red, pierced the fog. The sea roared in protest at its passing. Would you like to hear more about its dread appearance or the omens that foretold its arrival?");
                            
                            monstersModule.AddOption("Describe the creature in vivid detail.",
                                p => true,
                                p =>
                                {
                                    DialogueModule creatureDetailModule = new DialogueModule("It was a behemoth of the deep: a swirling mass of murky water and glistening scales. Its form shifted like smoke in a gale, and each movement sent ripples that shattered the calm of the dark sea. I remember the overwhelming sense of foreboding, as if the ocean itself were mourning.");
                                    p.SendGump(new DialogueGump(p, creatureDetailModule));
                                });
                            
                            monstersModule.AddOption("What omens warned you of its approach?",
                                p => true,
                                p =>
                                {
                                    DialogueModule omensModule = new DialogueModule("Before its monstrous form emerged, the air grew stiflingly still. A dense fog rolled in without warning, and our nets, once full, lay eerily empty. Superstitions ran rampant among us—omens of a wrathful deity stirring in the deeps. Such moments still echo in my dreams.");
                                    p.SendGump(new DialogueGump(p, omensModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, monstersModule));
                        });
                    
                    fishermanModule.AddOption("What about the hidden islands you once discovered?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule islandsModule = new DialogueModule("I remember a night so tempestuous that the heavens themselves seemed to weep. Amid the fury of a raging storm, a faint glimmer appeared on the horizon—a secret island shrouded in mist and mystery. It was a glimpse of paradise lost, a place whispered about in half-believed legends. Do you wish to hear the tale of my discovery or the secrets the island may hold?");
                            
                            islandsModule.AddOption("Tell me how you discovered that hidden island.",
                                p => true,
                                p =>
                                {
                                    DialogueModule discoveryModule = new DialogueModule("Our little boat was battered by furious waves when an otherworldly light cut through the darkness. Compelled by both dread and wonder, we followed it to an island that seemed suspended in time. Its shores were lined with strange flora and ruins that hummed with forgotten magic—a silent witness to a lost civilization.");
                                    p.SendGump(new DialogueGump(p, discoveryModule));
                                });
                            
                            islandsModule.AddOption("What secrets does that island hide?",
                                p => true,
                                p =>
                                {
                                    DialogueModule secretsModule = new DialogueModule("Legends say that the island is a sanctuary of ancient magic. Within its hidden coves lie relics that might bend fate itself—a place where the veil between this world and another thins, allowing whispers of the divine and the damned to intermingle.");
                                    p.SendGump(new DialogueGump(p, secretsModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, islandsModule));
                        });
                    
                    fishermanModule.AddOption("How have these experiences at sea shaped you?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule beliefsModule = new DialogueModule("The relentless sea taught me humility and the vast limits of human control. Every voyage was a lesson in survival—a constant balance between defiance and respect for nature's unpredictable might. I grew superstitious, always mindful of subtle signs: a snapped rope, an inexplicable chill, or a sudden, disquieting silence.");
                            
                            beliefsModule.AddOption("Do you still fear the sea?",
                                p => true,
                                p =>
                                {
                                    DialogueModule fearModule = new DialogueModule("I do not fear it as much as I revere it. The sea can be both a nurturing mother and a vengeful force. That duality instilled in me a deep respect, reminding me that courage and caution must always walk hand in hand.");
                                    p.SendGump(new DialogueGump(p, fearModule));
                                });
                            
                            beliefsModule.AddOption("Have others believed your tales of the deep?",
                                p => true,
                                p =>
                                {
                                    DialogueModule skepticismModule = new DialogueModule("Many dismissed my accounts as drunken ramblings of a weathered sailor. Yet every tale, no matter how fantastical, holds a seed of truth. The old stories survive in whispers and dreams, and they sustain me even when others doubt.");
                                    p.SendGump(new DialogueGump(p, skepticismModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, beliefsModule));
                        });
                    
                    fishermanModule.AddOption("What lessons did the sea impart to you?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule lessonsModule = new DialogueModule("Every voyage was a masterclass in endurance and respect. I learned that the sea, with all its beauty and terror, rewards those who heed its omens. It taught me that even in the face of relentless storms, hope can be found in the smallest ripple on a calm sea.");
                            
                            lessonsModule.AddOption("How do you use these lessons today?",
                                p => true,
                                p =>
                                {
                                    DialogueModule applicationModule = new DialogueModule("The wisdom I gleaned from those wild, haunted waters guides every aspect of my life now—whether deciphering Miko's cryptic maps, planning the reconstruction of Dawn, or chasing even the faintest hope of finding my lost brother. The sea’s teachings remain my constant companion.");
                                    p.SendGump(new DialogueGump(p, applicationModule));
                                });
                            
                            lessonsModule.AddOption("Is there one memory that stands out above the rest?",
                                p => true,
                                p =>
                                {
                                    DialogueModule memoryModule = new DialogueModule("One unforgettable night, as a tempest nearly swallowed our boat, I saw a flicker—a spectral light among the waves—that seemed to beckon me towards destiny. In that moment, fear gave way to awe, and I knew that the mysteries of the deep were not mere fables, but echoes of a reality far grander than our understanding.");
                                    p.SendGump(new DialogueGump(p, memoryModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, lessonsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, fishermanModule));
                });
            
            // Option 5: Exit dialogue
            greeting.AddOption("I should be going. Thanks for sharing your story.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule()))
            );

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
