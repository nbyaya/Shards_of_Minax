using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Joan of Arc")]
    public class LadyJoanOfArc : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyJoanOfArc() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Joan of Arc";
            Body = 0x191; // Human female body

            // Stats
            Str = 140;
            Dex = 40;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new PlateLegs() { Hue = 38 });
            AddItem(new PlateChest() { Hue = 38 });
            AddItem(new PlateHelm() { Hue = 38 });
            AddItem(new PlateGloves() { Hue = 38 });
            AddItem(new PlateArms() { Hue = 38 });
            AddItem(new PlateGorget() { Hue = 38 });
            AddItem(new Boots() { Hue = 38 });
            AddItem(new Halberd() { Name = "Lady Joan of Arc's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue;
        }

        public LadyJoanOfArc(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Joan of Arc. How may I assist you?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => 
                {
                    DialogueModule healthModule = new DialogueModule("My health is quite robust, for I strive to maintain a body and spirit in harmony.");
                    healthModule.AddOption("That's good to hear.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    healthModule.AddOption("What do you do to stay healthy?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule healthTipsModule = new DialogueModule("I practice both physical training and meditation. A balanced diet and plenty of water are crucial as well. Would you like to know more about training or meditation?");
                            healthTipsModule.AddOption("Tell me about your training.",
                                p => true,
                                p =>
                                {
                                    DialogueModule trainingModule = new DialogueModule("Training is vital for a protector like myself. I engage in various forms of combat training, including swordplay and archery. Have you ever trained in combat yourself?");
                                    trainingModule.AddOption("Yes, I have.",
                                        plq => true,
                                        plq => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    trainingModule.AddOption("No, but I wish to learn.",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule learnCombatModule = new DialogueModule("It's never too late to start! I could recommend a few trainers in the area. Would you like their names?");
                                            learnCombatModule.AddOption("Yes, please!",
                                                pe => true,
                                                pe => 
                                                {
                                                    p.SendMessage("I recommend seeking out Master Atreus or Lady Selene. They are both skilled and can provide guidance.");
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            learnCombatModule.AddOption("Maybe another time.",
                                                pr => true,
                                                pr => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, learnCombatModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, trainingModule));
                                });
                            healthTipsModule.AddOption("What about meditation?",
                                p => true,
                                p =>
                                {
                                    DialogueModule meditationModule = new DialogueModule("Meditation allows me to center my thoughts and emotions. I often meditate at dawn to greet the new day. Have you tried meditating before?");
                                    meditationModule.AddOption("Yes, it helps me as well.",
                                        plt => true,
                                        plt => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    meditationModule.AddOption("No, but I’m curious.",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule meditationExplainModule = new DialogueModule("It involves finding a quiet place, closing your eyes, and focusing on your breath. It can be quite calming. Would you like to try it?");
                                            meditationExplainModule.AddOption("I would like to try.",
                                                pu => true,
                                                pu => 
                                                {
                                                    p.SendMessage("Close your eyes, take a deep breath, and let your worries fade away for a moment.");
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            meditationExplainModule.AddOption("Not right now.",
                                                pi => true,
                                                pi => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, meditationExplainModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, meditationModule));
                                });
                            player.SendGump(new DialogueGump(player, healthTipsModule));
                        });
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player => 
                {
                    DialogueModule jobModule = new DialogueModule("My noble calling is that of a protector. I defend the weak and uphold justice. Would you like to hear more about my adventures or my duties?");
                    jobModule.AddOption("Tell me about your adventures.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule adventureModule = new DialogueModule("I have faced many trials, from battling marauding brigands to aiding villagers in distress. Each adventure teaches me valuable lessons. Do you have a tale of your own?");
                            adventureModule.AddOption("Yes, I have an adventure to share.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("I would love to hear it!");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Placeholder for player storytelling
                                });
                            adventureModule.AddOption("Not at the moment.",
                                p => true,
                                p => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            player.SendGump(new DialogueGump(player, adventureModule));
                        });
                    jobModule.AddOption("What about your duties?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule dutyModule = new DialogueModule("My duties involve protecting the innocent, providing guidance, and sometimes engaging in combat to ensure peace. It is a heavy burden, but one I bear with pride. How do you feel about duty?");
                            dutyModule.AddOption("Duty is important to me.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("I admire your conviction. Duty often shapes our character and actions.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            dutyModule.AddOption("I struggle with duty.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("It is natural to feel that way. Perhaps reflecting on your responsibilities will bring clarity.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, dutyModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What is valor?",
                player => true,
                player =>
                {
                    DialogueModule valorModule = new DialogueModule("Valor is a virtue of great importance. It is the strength of one's heart that shines in times of adversity. What does valor mean to you?");
                    valorModule.AddOption("To me, it's about courage.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule courageModule = new DialogueModule("Indeed, courage is a vital component of valor. It's about facing fears and challenges head-on. Have you faced a fear recently?");
                            courageModule.AddOption("Yes, I confronted my fear of heights.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("That is commendable! Conquering fears is a great way to build character.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            courageModule.AddOption("No, but I want to.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Facing fears often brings growth. I encourage you to take small steps.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, courageModule));
                        });
                    valorModule.AddOption("It's about standing up for what is right.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule rightModule = new DialogueModule("Absolutely! Standing for justice often requires great bravery. Have you ever had to stand up for someone else?");
                            rightModule.AddOption("Yes, I defended a friend.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("That is noble! Protecting others is the essence of valor.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            rightModule.AddOption("No, but I would like to.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("There may come a time when you must rise for someone else. Be ready.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, rightModule));
                        });
                    player.SendGump(new DialogueGump(player, valorModule));
                });

            greeting.AddOption("Do you have any wisdom to share?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                        return;
                    }

                    DialogueModule wisdomModule = new DialogueModule("Wisdom is the compass that guides us through the labyrinth of life. As a token of my appreciation for seeking wisdom, please accept this gift.");
                    player.AddToBackpack(new PeacemakingAugmentCrystal());
                    lastRewardTime = DateTime.UtcNow;
                    wisdomModule.AddOption("Thank you, Lady Joan.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, wisdomModule));
                });

            greeting.AddOption("What do you think about life?",
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("Life is the greatest journey we will ever embark upon. Every step, every decision molds our path. Tell me, traveler, what drives you forward in this journey?");
                    lifeModule.AddOption("I seek knowledge.",
                        pl => true,
                        pl => 
                        {
                            pl.SendMessage("Knowledge is a powerful motivator. It can lead to understanding and growth.");
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    lifeModule.AddOption("I seek adventure.",
                        pl => true,
                        pl => 
                        {
                            pl.SendMessage("Adventure shapes the spirit. It brings excitement and discovery to life.");
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
