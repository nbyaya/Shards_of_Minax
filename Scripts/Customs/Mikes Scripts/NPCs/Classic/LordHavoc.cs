using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lord Havoc")]
    public class LordHavoc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LordHavoc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lord Havoc";
            Body = 0x190; // Human male body

            // Stats
            SetStr(130);
            SetDex(75);
            SetInt(50);
            SetHits(90);

            // Appearance
            AddItem(new PlateLegs() { Hue = 1175 });
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new PlateArms() { Hue = 1175 });
            AddItem(new PlateHelm() { Hue = 1175 });
            AddItem(new PlateGloves() { Hue = 1175 });
            AddItem(new ExecutionersAxe() { Name = "Lord Havoc's Axe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("I am Lord Havoc, the Black Knight. What do you want?");

            greeting.AddOption("Tell me about yourself.",
                player => true,
                player => {
                    DialogueModule selfModule = new DialogueModule("I was once a respected knight, but betrayal turned me into a shadow of my former self. My past is filled with glory and pain.");
                    selfModule.AddOption("What glories did you achieve?",
                        p => true,
                        p => {
                            DialogueModule gloriesModule = new DialogueModule("I fought valiantly in many battles, defending the realm from invaders. The tales of my victories are whispered in taverns.");
                            gloriesModule.AddOption("What was your greatest victory?",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, CreateVictoryModule()));
                                });
                            p.SendGump(new DialogueGump(p, gloriesModule));
                        });
                    selfModule.AddOption("What about your pain?",
                        p => true,
                        p => {
                            DialogueModule painModule = new DialogueModule("The greatest pain was not the wounds I suffered, but the betrayal of those I trusted. It shattered my heart.");
                            painModule.AddOption("Who betrayed you?",
                                pl => true,
                                pl => {
                                    DialogueModule betrayalModule = new DialogueModule("It was a close friend, a fellow knight. He led the charge against me during the siege. I never saw it coming.");
                                    betrayalModule.AddOption("Tell me more about the siege.",
                                        p2 => true,
                                        p2 => {
                                            p2.SendGump(new DialogueGump(p2, CreateSiegeModule()));
                                        });
                                    pl.SendGump(new DialogueGump(pl, betrayalModule));
                                });
                            p.SendGump(new DialogueGump(p, painModule));
                        });
                    player.SendGump(new DialogueGump(player, selfModule));
                });

            greeting.AddOption("What can you tell me about the cursed amulet?",
                player => true,
                player => {
                    DialogueModule amuletModule = new DialogueModule("The cursed amulet is a symbol of the treachery that befell me. It is said to be imbued with dark magic, a token of my lost honor.");
                    amuletModule.AddOption("How can I help you?",
                        pl => true,
                        pl => {
                            DialogueModule helpModule = new DialogueModule("Seek out the cursed amulet in the Whispering Caverns. Return it to me, and I shall reward you.");
                            helpModule.AddOption("What reward will I receive?",
                                p => true,
                                p => {
                                    DialogueModule rewardModule = new DialogueModule("Bring the amulet to me, and I shall grant you something of great value from my personal collection. Perhaps a rare artifact or gold.");
                                    rewardModule.AddOption("What kind of artifacts do you have?",
                                        pla => true,
                                        pla => {
                                            DialogueModule artifactsModule = new DialogueModule("I possess many unique items: a ring that enhances strength, a cloak that grants invisibility, and potions of incredible power.");
                                            pla.SendGump(new DialogueGump(pla, artifactsModule));
                                        });
                                    p.SendGump(new DialogueGump(p, rewardModule));
                                });
                            pl.SendGump(new DialogueGump(pl, helpModule));
                        });
                    player.SendGump(new DialogueGump(player, amuletModule));
                });

            greeting.AddOption("What do you think about trust?",
                player => true,
                player => {
                    DialogueModule trustModule = new DialogueModule("Concern for others was my downfall. I once let my guard down and paid dearly for it.");
                    trustModule.AddOption("How did it affect your life?",
                        pl => true,
                        pl => {
                            DialogueModule effectModule = new DialogueModule("It made me wary of others. I trust no one, not even those who claim to be friends. My solitude is my only comfort.");
                            effectModule.AddOption("Is there anyone you trust now?",
                                p => true,
                                p => {
                                    DialogueModule trustNowModule = new DialogueModule("Trust is a fragile thing. The only being I trust is my steed, for it has never failed me. Humans, however, are another matter entirely.");
                                    p.SendGump(new DialogueGump(p, trustNowModule));
                                });
                            pl.SendGump(new DialogueGump(pl, effectModule));
                        });
                    player.SendGump(new DialogueGump(player, trustModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player => {
                    player.SendMessage("Lord Havoc nods at you as you depart.");
                });

            return greeting;
        }

        private DialogueModule CreateVictoryModule()
        {
            DialogueModule victoryModule = new DialogueModule("My greatest victory was at the Battle of Ember Hill. We faced insurmountable odds, but through sheer will, we triumphed.");
            victoryModule.AddOption("What was so special about that battle?",
                pl => true,
                pl => {
                    DialogueModule specialModule = new DialogueModule("The enemy was fierce, but our spirits were unyielding. We fought for our families and our homeland. It was a day of honor.");
                    specialModule.AddOption("Did you lose anyone close to you?",
                        p => true,
                        p => {
                            DialogueModule lossModule = new DialogueModule("Yes, my squire, a brave boy named Edgar. He fell protecting me. I carry the weight of his sacrifice with me.");
                            lossModule.AddOption("I’m sorry for your loss.",
                                pla => true,
                                pla => {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, lossModule));
                        });
                    pl.SendGump(new DialogueGump(pl, specialModule));
                });
            return victoryModule;
        }

        private DialogueModule CreateSiegeModule()
        {
            DialogueModule siegeModule = new DialogueModule("The siege of Falter's Keep was a bloodbath. I led my men bravely, but treachery from within led to our defeat.");
            siegeModule.AddOption("What happened during the siege?",
                pl => true,
                pl => {
                    DialogueModule detailsModule = new DialogueModule("We were besieged for weeks. Supplies dwindled, morale dropped. Just as we thought victory was at hand, betrayal struck.");
                    detailsModule.AddOption("How did you survive?",
                        p => true,
                        p => {
                            DialogueModule survivalModule = new DialogueModule("I managed to escape through a secret passage. It was dark, cold, and filled with the screams of those left behind. I still hear them at night.");
                            p.SendGump(new DialogueGump(p, survivalModule));
                        });
                    pl.SendGump(new DialogueGump(pl, detailsModule));
                });
            return siegeModule;
        }

        public LordHavoc(Serial serial) : base(serial) { }

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
