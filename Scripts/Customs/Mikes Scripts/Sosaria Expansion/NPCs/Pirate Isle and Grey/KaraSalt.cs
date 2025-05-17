using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Kara Salt")]
    public class KaraSalt : BaseCreature
    {
        [Constructable]
		public KaraSalt() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kara Salt";
            Body = 0x191; // Human body

            // Basic Stats
            SetStr(110);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance: A subtle nod to a life at sea and a hint of a dangerous past.
            AddItem(new FancyShirt() { Hue = 1150 });
            AddItem(new Skirt() { Hue = 750 });
            AddItem(new Boots() { Hue = 2350 });
            AddItem(new TricorneHat() { Hue = 1750, Name = "Pirate Hat" });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public KaraSalt(Serial serial) : base(serial) { }

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
                "Ahoy there, traveler! I’m Kara Salt—merchant by trade, adventurer by necessity, and if you listen close, a soul once scarred by the hunt. " +
                "Some say my tongue is sharper than the cutlasses I once wielded, and my secrets as deep as the abyssal trenches. " +
                "What is it that tickles your fancy today?");

            // Option 1: Seafaring Adventures & Trade Routes
            greeting.AddOption("Tell me about your seafaring adventures and perilous trade routes.",
                player => true,
                player =>
                {
                    DialogueModule seafaring = new DialogueModule(
                        "The sea is a fickle mistress, full of both wonder and treachery. I've sailed into territories where legends blur with reality—evading ferocious beasts and rival merchants alike. " +
                        "I recall once, as the waves roared like an angry crowd, a tentacled nightmare threatened to drag me under. " +
                        "Want to hear the tale of the kraken encounter or the secrets behind the hidden routes I've forged?");
                    
                    seafaring.AddOption("What happened when you encountered the kraken?",
                        p => true,
                        p =>
                        {
                            DialogueModule krakenEncounter = new DialogueModule(
                                "Picture it: a moonless night, a swirling maelstrom, and the monstrous kraken emerging from the deep. " +
                                "I danced on the deck with nerves of steel and a tongue laced with sarcasm—taunting the beast until its fury ebbed away. " +
                                "It was a moment when my quick reflexes and resourcefulness saved not just my life, but also cemented my reputation as someone who laughs in the face of danger.");
                            
                            krakenEncounter.AddOption("Your sarcastic quips really turned the tide?",
                                q => true,
                                q =>
                                {
                                    DialogueModule quipModule = new DialogueModule(
                                        "Indeed! I told that beast, 'If your aim were as poor as your manners, you'd be a harmless sea sponge!' " +
                                        "A little humor goes a long way when every second counts. It’s a lesson in wit and survival I carry with me to this day.");
                                    q.SendGump(new DialogueGump(q, quipModule));
                                });
                            p.SendGump(new DialogueGump(p, krakenEncounter));
                        });
                    
                    seafaring.AddOption("Explain more about these secret, treacherous trade routes.",
                        p => true,
                        p =>
                        {
                            DialogueModule tradeRoutes = new DialogueModule(
                                "Not all paths are marked on a map—I've uncovered coves and hidden channels where smugglers once thrived. " +
                                "These routes are woven from myths and steeped in danger, each one a labyrinth of opportunities and curses alike.");
                            
                            tradeRoutes.AddOption("Have you ever risked everything for a hidden treasure?",
                                q => true,
                                q =>
                                {
                                    DialogueModule treasureHunt = new DialogueModule(
                                        "Oh, many times! I chased a fabled galleon rumored to be packed with gold and relics. " +
                                        "Between rival pirates and mythical sea curses, every maneuver was a test of wits and daring. " +
                                        "I emerged victorious—though not without a few scars to remind me of the price of freedom.");
                                    
                                    treasureHunt.AddOption("What did that ordeal teach you?",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule lessonModule = new DialogueModule(
                                                "It taught me that fortune favors those who dare to defy destiny. " +
                                                "Resourcefulness, swift reflexes, and a caustic wit are the truest forms of armament—a lesson I learned amid the roar of crashing waves.");
                                            r.SendGump(new DialogueGump(r, lessonModule));
                                        });
                                    q.SendGump(new DialogueGump(q, treasureHunt));
                                });
                            p.SendGump(new DialogueGump(p, tradeRoutes));
                        });
                    player.SendGump(new DialogueGump(player, seafaring));
                });

            // Option 2: Forbidden Romance with the East Montor Merchant
            greeting.AddOption("I'd like to know about your secret romance with the merchant from East Montor.",
                player => true,
                player =>
                {
                    DialogueModule romance = new DialogueModule(
                        "Ah, romance—wild, forbidden, and as unpredictable as the tempest. " +
                        "I once indulged in clandestine trysts with a merchant whose eyes held both danger and desire. " +
                        "Our meetings, woven through smoky back-alleys of East Montor, were filled with whispered promises and razor-sharp repartee. " +
                        "Shall I tell you how our fated encounter unfolded or reveal the risks we faced behind closed doors?");
                    
                    romance.AddOption("How did you first meet your mysterious lover?",
                        p => true,
                        p =>
                        {
                            DialogueModule firstMeeting = new DialogueModule(
                                "It was at a bustling trade fair, when the air was thick with spices and ambition. " +
                                "Amid the chaos, our eyes locked—a fleeting instant that sparked a connection neither of us could ignore. " +
                                "I was there on business and survival, yet in that moment, the world seemed to stand still.");
                            
                            firstMeeting.AddOption("And what secrets passed between you then?",
                                q => true,
                                q =>
                                {
                                    DialogueModule secretExchange = new DialogueModule(
                                        "In hushed tones, we traded not just words but clues to forbidden deals, hidden routes, and even our deepest vulnerabilities. " +
                                        "I peppered our exchange with sarcastic jibes to hide the tenderness beneath—a delicate dance of wit and yearning.");
                                    q.SendGump(new DialogueGump(q, secretExchange));
                                });
                            p.SendGump(new DialogueGump(p, firstMeeting));
                        });
                    
                    romance.AddOption("What challenges do you both face in keeping your love alive?",
                        p => true,
                        p =>
                        {
                            DialogueModule challenges = new DialogueModule(
                                "Our love is a tightrope, balanced between the lure of wealth and the perils of enmity. " +
                                "Rival merchants conspire, and rivalries flare like uncontrolled fires. " +
                                "Yet, my past—those days hunting down scoundrels—taught me to remain ever alert and ready to vanish at a moment’s notice.");
                            
                            challenges.AddOption("How do you manage to outwit these threats?",
                                q => true,
                                q =>
                                {
                                    DialogueModule outwitModule = new DialogueModule(
                                        "With nerves of steel and a mind honed by danger, I use every trick in the book—sarcasm, subtlety, and sometimes, a daring escape. " +
                                        "Every threat is met with a counter-move that leaves my adversaries licking their wounds and questioning their own bravado.");
                                    q.SendGump(new DialogueGump(q, outwitModule));
                                });
                            p.SendGump(new DialogueGump(p, challenges));
                        });
                    player.SendGump(new DialogueGump(player, romance));
                });

            // Option 3: Deep Discussions with Marin and the Lore of the Deep
            greeting.AddOption("Speak to me about your meetings with Marin and the ancient maritime legends.",
                player => true,
                player =>
                {
                    DialogueModule marinLore = new DialogueModule(
                        "Marin, a venerable soul from Renika, is my confidant in deciphering the sea’s arcane tales. " +
                        "Together, we pore over timeworn maps and lost legends that blur reality and myth. " +
                        "Do you wish to hear about the mysterious spectral lighthouse or perhaps a tale of a cursed port?");
                    
                    marinLore.AddOption("Tell me about the spectral lighthouse.",
                        p => true,
                        p =>
                        {
                            DialogueModule lighthouse = new DialogueModule(
                                "Legend speaks of a lighthouse whose beam never falters—a beacon for lost souls. " +
                                "Marin tells me it’s haunted by the echoes of sailors who once longed for redemption. " +
                                "Its light is said to reveal truths hidden beneath layers of deceit and sorrow.");
                            lighthouse.AddOption("Does that light ever guide you?",
                                q => true,
                                q =>
                                {
                                    DialogueModule guideModule = new DialogueModule(
                                        "In moments of despair, I sometimes wonder if that unfaltering light mirrors the steadfast spirit I once had. " +
                                        "But then I smile—reminding myself that even the darkest nights yield to the promise of dawn.");
                                    q.SendGump(new DialogueGump(q, guideModule));
                                });
                            p.SendGump(new DialogueGump(p, lighthouse));
                        });
                    
                    marinLore.AddOption("What about the cursed port he mentioned?",
                        p => true,
                        p =>
                        {
                            DialogueModule portLegend = new DialogueModule(
                                "Ah, the cursed port—a place swallowed by time and tide. " +
                                "It is said that ships entering its mists never return, their crews forever bound to a watery purgatory. " +
                                "Marin believes a lost relic lies beneath its wreckage, a secret that could upend the balance of power among the merchant lords.");
                            portLegend.AddOption("Do you dare to seek such a relic?",
                                q => true,
                                q =>
                                {
                                    DialogueModule dareModule = new DialogueModule(
                                        "Daring? Perhaps. Foolhardy? Absolutely. " +
                                        "But life’s risks are the spice that seasons our existence, even if it means courting danger like an old friend.");
                                    q.SendGump(new DialogueGump(q, dareModule));
                                });
                            p.SendGump(new DialogueGump(p, portLegend));
                        });
                    player.SendGump(new DialogueGump(player, marinLore));
                });

            // Option 4: Secret Past as a Bounty Hunter
            greeting.AddOption("I've heard whispers that you were once a bounty hunter—did you really serve a powerful guild?",
                player => true,
                player =>
                {
                    DialogueModule bountyPast = new DialogueModule(
                        "Oh, so now you want to pry into the shadows of my past? " +
                        "It's true—I once hunted the worst of the worst for a powerful guild that promised glory but delivered chains. " +
                        "I learned quickly that no one likes to be shackled by orders, so I broke free using nothing but my wits, my reflexes, and a healthy dose of disdain.");
                    
                    bountyPast.AddOption("Tell me more about your days in that guild.",
                        p => true,
                        p =>
                        {
                            DialogueModule guildDays = new DialogueModule(
                                "Those were cutthroat days—tracking dangerous fugitives and executing contracts with precision. " +
                                "Every bounty was a test of both my mettle and my morals. But soon I saw that the guild was less about justice and more about profit and power plays.");
                            
                            guildDays.AddOption("What made you leave?",
                                q => true,
                                q =>
                                {
                                    DialogueModule leaveGuild = new DialogueModule(
                                        "One fateful night, as I witnessed the guild betray a trusted ally for mere coin, " +
                                        "I realized that loyalty bought with gold is a fool’s errand. " +
                                        "So, I left—with nothing but my sharp tongue and a burning need for independence.");
                                    leaveGuild.AddOption("That must have taken immense courage.",
                                        r => true,
                                        r =>
                                        {
                                            DialogueModule courage = new DialogueModule(
                                                "Courage, or stubborn defiance? Perhaps both. " +
                                                "I wear my choice like a badge of honor—even if it means laughing in the face of those who thought I was expendable.");
                                            r.SendGump(new DialogueGump(r, courage));
                                        });
                                    q.SendGump(new DialogueGump(q, leaveGuild));
                                });
                            
                            guildDays.AddOption("Did your bounty hunting skills help you on the high seas?",
                                q => true,
                                q =>
                                {
                                    DialogueModule bountyAtSea = new DialogueModule(
                                        "Absolutely. My days chasing down outlaws taught me to read danger like a map. " +
                                        "Whether dodging cannon fire or outsmarting pirates, every hard-won lesson made me ever more resourceful.");
                                    q.SendGump(new DialogueGump(q, bountyAtSea));
                                });
                            p.SendGump(new DialogueGump(p, guildDays));
                        });
                    
                    bountyPast.AddOption("How does that shadowy past influence you now?",
                        p => true,
                        p =>
                        {
                            DialogueModule influence = new DialogueModule(
                                "Every scar tells a story. My past as a bounty hunter honed my instincts, " +
                                "and now every decision is laced with a touch of cynicism and a refusal to bow to anyone's will. " +
                                "I’m not the same person I was—I've emerged smarter, sharper, and fiercely independent.");
                            influence.AddOption("It sounds like your past is both a curse and a blessing.",
                                q => true,
                                q =>
                                {
                                    DialogueModule balance = new DialogueModule(
                                        "Indeed, it's a bittersweet symphony—one that I play with a smirk and a swagger. " +
                                        "The lessons learned in darkness now light my way with a humor as dry as the salt on my skin.");
                                    q.SendGump(new DialogueGump(q, balance));
                                });
                            p.SendGump(new DialogueGump(p, influence));
                        });
                    
                    bountyPast.AddOption("Do you ever regret leaving that life behind?",
                        p => true,
                        p =>
                        {
                            DialogueModule regret = new DialogueModule(
                                "Regret? Ha! I’ve no time for regrets. " +
                                "The life of a bounty hunter was brutal and unforgiving. " +
                                "I left because I chose freedom over servitude, and that choice—however harsh—has made me who I am today.");
                            regret.AddOption("I admire your independence.",
                                q => true,
                                q =>
                                {
                                    DialogueModule admiration = new DialogueModule(
                                        "Thank you, traveler. Independence is worth more than a gilded cage any day. " +
                                        "And a witty remark is the best key to unlock the chains of oppression.");
                                    q.SendGump(new DialogueGump(q, admiration));
                                });
                            p.SendGump(new DialogueGump(p, regret));
                        });
                    
                    player.SendGump(new DialogueGump(player, bountyPast));
                });

            // Option 5: Personal Traits & Philosophy
            greeting.AddOption("What would you say defines your personality now?",
                player => true,
                player =>
                {
                    DialogueModule personality = new DialogueModule(
                        "My personality? I’m as sarcastic as I am resourceful, and as fiercely independent as the winds that drive a schooner. " +
                        "I choose to face each challenge with humor—a trait that transforms hardship into opportunity. " +
                        "Care to hear a tale that proves a well-placed barb can disarm even the most dangerous foes?");
                    
                    personality.AddOption("Give me an example of your sarcastic humor.",
                        p => true,
                        p =>
                        {
                            DialogueModule sarcasm = new DialogueModule(
                                "Once, a pompous rival claimed his wares were 'unmatched in all the lands.' " +
                                "I quipped, 'If mediocrity were a virtue, you'd be the patron saint.' " +
                                "That remark left him red in the face, a small victory etched in my memory.");
                            sarcasm.AddOption("That's brilliant!",
                                q => true,
                                q =>
                                {
                                    DialogueModule laugh = new DialogueModule(
                                        "Laughter, my friend, is the shield against life's brutal blows. " +
                                        "It proves that even when the odds are stacked, a quick wit can tilt the scales.");
                                    q.SendGump(new DialogueGump(q, laugh));
                                });
                            p.SendGump(new DialogueGump(p, sarcasm));
                        });
                    
                    personality.AddOption("How do these traits help you in a dangerous world?",
                        p => true,
                        p =>
                        {
                            DialogueModule survivalTraits = new DialogueModule(
                                "In a world where danger lurks like a squall on the horizon, my independence allows me to make swift, decisive choices. " +
                                "Resourcefulness transforms even the direst circumstances into opportunities for escape or advantage. " +
                                "And yes, sarcasm is my way of reminding both myself and my foes that I will never be subdued.");
                            survivalTraits.AddOption("Do you ever feel isolated by your independence?",
                                q => true,
                                q =>
                                {
                                    DialogueModule isolation = new DialogueModule(
                                        "At times, the solitude is palpable—but I'd rather walk alone than be tethered by the weight of false alliances. " +
                                        "My freedom is my compass, and every solitary step is a victory over conformity.");
                                    q.SendGump(new DialogueGump(q, isolation));
                                });
                            p.SendGump(new DialogueGump(p, survivalTraits));
                        });
                    player.SendGump(new DialogueGump(player, personality));
                });
            
            // Option 6: Farewell
            greeting.AddOption("I have nothing more to inquire. Farewell.",
                player => true,
                player =>
                {
                    DialogueModule farewell = new DialogueModule(
                        "Fair winds and calm seas, traveler. May your path be ever rich with stories of your own. Until we meet again, keep your wits about you.");
                    player.SendGump(new DialogueGump(player, farewell));
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
