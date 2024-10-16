using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Potionmaster Pia")]
    public class PotionmasterPia : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PotionmasterPia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Potionmaster Pia";
            Body = 0x191; // Human female body

            // Stats
            Str = 83;
            Dex = 39;
            Int = 123;
            Hits = 74;

            // Appearance
            AddItem(new PlainDress() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 38 });
            AddItem(new LeatherGloves() { Name = "Pia's Pouch Protecting Gloves" });

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
            DialogueModule greeting = new DialogueModule("I am Potionmaster Pia, the alchemist extraordinaire! What brings you to my humble abode? Perhaps you want to discuss my favorite potion ingredient—garlic!");

            greeting.AddOption("Tell me about your obsession with garlic.",
                player => true,
                player =>
                {
                    DialogueModule garlicObsessionModule = new DialogueModule("Ah, garlic! The wonder herb! It's not just an ingredient; it's the essence of life itself! You see, garlic is known for its numerous health benefits and magical properties. What would you like to know about it?");
                    
                    garlicObsessionModule.AddOption("What are the health benefits of garlic?",
                        p => true,
                        p =>
                        {
                            SendResponse("Garlic boosts the immune system, fights infections, and even improves cardiovascular health. It's also said to ward off evil spirits—how fascinating is that?");
                            p.SendGump(new DialogueGump(p, garlicObsessionModule));
                        });
                    
                    garlicObsessionModule.AddOption("Can garlic be used in potions?",
                        p => true,
                        p =>
                        {
                            DialogueModule potionUsageModule = new DialogueModule("Absolutely! Garlic is a key ingredient in many potions. Its properties enhance the effects of healing brews and can amplify the power of potions to combat darkness.");
                            potionUsageModule.AddOption("What kind of potions?",
                                pl => true,
                                pl =>
                                {
                                    SendResponse("Potions of Strength, Vitality, and even Anti-Vampire Elixirs! Garlic has a unique ability to draw out negativity, making it invaluable in potion-making.");
                                    pl.SendGump(new DialogueGump(pl, potionUsageModule));
                                });
                            potionUsageModule.AddOption("Do you have a favorite garlic potion?",
                                pl => true,
                                pl =>
                                {
                                    SendResponse("I do! My Garlic Infusion is my personal favorite. It’s known to restore vitality and has a delightful aroma. Would you like to learn how to make it?");
                                    pl.SendGump(new DialogueGump(pl, potionUsageModule));
                                });
                            p.SendGump(new DialogueGump(p, potionUsageModule));
                        });
                    
                    garlicObsessionModule.AddOption("Is garlic hard to find?",
                        p => true,
                        p =>
                        {
                            SendResponse("Not at all! It can be found in most gardens, but the finest garlic comes from the enchanted fields near the Whispering Forest. That’s where the best alchemical qualities reside!");
                            p.SendGump(new DialogueGump(p, garlicObsessionModule));
                        });

                    garlicObsessionModule.AddOption("Why do you think garlic is superior to other ingredients?",
                        p => true,
                        p =>
                        {
                            SendResponse("Garlic is versatile! While many ingredients have specific uses, garlic can be combined with almost anything. It’s the cornerstone of many potions, enhancing their effects and making them more potent.");
                            p.SendGump(new DialogueGump(p, garlicObsessionModule));
                        });

                    garlicObsessionModule.AddOption("Maybe I should try using garlic in my own potions.",
                        p => true,
                        p =>
                        {
                            SendResponse("Yes! Embrace the power of garlic! If you need help, I can provide a recipe or two to get you started. Are you interested in that?");
                            p.SendGump(new DialogueGump(p, garlicObsessionModule));
                        });

                    player.SendGump(new DialogueGump(player, garlicObsessionModule));
                });

            greeting.AddOption("What potions do you sell?",
                player => true,
                player =>
                {
                    SendResponse("I have a variety of potions, but my specialty is in garlic-infused potions! Would you like to browse my collection?");
                    // Implement the shop opening logic here
                });

            greeting.AddOption("Do you have any garlic-based recipes?",
                player => true,
                player =>
                {
                    DialogueModule recipesModule = new DialogueModule("I have several! The most popular ones include Garlic Infusion and Garlic Elixir. Would you like to hear about them?");
                    
                    recipesModule.AddOption("Yes, tell me about the Garlic Infusion.",
                        pl => true,
                        pl =>
                        {
                            SendResponse("The Garlic Infusion requires fresh garlic cloves, a sprig of rosemary, and a bottle of spring water. Simply blend the ingredients together and let it steep for a day.");
                            pl.SendGump(new DialogueGump(pl, recipesModule));
                        });

                    recipesModule.AddOption("What about the Garlic Elixir?",
                        pl => true,
                        pl =>
                        {
                            SendResponse("The Garlic Elixir is a bit more complex! It requires aged garlic, dragon's blood resin, and a hint of moonlit dew. It’s great for boosting energy!");
                            pl.SendGump(new DialogueGump(pl, recipesModule));
                        });

                    player.SendGump(new DialogueGump(player, recipesModule));
                });

            greeting.AddOption("What do you think about other ingredients?",
                player => true,
                player =>
                {
                    DialogueModule otherIngredientsModule = new DialogueModule("Well, other ingredients have their place, but none can compare to garlic's versatility. For instance, while some herbs may provide flavor, they lack the potency of garlic. Would you agree?");
                    
                    otherIngredientsModule.AddOption("I agree, garlic is unmatched!",
                        pl => true,
                        pl =>
                        {
                            SendResponse("Indeed! Garlic's effects are unparalleled! It is a true marvel of nature. Every alchemist should have it as their cornerstone ingredient.");
                            pl.SendGump(new DialogueGump(pl, otherIngredientsModule));
                        });

                    otherIngredientsModule.AddOption("What about potions made from other herbs?",
                        pl => true,
                        pl =>
                        {
                            SendResponse("Some herbs can be quite potent, like mandrake or belladonna, but they come with risks. Garlic, on the other hand, is safe and beneficial!");
                            pl.SendGump(new DialogueGump(pl, otherIngredientsModule));
                        });

                    player.SendGump(new DialogueGump(player, otherIngredientsModule));
                });

            greeting.AddOption("Why do you think garlic wards off evil spirits?",
                player => true,
                player =>
                {
                    SendResponse("Garlic has been considered a protective herb throughout history! It’s said to repel dark entities and negative energies. This makes it perfect for alchemy focused on healing and protection.");
                });

            greeting.AddOption("Can you tell me a story about garlic?",
                player => true,
                player =>
                {
                    SendResponse("Ah, there’s a tale of a great battle where warriors used garlic to protect themselves from a vampire lord. Their bravery, coupled with garlic, turned the tide of the battle! Such is the power of this humble herb.");
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player => { Say("Farewell, traveler! May the essence of garlic guide your path!"); });

            return greeting;
        }

        private void SendResponse(string message)
        {
            Say(message);
        }

        public PotionmasterPia(Serial serial) : base(serial) { }

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
