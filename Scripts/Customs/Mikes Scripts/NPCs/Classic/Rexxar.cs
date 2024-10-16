using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rexxar")]
    public class Rexxar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Rexxar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rexxar";
            Body = 0x190; // Human male body

            // Stats
            SetStr(120);
            SetDex(120);
            SetInt(40);
            SetHits(80);

            // Appearance
            AddItem(new PlateLegs() { Hue = 1909 });
            AddItem(new PlateChest() { Hue = 1909 });
            AddItem(new PlateHelm() { Hue = 1909 });
            AddItem(new PlateGloves() { Hue = 1909 });
            AddItem(new Boots() { Hue = 1909 });
            AddItem(new Halberd() { Name = "Rexxar's Plasma Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Rexxar, the Bionic Soldier! I fell through a portal to this world, and I am glad to be free from the tyranny of Apocalypse. What do you wish to know?");

            greeting.AddOption("What can you tell me about Apocalypse?",
                player => true,
                player =>
                {
                    DialogueModule apocalypseModule = new DialogueModule("Apocalypse is an arch mutant who rules with an iron fist. His powers are unmatched, and his armies are relentless. Many have fallen to his reign of terror. But here, I feel hope.");
                    apocalypseModule.AddOption("What do you mean by hope?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule hopeModule = new DialogueModule("In this world, we can fight back. We have freedom to explore, survive, and perhaps even thrive! There are remnants of humanity that refuse to bow to Apocalypse's will.");
                            hopeModule.AddOption("How can we fight back?",
                                p => true,
                                p =>
                                {
                                    DialogueModule fightModule = new DialogueModule("Unity is our greatest strength. We must band together, gather resources, and form alliances. Even the smallest spark can ignite a rebellion against tyranny.");
                                    fightModule.AddOption("What resources do we need?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule resourcesModule = new DialogueModule("We need food, medicine, and weapons. Look for supply caches left behind by those who fought against Apocalypse. The old world has secrets to uncover.");
                                            resourcesModule.AddOption("Where can I find these caches?",
                                                plq => true,
                                                plq =>
                                                {
                                                    pl.SendMessage("Search the ruins of the old cities. They may hold valuable resources, but beware of scavengers and mutant creatures.");
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            resourcesModule.AddOption("I will gather resources.",
                                                plaw => true,
                                                plaw =>
                                                {
                                                    pla.SendMessage("May luck be on your side!");
                                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                                });
                                            p.SendGump(new DialogueGump(p, resourcesModule));
                                        });
                                    fightModule.AddOption("I'll think about it.",
                                        ple => true,
                                        ple =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    player.SendGump(new DialogueGump(player, fightModule));
                                });
                            hopeModule.AddOption("Can you tell me more about the remnants of humanity?",
                                p => true,
                                p =>
                                {
                                    DialogueModule remnantsModule = new DialogueModule("Yes! There are pockets of resistance, each with their own ideals. Some seek to restore the old world, while others wish to forge a new path. It's a complex web of hope and despair.");
                                    remnantsModule.AddOption("What do they need?",
                                        plr => true,
                                        plr =>
                                        {
                                            DialogueModule needsModule = new DialogueModule("They need allies, information, and supplies. If you come across them, listen to their stories and see how you can help.");
                                            needsModule.AddOption("I will seek them out.",
                                                pla => true,
                                                pla =>
                                                {
                                                    pla.SendMessage("Your heart is in the right place!");
                                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                                });
                                            pl.SendGump(new DialogueGump(pl, needsModule));
                                        });
                                    remnantsModule.AddOption("What if I find them hostile?",
                                        plt => true,
                                        plt =>
                                        {
                                            DialogueModule hostileModule = new DialogueModule("Approach with caution. They may not trust outsiders. Earn their respect by showing your intentions. Actions speak louder than words.");
                                            hostileModule.AddOption("I understand.",
                                                pla => true,
                                                pla =>
                                                {
                                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, hostileModule));
                                        });
                                    player.SendGump(new DialogueGump(player, remnantsModule));
                                });
                            player.SendGump(new DialogueGump(player, hopeModule));
                        });
                    player.SendGump(new DialogueGump(player, apocalypseModule));
                });

            greeting.AddOption("Tell me about your bionic enhancements.",
                player => true,
                player =>
                {
                    DialogueModule bionicModule = new DialogueModule("My enhancements are the result of advanced technology designed to combat Apocalypse's forces. They amplify my strength, speed, and cognitive functions.");
                    bionicModule.AddOption("What specific abilities do you have?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule abilitiesModule = new DialogueModule("I possess enhanced reflexes and can analyze combat scenarios rapidly. This allows me to adapt and strategize in the heat of battle.");
                            abilitiesModule.AddOption("Can I learn to fight like you?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule trainingModule = new DialogueModule("Of course! I can teach you the basics of combat. We must focus on agility and precision. Would you like to train now?");
                                    trainingModule.AddOption("Yes, let's train!",
                                        p => true,
                                        p =>
                                        {
                                            p.SendMessage("You begin training with Rexxar, honing your skills.");
                                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                        });
                                    trainingModule.AddOption("Not right now.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                        });
                                    pla.SendGump(new DialogueGump(pla, trainingModule));
                                });
                            abilitiesModule.AddOption("Are there risks to such enhancements?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule riskModule = new DialogueModule("Indeed. Bionic enhancements can be unstable, especially in a chaotic environment. It's crucial to maintain them properly or face dire consequences.");
                                    riskModule.AddOption("How do I maintain my enhancements?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule maintenanceModule = new DialogueModule("Regular check-ups and spare parts are essential. Scavenging old tech from the ruins can help.");
                                            maintenanceModule.AddOption("I'll keep an eye out for parts.",
                                                ply => true,
                                                ply =>
                                                {
                                                    pl.SendMessage("Good luck on your search!");
                                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                                });
                                            p.SendGump(new DialogueGump(p, maintenanceModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, riskModule));
                                });
                            player.SendGump(new DialogueGump(player, abilitiesModule));
                        });
                    player.SendGump(new DialogueGump(player, bionicModule));
                });

            greeting.AddOption("What is life like in this world?",
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("Life is a constant struggle. Resources are scarce, and we face dangers from mutants, scavengers, and the elements. But every day we fight for survival.");
                    lifeModule.AddOption("How do you survive?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule survivalModule = new DialogueModule("We scavenge, trade, and form alliances. Knowledge is as valuable as food in this world. If you have the skills, you can barter for what you need.");
                            survivalModule.AddOption("What skills are most valuable?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule skillsModule = new DialogueModule("Combat skills, survival tactics, and technological knowledge are in high demand. If you can fix machines, you're a treasure in this wasteland!");
                                    skillsModule.AddOption("I'll work on my skills.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendMessage("That's the spirit!");
                                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                        });
                                    pla.SendGump(new DialogueGump(pla, skillsModule));
                                });
                            survivalModule.AddOption("What about food?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule foodModule = new DialogueModule("Food is scarce, but you can find some edible plants and hunt small creatures. Learning to identify safe food is vital.");
                                    foodModule.AddOption("I'll gather food.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendMessage("Good luck! Stay safe out there.");
                                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                        });
                                    pla.SendGump(new DialogueGump(pla, foodModule));
                                });
                            player.SendGump(new DialogueGump(player, survivalModule));
                        });
                    lifeModule.AddOption("Are there any safe havens?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule havensModule = new DialogueModule("There are a few fortified settlements where people band together for protection. They often trade with each other and share resources.");
                            havensModule.AddOption("How do I find these settlements?",
                                p => true,
                                p =>
                                {
                                    DialogueModule findModule = new DialogueModule("Ask around in the ruins. Some may have maps or information on the locations of these safe havens.");
                                    findModule.AddOption("I will seek them out.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendMessage("May fortune favor your journey!");
                                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                        });
                                    p.SendGump(new DialogueGump(p, findModule));
                                });
                            player.SendGump(new DialogueGump(player, havensModule));
                        });
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            greeting.AddOption("Do you have any tasks for me?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        DialogueModule taskCooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                        player.SendGump(new DialogueGump(player, taskCooldownModule));
                    }
                    else
                    {
                        DialogueModule taskModule = new DialogueModule("There is much to be done! If you find any valuable information from the Eldric Lab, please report back to me. I may have something for you as a reward.");
                        taskModule.AddOption("I'll investigate the lab.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Your courage is commendable! Report back with your findings.");
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            });
                        player.SendGump(new DialogueGump(player, taskModule));
                    }
                });

            greeting.AddOption("What do you know about combat?",
                player => true,
                player =>
                {
                    DialogueModule combatModule = new DialogueModule("Combat is a dance of tactics, strategy, and skill. Even with my enhancements, I constantly upgrade my combat protocols. Can you match my prowess?");
                    combatModule.AddOption("I would love to try!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, combatModule));
                });

            greeting.AddOption("How do you protect others?",
                player => true,
                player =>
                {
                    DialogueModule protectionModule = new DialogueModule("I have sworn to protect those in need. It's a duty I take seriously. If ever you find yourself in peril, just call for me.");
                    protectionModule.AddOption("Thank you, I'll remember that.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, protectionModule));
                });

            return greeting;
        }

        public Rexxar(Serial serial) : base(serial) { }

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
