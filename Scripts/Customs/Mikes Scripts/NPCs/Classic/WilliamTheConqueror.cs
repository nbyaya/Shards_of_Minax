using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of William the Conqueror")]
    public class WilliamTheConqueror : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WilliamTheConqueror() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "William the Conqueror";
            Body = 0x190; // Human male body

            // Stats
            SetStr(130);
            SetDex(70);
            SetInt(70);
            SetHits(100);

            // Appearance
            AddItem(new ChainLegs() { Hue = 1910 });
            AddItem(new ChainChest() { Hue = 1910 });
            AddItem(new ChainCoif() { Hue = 1910 });
            AddItem(new PlateGloves() { Hue = 1910 });
            AddItem(new Boots() { Hue = 1910 });
            AddItem(new VikingSword() { Name = "William's Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am William the Conqueror, the one they fear and despise. What brings you to my presence?");

            greeting.AddOption("Tell me about your conquests.",
                player => true,
                player => 
                {
                    DialogueModule conquestsModule = new DialogueModule("Ah, my conquests! England was a land of savages, ripe for the taking. I led my men across the sea to claim what was rightfully ours. The battles were fierce, but victory was inevitable.");
                    conquestsModule.AddOption("What was your strategy?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule strategyModule = new DialogueModule("I employed tactics of surprise and cunning. The element of surprise is a powerful weapon. We attacked under the cover of darkness and struck fear into their hearts.");
                            strategyModule.AddOption("Did you face many enemies?",
                                p => true,
                                p => 
                                {
                                    DialogueModule enemiesModule = new DialogueModule("Indeed! The savages fought fiercely, but their disarray worked to our advantage. We rallied our troops and outmaneuvered them on the battlefield.");
                                    enemiesModule.AddOption("What about their leaders?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule leadersModule = new DialogueModule("Their leaders were strong but lacked the unity to inspire their people. We exploited their weaknesses, turning their own against them.");
                                            leadersModule.AddOption("How did you gain the loyalty of your men?",
                                                pw => true,
                                                pw =>
                                                {
                                                    DialogueModule loyaltyModule = new DialogueModule("I led by example, showing my strength in battle. I rewarded bravery and punished cowardice. Loyalty is earned through respect and fear.");
                                                    loyaltyModule.AddOption("What rewards did you offer?",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            loyaltyModule.AddOption("Gold and land for the brave!",
                                                                pr => true,
                                                                pr => 
                                                                {
                                                                    p.SendGump(new DialogueGump(p, new DialogueModule("Those who fought valiantly were given spoils beyond their dreams. Gold, land, titles—riches for those who stood beside me.")));
                                                                });
                                                            loyaltyModule.AddOption("Honor and glory!",
                                                                pt => true,
                                                                pt => 
                                                                {
                                                                    p.SendGump(new DialogueGump(p, new DialogueModule("Each warrior knew they would be sung about in tales for generations. Honor is worth more than gold.")));
                                                                });
                                                        });
                                                    p.SendGump(new DialogueGump(p, loyaltyModule));
                                                });
                                            p.SendGump(new DialogueGump(p, leadersModule));
                                        });
                                    p.SendGump(new DialogueGump(p, enemiesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, strategyModule));
                        });

                    conquestsModule.AddOption("What were the greatest battles?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule battlesModule = new DialogueModule("The Battle of Hastings was the defining moment. The savages underestimated my strength. They charged, but we held our ground and struck with precision.");
                            battlesModule.AddOption("What did you learn from that battle?",
                                p => true,
                                p =>
                                {
                                    battlesModule.AddOption("Never underestimate your enemy.",
                                        pla => true,
                                        pla => 
                                        {
                                            p.SendGump(new DialogueGump(p, new DialogueModule("Indeed! Every foe has strengths; the wise general prepares for them.")));
                                        });
                                    battlesModule.AddOption("Always have a backup plan.",
                                        pls => true,
                                        pls => 
                                        {
                                            p.SendGump(new DialogueGump(p, new DialogueModule("Absolutely! Plans can falter, but adaptability is the hallmark of a true conqueror.")));
                                        });
                                });
                            player.SendGump(new DialogueGump(player, battlesModule));
                        });

                    player.SendGump(new DialogueGump(player, conquestsModule));
                });

            greeting.AddOption("Do you fear anyone?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Fear? Only the Gods can inspire fear in me. Mortals are but fleeting shadows in the face of my destiny.")));
                });

            greeting.AddOption("What of loyalty?",
                player => true,
                player => 
                {
                    DialogueModule loyaltyModule = new DialogueModule("Loyalty is a precious commodity. Those who show true loyalty are worth their weight in gold. But loyalty can be fickle, much like the wind.");
                    loyaltyModule.AddOption("How do you maintain loyalty?",
                        pl => true,
                        pl =>
                        {
                            loyaltyModule.AddOption("Through rewards and fear!",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I reward those who stand by me, but I also ensure that the consequences for betrayal are well known.")));
                                });
                            loyaltyModule.AddOption("By earning their respect!",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Respect is earned through action, both in peace and war. I strive to lead by example.")));
                                });
                        });
                    player.SendGump(new DialogueGump(player, loyaltyModule));
                });

            greeting.AddOption("What do you think of the current state of England?",
                player => true,
                player => 
                {
                    DialogueModule stateModule = new DialogueModule("England is strong, but it still bears the scars of our conflict. The land is united under my rule, yet the hearts of its people remain restless.");
                    stateModule.AddOption("What do you plan to do next?",
                        pl => true,
                        pl =>
                        {
                            stateModule.AddOption("Further expansion!",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("There are lands to the north and south that still wait for a ruler strong enough to claim them.")));
                                });
                            stateModule.AddOption("Consolidate power!",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I must ensure that my rule is secure before reaching for more. A strong foundation is key.")));
                                });
                        });
                    player.SendGump(new DialogueGump(player, stateModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Farewell, and may you conquer your challenges.")));
                });

            return greeting;
        }

        public WilliamTheConqueror(Serial serial) : base(serial) { }

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
