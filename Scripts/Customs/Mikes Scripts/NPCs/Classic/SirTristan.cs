using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Tristan")]
    public class SirTristan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirTristan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Tristan";
            Body = 0x190; // Human male body

            // Stats
            SetStr(160);
            SetDex(63);
            SetInt(22);
            SetHits(115);

            // Appearance
            AddItem(new ChainLegs() { Hue = 1100 });
            AddItem(new ChainChest() { Hue = 1100 });
            AddItem(new PlateHelm() { Hue = 1100 });
            AddItem(new PlateGloves() { Hue = 1100 });
            AddItem(new Boots() { Hue = 1100 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
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
            DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! You seem to have found me lost in thought about my beloved rabbits...");

            greeting.AddOption("Rabbits? Tell me more!",
                player => true,
                player =>
                {
                    DialogueModule rabbitsModule = new DialogueModule("Ah, yes! They are the most delightful creatures, full of life and joy. I often find solace in their company.");
                    rabbitsModule.AddOption("What do you love most about them?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loveModule = new DialogueModule("Their gentle nature and playful spirit remind me of simpler times. Watching them hop around brings me peace.");
                            loveModule.AddOption("Do you have a favorite breed?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule breedModule = new DialogueModule("I must say, the Netherland Dwarf holds a special place in my heart. Their tiny size and affectionate nature are simply enchanting.");
                                    breedModule.AddOption("How many do you have?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule countModule = new DialogueModule("Currently, I care for five rabbits: Flopsy, Mopsy, Cotton, Thumper, and the ever-mischievous Pippin. They each have their unique quirks!");
                                            countModule.AddOption("What are their quirks?",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule quirksModule = new DialogueModule("Flopsy loves to dig, Mopsy enjoys hiding in the tall grass, Cotton is a champion eater, Thumper is the most curious, and Pippin... well, he's a troublemaker!");
                                                    quirksModule.AddOption("Do you spend a lot of time with them?",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            DialogueModule timeModule = new DialogueModule("Every day! They bring joy to my heart, and I often talk to them as if they understand every word.");
                                                            AddCommonOptions(timeModule);
                                                            ple.SendGump(new DialogueGump(ple, timeModule));
                                                        });
                                                    plc.SendGump(new DialogueGump(plc, quirksModule));
                                                });
                                            plb.SendGump(new DialogueGump(plb, countModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, breedModule));
                                });
                            loveModule.AddOption("Do they help you forget your troubles?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule helpModule = new DialogueModule("Indeed, they do. Whenever I feel weighed down by my past, their playful hops lift my spirits.");
                                    AddCommonOptions(helpModule);
                                    pl.SendGump(new DialogueGump(pl, helpModule));
                                });
                            player.SendGump(new DialogueGump(player, loveModule));
                        });
                    player.SendGump(new DialogueGump(player, rabbitsModule));
                });

            greeting.AddOption("Did something happen to your rabbits?",
                player => true,
                player =>
                {
                    DialogueModule tragedyModule = new DialogueModule("Ah, it pains me to speak of it. A fierce storm once swept through, scattering my beloved rabbits. I lost dear Cotton that day.");
                    tragedyModule.AddOption("I'm sorry to hear that.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule comfortModule = new DialogueModule("Thank you, kind traveler. Remembering Cotton is bittersweet, but I cherish the memories we made.");
                            AddCommonOptions(comfortModule);
                            pl.SendGump(new DialogueGump(pl, comfortModule));
                        });
                    tragedyModule.AddOption("How did you cope?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule copeModule = new DialogueModule("It was hard. I spent time with the others, and their presence reminded me that life continues.");
                            copeModule.AddOption("What do you do to honor Cotton?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule honorModule = new DialogueModule("I created a small garden in his memory, filled with flowers he loved to nibble on. It's a peaceful spot.");
                                    AddCommonOptions(honorModule);
                                    plc.SendGump(new DialogueGump(plc, honorModule));
                                });
                            AddCommonOptions(copeModule);
                            pl.SendGump(new DialogueGump(pl, copeModule));
                        });
                    player.SendGump(new DialogueGump(player, tragedyModule));
                });

            greeting.AddOption("Do you have any rabbit-related stories?",
                player => true,
                player =>
                {
                    DialogueModule storyModule = new DialogueModule("Oh, many! One time, I caught Pippin trying to steal a carrot from my garden. He looked so guilty!");
                    storyModule.AddOption("What did you do?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule responseModule = new DialogueModule("I couldn't scold him; he looked too adorable with that carrot in his mouth! I ended up giving him the carrot as a treat.");
                            AddCommonOptions(responseModule);
                            pl.SendGump(new DialogueGump(pl, responseModule));
                        });
                    storyModule.AddOption("Do they have a favorite food?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule foodModule = new DialogueModule("Oh, definitely! They adore fresh greens and, of course, the sweetest carrots. It's a joy to see them hop around excitedly at feeding time.");
                            AddCommonOptions(foodModule);
                            pl.SendGump(new DialogueGump(pl, foodModule));
                        });
                    player.SendGump(new DialogueGump(player, storyModule));
                });

            greeting.AddOption("Why rabbits?",
                player => true,
                player =>
                {
                    DialogueModule whyModule = new DialogueModule("Rabbits embody gentleness and playfulness. In their presence, I find comfort that transcends the pain of my past.");
                    AddCommonOptions(whyModule);
                    player.SendGump(new DialogueGump(player, whyModule));
                });

            return greeting;
        }

        private void AddCommonOptions(DialogueModule module)
        {
            module.AddOption("Tell me more.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            module.AddOption("Goodbye.",
                player => true,
                player => player.SendMessage("Until we meet again, traveler."));
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

        public SirTristan(Serial serial) : base(serial) { }
    }
}
