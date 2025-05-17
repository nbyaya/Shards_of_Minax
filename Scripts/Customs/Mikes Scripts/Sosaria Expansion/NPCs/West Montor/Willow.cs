using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Willow : BaseCreature
    {
        [Constructable]
        public Willow() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Willow";
            Body = 0x191; // Human body

            // Set basic stats for her gentle yet determined character
            SetStr(80);
            SetDex(70);
            SetInt(90);
            SetHits(100);

            // Outfit: A modest dress and apron that belie a secret inner fire
            AddItem(new PlainDress() { Hue = 1370 });
            AddItem(new HalfApron() { Hue = 1370 });
            AddItem(new Shoes() { Hue = 1370 });
            // A satchel hinting at her role as a keeper of secrets
            AddItem(new Backpack() { Hue = 1350, Name = "Willow's Satchel" });
        }

        public Willow(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // The greeting reveals her friendly exterior—and hints at a deeper, more zealous nature.
            DialogueModule greeting = new DialogueModule("Greetings, dear traveler. I am Willow—the modest shopkeeper you see before you. Yet, beneath these well-worn robes lies a secret passion: I am also a devoted priest of the controversial deity Nycteris, the Veiled Flame. I spend my days trading rare goods and receiving mysterious packages, all while carrying the sacred word in my heart. What would you like to explore today?");
            
            // Option 1: Inquiry about the mysterious packages from Thistle.
            greeting.AddOption("Tell me about the mysterious packages from Thistle.", 
                player => true,
                player =>
                {
                    DialogueModule packagesModule = new DialogueModule("Ah, those enigmatic parcels arrive as if borne by the night itself. Thistle sends them wrapped in mysteries—a delicate interplay of commerce and divine symbolism. I recall a package draped in midnight silk, carrying a vial that glowed with an otherworldly light and a small key etched with ancient sigils. Tell me, do you sense that these items might be more than mere trade goods?");
                    
                    packagesModule.AddOption("They seem to carry divine marks—what do you mean by that?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule divineModule = new DialogueModule("You perceive well. Some shipments bear marks that echo the sacred texts of my order—symbols of Nycteris herself. These are not simply curiosities; they are subtle calls to the faithful. The scrolls and relics within hint at rites long forgotten. Would you like to learn more about these forbidden inscriptions?");
                            
                            divineModule.AddOption("Yes, I'd love to hear about these ancient texts.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule scrollsModule = new DialogueModule("These texts are inscribed in a language of passion and conviction—a language that only the devoted truly understand. They recount the fiery revelations of Nycteris, urging her followers to embrace the divine transformation. They speak of purification through flame and the renewal of the spirit. Do you believe such fervor can change a life?");
                                    
                                    scrollsModule.AddOption("I am intrigued—how might one follow this path?", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule followPathModule = new DialogueModule("The path is arduous but luminous. One must learn to decipher the sacred symbols and attend secret convocations held under starlight. In truth, I sometimes wonder if destiny has led you here. Would you consider attending a clandestine rite to glimpse the divine?");
                                            
                                            followPathModule.AddOption("Yes, I feel drawn to such mysteries.", 
                                                pl4 => true,
                                                pl4 =>
                                                {
                                                    DialogueModule meetingModule = new DialogueModule("Then your journey begins at the abandoned chapel near Devil Guard. Under the waning moon, a select few gather in reverence to Nycteris. Tread carefully—divine secrets can be as dangerous as they are enlightening.");
                                                    pl4.SendGump(new DialogueGump(pl4, meetingModule));
                                                });
                                            followPathModule.AddOption("No, I'll admire these mysteries from afar.", 
                                                pl4 => true,
                                                pl4 =>
                                                {
                                                    DialogueModule remainSafeModule = new DialogueModule("A cautious heart is not without merit. Yet remember, some truths shine only when embraced wholeheartedly.");
                                                    pl4.SendGump(new DialogueGump(pl4, remainSafeModule));
                                                });
                                            pl3.SendGump(new DialogueGump(pl3, followPathModule));
                                        });
                                    scrollsModule.AddOption("I fear such ancient powers might overwhelm the unprepared.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule cautionModule = new DialogueModule("A wise sentiment, though caution may sometimes mask the brilliance of revelation. Even the brightest flame can scorch if not respected. Perhaps one day, you will see its true beauty.");
                                            pl3.SendGump(new DialogueGump(pl3, cautionModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, scrollsModule));
                                });
                            divineModule.AddOption("I think these are just ornate collectibles meant for trade.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule dismissDivineModule = new DialogueModule("To the unobservant, they may appear so. But every artifact whispers of destiny, if one listens closely. Commerce and divinity are intertwined—sometimes in the most unexpected ways.");
                                    pl2.SendGump(new DialogueGump(pl2, dismissDivineModule));
                                });
                            pl.SendGump(new DialogueGump(pl, divineModule));
                        });
                    
                    packagesModule.AddOption("I suspect they’re simply lucrative items with an air of mystery.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule lucrativeModule = new DialogueModule("Even so, commerce is rarely devoid of purpose. In every traded item, there may lie a divine spark—an invitation to partake in something greater. Perhaps these shipments are part of a sacred pattern, a subtle orchestration of fate.");
                            
                            lucrativeModule.AddOption("That notion is fascinating. Could trade really be a ritual?", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule ritualModule = new DialogueModule("Indeed. Every deal, every exchange, is imbued with the potential for the transcendent. The mundane becomes holy when touched by destiny. Have you ever felt that chance and fortune are directed by unseen forces?");
                                    
                                    ritualModule.AddOption("Yes, I sometimes sense destiny in the everyday.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule destinyModule = new DialogueModule("Then perhaps you are more attuned than most. The interplay between profit and prophecy is subtle but profound. May the flame of fate guide you always.");
                                            pl3.SendGump(new DialogueGump(pl3, destinyModule));
                                        });
                                    ritualModule.AddOption("No, I believe in reason and logic alone.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule logicModule = new DialogueModule("A rational mind is noble indeed—but sometimes the most transformative truths lie beyond cold logic. Open your heart, and you might glimpse the divine.");
                                            pl3.SendGump(new DialogueGump(pl3, logicModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, ritualModule));
                                });
                            lucrativeModule.AddOption("I still think it’s just business with an exotic twist.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule businessModule = new DialogueModule("A pragmatic view, yet even the exotic must have a source. Sometimes, a rare trinket carries with it a spark of the sacred—a reminder that destiny is never far away.");
                                    pl2.SendGump(new DialogueGump(pl2, businessModule));
                                });
                            pl.SendGump(new DialogueGump(pl, lucrativeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, packagesModule));
                });

            // Option 2: Inquiry into trade news from Bryn at Dawn.
            greeting.AddOption("What's the latest trade news from Dawn, especially from Bryn?", 
                player => true,
                player =>
                {
                    DialogueModule tradeNewsModule = new DialogueModule("Dawn’s trade winds bring both opportunity and mystery. Bryn, the ever-cheerful baker, recounts news of exotic spices and enchanted harvests that seem touched by fate. Some even claim that the fluctuations in the market are guided by divine hands. Are you curious about the profit or the prophecy behind these trends?");
                    
                    tradeNewsModule.AddOption("I’m interested in the profit—explain these market trends.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule trendsModule = new DialogueModule("Markets have been unpredictable of late. The surge in rare herbs from West Montor and spice shipments from Dawn carries with it an ethereal charge—as if blessed, or perhaps cursed, by sacred energies. Do you see commerce as mere transactions, or as rituals of fate?");
                            
                            trendsModule.AddOption("I believe in the cosmic dance of trade.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule cosmicModule = new DialogueModule("Marvelous! In every negotiation and exchange, there exists a subtle interplay of destiny. One may even call it a ritual—a chance to harness the divine for personal gain. Would you like some advice on how to seize these opportunities?");
                                    
                                    cosmicModule.AddOption("Absolutely—share your wisdom on trading fate.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule wisdomModule = new DialogueModule("Listen well: Look beyond the price and quantity. Seek artifacts and rare ingredients that bear an aura of the sacred. Such items are the true currency of destiny—tools to unlock not just profit, but potential.");
                                            pl3.SendGump(new DialogueGump(pl3, wisdomModule));
                                        });
                                    cosmicModule.AddOption("No, I lean toward straightforward commerce.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule conventionalModule = new DialogueModule("Conventional wisdom has its merits, yet the divine often operates in mysterious ways—turning a modest profit into a fated fortune.");
                                            pl3.SendGump(new DialogueGump(pl3, conventionalModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, cosmicModule));
                                });
                            trendsModule.AddOption("I think it’s just market fluctuation.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule fluctuationModule = new DialogueModule("Perhaps so. But even fluctuation may be the subtle hand of fate. In every rise and fall, listen for the whisper of destiny.");
                                    pl2.SendGump(new DialogueGump(pl2, fluctuationModule));
                                });
                            pl.SendGump(new DialogueGump(pl, trendsModule));
                        });
                    
                    tradeNewsModule.AddOption("I should visit Dawn and speak with Bryn in person.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule visitModule = new DialogueModule("A fine idea! Bryn's bakery is not just a shop—it’s a wellspring of local lore and market insights. His latest ventures might even hint at celestial interventions. A visit may yield both profit and enlightenment.");
                            pl.SendGump(new DialogueGump(pl, visitModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, tradeNewsModule));
                });

            // Option 3: Gossip from Silas in Yew.
            greeting.AddOption("I hear you have intriguing gossip from Silas in Yew. Tell me more.", 
                player => true,
                player =>
                {
                    DialogueModule gossipModule = new DialogueModule("Oh, Silas is a font of secrets! His rambles speak of clandestine meetings, mysterious rituals, and strange omens near the ruins. Just recently, he described a midnight gathering where chants mixed with the rustle of ancient trees. Shall I recount his latest tale or elaborate on his own peculiar nature?");
                    
                    gossipModule.AddOption("Tell me his latest tale.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule accountModule = new DialogueModule("Silas reported that under a full moon, shadowed figures convened near the old mill. Their voices rose in fervent chant—a litany of hope and doom intertwined. Some say it was a prelude to a divine awakening; others, a signal of impending chaos. Do you find such prophecy compelling or unsettling?");
                            
                            accountModule.AddOption("I find such divine prophecy quite compelling.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule movementModule = new DialogueModule("Then perhaps you resonate with the call of destiny. Silas believes that these gatherings herald the rise of a new order—one destined to awaken the latent power within us all. Could you be among those chosen?");
                                    
                                    movementModule.AddOption("How might one be chosen?", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule chosenModule = new DialogueModule("The chosen are not marked by fate alone but by their willingness to see beyond the mundane. Their eyes must burn with the fire of conviction. Attend the secret rites, listen to the silent call, and perhaps the divine will reveal your destiny.");
                                            pl3.SendGump(new DialogueGump(pl3, chosenModule));
                                        });
                                    movementModule.AddOption("I remain uncertain about such destiny.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule uncertainModule = new DialogueModule("Uncertainty is natural in the face of the unknown. Yet, even the faintest ember of faith can kindle a blaze of transformation. Keep your heart open, and you may yet perceive the signs.");
                                            pl3.SendGump(new DialogueGump(pl3, uncertainModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, movementModule));
                                });
                            accountModule.AddOption("I think it’s all just overblown rumor.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule dismissModule = new DialogueModule("Perhaps to some. But remember, rumors often carry a seed of truth. The chaotic harmony of nature occasionally whispers of a larger, more profound design.");
                                    pl2.SendGump(new DialogueGump(pl2, dismissModule));
                                });
                            pl.SendGump(new DialogueGump(pl, accountModule));
                        });
                    
                    gossipModule.AddOption("Tell me more about Silas himself.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule silasInfoModule = new DialogueModule("Silas, that ever-watchful wanderer of Yew, is a man of contradictions—equal parts gossip and prophet. His rants are steeped in passion, and while some dismiss him as loony, I believe his words are laced with a hidden truth. Do you find his passion inspiring?");
                            
                            silasInfoModule.AddOption("Yes, his passion stokes my curiosity.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule passionModule = new DialogueModule("Indeed, passion can be a harbinger of change. Silas’s fervor for mystery and his uncanny insights into local omens reveal a soul not content with mediocrity. Perhaps his chaotic visions mirror the divine spark in us all.");
                                    pl2.SendGump(new DialogueGump(pl2, passionModule));
                                });
                            silasInfoModule.AddOption("Not really, I prefer solid facts over wild theories.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule factsModule = new DialogueModule("A prudent stance, yet even facts can be transformed by the power of belief. Perhaps the truth lies somewhere between order and chaos.");
                                    pl2.SendGump(new DialogueGump(pl2, factsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, silasInfoModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, gossipModule));
                });

            // Option 4: Browse the shop.
            greeting.AddOption("I don't need anything right now; I'll just browse your shop.", 
                player => true,
                player =>
                {
                    DialogueModule shopModule = new DialogueModule("Certainly, take your time. My shelves brim with rare goods, mystical trinkets, and relics steeped in history—and perhaps a touch of divine mystery. If you require counsel or wish to hear another tale, you know where I reside.");
                    player.SendGump(new DialogueGump(player, shopModule));
                });

            // Option 5: Reveal Secret True Calling
            greeting.AddOption("You carry an unusual aura… reveal your true self. Are you more than just a merchant?", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = new DialogueModule("So you’ve sensed the flicker of truth beneath these humble garments. I am not merely Willow the shopkeeper—I am a zealous priest of Nycteris, the Veiled Flame. My life is a sacred mission; I strive to awaken others to the divine fire that many fear to acknowledge. This calling, controversial as it is, drives me to be both persuasive and, some say, fanatic in my devotion. Would you care to learn more of this hidden path?");
                    
                    secretModule.AddOption("Yes, I wish to know about Nycteris and your sacred mission.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule nycterisModule = new DialogueModule("Nycteris is a deity shrouded in paradox. To the uninitiated, she appears as a harbinger of chaos, yet to her true followers, she is the purifier—a force that burns away corruption. I was touched by her divine light in my darkest hour. It transformed me, and now I serve her with every fiber of my being. Do you find such a calling compelling?");
                            
                            nycterisModule.AddOption("Indeed, I feel drawn to such fervor.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule originModule = new DialogueModule("My conversion was not gentle. In a moment of despair, I was visited by a vision—a roaring flame amidst darkness, a sign that shattered my old life. That day, I renounced a life of mediocrity and embraced a destiny of zeal. I invite you, should you wish, to walk a similar path—one of divine revelation and personal rebirth.");
                                    
                                    originModule.AddOption("How might I begin such a journey?", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule journeyModule = new DialogueModule("Begin by questioning the world around you. Seek out the forgotten places of worship, the crumbling altars hidden in plain sight. Attend our secret gatherings at the old chapel near Devil Guard. There, you will hear the sacred hymns of Nycteris and learn the rites that purify the soul. The path is arduous, but it illuminates the darkness within.");
                                            pl3.SendGump(new DialogueGump(pl3, journeyModule));
                                        });
                                    originModule.AddOption("I need time to ponder such a transformation.", 
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule ponderModule = new DialogueModule("The divine is not rushed; it unfolds in its own time. Ponder, meditate, and when your heart is ready, the flame will guide you. Until then, remember: every encounter holds the potential for a sacred awakening.");
                                            pl3.SendGump(new DialogueGump(pl3, ponderModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, originModule));
                                });
                            nycterisModule.AddOption("I remain skeptical of such intense devotion.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule skepticModule = new DialogueModule("Skepticism is the companion of the unenlightened. Yet even those who doubt may one day feel the pull of something greater—a fire that transforms doubt into conviction. Keep your eyes open; the divine often reveals itself when least expected.");
                                    pl2.SendGump(new DialogueGump(pl2, skepticModule));
                                });
                            pl.SendGump(new DialogueGump(pl, nycterisModule));
                        });
                    
                    secretModule.AddOption("No, such matters are best kept hidden.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule veilModule = new DialogueModule("Very well. Some truths are reserved for the chosen few. Remember, though, that even in silence, the sacred flame of Nycteris burns eternal. Should you ever wish to glimpse its light, you know where to find me.");
                            pl.SendGump(new DialogueGump(pl, veilModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, secretModule));
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
