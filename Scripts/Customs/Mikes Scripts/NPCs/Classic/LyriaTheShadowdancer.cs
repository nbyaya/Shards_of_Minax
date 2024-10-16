using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Shadowdancer")]
    public class LyriaTheShadowdancer : BaseCreature
    {
        [Constructable]
        public LyriaTheShadowdancer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Shadowdancer";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 110;
            Int = 80;
            Hits = 75;

            // Appearance
            AddItem(new ClothNinjaHood() { Hue = 1201 });
			AddItem(new Robe() { Hue = 2502 });
            AddItem(new Dagger() { Name = "Lyria's Dirk" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
        }

        public LyriaTheShadowdancer(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Lyria the Shadowdancer, a wretched creature of darkness!");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => {
                    DialogueModule healthModule = new DialogueModule("My existence is a torment, but I am untouched by mortal ailments.");
                    healthModule.AddOption("Why do you say that?",
                        p => true,
                        p => {
                            DialogueModule reasonModule = new DialogueModule("Life has been nothing but a chain of miseries. But amidst the pain, the shadows sometimes reveal a glimmer of hope.");
                            reasonModule.AddOption("What glimmer of hope?",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Hope is fleeting, but it can guide the lost to their true path.")));
                                });
                            p.SendGump(new DialogueGump(p, reasonModule));
                        });
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player => {
                    DialogueModule jobModule = new DialogueModule("My 'job' is to dance in the shadows, forever hidden from the world.");
                    jobModule.AddOption("Dance in shadows? What do you mean?",
                        pl => true,
                        pl => {
                            DialogueModule danceModule = new DialogueModule("I embrace the darkness and use it to mask my presence. It's a form of power and a curse.");
                            danceModule.AddOption("Can you teach me this power?",
                                p => true,
                                p => {
                                    DialogueModule teachModule = new DialogueModule("The art of shadow dancing requires great discipline and understanding of one's inner darkness. Few can master it.");
                                    teachModule.AddOption("I'm willing to learn.",
                                        pla => true,
                                        pla => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Then seek knowledge and confront your fears, and perhaps you will uncover the shadows within.")));
                                        });
                                    p.SendGump(new DialogueGump(p, teachModule));
                                });
                            pl.SendGump(new DialogueGump(pl, danceModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Why are you miserable?",
                player => true,
                player => {
                    DialogueModule miserableModule = new DialogueModule("Do you dare to ask me about my miserable existence, or are you just wasting my time?");
                    miserableModule.AddOption("I am genuinely curious.",
                        pl => true,
                        pl => {
                            DialogueModule curiosityModule = new DialogueModule("Very well. My past is filled with pain and regret. I chose the shadows, but it came at a cost.");
                            curiosityModule.AddOption("What cost?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I lost my connection to the light and the warmth of companionship. Shadows are my only allies now.")));
                                });
                            pl.SendGump(new DialogueGump(pl, curiosityModule));
                        });
                    player.SendGump(new DialogueGump(player, miserableModule));
                });

            greeting.AddOption("Tell me about the Shadowdancers.",
                player => true,
                player => {
                    DialogueModule shadowdancerModule = new DialogueModule("Ah, you've heard of the Shadowdancers? We harness the power of shadows to our will.");
                    shadowdancerModule.AddOption("How do you harness this power?",
                        pl => true,
                        pl => {
                            DialogueModule harnessModule = new DialogueModule("It requires focus and an understanding of the darkness within. We practice rituals to strengthen our bond with the shadows.");
                            harnessModule.AddOption("What kind of rituals?",
                                p => true,
                                p => {
                                    DialogueModule ritualModule = new DialogueModule("We perform various rites under the cover of night, invoking the power of the shadows to protect and empower us.");
                                    ritualModule.AddOption("Can I join your rituals?",
                                        pls => true,
                                        pls => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Joining us is not for the faint-hearted. You must prove your worthiness.")));
                                        });
                                    p.SendGump(new DialogueGump(p, ritualModule));
                                });
                            pl.SendGump(new DialogueGump(pl, harnessModule));
                        });
                    player.SendGump(new DialogueGump(player, shadowdancerModule));
                });

            greeting.AddOption("What about the shadows?",
                player => true,
                player => {
                    DialogueModule shadowsModule = new DialogueModule("The shadows whisper secrets to those who know how to listen.");
                    shadowsModule.AddOption("What secrets do they whisper?",
                        pl => true,
                        pl => {
                            DialogueModule secretModule = new DialogueModule("They reveal truths hidden from the light, guiding those who seek knowledge in dark times.");
                            secretModule.AddOption("Can you share a secret with me?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("One must be cautious; some secrets are burdensome to bear.")));
                                });
                            pl.SendGump(new DialogueGump(pl, secretModule));
                        });
                    player.SendGump(new DialogueGump(player, shadowsModule));
                });

            greeting.AddOption("What is hope to you?",
                player => true,
                player => {
                    DialogueModule hopeModule = new DialogueModule("Hope is but a fleeting moment in the vast expanse of eternity.");
                    hopeModule.AddOption("Why is it fleeting?",
                        pl => true,
                        pl => {
                            DialogueModule fleetingModule = new DialogueModule("Because life is filled with suffering and despair, yet it is the hope that keeps us moving forward.");
                            fleetingModule.AddOption("How can we nurture hope?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("By finding joy in small moments, even amidst darkness.")));
                                });
                            pl.SendGump(new DialogueGump(pl, fleetingModule));
                        });
                    player.SendGump(new DialogueGump(player, hopeModule));
                });

            greeting.AddOption("What do you know about the Night?",
                player => true,
                player => {
                    DialogueModule nightModule = new DialogueModule("The Night holds mysteries, dreams, and the essence of what the Shadowdancers represent.");
                    nightModule.AddOption("What kind of mysteries?",
                        pl => true,
                        pl => {
                            DialogueModule mysteriesModule = new DialogueModule("Mysteries of life, death, and the balance between light and dark. Each night, new secrets unfold.");
                            mysteriesModule.AddOption("Can you reveal one of these mysteries?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("To know the mystery is to understand its weight. Choose wisely.")));
                                });
                            pl.SendGump(new DialogueGump(pl, mysteriesModule));
                        });
                    player.SendGump(new DialogueGump(player, nightModule));
                });

            greeting.AddOption("What are those forbidden texts?",
                player => true,
                player => {
                    DialogueModule forbiddenModule = new DialogueModule("Those forbidden texts are not meant for the likes of common folk. They carry power and danger in equal measure.");
                    forbiddenModule.AddOption("What kind of danger?",
                        pl => true,
                        pl => {
                            DialogueModule dangerModule = new DialogueModule("The knowledge contained within can twist the mind and lead to dark paths. Not all who seek power are prepared for its consequences.");
                            dangerModule.AddOption("What if I seek that power?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Then you must be ready to face the darkness within yourself.")));
                                });
                            pl.SendGump(new DialogueGump(pl, dangerModule));
                        });
                    player.SendGump(new DialogueGump(player, forbiddenModule));
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
