using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the withered remains of Old Bramble")]
    public class OldBramble : BaseCreature
    {
        [Constructable]
        public OldBramble() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Old Bramble";
            Body = 0x190; // Human-like body
            // Stats reflecting a wizened, nature-bound sage
            SetStr(80);
            SetDex(50);
            SetInt(90);
            SetHits(70);

            // Appearance: earthy, rustic, and marked by time
            AddItem(new Robe() { Hue = 1100 });
            AddItem(new LeatherCap() { Hue = 1100, Name = "Tattered Straw Hat" });
            AddItem(new Sandals() { Hue = 1100 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public OldBramble(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greeting = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greeting));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Ah, greetings, young traveler. I am Old Bramble, keeper of orchard lore and chronicler of ancient folktales. " +
                "I have spent many a year beneath the boughs of these venerable trees, learning the secrets of the soil and the stories " +
                "whispered by the wind. What wisdom or tale may I share with you today?"
            );

            // Option 1: Orchard Wisdom
            greeting.AddOption("Tell me about the orchard wisdom you share with Merrin the Gardener.",
                player => true,
                player =>
                {
                    DialogueModule orchardModule = new DialogueModule(
                        "Ah, the orchards! They are more than just groves of fruit-bearing trees—they are living chronicles of our land. " +
                        "I have nurtured these trees since my youth, learning their subtle language and the hidden rhythms of nature. " +
                        "Merrin, the diligent Gardener from Castle British & Britain, often visits to trade his bountiful harvest " +
                        "and exchange techniques. Together, we debate the secret methods that coax life from even the most stubborn soil. " +
                        "Would you care to learn about the rare fruit trees or the mystical herbs that awaken only under the full moon?"
                    );

                    orchardModule.AddOption("Tell me about the rare fruit trees.",
                        p => true,
                        p =>
                        {
                            DialogueModule fruitModule = new DialogueModule(
                                "These rare fruit trees are steeped in legend. It is said that their fruits possess restorative powers—one taste " +
                                "might mend a weary soul or restore lost vigor. Merrin and I have long argued over the precise moment when these " +
                                "magical fruits ripen, for it is not merely the season but the alignment of nature’s forces that heralds their bloom. " +
                                "Would you like to hear the lore behind them, or learn of the cultivation techniques passed down through generations?"
                            );

                            fruitModule.AddOption("Tell me the lore behind them.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule loreModule = new DialogueModule(
                                        "The lore speaks of an ancient covenant between the earth and the celestial spirits. " +
                                        "Our ancestors believed that the trees were a gift from the moon itself—a token of hope and renewal. " +
                                        "Merrin, ever the modern sage, cherishes these tales as much as I do. Do you find such enchantments " +
                                        "a delightful mystery, or are you a skeptic of old wives’ tales?"
                                    );
                                    loreModule.AddOption("I find the magic of nature enchanting!",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    loreModule.AddOption("I'm skeptical, but the mystery intrigues me.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, loreModule));
                                });

                            fruitModule.AddOption("Explain the cultivation methods.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule cultivationModule = new DialogueModule(
                                        "Cultivating these trees is an art—a delicate balance of timing, care, and a whisper of magic. " +
                                        "Our techniques, refined over many harvests, involve speaking softly to the saplings, nurturing them with " +
                                        "special elixirs, and even playing gentle melodies at dawn. Merrin has incorporated some modern twists, " +
                                        "yet the essence remains unchanged. Does this blend of tradition and innovation not captivate your curiosity?"
                                    );
                                    cultivationModule.AddOption("Indeed, it is a fascinating blend.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    cultivationModule.AddOption("I prefer more conventional methods, I suppose.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, cultivationModule));
                                });

                            p.SendGump(new DialogueGump(p, fruitModule));
                        });

                    orchardModule.AddOption("Tell me about the magical herbs that bloom under the full moon.",
                        p => true,
                        p =>
                        {
                            DialogueModule herbsModule = new DialogueModule(
                                "Under the silver light of the full moon, the land reveals secrets hidden in plain sight. " +
                                "There grow the 'Lunar Leaves'—herbs that only unfurl their mystic beauty when bathed in moonlight. " +
                                "Their petals, shimmering with an ethereal glow, are said to brew potions of great healing and clarity. " +
                                "Merrin and I have long cherished these herbs, ensuring their legacy endures. Would you like to learn " +
                                "about a specific herb or the legend that shrouds them?"
                            );

                            herbsModule.AddOption("Describe one herb and its effects.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule specificHerbModule = new DialogueModule(
                                        "One such marvel is the Moonlit Marigold. Its delicate petals capture the light of the moon, " +
                                        "and a brew made from them can soothe pain and ease troubled minds. Merrin swears by its restorative " +
                                        "powers, especially during the harsh chill of winter. Does the thought of such a remedy stir your interest?"
                                    );
                                    specificHerbModule.AddOption("Absolutely, nature's remedies are wondrous.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    specificHerbModule.AddOption("I lean toward more traditional medicines.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, specificHerbModule));
                                });

                            herbsModule.AddOption("Tell me the legend behind these herbs.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule legendHerbModule = new DialogueModule(
                                        "The legend recounts a time when the moon descended to bless the earth, gifting our ancestors with " +
                                        "the secret of these luminous herbs. In gratitude, the people nurtured them, and over time, their " +
                                        "miraculous properties became the stuff of folklore. Merrin often recites these tales during harvest feasts, " +
                                        "reminding us that nature's miracles are as enduring as time itself."
                                    );
                                    legendHerbModule.AddOption("A truly timeless tale.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    legendHerbModule.AddOption("I wonder how much truth is woven into myth.",
                                        plq => true,
                                        plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, legendHerbModule));
                                });

                            p.SendGump(new DialogueGump(p, herbsModule));
                        });

                    player.SendGump(new DialogueGump(player, orchardModule));
                });

            // Option 2: Folktales and Local History
            greeting.AddOption("Share with me one of the folktales you relay to Jasper the Scribe.",
                player => true,
                player =>
                {
                    DialogueModule folktaleModule = new DialogueModule(
                        "Ah, Jasper the Scribe has an ear for the whispers of the past. Let me recount a cherished folktale: " +
                        "Long ago, under a moonless sky, a wandering spirit graced our orchards with a prophecy of both despair and hope. " +
                        "Its voice was carried by the rustling leaves, urging our forebears to cherish the land and its hidden secrets. " +
                        "Jasper recorded every detail, ensuring that the memory of that fateful night endures. Would you like to " +
                        "hear how this tale interweaves with our local history, or learn of the moral it imparts?"
                    );

                    folktaleModule.AddOption("Tell me how the legend weaves into local history.",
                        p => true,
                        p =>
                        {
                            DialogueModule historyModule = new DialogueModule(
                                "In a time when our lands were wild and unyielding, this very prophecy spurred a band of brave souls " +
                                "to rescue a dying orchard from an unrelenting blight. Their courage, etched into the annals of our history, " +
                                "was chronicled by Jasper so that even in our darkest hours, we remember that hope can blossom against all odds."
                            );
                            historyModule.AddOption("A moving testament to resilience.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            historyModule.AddOption("It’s remarkable how myth and history converge.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, historyModule));
                        });

                    folktaleModule.AddOption("What moral does this tale impart?",
                        p => true,
                        p =>
                        {
                            DialogueModule moralModule = new DialogueModule(
                                "The essence of the tale is a reminder to honor our bond with the land and the voices of those who came before us. " +
                                "It speaks of balance, respect, and the enduring cycle of renewal—lessons that Jasper carefully pens down for future generations."
                            );
                            moralModule.AddOption("I shall carry this wisdom with me.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            moralModule.AddOption("Indeed, such truths are timeless.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, moralModule));
                        });

                    player.SendGump(new DialogueGump(player, folktaleModule));
                });

            // Option 3: Life Story
            greeting.AddOption("Tell me about your life, Old Bramble. How did you become the keeper of such lore?",
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule(
                        "My life, dear traveler, is as winding as the ancient paths through these orchards. " +
                        "Born beneath the shelter of mighty oaks, I learned early to listen to the murmur of the wind and the rustle of leaves. " +
                        "Through decades of wandering, observing, and recording, I gathered the stories of our land. " +
                        "My encounters with Merrin—whose modern insights keep the orchards thriving—and with Jasper, " +
                        "who immortalizes our history in ink and parchment, have all guided me to this venerable station. " +
                        "Would you like to hear about my early wanderings, or the defining moments that shaped my journey?"
                    );

                    lifeModule.AddOption("Tell me about your early wanderings.",
                        p => true,
                        p =>
                        {
                            DialogueModule earlyDaysModule = new DialogueModule(
                                "In the days of my youth, I roamed far beyond the familiar groves, seeking the hidden glades " +
                                "where nature’s secrets lay buried. Every rustle, every whisper, was a lesson—a gentle urging to " +
                                "trust in the timeless rhythms of life. Those were days of wonder, when the land itself spoke to me."
                            );
                            earlyDaysModule.AddOption("That sounds truly magical.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            earlyDaysModule.AddOption("I wish I could have witnessed such mysteries.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, earlyDaysModule));
                        });

                    lifeModule.AddOption("What were the defining moments of your journey?",
                        p => true,
                        p =>
                        {
                            DialogueModule definingModule = new DialogueModule(
                                "One defining moment was the day I met Merrin—a youthful, passionate soul whose ideas rekindled my own " +
                                "dormant fervor for nature’s wonders. Another was when a wandering bard’s tale led me to cross paths with Jasper, " +
                                "ensuring that every secret of our land would be remembered. Each encounter, each shared story, has been a " +
                                "vital stitch in the tapestry of my life."
                            );
                            definingModule.AddOption("Every encounter does shape us, indeed.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            definingModule.AddOption("I find your journey as inspiring as it is mysterious.",
                                pl => true,
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, definingModule));
                        });

                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // Option 4: Farewell
            greeting.AddOption("I must take my leave now. Farewell, Old Bramble.",
                player => true,
                player => player.SendMessage("Old Bramble smiles gently, his eyes reflecting decades of wisdom, as you depart.")
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
