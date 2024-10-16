using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Louis Riel")]
    public class LouisRiel : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LouisRiel() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Louis Riel";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(85);
            SetInt(95);
            SetHits(75);

            // Appearance
            AddItem(new LongPants() { Hue = 1105 });
            AddItem(new Doublet() { Hue = 1140 });
            AddItem(new Boots() { Hue = 1170 });
            AddItem(new WideBrimHat() { Hue = 1140 });

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
            DialogueModule greeting = new DialogueModule("Oh, another curious traveler, I suppose. What do you want?");
            
            greeting.AddOption("Tell me your name.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateNameModule())));

            greeting.AddOption("How is your health?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthModule())));

            greeting.AddOption("What do you do?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateJobModule())));

            greeting.AddOption("Tell me about battles.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateBattlesModule())));

            greeting.AddOption("What about redemption?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateRedemptionModule())));

            return greeting;
        }

        private DialogueModule CreateNameModule()
        {
            DialogueModule nameModule = new DialogueModule("My name? I'm Louis Riel. But a name is just a label in this world, isn't it?");
            nameModule.AddOption("Tell me more about your legend.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            return nameModule;
        }

        private DialogueModule CreateHealthModule()
        {
            DialogueModule healthModule = new DialogueModule("My health? Why do you care? I'm not your healer, am I?");
            healthModule.AddOption("You seem troubled.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateTroubledHealthModule())));
            return healthModule;
        }

        private DialogueModule CreateTroubledHealthModule()
        {
            DialogueModule troubledHealthModule = new DialogueModule("Yes, the weight of the past is heavy. But what of you? Do you carry burdens of your own?");
            troubledHealthModule.AddOption("I have my struggles.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateStrugglesModule())));
            troubledHealthModule.AddOption("What can I do to help?",
                player => true,
                player => {
                    troubledHealthModule.AddOption("I could gather herbs or potions for you.",
                        pl => true,
                        pl => {
                            // Implement quest or task for the player
                            pl.SendMessage("Louis considers your offer.");
                        });
                    player.SendGump(new DialogueGump(player, troubledHealthModule));
                });
            return troubledHealthModule;
        }

        private DialogueModule CreateStrugglesModule()
        {
            return new DialogueModule("Struggles are a part of life, but some are harder to bear than others. Tell me, what weighs on your heart?");
        }

        private DialogueModule CreateJobModule()
        {
            DialogueModule jobModule = new DialogueModule("My job? What a laugh! I'm just another pawn in this wretched world's game.");
            jobModule.AddOption("Do you regret it?",
                player => true,
                player => {
                    jobModule.AddOption("Sometimes. But there are moments of joy.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateJoyfulMomentsModule())));
                    jobModule.AddOption("Not at all. It shapes who I am.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateNoRegretModule())));
                    player.SendGump(new DialogueGump(player, jobModule));
                });
            return jobModule;
        }

        private DialogueModule CreateJoyfulMomentsModule()
        {
            return new DialogueModule("Ah, those fleeting moments when hope shines through the darkness. Perhaps we should cherish them more.");
        }

        private DialogueModule CreateNoRegretModule()
        {
            return new DialogueModule("Every choice, every hardship, has led me to this moment. I wouldn't change a thing.");
        }

        private DialogueModule CreateBattlesModule()
        {
            DialogueModule battlesModule = new DialogueModule("Valor? Ha! There's no valor in a world full of deceit and betrayal. But enough about me, what about you?");
            battlesModule.AddOption("I've faced my share of battles.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateYourBattlesModule())));
            battlesModule.AddOption("Tell me about your battles.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateMyBattlesModule())));
            return battlesModule;
        }

        private DialogueModule CreateYourBattlesModule()
        {
            return new DialogueModule("Each battle shapes us, doesn't it? What have you fought for? Is it honor, revenge, or perhaps something more?");
        }

        private DialogueModule CreateMyBattlesModule()
        {
            DialogueModule myBattlesModule = new DialogueModule("I've fought for causes I believed in, yet often found myself surrounded by betrayal. It is a harsh lesson in trust.");
            myBattlesModule.AddOption("What did you learn from it?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateBattleLessonsModule())));
            return myBattlesModule;
        }

        private DialogueModule CreateBattleLessonsModule()
        {
            return new DialogueModule("Trust is fragile, and loyalty is rare. Sometimes, the fiercest battles are fought within ourselves.");
        }

        private DialogueModule CreateRedemptionModule()
        {
            DialogueModule redemptionModule = new DialogueModule("Redemption is a tricky thing. Some say it's never too late, others believe some deeds can never be undone. What's your take on forgiveness?");
            redemptionModule.AddOption("Forgiveness is essential.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateForgivenessModule())));
            redemptionModule.AddOption("I don't believe in redemption.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            return redemptionModule;
        }

        private DialogueModule CreateForgivenessModule()
        {
            DialogueModule forgivenessModule = new DialogueModule("Yes, it can lighten the soul. But can we ever truly forgive ourselves? That’s a burden many carry.");
            forgivenessModule.AddOption("How do you seek forgiveness?",
                player => true,
                player => {
                    forgivenessModule.AddOption("By making amends and helping others.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateMakingAmendsModule())));
                    forgivenessModule.AddOption("I struggle with it.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateStruggleForgivenessModule())));
                    player.SendGump(new DialogueGump(player, forgivenessModule));
                });
            return forgivenessModule;
        }

        private DialogueModule CreateMakingAmendsModule()
        {
            return new DialogueModule("That is noble. Helping others is a step towards healing the wounds of the past.");
        }

        private DialogueModule CreateStruggleForgivenessModule()
        {
            return new DialogueModule("It's a difficult path, but recognizing our mistakes is the first step. You are not alone in this journey.");
        }

        private DialogueModule CreateCurseModule()
        {
            DialogueModule curseModule = new DialogueModule("The curse? It's not just of ill health. It's the curse of memories, of dreams lost. But for your kindness in asking, take this reward. It might help you on your journey.");

            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                curseModule.AddOption("What about the reward?",
                    player => true,
                    player => player.SendGump(new DialogueGump(player, CreateNoRewardModule())));
            }
            else
            {
                curseModule.AddOption("Thank you for the reward!",
                    player => true,
                    player => {
                        player.AddToBackpack(new MaxxiaScroll()); // Adjust the item type as needed
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    });
            }

            return curseModule;
        }

        private DialogueModule CreateNoRewardModule()
        {
            return new DialogueModule("I have no reward right now. Please return later.");
        }

        public LouisRiel(Serial serial) : base(serial) { }

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
