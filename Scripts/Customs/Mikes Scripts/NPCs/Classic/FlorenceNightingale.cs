using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Florence Nightingale")]
    public class FlorenceNightingale : BaseCreature
    {
        [Constructable]
        public FlorenceNightingale() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Florence Nightingale";
            Body = 0x191; // Human female body

            // Stats
            SetStr(80);
            SetDex(60);
            SetInt(100);
            SetHits(50);

            // Appearance
            AddItem(new Skirt() { Hue = 1908 });
            AddItem(new FancyShirt() { Hue = 1908 });
            AddItem(new Bonnet() { Hue = 1908 });
            AddItem(new Sandals() { Hue = 1908 });
            AddItem(new Crossbow() { Name = "Florence's Remedy" });

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

		public FlorenceNightingale(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("I am Florence Nightingale from the land of Canada. What brings you here, seeker?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("I am in perfect health, as the land itself heals my wounds.");
                    healthModule.AddOption("That's good to hear.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("I am a keeper of ancient wisdom and secrets.");
                    jobModule.AddOption("Can you share some wisdom?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule wisdomModule = new DialogueModule("True wisdom lies not in the answers, but in the questions. What queries do you bring, seeker?");
                            wisdomModule.AddOption("What mysteries do you speak of?",
                                p => true,
                                p =>
                                {
                                    DialogueModule mysteryModule = new DialogueModule("Your words dance like leaves on the wind. What say you about the mysteries of the night?");
                                    mysteryModule.AddOption("I wish to learn more.",
                                        pla => true,
                                        pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                    p.SendGump(new DialogueGump(p, mysteryModule));
                                });
                            pl.SendGump(new DialogueGump(pl, wisdomModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Tell me about Canada.",
                player => true,
                player =>
                {
                    DialogueModule canadaModule = new DialogueModule("Ah, Canada! A vast and wondrous land to the north. The crisp air and deep woods taught me much of nature's ways.");
                    canadaModule.AddOption("What is its history?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule historyModule = new DialogueModule("Canada's history is rich and complex, filled with tales of resilience and discovery. Would you like to hear about its early exploration, indigenous cultures, or perhaps the Confederation?");
                            historyModule.AddOption("Tell me about the indigenous cultures.",
                                p => true,
                                p =>
                                {
                                    DialogueModule indigenousModule = new DialogueModule("Before the Europeans arrived, Canada was home to many indigenous tribes, each with its own unique traditions, languages, and histories. The First Nations, Inuit, and Métis people have lived on this land for thousands of years.");
                                    indigenousModule.AddOption("What can you tell me about their traditions?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule traditionsModule = new DialogueModule("Indigenous cultures are diverse and rich in tradition. Many tribes celebrate seasonal events, have strong connections to nature, and use storytelling to pass down their history and wisdom.");
                                            traditionsModule.AddOption("What stories do they share?",
                                                plq => true,
                                                plq =>
                                                {
                                                    DialogueModule storiesModule = new DialogueModule("Stories of creation, nature, and heroism abound. One popular tale is that of the Trickster, a figure found in many cultures who teaches lessons through mischief and cleverness.");
                                                    storiesModule.AddOption("I'd love to hear more about these stories.",
                                                        pw => true,
                                                        pw =>
                                                        {
                                                            p.SendGump(new DialogueGump(p, storiesModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, storiesModule));
                                                });
                                            pla.SendGump(new DialogueGump(pla, traditionsModule));
                                        });
                                    p.SendGump(new DialogueGump(p, indigenousModule));
                                });
                            historyModule.AddOption("What about the early exploration?",
                                p => true,
                                p =>
                                {
                                    DialogueModule explorationModule = new DialogueModule("The land saw its first European explorers in the 15th century. Figures like John Cabot and Jacques Cartier paved the way for future settlement. Their journeys revealed the vast potential of this land.");
                                    explorationModule.AddOption("What did they discover?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule discoveriesModule = new DialogueModule("They found resources, such as furs and timber, and established trade routes with the indigenous peoples. The fur trade became a significant industry that shaped early Canadian society.");
                                            discoveriesModule.AddOption("What about the Confederation?",
                                                ple => true,
                                                ple =>
                                                {
                                                    DialogueModule confederationModule = new DialogueModule("In 1867, Canada became a self-governing dominion within the British Empire. This was a pivotal moment, marking the beginning of Canada as a nation. The initial provinces included Ontario, Quebec, New Brunswick, and Nova Scotia.");
                                                    confederationModule.AddOption("What led to this unification?",
                                                        pr => true,
                                                        pr =>
                                                        {
                                                            DialogueModule unificationModule = new DialogueModule("The desire for a united front against external threats and economic cooperation played crucial roles. The idea was to foster a stronger political entity that could support its provinces.");
                                                            unificationModule.AddOption("What challenges did they face?",
                                                                plat => true,
                                                                plat =>
                                                                {
                                                                    DialogueModule challengesModule = new DialogueModule("There were many challenges, including differing interests among provinces, debates over governance, and the inclusion of indigenous rights in the new framework.");
                                                                    challengesModule.AddOption("What about the role of women during this time?",
                                                                        ply => true,
                                                                        ply =>
                                                                        {
                                                                            DialogueModule womenModule = new DialogueModule("Women played essential roles in the development of Canada, often managing homes and businesses while men were away. Figures like Nellie McClung were early advocates for women's rights.");
                                                                            womenModule.AddOption("Tell me more about Nellie McClung.",
                                                                                pu => true,
                                                                                pu =>
                                                                                {
                                                                                    p.SendGump(new DialogueGump(p, womenModule));
                                                                                });
                                                                            pla.SendGump(new DialogueGump(pla, womenModule));
                                                                        });
                                                                    p.SendGump(new DialogueGump(p, challengesModule));
                                                                });
                                                            pla.SendGump(new DialogueGump(pla, unificationModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, confederationModule));
                                                });
                                            player.SendGump(new DialogueGump(player, discoveriesModule));
                                        });
                                    p.SendGump(new DialogueGump(p, explorationModule));
                                });
                            player.SendGump(new DialogueGump(player, historyModule));
                        });
                    canadaModule.AddOption("What secrets does this land hold?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule landModule = new DialogueModule("This land holds many secrets. It's alive, breathing, and watching. It's more than just the ground we walk on. If you are keen, it might reveal a secret to you.");
                            landModule.AddOption("I am eager to learn.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, landModule));
                        });
                    player.SendGump(new DialogueGump(player, canadaModule));
                });

            greeting.AddOption("Do you have any treasures?",
                player => true,
                player =>
                {
                    DialogueModule treasureModule = new DialogueModule("Your interest pleases me, seeker. Here, take this. It is a small token from my own collection. Use it wisely.");
                    treasureModule.AddOption("Thank you!",
                        pl => true,
                        pl =>
                        {
                            pl.AddToBackpack(new HeadSlotChangeDeed()); // Give the reward
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, treasureModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
