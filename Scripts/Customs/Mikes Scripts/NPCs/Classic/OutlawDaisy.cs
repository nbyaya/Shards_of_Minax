using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Outlaw Daisy")]
    public class OutlawDaisy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OutlawDaisy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Outlaw Daisy";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 105;
            Int = 70;
            Hits = 70;

            // Appearance
            AddItem(new LeatherSkirt() { Hue = 1178 });
            AddItem(new FemaleLeatherChest() { Hue = 1178 });
            AddItem(new Bandana() { Hue = 1185 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new Dagger() { Name = "Daisy's Dagger" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // true for female
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public OutlawDaisy(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Howdy, stranger! They call me Outlaw Daisy. I reckon life’s too short to waste on what comes after. How can I help you live your best life right now?");
            
            greeting.AddOption("What do you mean by that?",
                player => true,
                player =>
                {
                    DialogueModule beliefModule = new DialogueModule("You see, I don't believe in an afterlife. When it's over, it's over. So why not enjoy the here and now, right? Life's a gamble, and I intend to cash in all my chips!");
                    beliefModule.AddOption("So you're saying you want to live in luxury?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule luxuryModule = new DialogueModule("Absolutely! Luxury isn't just a want; it’s a need. I want silk sheets, rare wines, and the finest jewelry. If I have to step on a few toes to get it, so be it!");
                            luxuryModule.AddOption("How do you plan to acquire all that?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule methodModule = new DialogueModule("Oh, you know... a bit of charm, a bit of cunning, and perhaps a dash of mischief. A lady has to be resourceful in this world.");
                                    methodModule.AddOption("Have you ever gotten in trouble for that?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule troubleModule = new DialogueModule("Trouble? Ha! If you’re not causing a little chaos, you’re not living. I’ve had my run-ins with the law, but it just adds to my reputation.");
                                            troubleModule.AddOption("Sounds risky!",
                                                p => true,
                                                p => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, troubleModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, methodModule));
                                });
                            luxuryModule.AddOption("What’s the most extravagant thing you own?",
                                plw => true,
                                plw =>
                                {
                                    DialogueModule extravaganceModule = new DialogueModule("My prized possession? A diamond necklace I... liberated from a particularly snooty noble. It sparkles like the stars, and it’s a constant reminder to live life lavishly!");
                                    extravaganceModule.AddOption("Do you ever feel guilty about that?",
                                        p => true,
                                        p => {
                                            DialogueModule guiltModule = new DialogueModule("Guilt? Not in the slightest! If they had the guts to flaunt their wealth, they should be prepared to lose it. Besides, I make it look good!");
                                            guiltModule.AddOption("You’re quite the character.",
                                                ple => true,
                                                ple => {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, guiltModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, extravaganceModule));
                                });
                            player.SendGump(new DialogueGump(player, luxuryModule));
                        });
                    player.SendGump(new DialogueGump(player, beliefModule));
                });

            greeting.AddOption("What do you think happens after death?",
                player => true,
                player =>
                {
                    DialogueModule afterlifeModule = new DialogueModule("Honestly? I think nothing happens. Just darkness. So why waste my life worrying about what comes next? I'd rather savor each moment.");
                    afterlifeModule.AddOption("That’s a bold stance.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, afterlifeModule));
                });

            greeting.AddOption("What’s your favorite luxury?",
                player => true,
                player =>
                {
                    DialogueModule favoriteLuxuryModule = new DialogueModule("Oh, that’s easy! A fine bottle of wine, perhaps a rare vintage that dances on your tongue. Pair that with a lavish feast, and you've got my heart!");
                    favoriteLuxuryModule.AddOption("What do you enjoy feasting on?",
                        pl => true,
                        pl => {
                            DialogueModule feastModule = new DialogueModule("Anything that tempts the palate! Roasted meats, fresh fruits, and decadent desserts. Life is too short to skimp on good food!");
                            feastModule.AddOption("I can agree with that.",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, feastModule));
                        });
                    player.SendGump(new DialogueGump(player, favoriteLuxuryModule));
                });

            greeting.AddOption("Do you have any tales of your exploits?",
                player => true,
                player =>
                {
                    DialogueModule talesModule = new DialogueModule("Oh, I’ve got plenty! Like the time I snuck into a high-society gala and left with a king’s ransom in jewelry. I still chuckle thinking about it!");
                    talesModule.AddOption("Tell me the details!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule detailModule = new DialogueModule("Picture this: I was dressed to the nines, charm oozing from every pore. I mingled, danced, and when they least expected it, I made my move!");
                            detailModule.AddOption("What happened next?",
                                p => true,
                                p => {
                                    DialogueModule climaxModule = new DialogueModule("I slipped out the back with a pouch full of gems. The thrill of the heist was exhilarating! And that’s what life is all about, isn’t it?");
                                    climaxModule.AddOption("Sounds like quite the adventure!",
                                        plr => true,
                                        plr => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, climaxModule));
                                });
                            pl.SendGump(new DialogueGump(pl, detailModule));
                        });
                    player.SendGump(new DialogueGump(player, talesModule));
                });

            greeting.AddOption("Is there a line you won't cross?",
                player => true,
                player =>
                {
                    DialogueModule lineModule = new DialogueModule("Hmm... that's a tough one. I believe in survival and luxury above all else. But I have my code; no harm to innocents, at least not directly.");
                    lineModule.AddOption("What if they get in your way?",
                        pl => true,
                        pl => {
                            DialogueModule conflictModule = new DialogueModule("Then it becomes a game of survival! Out of my way, or be swept along in my wake. But I prefer a clever strategy over brute force.");
                            conflictModule.AddOption("Cleverness over strength, I like that.",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, conflictModule));
                        });
                    player.SendGump(new DialogueGump(player, lineModule));
                });

            greeting.AddOption("What’s your view on true happiness?",
                player => true,
                player =>
                {
                    DialogueModule happinessModule = new DialogueModule("True happiness? It’s about indulgence and freedom! If I can pamper myself and enjoy life without restraint, that’s all I need.");
                    happinessModule.AddOption("What about friendships and connections?",
                        pl => true,
                        pl => {
                            DialogueModule connectionModule = new DialogueModule("Those can be fleeting, just like life itself. I enjoy the company of good folks when it suits me, but they won't take precedence over my luxuries.");
                            connectionModule.AddOption("So you’re more self-centered?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, connectionModule));
                        });
                    player.SendGump(new DialogueGump(player, happinessModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Outlaw Daisy tips her hat with a sly grin, wishing you well.");
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
