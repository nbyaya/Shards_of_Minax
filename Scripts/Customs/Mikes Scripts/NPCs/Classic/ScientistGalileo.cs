using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Scientist Galileo")]
    public class ScientistGalileo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ScientistGalileo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Scientist Galileo";
            Body = 0x190; // Human male body

            // Stats
            SetStr(80);
            SetDex(60);
            SetInt(110);
            SetHits(60);

            // Appearance
            AddItem(new LongPants() { Hue = 1154 });
            AddItem(new FancyShirt() { Hue = 1156 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new WideBrimHat() { Hue = 1103 });
            AddItem(new LeatherArms() { Name = "Galileo's Arms" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Scientist Galileo. It's a pleasure to meet someone from this fascinating land. Tell me, have you encountered any elves here?");

            greeting.AddOption("Yes, I've met some elves!",
                player => true,
                player => {
                    DialogueModule elvesModule = new DialogueModule("Elves! Marvelous beings! Their connection to nature is extraordinary. What were they like?");
                    elvesModule.AddOption("They were wise and graceful.",
                        pl => true,
                        pl => {
                            DialogueModule wiseElvesModule = new DialogueModule("Ah, wisdom is a treasure in itself! Did they share any secrets of their long lives with you?");
                            wiseElvesModule.AddOption("They spoke of ancient knowledge.",
                                p => true,
                                p => {
                                    DialogueModule ancientKnowledgeModule = new DialogueModule("Fascinating! Ancient knowledge is like a beacon in the darkness. What did they reveal?");
                                    ancientKnowledgeModule.AddOption("They mentioned the Great Tree of Life.",
                                        plq => true,
                                        plq => {
                                            DialogueModule treeModule = new DialogueModule("The Great Tree of Life! A legendary source of wisdom and power. It is said to be sacred to them. Do you know its location?");
                                            treeModule.AddOption("They said it's hidden deep in the forest.",
                                                pw => true,
                                                pw => p.SendGump(new DialogueGump(p, greeting)));
                                            treeModule.AddOption("No, they didn't share that information.",
                                                pe => true,
                                                pe => p.SendGump(new DialogueGump(p, greeting)));
                                            pl.SendGump(new DialogueGump(pl, treeModule));
                                        });
                                    ancientKnowledgeModule.AddOption("They shared tales of their history.",
                                        plr => true,
                                        plr => p.SendGump(new DialogueGump(pl, greeting)));
                                    p.SendGump(new DialogueGump(p, ancientKnowledgeModule));
                                });
                            wiseElvesModule.AddOption("They were elusive and mysterious.",
                                plt => true,
                                plt => {
                                    DialogueModule elusiveModule = new DialogueModule("Mysteries! The essence of exploration. Did they speak of their magic or their relationship with nature?");
                                    elusiveModule.AddOption("Yes, they mentioned their magical abilities.",
                                        p => true,
                                        p => {
                                            DialogueModule magicModule = new DialogueModule("Magic! An ancient art. Their magic must be tied to the land itself. What types did they practice?");
                                            magicModule.AddOption("They talked about healing magic.",
                                                ply => true,
                                                ply => {
                                                    DialogueModule healingModule = new DialogueModule("Healing magic is a profound gift. It shows their compassion for all living things. Did they offer to teach you?");
                                                    healingModule.AddOption("They did! I learned a few spells.",
                                                        pu => true,
                                                        pu => p.SendGump(new DialogueGump(p, greeting)));
                                                    healingModule.AddOption("No, but they were willing to share.",
                                                        pi => true,
                                                        pi => p.SendGump(new DialogueGump(p, greeting)));
                                                    pl.SendGump(new DialogueGump(pl, healingModule));
                                                });
                                            magicModule.AddOption("They mentioned elemental magic.",
                                                plo => true,
                                                plo => p.SendGump(new DialogueGump(pl, greeting)));
                                            p.SendGump(new DialogueGump(p, magicModule));
                                        });
                                    elusiveModule.AddOption("No, they kept their secrets close.",
                                        plp => true,
                                        plp => plp.SendGump(new DialogueGump(plp, greeting)));
                                    pl.SendGump(new DialogueGump(pl, elusiveModule));
                                });
                            player.SendGump(new DialogueGump(player, wiseElvesModule));
                        });

                    elvesModule.AddOption("They were distant and reserved.",
                        pl => true,
                        pl => {
                            DialogueModule distantModule = new DialogueModule("A cautious nature! Elves can be wary of outsiders. Did they share any lore about their culture?");
                            distantModule.AddOption("They spoke of their ancient traditions.",
                                p => true,
                                p => {
                                    DialogueModule traditionsModule = new DialogueModule("Ancient traditions are the soul of their people. What traditions did they share with you?");
                                    traditionsModule.AddOption("They told me about their festivals.",
                                        pla => true,
                                        pla => p.SendGump(new DialogueGump(pl, greeting)));
                                    traditionsModule.AddOption("They mentioned their rituals to honor nature.",
                                        pls => true,
                                        pls => {
                                            DialogueModule ritualsModule = new DialogueModule("Rituals to honor nature! How beautiful! Did they invite you to witness any of their ceremonies?");
                                            ritualsModule.AddOption("Yes, it was a mesmerizing experience.",
                                                pd => true,
                                                pd => p.SendGump(new DialogueGump(p, greeting)));
                                            ritualsModule.AddOption("No, but they spoke highly of them.",
                                                pf => true,
                                                pf => p.SendGump(new DialogueGump(p, greeting)));
                                            pl.SendGump(new DialogueGump(pl, ritualsModule));
                                        });
                                    player.SendGump(new DialogueGump(player, traditionsModule));
                                });
                            distantModule.AddOption("No, they kept their stories private.",
                                plg => true,
                                plg => plg.SendGump(new DialogueGump(plg, greeting)));
                            player.SendGump(new DialogueGump(player, distantModule));
                        });

                    player.SendGump(new DialogueGump(player, elvesModule));
                });

            greeting.AddOption("No, I haven't met any elves.",
                player => true,
                player => {
                    DialogueModule noElvesModule = new DialogueModule("Oh, what a pity! Elves are such fascinating beings. Their connection to the natural world is unparalleled. Perhaps you'll have the chance to meet them in your travels. What do you think?");
                    noElvesModule.AddOption("I hope to meet them someday!",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, greeting)));
                    noElvesModule.AddOption("They sound intriguing.",
                        pl => true,
                        pl => {
                            DialogueModule intriguingModule = new DialogueModule("Indeed! Their lore is filled with wonders. If you ever hear of their gatherings or sightings, please let me know. Knowledge is our greatest treasure!");
                            intriguingModule.AddOption("I will! Thank you for sharing.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, greeting)));
                            player.SendGump(new DialogueGump(player, intriguingModule));
                        });
                    player.SendGump(new DialogueGump(player, noElvesModule));
                });

            return greeting;
        }

        public ScientistGalileo(Serial serial) : base(serial) { }

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
