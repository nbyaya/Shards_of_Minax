using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kael Thunderarrow")]
    public class KaelThunderarrow : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KaelThunderarrow() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kael Thunderarrow";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Dex = 90;
            Int = 60;
            Hits = 120;

            // Appearance
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Tunic() { Hue = 1108 });
            AddItem(new Boots() { Hue = 1106 });
            AddItem(new Crossbow() { Name = "Kael's Crossbow" });

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
            DialogueModule greeting = new DialogueModule("I am Kael Thunderarrow, the archer of virtue! What brings you to this part of the world?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("I have always maintained good health. My training keeps me fit and ready."))));

            greeting.AddOption("What is your job?",
                player => true,
                player => player.SendGump(new DialogueGump(player, new DialogueModule("My job is to protect the virtues with my keen archery skills. Each arrow I shoot is guided by the principles I uphold."))));

            greeting.AddOption("What do you think of the virtues?",
                player => true,
                player =>
                {
                    DialogueModule virtuesModule = new DialogueModule("The virtue of Humility is often overlooked, but it's the foundation of all virtues. Do you agree?");
                    virtuesModule.AddOption("Yes, humility is important.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule followUp = new DialogueModule("Your response is wise. Humility allows us to learn and grow. What do you think is the next most important virtue?");
                            followUp.AddOption("Compassion.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Compassion is indeed vital. It helps us connect with others and understand their pain."))));
                            followUp.AddOption("Courage.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Courage is the force that drives us to face our fears and stand for what's right."))));
                            pl.SendGump(new DialogueGump(pl, followUp));
                        });
                    virtuesModule.AddOption("No, I have my doubts.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("That's a different perspective. Care to elaborate on why you feel that way?"))));
                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("What can you tell me about patience?",
                player => true,
                player =>
                {
                    DialogueModule patienceModule = new DialogueModule("Patience is not merely waiting; it's about how we behave while we're waiting. It's essential for mastering any skill. Do you value patience?");
                    patienceModule.AddOption("Yes, it's crucial.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("It's a virtue that many overlook. True patience brings peace and understanding."))));
                    patienceModule.AddOption("No, it's just wasting time.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule response = new DialogueModule("Interesting viewpoint. Some believe that patience is a sign of weakness. What would you suggest instead?");
                            response.AddOption("Quick action.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Ah, a decisive approach. There’s merit in swift actions, but be cautious—hasty decisions can lead to regret."))));
                            response.AddOption("Calculating strategies.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! A balance of patience and strategy often yields the best outcomes."))));
                            pl.SendGump(new DialogueGump(pl, response));
                        });
                    player.SendGump(new DialogueGump(player, patienceModule));
                });

            greeting.AddOption("What about focus?",
                player => true,
                player =>
                {
                    DialogueModule focusModule = new DialogueModule("With focus, one can aim true and never miss the target. Even beyond archery, it helps us concentrate on our goals. Have you ever lost focus?");
                    focusModule.AddOption("No, I stay committed.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("That's commendable! Staying true to one's path is essential."))));
                    focusModule.AddOption("Yes, frequently.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule response = new DialogueModule("Losing focus can be detrimental. What distracts you the most?");
                            response.AddOption("Social media.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Ah, the lure of instant gratification. It's easy to lose track of time. Perhaps setting boundaries could help."))));
                            response.AddOption("Personal doubts.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Those can be challenging. Remember, self-reflection can guide you back on track."))));
                            pl.SendGump(new DialogueGump(pl, response));
                        });
                    player.SendGump(new DialogueGump(player, focusModule));
                });

            greeting.AddOption("Tell me about compassion.",
                player => true,
                player =>
                {
                    DialogueModule compassionModule = new DialogueModule("Compassion is understanding and caring for the pain and joy of others. Would you like to hear more about it?");
                    compassionModule.AddOption("Yes, please.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("Compassion binds us all in this vast world. It’s essential to extend a helping hand, for the ripple of one act can touch countless souls."))));
                    compassionModule.AddOption("No, not really.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("That's fine. Just know the virtues are always there for guidance. Perhaps another time."))));
                    player.SendGump(new DialogueGump(player, compassionModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        player.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                        player.SendGump(new DialogueGump(player, new DialogueModule("For your thoughtful inquiry, please accept this reward. It’s a small token of appreciation.")));
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            return greeting;
        }

        public KaelThunderarrow(Serial serial) : base(serial) { }

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
