using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Serra : BaseCreature
    {
        [Constructable]
        public Serra() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Serra the Coral Sculptor";
            Body = 0x191; // using a human body type

            // Stats
            SetStr(90);
            SetDex(80);
            SetInt(120);
            SetHits(100);

            // Appearance and Clothing
            AddItem(new Robe() { Hue = 1150, Name = "Flowing Seashell Robe" });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new SilverBracelet() { Hue = 1150, Name = "Coral Circlet" });

            // Aesthetic details: hints of seaspray and an otherworldly allure
            Hue = 1150;
            HairItemID = 0x203B; // creative style hair
            HairHue = 1150;
        }

        public Serra(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Serra, a sculptor of coral and keeper of secrets from the deep. My art—born amidst the icy whispers of the Ice Cavern—is but a veil for truths much darker. Tell me, what intrigues your soul today?");

            // Option: Ice Cavern experiences
            greeting.AddOption("Tell me more about your journeys to the Ice Cavern.", 
                player => true, 
                player =>
                {
                    DialogueModule iceCavernModule = new DialogueModule("The Ice Cavern is where light and shadow contend, where crystalline echoes guide the way. I have wandered its frozen halls many times, each visit etching a deeper mystery into my soul. Do you wish to hear of the marvels that captivate the eye—or the transformation that reshaped my very being?");
                    
                    iceCavernModule.AddOption("Speak of the majestic marvels.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule marvelsModule = new DialogueModule("In the cavern, ice forms natural cathedrals, and hidden glyphs pulse with life. I witnessed stalactites refracting auroras and heard the distant chanting of forgotten guardians. Those images still burn in my memory, urging my hand when I sculpt. Does this wondrous sight stir your inner imagination?");
                            
                            marvelsModule.AddOption("Indeed, the vision is stirring!", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule reflectionModule = new DialogueModule("It is as if each shard of ice was imbued with the sacred breath of the deep. Every glimmer tells a tale of creation, of divine madness. Such visions compel me to seek even greater truths in my work—truths that few dare to acknowledge.");
                                    plc.SendGump(new DialogueGump(plc, reflectionModule));
                                });
                            marvelsModule.AddOption("I sense there is more than mere beauty here...", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule subtleHintModule = new DialogueModule("You are perceptive, traveler. Beneath the ice lies not only wonder but also a calling—a divine spark that once set me on a path of zeal and revelation. But such matters I reveal only to those who truly listen.");
                                    plc.SendGump(new DialogueGump(plc, subtleHintModule));
                                });
                            pl.SendGump(new DialogueGump(pl, marvelsModule));
                        });
                    
                    iceCavernModule.AddOption("Reveal how the cavern transformed you.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule transformationModule = new DialogueModule("Before the cavern’s embrace, my art was simple, almost mundane. But in that frozen solitude, I encountered voices—whispers of a power older than time. They spoke of destiny and sacrifice, changing me forever. My hands began to shape not just coral, but a symbol of a greater calling.");
                            
                            transformationModule.AddOption("What greater calling?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule secretModule = new DialogueModule("There was a time when I served a deity shunned by many—a controversial force known only to a devoted few. My faith in this deity was absolute, even fanatical. The cavern amplified that vision, revealing that my art was to become a beacon for the lost and the fervent. I invite those of true spirit to join me in this sacred quest.");
                                    
                                    secretModule.AddOption("Tell me about this deity.", 
                                        plx => true, 
                                        plx =>
                                        {
                                            DialogueModule deityModule = new DialogueModule("I once worshipped the Deep Mother—a force as enigmatic as the abyss, whose dominion over life and decay remains disputed by common folk. Her wrath and benevolence intertwine, and only those with relentless zeal dare approach her mysteries.");
                                            plx.SendGump(new DialogueGump(plx, deityModule));
                                        });
                                    secretModule.AddOption("I am intrigued by your fanatic zeal.", 
                                        plx => true, 
                                        plx =>
                                        {
                                            DialogueModule zealModule = new DialogueModule("Your words spark warmth in this long-chilled heart. My zeal, my passion, is not mere madness—it is conviction. Conviction that the world is blind to the true essence of life, and only through embracing the forbidden can we unlock destiny. Will you open your mind to these forbidden truths?");
                                            
                                            zealModule.AddOption("I want to learn more about these truths.", 
                                                ply => true, 
                                                ply =>
                                                {
                                                    DialogueModule joinModule = new DialogueModule("Then know this: every piece I sculpt, every whisper from the deep, is a call to awaken. Follow me, and together we shall venture into realms where others fear to tread. Let the Deep Mother's vision guide you.");
                                                    ply.SendGump(new DialogueGump(ply, joinModule));
                                                });
                                            zealModule.AddOption("I need more time to consider your words.", 
                                                ply => true, 
                                                ply =>
                                                {
                                                    DialogueModule contemplateModule = new DialogueModule("Contemplate well, for the path I tread is steeped in both ecstasy and despair. But remember, the call of the divine cannot be silenced forever.");
                                                    ply.SendGump(new DialogueGump(ply, contemplateModule));
                                                });
                                            plx.SendGump(new DialogueGump(plx, zealModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, secretModule));
                                });
                            
                            transformationModule.AddOption("How did you overcome the cost of that transformation?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule costModule = new DialogueModule("Every revelation comes with its burden. The sacrifices I made on that fateful day in the cavern were immense—lost friendships, doubts, and the weight of divine expectation. Yet in that suffering, I found purpose. Do you understand that true art is forged through pain and passion?");
                                    plc.SendGump(new DialogueGump(plc, costModule));
                                });
                            pl.SendGump(new DialogueGump(pl, transformationModule));
                        });
                    player.SendGump(new DialogueGump(player, iceCavernModule));
                });

            // Option: Collaboration with Iris and elemental magic
            greeting.AddOption("How do you work with Iris to infuse your art with magic?", 
                player => true, 
                player =>
                {
                    DialogueModule irisModule = new DialogueModule("Ah, Iris of Devil Guard—my esteemed counterpart in the arcane arts. Together, we challenge the boundaries of natural magic. I mold the coral from the gifts of the sea, and Iris weaves elemental incantations into each piece. Would you like to hear the intricacies of this process, or perhaps learn about how our shared passion transcends the mundane?");
                    
                    irisModule.AddOption("Explain the process of magical infusion.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule processModule = new DialogueModule("It is a delicate ritual: I first select a coral shard that sings with latent energy. Then, under the watchful gaze of the moon, Iris murmurs forbidden chants—her voice a symphony that binds the elemental forces. This union of art and magic is both unpredictable and divine, filled with the raw potential of creation and destruction.");
                            
                            processModule.AddOption("What happens if the ritual fails?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule failureModule = new DialogueModule("Failure is but a step upon the path to mastery. In those turbulent moments, the raw elements rebel, yet they also offer lessons in power and control. We embrace these setbacks with fervor, each misstep a chance to refine our art. Such is the nature of magic—it is as wild and unyielding as the deity I once served.");
                                    plc.SendGump(new DialogueGump(plc, failureModule));
                                });
                            processModule.AddOption("And when it succeeds?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule successModule = new DialogueModule("Success manifests in a breathtaking fusion of light, water, and stone—a living testament to the deep magic that courses through our veins. It is a reminder that the universe favors the bold and the zealous, those unafraid to challenge its order. This triumph is shared by all who dare to dream.");
                                    plc.SendGump(new DialogueGump(plc, successModule));
                                });
                            pl.SendGump(new DialogueGump(pl, processModule));
                        });
                    
                    irisModule.AddOption("Tell me more about Iris herself.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule irisBioModule = new DialogueModule("Iris is more than just a mage; she is a visionary with a mind honed by arcane study and personal sacrifice. Her insights into the balance of elemental forces border on prophetic. Our debates are as passionate as they are scholarly. Sometimes, she reminds me that the flames of belief should never be doused, only stoked higher.");
                            pl.SendGump(new DialogueGump(pl, irisBioModule));
                        });
                    
                    irisModule.AddOption("Is there a hidden connection between your work with Iris and your secret past?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule hiddenModule = new DialogueModule("There are truths hidden even within the most sacred rituals. I once walked a shadowed path as a priest of a controversial deity—a zealotry that shaped my very destiny. My collaboration with Iris is a tribute to that past, a reminder that our magical pursuits are entwined with power, destiny, and—if one dares to listen—the call to a higher purpose.");
                            
                            hiddenModule.AddOption("Reveal more about this secret past.", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule secretPastModule = new DialogueModule("In the depths of my memory lies a chapter of fanatic devotion. I served the Deep Mother—an enigmatic deity whose blessings and curses are whispered in forbidden circles. Her tenets demand total surrender, a merging of soul and sacred art. My sculptures are not merely beauty; they are relics of that exalted, controversial faith. Dare you consider embracing such forbidden truth?");
                                    plc.SendGump(new DialogueGump(plc, secretPastModule));
                                });
                            hiddenModule.AddOption("I am not ready to hear such things.", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule reluctantModule = new DialogueModule("Very well, the secrets of the deep are not for everyone. Yet, remember—truths buried in passion can one day find their way to light.");
                                    plc.SendGump(new DialogueGump(plc, reluctantModule));
                                });
                            pl.SendGump(new DialogueGump(pl, hiddenModule));
                        });
                    player.SendGump(new DialogueGump(player, irisModule));
                });

            // Option: Confidante exchanges with Orwin and secret underwater lore
            greeting.AddOption("What secrets do you share with Orwin about the depths?", 
                player => true, 
                player =>
                {
                    DialogueModule orwinModule = new DialogueModule("Orwin of Renika is not just a diver; he is a sage of the ocean's mysteries. In our twilight meetings by the shore, he reveals legends of drowned kingdoms and the eternal song of the deep. Would you like to hear a secret tale, or learn the philosophy behind these submerged truths?");
                    
                    orwinModule.AddOption("Tell me a secret underwater tale.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule secretTaleModule = new DialogueModule("Deep in the abyss, Orwin once spoke of a luminous grotto where time stands still—a sanctum for those chosen by the ocean's spirit. I fashioned a sculpture from coral found near that site, infused with the memory of that spectral light. It is said that the very essence of the Deep Mother lingers within it, calling to those who seek enlightenment.");
                            
                            secretTaleModule.AddOption("What does that sanctum represent?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule sanctumModule = new DialogueModule("It represents a convergence of fate and faith—a place where the wild heart of the sea meets divine inspiration. For me, it is both a warning and a promise: that great power and great peril lie just beneath the surface.");
                                    plc.SendGump(new DialogueGump(plc, sanctumModule));
                                });
                            pl.SendGump(new DialogueGump(pl, secretTaleModule));
                        });
                    
                    orwinModule.AddOption("Describe your friendship with Orwin.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule friendshipModule = new DialogueModule("Orwin is my confidante, a mirror reflecting the hidden mysteries of the deep. Our bond runs deeper than mere camaraderie—it is founded upon mutual respect for forces that defy mortal understanding. Every whispered secret between us serves as a spark to ignite our shared destiny.");
                            
                            friendshipModule.AddOption("How has this bond shaped your art?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule artBondModule = new DialogueModule("Our exchanges have unlocked visions that permeate my every creation. It is not uncommon for me to infuse my sculptures with symbols gleaned from Orwin’s tales—a nod to an ancient ritual meant to awaken those destined for a higher calling.");
                                    plc.SendGump(new DialogueGump(plc, artBondModule));
                                });
                            pl.SendGump(new DialogueGump(pl, friendshipModule));
                        });
                    player.SendGump(new DialogueGump(player, orwinModule));
                });

            // Option: Broader inspirations and the persuasive side of her secret faith
            greeting.AddOption("What fuels your creative passion beyond the sea and ice?", 
                player => true, 
                player =>
                {
                    DialogueModule inspirationModule = new DialogueModule("Inspiration is a tide that rises from countless sources—the cosmic dance of stars above, the murmur of distant legends, and even the subtle call of forbidden faith. I once carried within me a zealous conviction—a sacred duty to propagate the truth of a forgotten deity. Would you like to hear how that passion drives my art, or how I persuade others to embrace this hidden path?");
                    
                    inspirationModule.AddOption("Explain how your passion drives your art.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule creativeProcessModule = new DialogueModule("My creative process is both a meditative ritual and an act of divine persuasion. I wander the shores at dusk, allowing the whispers of the waves to awaken memories of my past devotion. Each stroke of my chisel carries the fervor of my old faith, a legacy of the Deep Mother's enigmatic power. I sculpt not only for beauty but to kindle the sparks of revelation in others.");
                            
                            creativeProcessModule.AddOption("How do you channel this zeal?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule channelModule = new DialogueModule("I channel my zeal through prayer and practice—each piece of coral is a sacrament, and each sculpture a testament. In these moments, my words become incantations, persuasive appeals to the heart of anyone who listens. It is both art and a call to arms against the mundane.");
                                    plc.SendGump(new DialogueGump(plc, channelModule));
                                });
                            pl.SendGump(new DialogueGump(pl, creativeProcessModule));
                        });
                    
                    inspirationModule.AddOption("How do you persuade others to join your mysterious cause?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule persuadeModule = new DialogueModule("Ah, persuasion—the art of weaving truth with desire. My words are laced with the fervor of one who has glimpsed destiny. I speak of the Deep Mother, urging the worthy to shed their doubts and embrace a life of sacred purpose. I tell them that to follow this path is to ignite an eternal flame within, a beacon that shall guide us to a brighter, albeit controversial, destiny.");
                            
                            persuadeModule.AddOption("I find that very compelling. Tell me more.", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule compellingModule = new DialogueModule("When I speak, I invoke visions of ancient power—a tidal surge of passion and belief. I recount tales of miracles wrought through unyielding faith and challenge the listener to imagine a world reborn by divine insight. My words are a call to awaken, to reject complacency and join me in a quest that transcends mortal limitations.");
                                    plc.SendGump(new DialogueGump(plc, compellingModule));
                                });
                            persuadeModule.AddOption("I am not sure I can follow such a path.", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule doubtModule = new DialogueModule("Doubt is the anchor that holds us from soaring to destiny. Yet, I understand hesitation. Consider this a seed planted—a gentle reminder that once awakened, the soul craves illumination. If ever your heart desires more, you know where to find me.");
                                    plc.SendGump(new DialogueGump(plc, doubtModule));
                                });
                            pl.SendGump(new DialogueGump(pl, persuadeModule));
                        });
                    player.SendGump(new DialogueGump(player, inspirationModule));
                });
            
            // Final persuasive invitation (optional secret branch)
            greeting.AddOption("Is there a deeper secret you have not yet shared?", 
                player => true, 
                player =>
                {
                    DialogueModule secretInvitationModule = new DialogueModule("You have a keen eye, traveler. Beneath the veneer of coral and artistry, I conceal a truth of unfathomable depth: my past as a zealous priest of the Deep Mother, a deity both reviled and revered. This secret, wrapped in divine madness, offers a path to transcendence. Would you dare to learn the forbidden rites, and perhaps, embrace the calling for yourself?");
                    
                    secretInvitationModule.AddOption("Yes, I wish to learn the forbidden rites.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule forbiddenRiteModule = new DialogueModule("Then come closer and listen. The rites involve a sacred communion with the ocean’s pulse—a melding of spirit and salt. I shall guide you through the first step, so that you might glimpse the profound power of the Deep Mother's blessing. But be warned: this path is not for the faint-hearted, and its revelations are as perilous as they are liberating.");
                            pl.SendGump(new DialogueGump(pl, forbiddenRiteModule));
                        });
                    secretInvitationModule.AddOption("I must decline for now.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule declineModule = new DialogueModule("Very well. The currents of fate are ever-changing. Know that this secret remains, waiting for the day your heart is ready to break its chains. I will be here, as steadfast as the tides.");
                            pl.SendGump(new DialogueGump(pl, declineModule));
                        });
                    player.SendGump(new DialogueGump(player, secretInvitationModule));
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
