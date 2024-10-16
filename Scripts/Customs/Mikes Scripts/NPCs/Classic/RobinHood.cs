using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network; // Add this line to include the NetState class

namespace Server.Mobiles
{
    [CorpseName("the corpse of Robin Hood")]
    public class RobinHood : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RobinHood() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Robin Hood";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(120);
            SetInt(80);
            SetHits(60);

            // Appearance
            AddItem(new StuddedLegs() { Hue = 2126 });
            AddItem(new StuddedChest() { Hue = 2126 });
            AddItem(new StuddedGloves() { Hue = 2126 });
            AddItem(new StuddedArms() { Hue = 2126 });
            AddItem(new StuddedGorget() { Hue = 2126 });
            AddItem(new Boots() { Hue = 2126 });
            AddItem(new Bow() { Name = "Sir Robin Hood's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime
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
            DialogueModule greeting = new DialogueModule("I am Robin Hood, the outlaw of Sherwood Forest! What brings you to my hideout?");

            greeting.AddOption("I've heard whispers of your deeds.",
                player => true,
                player =>
                {
                    DialogueModule deedsModule = new DialogueModule("Indeed! I steal from the rich to help the poor. The miners of Minoc suffer greatly while Lord British basks in luxury at his castle. It's unjust!");
                    deedsModule.AddOption("Why do you target Lord British?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule lordModule = new DialogueModule("Lord British has turned a blind eye to the suffering of his subjects. The miners work tirelessly, yet they live in tents, while he enjoys feasts in his grand castle. It's time for that to change!");
                            lordModule.AddOption("How can we help the miners?",
                                p => true,
                                p =>
                                {
                                    DialogueModule helpModule = new DialogueModule("First, we must gather allies willing to join our cause. Together, we can raid the Lord's treasury and redistribute the wealth!");
                                    helpModule.AddOption("What if we get caught?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule cautionModule = new DialogueModule("That's a risk we must be willing to take. The suffering of the miners is far greater than the risk to our own safety.");
                                            pla.SendGump(new DialogueGump(pla, cautionModule));
                                        });
                                    helpModule.AddOption("Count me in! How do we start?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule startModule = new DialogueModule("Excellent! We will need to scout the castle first. The guards are numerous, but there are weaknesses we can exploit. Meet me at the edge of the forest at dusk.");
                                            pla.SendGump(new DialogueGump(pla, startModule));
                                        });
                                    player.SendGump(new DialogueGump(player, helpModule));
                                });
                            pl.SendGump(new DialogueGump(pl, lordModule));
                        });
                    player.SendGump(new DialogueGump(player, deedsModule));
                });

            greeting.AddOption("What can you tell me about the miners?",
                player => true,
                player =>
                {
                    DialogueModule minersModule = new DialogueModule("The miners of Minoc are hardworking folk, but they live in terrible conditions. They often sleep in tents, exposed to the elements, while their hard-earned gold fills Lord British's coffers.");
                    minersModule.AddOption("What do they need most?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule needsModule = new DialogueModule("They need gold to build proper homes, food to nourish their families, and tools to work more efficiently. Without our help, they will continue to suffer.");
                            needsModule.AddOption("How can I contribute?",
                                p => true,
                                p =>
                                {
                                    DialogueModule contributeModule = new DialogueModule("You can help by gathering supplies and gold from your own adventures. Every little bit counts! Together, we can make a difference.");
                                    p.SendGump(new DialogueGump(p, contributeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, needsModule));
                        });
                    player.SendGump(new DialogueGump(player, minersModule));
                });

            greeting.AddOption("What about the Sheriff?",
                player => true,
                player =>
                {
                    DialogueModule sheriffModule = new DialogueModule("The Sheriff is another villain who enforces the oppressive laws of Lord British. He profits from the miners' labor while ensuring they receive little in return.");
                    sheriffModule.AddOption("How can we deal with him?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dealModule = new DialogueModule("We could infiltrate his office and uncover evidence of his corruption. We can expose him to the people and turn them against him!");
                            dealModule.AddOption("Sounds risky.",
                                p => true,
                                p =>
                                {
                                    DialogueModule riskModule = new DialogueModule("All worthwhile endeavors come with risk. If we expose the Sheriff, we could rally the people of Minoc to our cause.");
                                    p.SendGump(new DialogueGump(p, riskModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dealModule));
                        });
                    player.SendGump(new DialogueGump(player, sheriffModule));
                });

            greeting.AddOption("Do you have any plans to raid Lord British's treasury?",
                player => true,
                player =>
                {
                    DialogueModule plansModule = new DialogueModule("Yes, I've been devising a plan! We can strike when the guards are least expecting it, perhaps during one of their lavish feasts. We can slip in and out before they realize what’s happening!");
                    plansModule.AddOption("What if we fail?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule failureModule = new DialogueModule("Failure is always a possibility, but we must believe in our cause. If we succeed, it will change the lives of the miners forever!");
                            pl.SendGump(new DialogueGump(pl, failureModule));
                        });
                    plansModule.AddOption("Count me in for the raid!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule joinModule = new DialogueModule("Wonderful! We will gather at the old oak tree at twilight. Be ready, for we will make history together!");
                            pl.SendGump(new DialogueGump(pl, joinModule));
                        });
                    player.SendGump(new DialogueGump(player, plansModule));
                });

            greeting.AddOption("What happens after the raid?",
                player => true,
                player =>
                {
                    DialogueModule afterRaidModule = new DialogueModule("After we raid the treasury, we will distribute the wealth to the miners. They will finally have the means to improve their lives, and they will remember who helped them!");
                    afterRaidModule.AddOption("What if Lord British retaliates?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule retaliationModule = new DialogueModule("We must be prepared for that possibility. But if the miners rally behind us, their strength in numbers will be our greatest weapon.");
                            pl.SendGump(new DialogueGump(pl, retaliationModule));
                        });
                    player.SendGump(new DialogueGump(player, afterRaidModule));
                });

            greeting.AddOption("What is your ultimate goal?",
                player => true,
                player =>
                {
                    DialogueModule goalModule = new DialogueModule("My goal is to bring equality to this land. No man or woman should suffer while others live in excess. We must unite the oppressed and challenge those who abuse their power.");
                    goalModule.AddOption("I admire your determination.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule praiseModule = new DialogueModule("Thank you! Together, we can change the course of history. Every step we take towards justice is a step towards a better world.");
                            pl.SendGump(new DialogueGump(pl, praiseModule));
                        });
                    player.SendGump(new DialogueGump(player, goalModule));
                });

            return greeting;
        }

        public RobinHood(Serial serial) : base(serial) { }

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
