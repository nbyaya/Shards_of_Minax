using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sassy Selena")]
    public class SassySelena : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SassySelena() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sassy Selena";
            Body = 0x191; // Human female body

            // Stats
            SetStr(85);
            SetDex(75);
            SetInt(55);
            SetHits(65);

            // Appearance
            AddItem(new LeatherArms() { Hue = 2956 });
            AddItem(new Kilt() { Hue = 2957 });
            AddItem(new ThighBoots() { Hue = 2960 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
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
            DialogueModule greeting = new DialogueModule("Greetings, sweet traveler. I am Sassy Selena, a courtesan of the night. Recently, my beloved blueberry crop has been devastated by a dreadful drought. How may I share my sorrow with you?");

            greeting.AddOption("Tell me about your blueberry crop.",
                player => true,
                player =>
                {
                    DialogueModule cropModule = new DialogueModule("Ah, my precious blueberries! They were the pride of my garden, sweet and succulent. But the relentless sun has scorched them, leaving nothing but withered leaves and dry earth. Would you care to hear more about my struggles?");
                    cropModule.AddOption("Yes, what happened?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule storyModule = new DialogueModule("I had nurtured those bushes for years, watching them flourish. This year, however, the rains never came. As the days grew hotter, I tried everything I could think of—watering by hand, seeking out hidden springs—but to no avail.");
                            storyModule.AddOption("That sounds heartbreaking.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            storyModule.AddOption("Have you considered planting drought-resistant crops?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule solutionModule = new DialogueModule("I've thought about it! Perhaps I could plant more resilient varieties next season. But it pains me to abandon my beloved blueberries. Do you think I should consider a change?");
                                    solutionModule.AddOption("Sometimes change is necessary.",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    solutionModule.AddOption("I believe in sticking with your passion.",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    pla.SendGump(new DialogueGump(pla, solutionModule));
                                });
                            player.SendGump(new DialogueGump(player, storyModule));
                        });
                    cropModule.AddOption("How do you cope with this loss?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule copingModule = new DialogueModule("Coping is difficult. I find solace in my friends and the stories I tell. Sometimes, I even imagine my blueberries singing songs in the breeze. It brings me comfort, but the ache remains. Would you like to share a tale of your own?");
                            copingModule.AddOption("Yes, I have a tale.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            copingModule.AddOption("Not at this moment.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, copingModule));
                        });
                    player.SendGump(new DialogueGump(player, cropModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
                player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                player =>
                {
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    player.AddToBackpack(new BeggingAugmentCrystal()); // Give the reward
                    player.SendMessage("Here you go, dear traveler. This essence will help rejuvenate your spirit. Use it wisely.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            greeting.AddOption("What do you think about beauty?",
                player => true,
                player =>
                {
                    DialogueModule beautyModule = new DialogueModule("Beauty is not just about good health, but also about the secrets you keep to maintain it. Care to learn some of mine?");
                    beautyModule.AddOption("Yes, I would love to learn.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, beautyModule));
                });

            greeting.AddOption("What tales do you have?",
                player => true,
                player =>
                {
                    DialogueModule talesModule = new DialogueModule("Every tale holds a lesson, every adventure a memory. I have a keepsake from one such tale. Would you like to see?");
                    talesModule.AddOption("Yes, show me the keepsake.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateKeepsakeModule()));
                        });
                    player.SendGump(new DialogueGump(player, talesModule));
                });

            return greeting;
        }

        private DialogueModule CreateKeepsakeModule()
        {
            DialogueModule keepsakeModule = new DialogueModule("This is a locket, a memory of a past love. It reminds me of the fleeting nature of time. Life is but a series of moments. Cherish them.");
            keepsakeModule.AddOption("Thank you for sharing.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });
            return keepsakeModule;
        }

        public SassySelena(Serial serial) : base(serial) { }

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
