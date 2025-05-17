using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Pia : BaseCreature
    {
        [Constructable]
        public Pia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pia";
            Body = 0x191; // Human female body.
            
            // Basic Stats
            SetStr(100);
            SetDex(90);
            SetInt(110);
            SetHits(120);

            // Appearance & Equipment: Practical hunting attire with a secret nod to her noble dueling past.
            AddItem(new Tunic() { Hue = 1175, Name = "Hunting Tunic" });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new Cloak() { Hue = 1175, Name = "Cloak of the Woodland" });
            AddItem(new LeatherGloves() { Hue = 1175 });
            // A hidden accessory hinting at her noble origins.
            AddItem(new Quiver() { Hue = 1500, Name = "Hidden Noble's Scabbard" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Pia(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Pia—a hunter of Yew whose expertise in tracking, lore, and even the finesse of the duel is whispered from the woodland paths to noble courts. My life is a tapestry woven from the simple duties of protecting livestock alongside a secret, ambitious past as a noble duelist. What intrigues your mind today?");
            
            // Option 1: Discussion about her work with Silas.
            greeting.AddOption("Tell me about your work with Silas in Yew.", player => true, player =>
            {
                DialogueModule silasModule = new DialogueModule("Silas, my steadfast companion, and I patrol these ancient groves ensuring that our livestock and the delicate balance of nature remain undisturbed. We blend the old ways with rigorous observation. Would you like to delve into our tracking methods or the time-honored protective rites we perform?");
                
                silasModule.AddOption("Reveal your tracking methods.", pl => true, pl =>
                {
                    DialogueModule trackingModule = new DialogueModule("Tracking in Yew is an art—a ballet with the wilderness. I observe the subtle shifts in the wind, the crackle of dry leaves, and the silent trails left behind by animals. Every move is honed by instinct and skill, a talent refined over countless seasons.");
                    pl.SendGump(new DialogueGump(pl, trackingModule));
                });
                
                silasModule.AddOption("Explain your protective rites.", pl => true, pl =>
                {
                    DialogueModule ritesModule = new DialogueModule("Before every hunt, Silas and I perform rituals beneath the ancient oaks. We offer tokens—a sprig of holly, a smooth stone from the river—to appease the spirits. These rites are not mere superstition; they are part of a legacy of respect for nature and an indication of my ambitious desire to command both tradition and destiny.");
                    pl.SendGump(new DialogueGump(pl, ritesModule));
                });
                
                player.SendGump(new DialogueGump(player, silasModule));
            });

            // Option 2: Sharing tales with Jonas.
            greeting.AddOption("I hear you share family legends with Jonas. Can you recount one of those stories?", player => true, player =>
            {
                DialogueModule jonasModule = new DialogueModule("Indeed, my friend Jonas from West Montor and I exchange lore that echoes through generations. One storied legend is the 'Great Hunt'—a time when valor was measured by facing down creatures of myth. Would you prefer to hear the details of that fabled night or learn what inner drive compels me to surpass such legends?");
                
                jonasModule.AddOption("Tell me the details of the Great Hunt.", pl => true, pl =>
                {
                    DialogueModule greatHuntModule = new DialogueModule("In a harsh winter long past, our ancestors banded together against a horde of dire beasts. I recall the thrill of combat under a blood-red moon, each strike an assertion of skill. Even now, the memory of dueling those shadows ignites my resolve to challenge every legend.");
                    pl.SendGump(new DialogueGump(pl, greatHuntModule));
                });
                
                jonasModule.AddOption("What drives you to challenge these legends?", pl => true, pl =>
                {
                    DialogueModule driveModule = new DialogueModule("My ambition burns bright—a fire kindled from noble blood and the desire to sculpt my own legend. I was once groomed for the dueling arena, and that legacy whispers in my veins. Every challenge I accept is a step toward surpassing the old myths and etching my name into the chronicles of our land.");
                    driveModule.AddOption("What makes you so certain of your talents?", pl2 => true, pl2 =>
                    {
                        DialogueModule confidentModule = new DialogueModule("I am not simply a hunter; I am a virtuoso of the duel. My techniques are a blend of precision, artistry, and ruthless determination. I have faced formidable opponents, and my victories are the measure of my skill. Such talent, some might say, borders on arrogance.");
                        confidentModule.AddOption("Your arrogance only fuels your greatness.", pl3 => true, pl3 =>
                        {
                            DialogueModule prideModule = new DialogueModule("Arrogance? Perhaps. Yet, it is the fire that drives me to excel and to shatter every expectation. I embrace my noble heritage and my undeniable prowess. Each duel is a performance, every victory a testament to my ascendancy over mediocrity.");
                            pl3.SendGump(new DialogueGump(pl3, prideModule));
                        });
                        confidentModule.AddOption("I wonder if humility might serve you well.", pl3 => true, pl3 =>
                        {
                            DialogueModule humilityModule = new DialogueModule("Humility is for those content with obscurity. I choose the path of bold ambition, where every action reverberates in the annals of legend. My destiny is self-forged, and I will not be shackled by the timid ways of the past.");
                            pl3.SendGump(new DialogueGump(pl3, humilityModule));
                        });
                        pl2.SendGump(new DialogueGump(pl2, confidentModule));
                    });
                    driveModule.AddOption("And when you conquer these legends, what then?", pl2 => true, pl2 =>
                    {
                        DialogueModule futureModule = new DialogueModule("When I have bested the legends that loom over our history, I shall rise as an icon—a paragon of skill and ambition. My victories will rewrite the narrative of our realm, and my name will be spoken with reverence and awe.");
                        pl2.SendGump(new DialogueGump(pl2, futureModule));
                    });
                    pl.SendGump(new DialogueGump(pl, driveModule));
                });
                player.SendGump(new DialogueGump(player, jonasModule));
            });

            // Option 3: Discussing Pia’s personal journey and trials.
            greeting.AddOption("Tell me about your journey—the trials that molded you.", player => true, player =>
            {
                DialogueModule journeyModule = new DialogueModule("My life began amidst the whispering groves of Yew—a place where nature's harsh lessons blend with the refined tutelage of noble dueling. I learned to respect the wild while secretly perfecting the art of the duel. Which part of my journey would you like to explore?");
                
                journeyModule.AddOption("What trials have you endured?", pl => true, pl =>
                {
                    DialogueModule trialsModule = new DialogueModule("I have survived encounters with beasts that defy mortal lore and skirmishes that test both body and spirit. Every trial, every scar, is a stepping stone toward greatness. The duels I fought under starlit skies—those battles forged my unyielding resolve.");
                    trialsModule.AddOption("Describe a duel that redefined you.", pl2 => true, pl2 =>
                    {
                        DialogueModule duelModule = new DialogueModule("Picture a moonlit clearing where steel clashed like thunder. I faced an opponent renowned across the courts—yet my blade found its mark with an elegance borne of years of training. That single duel was a rebirth, a declaration that I would not be confined by the expectations of nobility or mediocrity.");
                        pl2.SendGump(new DialogueGump(pl2, duelModule));
                    });
                    trialsModule.AddOption("How have these trials shaped your ambition?", pl2 => true, pl2 =>
                    {
                        DialogueModule heartModule = new DialogueModule("Each trial has etched ambition into my very soul. My noble blood courses with the legacy of warriors, and every challenge strengthens my resolve to defy the legends. My past is both a burden and a beacon—a secret wellspring of power fueling my quest for ultimate mastery.");
                        pl2.SendGump(new DialogueGump(pl2, heartModule));
                    });
                    pl.SendGump(new DialogueGump(pl, trialsModule));
                });
                
                journeyModule.AddOption("How do you balance your duties as a hunter with your dueling prowess?", pl => true, pl =>
                {
                    DialogueModule balanceModule = new DialogueModule("By day, I blend with the hunters of Yew, safeguarding our land. But beneath that unassuming exterior lies a duelist of noble stock—a master of refined combat. This dual life is a balance of raw survival and the cultured art of the duel, each complementing my relentless pursuit of glory.");
                    balanceModule.AddOption("Does the life of a hunter humble your noble spirit?", pl2 => true, pl2 =>
                    {
                        DialogueModule temperModule = new DialogueModule("The wilderness strips away pretension and reinforces the essentials of survival. Yet my noble past sharpens my resolve rather than humbling it. I harness both extremes—embracing the rawness of nature and the precision of dueling—to become greater than the sum of my parts.");
                        pl2.SendGump(new DialogueGump(pl2, temperModule));
                    });
                    balanceModule.AddOption("Is your noble heritage a burden or an asset?", pl2 => true, pl2 =>
                    {
                        DialogueModule giftModule = new DialogueModule("It is both—a burden of expectation and an immeasurable asset. I was born into privilege, yet I have chosen the arduous road of self-made excellence. This duality drives me to excel, a secret flame that fuels every endeavor in the field and on the duel floor.");
                        pl2.SendGump(new DialogueGump(pl2, giftModule));
                    });
                    pl.SendGump(new DialogueGump(pl, balanceModule));
                });
                
                player.SendGump(new DialogueGump(player, journeyModule));
            });
            
            // Option 4: The secret inquiry about her noble dueling past.
            greeting.AddOption("There are whispers that you are more than a simple hunter. Will you reveal your secret?", player => true, player =>
            {
                DialogueModule secretModule = new DialogueModule("Ah, perceptive traveler—you have glimpsed the truth behind my humble guise. Indeed, I was born of noble lineage and trained from youth in the art of the duel. I concealed this heritage to carve my own path among the common folk, yet it is the very source of my ambition. What would you like to know?");
                
                secretModule.AddOption("How did you abandon nobility for the wild?", pl => true, pl =>
                {
                    DialogueModule concealModule = new DialogueModule("I rejected the decadence of courtly life to embrace a realm where skill speaks louder than titles. In the woodlands of Yew, I learned that true honor is won with sweat, steel, and unwavering resolve. My choice was a deliberate renunciation of comfort in favor of forging a legacy through combat.");
                    concealModule.AddOption("Was this transition painful?", pl2 => true, pl2 =>
                    {
                        DialogueModule pathModule = new DialogueModule("It was no gentle transition. I endured rigorous training, bitter rivalries, and countless duels that tested my mettle. Every hardship was a lesson, molding my talent and stoking the fires of my ambition to surpass every legend.");
                        pl2.SendGump(new DialogueGump(pl2, pathModule));
                    });
                    pl.SendGump(new DialogueGump(pl, concealModule));
                });
                
                secretModule.AddOption("What fuels your obsession with defeating legends?", pl => true, pl =>
                {
                    DialogueModule obsessionModule = new DialogueModule("Legends are the benchmarks of greatness, and I seek to redefine them. My ambition is to vanquish those whose reputations have grown larger than life, proving that I am the superior force. I relish every challenge as an opportunity to inscribe my name into the saga of our realm.");
                    obsessionModule.AddOption("Do you fear any of these legendary foes?", pl2 => true, pl2 =>
                    {
                        DialogueModule fearModule = new DialogueModule("Fear is a luxury I do not permit myself. Every duel is met with certainty—my blade is an extension of my indomitable will. The mere shadow of a legend only serves to embolden me further.");
                        fearModule.AddOption("Your confidence seems almost arrogant.", pl3 => true, pl3 =>
                        {
                            DialogueModule arrogantModule = new DialogueModule("Arrogance? If one must call ambition by that name, then so be it. I am talented, and I wear my pride as a badge of honor. It is this same audacity that drives me to challenge every myth and emerge victorious.");
                            pl3.SendGump(new DialogueGump(pl3, arrogantModule));
                        });
                        pl2.SendGump(new DialogueGump(pl2, fearModule));
                    });
                    obsessionModule.AddOption("What do you plan to do once you conquer these legends?", pl2 => true, pl2 =>
                    {
                        DialogueModule futureModule = new DialogueModule("Upon besting these legends, I will ascend beyond mortal constraints—a living testament to noble valor and unmatched skill. My victories will not only honor my heritage but will forever redefine what it means to be truly great.");
                        pl2.SendGump(new DialogueGump(pl2, futureModule));
                    });
                    pl.SendGump(new DialogueGump(pl, obsessionModule));
                });
                
                secretModule.AddOption("Are you proud of your noble heritage?", pl => true, pl =>
                {
                    DialogueModule proudModule = new DialogueModule("Pride is not mere vanity; it is the acknowledgment of a legacy steeped in honor and conquest. I take pride in my bloodline and channel it into every duel, every decision, ensuring that my name is forever etched in greatness.");
                    pl.SendGump(new DialogueGump(pl, proudModule));
                });
                
                player.SendGump(new DialogueGump(player, secretModule));
            });
            
            // Option 5: Farewell.
            greeting.AddOption("Thank you, Pia. I must take my leave.", player => true, player =>
            {
                player.SendMessage("Pia offers a curt, knowing nod—a blend of genuine camaraderie and the cool detachment of nobility—as you depart.");
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
