using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Niko")]
    public class Niko : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Niko() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Niko";
            Body = 0x190; // Human male body

            // Stats
            SetStr(130);
            SetDex(65);
            SetInt(45);
            SetHits(85);

            // Appearance
            AddItem(new ShortPants() { Hue = 1230 });
            AddItem(new Shirt() { Hue = 1230 });
            AddItem(new Boots() { Hue = 1230 });
            AddItem(new Pickaxe() { Name = "Niko's Pickaxe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

		public Niko(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Greetings, exile. What brings you to these depths?");

            greeting.AddOption("Tell me about your adventures.",
                player => true,
                player =>
                {
                    DialogueModule adventureModule = new DialogueModule("Ah, my adventures with Kirac! Those were perilous times, battling against the dimensional raiders that threatened Wraeclast.");
                    adventureModule.AddOption("What were the dimensional raiders?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule raiderModule = new DialogueModule("They were vile beings from another realm, seeking to exploit the chaos of Wraeclast for their gain. We fought hard to defend our lands against their incursions.");
                            raiderModule.AddOption("How did you and Kirac fight them?",
                                p => true,
                                p =>
                                {
                                    DialogueModule fightModule = new DialogueModule("Kirac and I devised clever strategies, using the environment to our advantage. We set traps, ambushed them at key points, and leveraged the skills of our allies.");
                                    fightModule.AddOption("Tell me about one of your battles.",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule battleModule = new DialogueModule("There was one battle near the ruins of Sarn, where we faced a particularly cunning raider leader. We laid an elaborate trap involving the remnants of an old siege weapon.");
                                            battleModule.AddOption("What happened in that battle?",
                                                pw => true,
                                                pw =>
                                                {
                                                    DialogueModule detailsModule = new DialogueModule("As the raiders advanced, we triggered the trap, unleashing a barrage of stone projectiles. It turned the tide of battle, scattering their forces. However, we sustained heavy losses.");
                                                    detailsModule.AddOption("Did you lose any friends?",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateLossModule()));
                                                        });
                                                    battleModule.AddOption("That sounds intense. What did you learn from it?",
                                                        plr => true,
                                                        plr =>
                                                        {
                                                            DialogueModule lessonModule = new DialogueModule("I learned that strategy and teamwork are vital in the face of overwhelming odds. We must adapt to survive in such chaotic conditions.");
                                                            lessonModule.AddOption("Wise words indeed.",
                                                                pla => true,
                                                                pla =>
                                                                {
                                                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                });
                                                            p.SendGump(new DialogueGump(p, lessonModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, detailsModule));
                                                });
                                            battleModule.AddOption("What happened after the battle?",
                                                plt => true,
                                                plt =>
                                                {
                                                    DialogueModule aftermathModule = new DialogueModule("Afterward, we gathered what remains of our forces and regrouped. Kirac had a vision to further strengthen our defenses.");
                                                    aftermathModule.AddOption("Did you follow him?",
                                                        py => true,
                                                        py =>
                                                        {
                                                            p.SendGump(new DialogueGump(p, CreateStrengthModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, aftermathModule));
                                                });
                                                p.SendGump(new DialogueGump(p, battleModule));
                                            });
                                        });
                                    raiderModule.AddOption("What was Kirac like?",
                                        plu => true,
                                        plu =>
                                        {
                                            DialogueModule kiracModule = new DialogueModule("Kirac was a natural leader, always calm under pressure. His knowledge of the raiders was invaluable, allowing us to anticipate their moves.");
                                            kiracModule.AddOption("What did you learn from him?",
                                                p => true,
                                                p =>
                                                {
                                                    DialogueModule learnFromKiracModule = new DialogueModule("I learned the importance of understanding one's enemy. Kirac emphasized that knowledge is as powerful as any weapon.");
                                                    learnFromKiracModule.AddOption("Did you ever disagree?",
                                                        pli => true,
                                                        pli =>
                                                        {
                                                            DialogueModule disagreementModule = new DialogueModule("Yes, there were times. Kirac believed in a more defensive approach, while I often pushed for aggressive tactics. It led to some heated debates.");
                                                            disagreementModule.AddOption("How did you resolve those differences?",
                                                                po => true,
                                                                po =>
                                                                {
                                                                    DialogueModule resolutionModule = new DialogueModule("We learned to compromise, using a mix of both strategies. That balance became key to our survival.");
                                                                    resolutionModule.AddOption("It's good to have such alliances.",
                                                                        pla => true,
                                                                        pla =>
                                                                        {
                                                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                        });
                                                                    p.SendGump(new DialogueGump(p, resolutionModule));
                                                                });
                                                            p.SendGump(new DialogueGump(p, disagreementModule));
                                                        });
                                                });
                                            pl.SendGump(new DialogueGump(pl, kiracModule));
                                        });
                                    player.SendGump(new DialogueGump(player, raiderModule));
                                });
                            adventureModule.AddOption("How did you end up here?",
                                playerp => true,
                                playerp =>
                                {
                                    DialogueModule portalModule = new DialogueModule("Ah, that's quite a tale. During one of our battles, we were caught in a powerful surge of energy from a portal that opened unexpectedly. It sucked us in, separating us from our allies.");
                                    portalModule.AddOption("What happened when you arrived here?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule arrivalModule = new DialogueModule("I found myself alone in this strange new world, disoriented and without my companions. It took time to adjust, and I spent many days wandering, seeking to find a way back.");
                                            arrivalModule.AddOption("Did you find Kirac again?",
                                                p => true,
                                                p =>
                                                {
                                                    DialogueModule searchModule = new DialogueModule("Not yet. I hope he made it through as well. I fear for him, knowing how dangerous the dimensions can be.");
                                                    searchModule.AddOption("What will you do now?",
                                                        pla => true,
                                                        pla =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateFuturePlansModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, searchModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, arrivalModule));
                                        });
                                    player.SendGump(new DialogueGump(player, portalModule));
                                });
                            player.SendGump(new DialogueGump(player, adventureModule));
                        });

            greeting.AddOption("Do you have any rewards for me?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        Say("Some rewards are treasures of gold and jewels. Others are ancient artifacts with powers unknown. Once, I found this...");
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            return greeting;
        }

        private DialogueModule CreateLossModule()
        {
            DialogueModule lossModule = new DialogueModule("Yes, we lost some good friends that day. Their sacrifices weigh heavily on my heart. Each loss reminds me of the cost of our fight against the darkness.");
            lossModule.AddOption("It's a heavy burden to bear.",
                pl => true,
                pl =>
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return lossModule;
        }

        private DialogueModule CreateStrengthModule()
        {
            DialogueModule strengthModule = new DialogueModule("Absolutely! We knew we had to fortify our defenses against future raids. Kirac had plans to build stronger barriers and to recruit more fighters.");
            strengthModule.AddOption("Did you find more allies?",
                pl => true,
                pl =>
                {
                    DialogueModule alliesModule = new DialogueModule("Yes, we connected with other survivors who shared our cause. Together, we became a force to be reckoned with against the dimensional raiders.");
                    alliesModule.AddOption("That sounds encouraging.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pl.SendGump(new DialogueGump(pl, alliesModule));
                });
            return strengthModule;
        }

        private DialogueModule CreateFuturePlansModule()
        {
            DialogueModule futurePlansModule = new DialogueModule("Now that I am here, I seek to adapt and thrive in this world. Perhaps I can find a way back to Wraeclast or at least ensure the raiders do not follow.");
            futurePlansModule.AddOption("How do you plan to do that?",
                pl => true,
                pl =>
                {
                    DialogueModule plansModule = new DialogueModule("I will gather resources, strengthen my skills, and connect with those who understand these dimensions. Knowledge is power, after all.");
                    plansModule.AddOption("Wise plan! I wish you luck.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    pl.SendGump(new DialogueGump(pl, plansModule));
                });
            return futurePlansModule;
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
