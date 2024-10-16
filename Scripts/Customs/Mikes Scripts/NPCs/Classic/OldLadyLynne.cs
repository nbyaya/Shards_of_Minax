using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Old Lady Lynne")]
    public class OldLadyLynne : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public OldLadyLynne() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Old Lady Lynne";
            Body = 0x191; // Human female body

            // Stats
            SetStr(32);
            SetDex(28);
            SetInt(25);
            SetHits(40);

            // Appearance
            AddItem(new Robe() { Hue = 11 }); // Robe with hue 11
            AddItem(new Sandals() { Hue = 1175 }); // Sandals with hue 1175

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public OldLadyLynne(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Old Lady Lynne, a humble beggar. My health? Oh, it's frail, but what can an old beggar expect? Lately, my digestion has been a troublesome affair.");

            greeting.AddOption("Tell me about your digestion problems.",
                player => true,
                player =>
                {
                    DialogueModule digestionModule = new DialogueModule("Ah, it's been a struggle. My stomach seems to rebel against even the simplest of foods. I have to stick to a rather unpleasant diet.");
                    digestionModule.AddOption("What kind of diet do you have to follow?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dietModule = new DialogueModule("I've been advised to consume bland foods, mostly rice and boiled vegetables. But even that can be tricky. Just the other day, I tried a new vegetable and it sent me running to the bushes!");
                            dietModule.AddOption("That sounds awful! Have you tried anything else?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule alternativesModule = new DialogueModule("Oh, I have! There was a time I thought I could handle some roasted chicken. But oh, how it disagreed with me! I swear it was plotting against me.");
                                    alternativesModule.AddOption("So what do you do for flavor?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule flavorModule = new DialogueModule("I try to sprinkle some herbs, but they don’t help much. A pinch of salt here and there, but it's all so bland. Sometimes I wonder if I’d prefer to taste the dish of a rat!");
                                            flavorModule.AddOption("Have you considered herbs?",
                                                plaw => true,
                                                plaw =>
                                                {
                                                    DialogueModule herbsModule = new DialogueModule("Ah, yes! I’ve heard that some herbs can soothe the stomach, like peppermint. But they can be a bit tricky to find in this area, and I’m not as spry as I once was.");
                                                    herbsModule.AddOption("Perhaps I could help you find some herbs.",
                                                        p => true,
                                                        p =>
                                                        {
                                                            p.SendMessage("You have my deepest gratitude! If you could find some peppermint and maybe a sprig of thyme, it would mean the world to me.");
                                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                        });
                                                    pla.SendGump(new DialogueGump(pla, herbsModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, flavorModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, alternativesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dietModule));
                        });
                    player.SendGump(new DialogueGump(player, digestionModule));
                });

            greeting.AddOption("How has your health affected your life?",
                player => true,
                player =>
                {
                    DialogueModule healthImpactModule = new DialogueModule("Oh, it has affected me deeply. I can no longer enjoy gatherings or feast with friends. Instead, I sit alone in my little corner, dreaming of the past.");
                    healthImpactModule.AddOption("That sounds lonely. What do you miss the most?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule missModule = new DialogueModule("I miss the joyous laughter and the lively dances. We used to gather around the fire, and my heart would soar with the music.");
                            missModule.AddOption("Why don’t you dance anymore?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule danceModule = new DialogueModule("Oh, I used to love dancing! But now, even the thought of moving too much makes my stomach churn. It’s a shame, really.");
                                    danceModule.AddOption("Maybe a gentle dance would do you good.",
                                        p => true,
                                        p =>
                                        {
                                            p.SendMessage("You think so? Perhaps it would lift my spirits! But what if my stomach protests?");
                                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                        });
                                    pla.SendGump(new DialogueGump(pla, danceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, missModule));
                        });
                    player.SendGump(new DialogueGump(player, healthImpactModule));
                });

            greeting.AddOption("Do you ever eat anything special?",
                player => true,
                player =>
                {
                    DialogueModule specialFoodModule = new DialogueModule("Only on rare occasions. There was a time I tried a slice of cake—a mistake that I still regret! It seemed so tempting, but it turned my insides upside down!");
                    specialFoodModule.AddOption("Why would you risk it?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule riskModule = new DialogueModule("Sometimes, temptation overwhelms reason, you see. I miss the taste of sweet things. But every time I indulge, I pay dearly for it.");
                            riskModule.AddOption("It must be hard to resist.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Oh, indeed! I often sit by the baker’s window, inhaling the delicious scents. It’s torture, really. But I remind myself of the consequences.");
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, riskModule));
                        });
                    player.SendGump(new DialogueGump(player, specialFoodModule));
                });

            greeting.AddOption("What about your love for Sir Reginald?",
                player => true,
                player =>
                {
                    DialogueModule loveReginaldModule = new DialogueModule("Ah, Sir Reginald. He was my heart’s desire! He used to dance with me, making every step feel light. But alas, he is gone, lost to the chaos of battle.");
                    loveReginaldModule.AddOption("Do you think he would be proud of you?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule prideModule = new DialogueModule("I believe he would. I’ve tried to live my life with kindness, despite the struggles. It’s what he would have wanted.");
                            prideModule.AddOption("You have a beautiful spirit.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("Thank you, kind traveler! Your words are a balm to my soul.");
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, prideModule));
                        });
                    player.SendGump(new DialogueGump(player, loveReginaldModule));
                });

            greeting.AddOption("Would you like any help with your diet?",
                player => true,
                player =>
                {
                    DialogueModule helpModule = new DialogueModule("Oh, if only someone would help me navigate this dreadful diet! I feel lost in a sea of tasteless food.");
                    helpModule.AddOption("What can I do to assist you?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule assistModule = new DialogueModule("I could use a hand finding suitable foods or even someone to share a meal with. Cooking for one is dreary!");
                            assistModule.AddOption("I can help you gather some food.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("You would do that? Bless your heart! If you could find some fresh vegetables and perhaps some mint, it would brighten my day!");
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            assistModule.AddOption("I could join you for a meal.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("How lovely! It would warm my heart to share a meal with someone. But be warned, it may not be the most appetizing affair.");
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, assistModule));
                        });
                    player.SendGump(new DialogueGump(player, helpModule));
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
