using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Darvin the Tavern Keeper")]
    public class Darvin : BaseCreature
    {
        [Constructable]
		public Darvin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Darvin, the Tavern Keeper";
            Body = 0x190; // Human male body

            // Stats & Health
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance (A mix of farmer’s practicality and adventurer’s flair)
            AddItem(new Doublet() { Hue = 2100 }); // A rustic tunic
            AddItem(new Boots() { Hue = 2100 });
            AddItem(new WideBrimHat() { Hue = 2110, Name = "Farmer Hat" });
            AddItem(new Cloak() { Hue = 2110, Name = "Cloak of Wandered Days" }); // A nod to his adventurous past

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Darvin(Serial serial) : base(serial) { }

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
                "Howdy, friend! I'm Darvin—the jovial, courageous, and devoted keeper of this tavern. " +
                "Once upon a time I roamed as a legendary adventurer, and now I serve hearty ales and " +
                "warm smiles. But there’s more beneath these boards than meets the eye… What would you like to talk about today?"
            );

            // Option 1: Inquire about the tavern and its hidden secrets.
            greeting.AddOption("Tell me about this humble tavern.", 
                player => true,
                player =>
                {
                    DialogueModule tavernModule = new DialogueModule(
                        "This tavern has seen it all—from grand feasts to raucous brawls. " +
                        "Every corner hides a memory, and if you listen closely, you might hear echoes " +
                        "of my past adventures. Would you like to hear about its history, or perhaps the secret kept below these very floors?"
                    );

                    tavernModule.AddOption("I'm curious about the tavern's history...", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule tavernHistoryModule = new DialogueModule(
                                "Ah, the history of these walls is as rich as a king’s treasury! " +
                                "I built this place after a long career of battling monsters and unearthing forgotten lore. " +
                                "Every plank and stone was laid with the lessons I learned as an adventurer. " +
                                "Do you want to hear about my daring adventures during its founding, or maybe the nights " +
                                "when foes dared approach its door?"
                            );

                            tavernHistoryModule.AddOption("Tell me about your daring adventures.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule adventuresModule = new DialogueModule(
                                        "I’ve trekked through cursed ruins, sailed tempestuous seas, " +
                                        "and battled dark sorcerers—all with a smile on my face and fire in my heart. " +
                                        "For instance, there was the perilous night in the Midnight Marsh when the mists " +
                                        "whispered of doom and I stood firm against a horde of spectral foes. " +
                                        "Would you like to hear the details of that harrowing night?"
                                    );

                                    adventuresModule.AddOption("Yes, share that perilous tale.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule perilModule = new DialogueModule(
                                                "Under a moon shrouded in mist, I fought tooth and nail amidst swirling shadows. " +
                                                "Spectral warriors emerged from the marsh, their eyes burning with ancient malice. " +
                                                "I swung my blade with the courage only true adventurers know. " +
                                                "Though the battle nearly broke me, my determination prevailed and I earned my oath as a guardian."
                                            );
                                            r.SendGump(new DialogueGump(r, perilModule));
                                        });

                                    adventuresModule.AddOption("Actually, I’d prefer a lighter tale.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule lighterModule = new DialogueModule(
                                                "Once, in a fit of mischief, I chased a runaway pig through secret tunnels under this tavern! " +
                                                "It was absurd, a moment of pure, unbridled hilarity that reminds me life need not always be grim—even " +
                                                "when danger lurks around every corner."
                                            );
                                            r.SendGump(new DialogueGump(r, lighterModule));
                                        });

                                    q.SendGump(new DialogueGump(q, adventuresModule));
                                });

                            tavernHistoryModule.AddOption("When foes gathered at your door...", 
                                q => true,
                                q =>
                                {
                                    DialogueModule foesModule = new DialogueModule(
                                        "There were nights when enemy forces, dark bandits, and even creatures from nightmare realms " +
                                        "were drawn to this haven. I rallied a band of brave souls and defended this tavern as if it were " +
                                        "my last bastion. Our courageous hearts lit the darkness, and that night, valor stood triumphant."
                                    );
                                    q.SendGump(new DialogueGump(q, foesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, tavernHistoryModule));
                        });

                    tavernModule.AddOption("What's the secret hidden below?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "Now you're truly perceptive! Beneath these very floors lies a chamber where I guard ancient artifacts—relics " +
                                "of an age when magic thrived. I discovered these powerful items in my travels and swore an oath to keep them " +
                                "safe from those who would abuse their might. Would you like to know more about these artifacts, or the oath " +
                                "I made to protect them?"
                            );

                            secretModule.AddOption("Tell me about the ancient artifacts.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule artifactsModule = new DialogueModule(
                                        "Among the relics is the Orb of Dawning Light, said to contain the very spark of a newborn star, " +
                                        "and the Blade of Lost Echoes, forged in the heart of a dormant volcano. Their magic is fierce and unpredictable. " +
                                        "I guard them with a devotion only matched by my past exploits."
                                    );

                                    artifactsModule.AddOption("How do you keep such power safe?", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule guardModule = new DialogueModule(
                                                "Every night, after the laughter dies down and the last ale is served, I personally secure the hidden vault. " +
                                                "Traps, enchanted seals, and a vigilant spirit ensure that these artifacts remain untouched by greed or malice."
                                            );
                                            r.SendGump(new DialogueGump(r, guardModule));
                                        });

                                    artifactsModule.AddOption("I wish I could see them someday.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule viewModule = new DialogueModule(
                                                "Patience, my friend. Trust must grow as steady as the oak before I unveil these ancient wonders. " +
                                                "Let our conversations deepen, and perhaps one day I'll grant you a glimpse into the heart of my secret."
                                            );
                                            r.SendGump(new DialogueGump(r, viewModule));
                                        });
                                    q.SendGump(new DialogueGump(q, artifactsModule));
                                });

                            secretModule.AddOption("Tell me about the oath you swore.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule oathModule = new DialogueModule(
                                        "In the heat of battle, when the world was aflame, I vowed to protect these relics at any cost. " +
                                        "That oath—sacred and unyielding—turned my life from a freewheeling adventurer to a devoted guardian. " +
                                        "It is a duty I bear with both pride and the solemnity of a man who has seen the harsh truths of destiny."
                                    );

                                    oathModule.AddOption("A heavy burden indeed.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule burdenModule = new DialogueModule(
                                                "It is heavy, but I bear it with a smile as warm as the hearth. The laughter of my patrons " +
                                                "and the promise of a safer tomorrow are worth every scar and every sleepless night."
                                            );
                                            r.SendGump(new DialogueGump(r, burdenModule));
                                        });

                                    oathModule.AddOption("What if you were ever to falter?", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule falterModule = new DialogueModule(
                                                "I have weathered more storms than most could imagine. Every battle, every fall, " +
                                                "has only steeled my resolve. I would never allow my guard to drop—courage and devotion are my lifeblood."
                                            );
                                            r.SendGump(new DialogueGump(r, falterModule));
                                        });
                                    q.SendGump(new DialogueGump(q, oathModule));
                                });
                            pl.SendGump(new DialogueGump(pl, secretModule));
                        });
                    player.SendGump(new DialogueGump(player, tavernModule));
                });

            // Option 2: Ask Darvin about his personal story.
            greeting.AddOption("Tell me your personal story, Darvin.", 
                player => true,
                player =>
                {
                    DialogueModule storyModule = new DialogueModule(
                        "My tale is long and winding. I wasn't always the jovial tavern keeper you see today. " +
                        "I once roamed as a legendary adventurer, a fearless soul facing down nightmares and forging my path " +
                        "with courage. Those days are behind me—but they haunt these halls in the form of secret relics, " +
                        "and in the bittersweet memories of a love lost."
                    );

                    storyModule.AddOption("What adventures did you embark on?", 
                        p => true,
                        p =>
                        {
                            DialogueModule adventuresPastModule = new DialogueModule(
                                "Oh, the adventures were grand! I battled monstrous creatures in cursed ruins, " +
                                "explored forgotten temples, and even outwitted treacherous foes in a realm of shifting shadows. " +
                                "Would you care to hear about the harrowing battle at the Midnight Marsh or my daring escape from the Iron Citadel?"
                            );

                            adventuresPastModule.AddOption("I’d love to hear about the Midnight Marsh.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule marshModule = new DialogueModule(
                                        "In the heart of the marsh, under a moon cloaked in mist, I found myself " +
                                        "outnumbered by spectral warriors. The air was thick with dread, but my sword " +
                                        "sang with the clarity of resolve. I fought with all the courage I could muster, " +
                                        "and that night, my heart learned what it meant to stand as a guardian of hope."
                                    );
                                    q.SendGump(new DialogueGump(q, marshModule));
                                });

                            adventuresPastModule.AddOption("Tell me about the Iron Citadel escape.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule citadelModule = new DialogueModule(
                                        "The Iron Citadel was a fortress of unyielding stone and unbreakable will. " +
                                        "Surrounded by foes and ensnared in a maze of corridors, I relied on wit and sheer determination " +
                                        "to slip away in the dead of night. The thrill of that escape still quickens my pulse even now."
                                    );
                                    q.SendGump(new DialogueGump(q, citadelModule));
                                });
                            p.SendGump(new DialogueGump(p, adventuresPastModule));
                        });

                    storyModule.AddOption("You mentioned a secret—what is it?", 
                        p => true,
                        p =>
                        {
                            DialogueModule secretPastModule = new DialogueModule(
                                "Ah, keen of ear, my friend! Beneath this very tavern lies a hidden vault filled with powerful artifacts " +
                                "collected during my adventures. I discovered these relics on a fateful journey through ruins long lost to time, " +
                                "and I swore an oath to protect them from falling into the wrong hands. They are my burden and my legacy."
                            );

                            secretPastModule.AddOption("How did you come to possess these relics?", 
                                q => true,
                                q =>
                                {
                                    DialogueModule artifactsOriginModule = new DialogueModule(
                                        "In a crumbling crypt deep beneath an ancient ruin, I uncovered an array of relics—each pulsing " +
                                        "with a magic older than the stars. I fought off mercenaries and dark sorcery to claim them, and ever since, " +
                                        "I have guarded them here with unwavering devotion."
                                    );
                                    q.SendGump(new DialogueGump(q, artifactsOriginModule));
                                });

                            secretPastModule.AddOption("Your courage is admirable.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule respectModule = new DialogueModule(
                                        "Thank you, kind soul. My past was forged in the heat of battle and tempered by loss. " +
                                        "Though I bear the scars of those days, I remain devoted to protecting the fragile hope of tomorrow."
                                    );
                                    q.SendGump(new DialogueGump(q, respectModule));
                                });

                            secretPastModule.AddOption("Is it true that you're divorced?", 
                                q => true,
                                q =>
                                {
                                    DialogueModule divorceModule = new DialogueModule(
                                        "Aye, it's true. In the pursuit of adventure and duty, my heart and home were torn asunder. " +
                                        "My marriage could not withstand the call of destiny and the weight of the secrets I carry. " +
                                        "Yet, even in solitude, I find strength and a fierce commitment to my duty."
                                    );
                                    divorceModule.AddOption("That must have been incredibly painful.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule hardModule = new DialogueModule(
                                                "Indeed, it was a bitter draught to swallow. Every wound, both seen and unseen, " +
                                                "has fueled my determination to forge a better path—not only for myself but for those who seek hope."
                                            );
                                            r.SendGump(new DialogueGump(r, hardModule));
                                        });
                                    divorceModule.AddOption("I admire your honesty and resolve.", 
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule honestModule = new DialogueModule(
                                                "Honesty, though painful, is the best potion to clear the mists of regret. " +
                                                "I choose to share my truth so that others may learn that even the most difficult losses " +
                                                "can lead to a future filled with newfound courage and passion."
                                            );
                                            r.SendGump(new DialogueGump(r, honestModule));
                                        });
                                    q.SendGump(new DialogueGump(q, divorceModule));
                                });
                            p.SendGump(new DialogueGump(p, secretPastModule));
                        });

                    storyModule.AddOption("Your cheerful nature is infectious. How do you keep laughing?", 
                        p => true,
                        p =>
                        {
                            DialogueModule cheerfulModule = new DialogueModule(
                                "Oh, my friend, laughter is the very spice of life! Even when danger looms and memories weigh heavy, " +
                                "I find joy in a well-spun joke or a hearty guffaw with my patrons. Would you like to hear one of my favorite quips?"
                            );

                            cheerfulModule.AddOption("Yes, share a joke!", 
                                q => true,
                                q =>
                                {
                                    DialogueModule jokeModule = new DialogueModule(
                                        "Alright then: Why did the adventurer bring a ladder to the tavern? " +
                                        "Because he heard the drinks were on the house! Ha ha—sometimes the simplest jests lift the heaviest hearts."
                                    );
                                    q.SendGump(new DialogueGump(q, jokeModule));
                                });

                            cheerfulModule.AddOption("I’m already smiling, thanks to you.", 
                                q => true,
                                q =>
                                {
                                    DialogueModule appreciateModule = new DialogueModule(
                                        "I’m glad to hear that! A smile shared is a burden halved. Remember, no matter how dark the night, dawn always follows."
                                    );
                                    q.SendGump(new DialogueGump(q, appreciateModule));
                                });
                            p.SendGump(new DialogueGump(p, cheerfulModule));
                        });

                    storyModule.AddOption("Goodbye.", 
                        p => true,
                        p =>
                        {
                            DialogueModule farewellModule = new DialogueModule(
                                "Farewell, friend. May your journey ahead be as bright as the morning sun and as bold as the adventures we once knew."
                            );
                            p.SendGump(new DialogueGump(p, farewellModule));
                        });
                    player.SendGump(new DialogueGump(player, storyModule));
                });

            // Option 3: Say goodbye from the main greeting.
            greeting.AddOption("Goodbye, Darvin.", 
                player => true,
                player =>
                {
                    DialogueModule goodbyeModule = new DialogueModule(
                        "Take care, and remember—the door to this tavern is always open for friends, old and new alike!"
                    );
                    player.SendGump(new DialogueGump(player, goodbyeModule));
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
