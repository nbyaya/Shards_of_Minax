using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lilith the Deathweaver")]
    public class LilithTheDeathweaver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilithTheDeathweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lilith the Deathweaver";
            Body = 0x191; // Human female body

            // Stats
            SetStr(125);
            SetDex(55);
            SetInt(155);
            SetHits(105);

            // Appearance
            AddItem(new Robe() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1109 });
            AddItem(new BoneHelm());

			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(Female);
			HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime
        }

        public LilithTheDeathweaver(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Lilith the Deathweaver, a master of dark arts! What do you seek, traveler?");

            greeting.AddOption("What is your health?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My health is sustained by the power of the dark forces.")));
                });

            greeting.AddOption("What do you do?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I delve into the secrets of necromancy and control the undead.")));
                });

            greeting.AddOption("Tell me about dark arts.",
                player => true,
                player => 
                {
                    DialogueModule darkArtsModule = new DialogueModule("Dark arts are not for the faint of heart. Are you brave enough to learn?");
                    darkArtsModule.AddOption("Yes, I'm brave!",
                        p => true,
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Then prove your dedication by completing this dark task.")));
                        });
                    player.SendGump(new DialogueGump(player, darkArtsModule));
                });

            greeting.AddOption("What is necromancy?",
                player => true,
                player => 
                {
                    DialogueModule necromancyModule = new DialogueModule("Necromancy, the art of communing with and controlling the dead. There is power beyond imagination for those who dare. Would you like to learn more?");
                    necromancyModule.AddOption("Yes, tell me more.",
                        p => true,
                        p => 
                        {
                            DialogueModule moreInfoModule = new DialogueModule("Necromancers wield the forces of life and death. They are feared and often misunderstood. Many seek the power to raise the fallen, but it comes with a heavy price. Are you prepared for such a burden?");
                            moreInfoModule.AddOption("I am ready for the burden.",
                                pl => true,
                                pl => 
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well! Your journey begins with the study of dark tomes and forbidden rituals.")));
                                });
                            moreInfoModule.AddOption("Maybe another time.",
                                pl => true,
                                pl => 
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Take your time. The path of a necromancer is not to be taken lightly.")));
                                });
                            p.SendGump(new DialogueGump(p, moreInfoModule));
                        });
                    player.SendGump(new DialogueGump(player, necromancyModule));
                });

            greeting.AddOption("Do you have a task for me?",
                player => true,
                player => 
                {
                    DialogueModule taskModule = new DialogueModule("Deep in the Whispering Woods, there lies a crypt. Retrieve the Black Orb resting there, and bring it to me. Do this, and you shall receive what you seek.");
                    taskModule.AddOption("What will I receive?",
                        p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Ah, curious about your reward? Complete the task, and you'll find out. Rest assured, it will be worth your effort.")));
                            lastRewardTime = DateTime.UtcNow; // Update lastRewardTime
                        });
                    taskModule.AddOption("I am not ready for such a task.",
                        p => true,
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Very well. Return when you are prepared.")));
                        });
                    taskModule.AddOption("What dangers lie ahead?",
                        p => true,
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("The crypt is filled with restless spirits and dark creatures that guard the orb. Use caution, and your wits will serve you well.")));
                        });
                    player.SendGump(new DialogueGump(player, taskModule));
                });

            greeting.AddOption("What about the undead?",
                player => true,
                player => 
                {
                    DialogueModule undeadModule = new DialogueModule("The undead are misunderstood beings, merely tools to be wielded. With the right incantations, they can be made to serve any purpose. Would you like to learn how?");
                    undeadModule.AddOption("Yes, teach me!",
                        pl => true,
                        pl => 
                        {
                            DialogueModule learningModule = new DialogueModule("To control the undead, one must first learn the words of power. Study the ancient scripts, and perform the rituals. Do you wish to start this path?");
                            learningModule.AddOption("I wish to begin.",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Very well. Your journey into the unknown starts now. Be cautious, for knowledge can be a double-edged sword.")));
                                });
                            learningModule.AddOption("I'm not sure yet.",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Think carefully. The path you choose will shape your destiny.")));
                                });
                            pl.SendGump(new DialogueGump(pl, learningModule));
                        });
                    player.SendGump(new DialogueGump(player, undeadModule));
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
