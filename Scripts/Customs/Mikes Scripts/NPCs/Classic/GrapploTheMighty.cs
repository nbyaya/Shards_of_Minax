using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Grapplo the Mighty")]
    public class GrapploTheMighty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrapploTheMighty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Grapplo the Mighty";
            Body = 0x190; // Human male body

            // Stats
            SetStr(150);
            SetDex(100);
            SetInt(60);
            SetHits(100);

            // Appearance
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Equipment
            AddItem(new FancyShirt() { Hue = 2126 });
            AddItem(new ShortPants() { Hue = 2126 });
            AddItem(new ThighBoots() { Hue = 1904 });
            AddItem(new PlateGloves() { Name = "Grapplo's Gloves" });
        }

        public GrapploTheMighty(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("I am Grapplo the Mighty, the undisputed champion of the wrestling ring! What brings you to my domain?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthModule()))
            );

            greeting.AddOption("What is your job?",
                player => true,
                player => 
                {
                    DialogueModule jobModule = new DialogueModule("I am a professional wrestler, and I live for the thrill of the fight! Are you here to test your strength?");
                    jobModule.AddOption("Yes, I want to wrestle!",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateWrestleModule()))
                    );
                    jobModule.AddOption("No, I have other matters to attend to.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateGreetingModule()))
                    );
                    player.SendGump(new DialogueGump(player, jobModule));
                }
            );

            greeting.AddOption("What do you mean by 'mighty'?",
                player => true,
                player => 
                {
                    DialogueModule mightyModule = new DialogueModule("Ah, 'mighty' is not just a title, but a legacy. Passed down from my ancestors, who were all champions in their own right!");
                    mightyModule.AddOption("Tell me more about your ancestors.",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, CreateAncestorsModule()))
                    );
                    player.SendGump(new DialogueGump(player, mightyModule));
                }
            );

            greeting.AddOption("What parts of the body do you enjoy gripping?",
                player => true,
                player => 
                {
                    DialogueModule gripModule = new DialogueModule("Ah, the art of gripping is essential in wrestling! Different parts provide different advantages. Let me share my favorites with you.");
                    gripModule.AddOption("Tell me about the arms.",
                        p => true,
                        p =>
                        {
                            DialogueModule armsModule = new DialogueModule("The arms are crucial for both offense and defense. A strong grip on the opponent's arm can control their movements. It's satisfying to twist and turn them to your advantage!");
                            armsModule.AddOption("How do you grip the arms?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule gripDetailModule = new DialogueModule("When gripping an opponent's arm, I prefer to use a technique called the 'Arm Drag.' It allows me to maneuver them off-balance, setting me up for a follow-up move. Have you tried this technique?");
                                    gripDetailModule.AddOption("Yes, it's a great move!",
                                        plq => true,
                                        plq => pl.SendGump(new DialogueGump(pl, CreateGreetingModule()))
                                    );
                                    gripDetailModule.AddOption("No, but I’d like to learn more.",
                                        plw => true,
                                        plw => 
                                        {
                                            DialogueModule learnModule = new DialogueModule("I can show you some basics! The key is to maintain a strong grip while using your body weight to throw them off balance. Want to practice with me?");
                                            learnModule.AddOption("Yes, let's practice!",
                                                pe => true,
                                                pe => 
                                                {
                                                    p.SendMessage("You and Grapplo practice the Arm Drag technique together.");
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                }
                                            );
                                            learnModule.AddOption("Maybe another time.",
                                                pr => true,
                                                pr => p.SendGump(new DialogueGump(p, CreateGreetingModule()))
                                            );
                                            pl.SendGump(new DialogueGump(pl, learnModule));
                                        }
                                    );
                                    pl.SendGump(new DialogueGump(pl, gripDetailModule));
                                }
                            );
                            armsModule.AddOption("What about the legs?",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule legsModule = new DialogueModule("Gripping the legs is equally important! A well-timed takedown can bring an opponent down swiftly. I enjoy the challenge of grappling their legs!");
                                    legsModule.AddOption("How do you grip the legs?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule legGripDetailModule = new DialogueModule("I often use the 'Leg Sweep' technique. It involves sweeping the leg out from under them while maintaining a firm grip on their body. Have you ever tried this?");
                                            legGripDetailModule.AddOption("Yes, it's effective!",
                                                plat => true,
                                                plat => pla.SendGump(new DialogueGump(pla, CreateGreetingModule()))
                                            );
                                            legGripDetailModule.AddOption("No, tell me more!",
                                                play => true,
                                                play => 
                                                {
                                                    DialogueModule learnLegsModule = new DialogueModule("To perform the Leg Sweep, you need to time it perfectly with your opponent's movement. It's all about anticipating their next move. Want to try it out?");
                                                    learnLegsModule.AddOption("Absolutely, let's give it a shot!",
                                                        pu => true,
                                                        pu => 
                                                        {
                                                            p.SendMessage("You and Grapplo practice the Leg Sweep technique.");
                                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                        }
                                                    );
                                                    learnLegsModule.AddOption("Not right now, thanks.",
                                                        pi => true,
                                                        pi => p.SendGump(new DialogueGump(p, CreateGreetingModule()))
                                                    );
                                                    pla.SendGump(new DialogueGump(pla, learnLegsModule));
                                                }
                                            );
                                            pla.SendGump(new DialogueGump(pla, legGripDetailModule));
                                        }
                                    );
                                    legsModule.AddOption("What about the torso?",
                                        pla => true,
                                        pla => 
                                        {
                                            DialogueModule torsoModule = new DialogueModule("The torso provides a larger surface area for gripping, making it both advantageous and challenging. A strong grip here can lead to various holds!");
                                            torsoModule.AddOption("How do you grip the torso?",
                                                plo => true,
                                                plo => 
                                                {
                                                    DialogueModule torsoGripDetailModule = new DialogueModule("I often employ the 'Bear Hug' technique. It allows me to squeeze my opponent, limiting their movement. Have you ever experienced this move?");
                                                    torsoGripDetailModule.AddOption("Yes, it's quite effective!",
                                                        plp => true,
                                                        plp => pl.SendGump(new DialogueGump(pl, CreateGreetingModule()))
                                                    );
                                                    torsoGripDetailModule.AddOption("No, but I want to learn!",
                                                        plaz => true,
                                                        plaz => 
                                                        {
                                                            DialogueModule torsoLearnModule = new DialogueModule("To execute the Bear Hug, you need to close the distance and secure your arms around their torso. It's about control! Want to give it a try?");
                                                            torsoLearnModule.AddOption("Yes, let's practice!",
                                                                ps => true,
                                                                ps => 
                                                                {
                                                                    p.SendMessage("You and Grapplo practice the Bear Hug technique.");
                                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                }
                                                            );
                                                            torsoLearnModule.AddOption("Not today, thank you.",
                                                                pd => true,
                                                                pd => p.SendGump(new DialogueGump(p, CreateGreetingModule()))
                                                            );
                                                            pl.SendGump(new DialogueGump(pl, torsoLearnModule));
                                                        }
                                                    );
                                                    pl.SendGump(new DialogueGump(pl, torsoGripDetailModule));
                                                }
                                            );
                                            pla.SendGump(new DialogueGump(pla, torsoModule));
                                        }
                                    );
                                    pl.SendGump(new DialogueGump(pl, legsModule));
                                }
                            );
                            p.SendGump(new DialogueGump(p, armsModule));
                        }
                    );
                    player.SendGump(new DialogueGump(player, gripModule));
                }
            );

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            DialogueModule healthModule = new DialogueModule("My health is always at its peak, ready for a challenge! Do you doubt my abilities?");
            healthModule.AddOption("Not at all! You're a champion!",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule()))
            );
            healthModule.AddOption("I do have my doubts.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule()))
            );
            return healthModule;
        }

        private DialogueModule CreateWrestleModule()
        {
            DialogueModule wrestleModule = new DialogueModule("Excellent! Prepare yourself for a showdown of epic proportions! Do you have what it takes to step into the ring with me?");
            wrestleModule.AddOption("I'm ready!",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule()))
            );
            wrestleModule.AddOption("Not right now.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule()))
            );
            return wrestleModule;
        }

        private DialogueModule CreateAncestorsModule()
        {
            DialogueModule ancestorsModule = new DialogueModule("My ancestors were warriors, gladiators, and champions of old. Their legacy lives within me. Would you like to know more about them or perhaps receive a reward for your interest?");
            
            ancestorsModule.AddOption("Yes, I would love a reward!",
                player => (DateTime.Now - lastRewardTime).TotalMinutes >= 10,
                player =>
                {
                    player.AddToBackpack(new StatCapDeed());
                    lastRewardTime = DateTime.Now;
                    player.SendMessage("You have received a reward from Grapplo the Mighty!");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
            );

            ancestorsModule.AddOption("What if I want a reward now?",
                player => (DateTime.Now - lastRewardTime).TotalMinutes < 10,
                player =>
                {
                    player.SendMessage("Please wait a bit longer before asking for another reward.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
            );

            return ancestorsModule;
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
