using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Spectral Sarah")]
    public class SpectralSarah : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SpectralSarah() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Spectral Sarah";
            Body = 0x191; // Human female body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 100;
            Hits = 90;

            // Appearance
            AddItem(new PlainDress() { Hue = 64 });
            AddItem(new ThighBoots() { Hue = 38 });
            AddItem(new BoneGloves() { Name = "Sarah's Spirit Siphons" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public SpectralSarah(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Spectral Sarah, a student of the darkest arts. Do you wish to hear about the finest delicacies of the spectral realm?");

            greeting.AddOption("Yes, tell me about the best food.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

            greeting.AddOption("What else do you know?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateGeneralKnowledgeModule())); });

            return greeting;
        }

        private DialogueModule CreateFoodModule()
        {
            DialogueModule foodModule = new DialogueModule("In the spectral realm, we indulge in foods that tantalize both the body and the soul. What aspect of our cuisine interests you?");

            foodModule.AddOption("What are the signature dishes?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateSignatureDishesModule())); });

            foodModule.AddOption("Are there any drinks?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDrinksModule())); });

            foodModule.AddOption("What about desserts?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDessertsModule())); });

            return foodModule;
        }

        private DialogueModule CreateSignatureDishesModule()
        {
            DialogueModule signatureDishes = new DialogueModule("Our signature dish is the *Ethereal Feast*, a blend of ingredients that shimmer with spectral energy. It invigorates the spirit!");

            signatureDishes.AddOption("What ingredients are used?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEtherealFeastIngredientsModule())); });

            signatureDishes.AddOption("Is it hard to prepare?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEtherealFeastPreparationModule())); });

            signatureDishes.AddOption("Tell me about another dish.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateOtherDishesModule())); });

            return signatureDishes;
        }

        private DialogueModule CreateEtherealFeastIngredientsModule()
        {
            DialogueModule ingredientsModule = new DialogueModule("To create the Ethereal Feast, you'll need Spectral Vegetables, Phantasmal Herbs, and a hint of Ghostly Essence. Each ingredient adds a unique flavor that dances on the palate!");

            ingredientsModule.AddOption("Where can I find these ingredients?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateIngredientLocationsModule())); });

            ingredientsModule.AddOption("Sounds delicious! What about cooking it?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEtherealFeastPreparationModule())); });

            return ingredientsModule;
        }

        private DialogueModule CreateIngredientLocationsModule()
        {
            DialogueModule locationsModule = new DialogueModule("The Spectral Vegetables can be found in the Phantom Gardens, while Phantasmal Herbs grow near the Haunted Lake. Ghostly Essence is harvested during the full moon in the Lost Valley.");

            locationsModule.AddOption("I’ll go searching for them!",
                player => true,
                player => { player.SendMessage("You set off on your quest for ingredients!"); });

            locationsModule.AddOption("Perhaps another time.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

            return locationsModule;
        }

        private DialogueModule CreateEtherealFeastPreparationModule()
        {
            DialogueModule prepModule = new DialogueModule("The preparation involves a delicate balance of energies. You must first purify the ingredients under moonlight, then mix them while chanting an ancient incantation. Timing is crucial!");

            prepModule.AddOption("That sounds fascinating!",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

            return prepModule;
        }

        private DialogueModule CreateOtherDishesModule()
        {
            DialogueModule otherDishesModule = new DialogueModule("Another popular dish is *Wraith's Delight*, made from Phantom Fish caught in the Shadowy Waters. It’s served with a side of Twilit Greens.");

            otherDishesModule.AddOption("What makes it special?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateWraithsDelightSpecialtyModule())); });

            otherDishesModule.AddOption("I’d love to try it!",
                player => true,
                player => { player.SendMessage("You express your desire to try Wraith's Delight!"); });

            return otherDishesModule;
        }

        private DialogueModule CreateWraithsDelightSpecialtyModule()
        {
            DialogueModule specialtyModule = new DialogueModule("Wraith's Delight is infused with the essence of sorrow, giving it a unique taste that resonates with the soul. It’s said to evoke visions of the past!");

            specialtyModule.AddOption("How can I prepare it?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateWraithsDelightPreparationModule())); });

            return specialtyModule;
        }

        private DialogueModule CreateWraithsDelightPreparationModule()
        {
            DialogueModule prepModule = new DialogueModule("To prepare Wraith's Delight, you must catch the Phantom Fish under the cover of darkness, then marinate it in a blend of ethereal spices before grilling it over a spectral flame.");

            prepModule.AddOption("I’ll remember that!",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateFoodModule())); });

            return prepModule;
        }

        private DialogueModule CreateDrinksModule()
        {
            DialogueModule drinksModule = new DialogueModule("We serve a variety of spectral beverages, such as *Ectoplasmic Elixir* and *Phantom Brew*. Each offers a unique experience that refreshes the spirit.");

            drinksModule.AddOption("What’s in the Ectoplasmic Elixir?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEctoplasmicElixirModule())); });

            drinksModule.AddOption("Tell me about Phantom Brew.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreatePhantomBrewModule())); });

            return drinksModule;
        }

        private DialogueModule CreateEctoplasmicElixirModule()
        {
            DialogueModule elixirModule = new DialogueModule("The Ectoplasmic Elixir is made from distilled spirit essence and infused with echoes of the past. It glows softly and gives the drinker visions of bygone days.");

            elixirModule.AddOption("How is it made?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateEctoplasmicElixirPreparationModule())); });

            return elixirModule;
        }

        private DialogueModule CreateEctoplasmicElixirPreparationModule()
        {
            DialogueModule prepModule = new DialogueModule("To create the elixir, you must gather spirit essence from a Spectral Wraith and distill it under a full moon, adding whispers of the past as a flavoring.");

            prepModule.AddOption("I’ll try to find those ingredients!",
                player => true,
                player => { player.SendMessage("You decide to embark on a quest for the ingredients!"); });

            return prepModule;
        }

        private DialogueModule CreatePhantomBrewModule()
        {
            DialogueModule brewModule = new DialogueModule("Phantom Brew is a refreshing drink made from spectral hops and flavored with shadowberries. It’s popular among the denizens of the spectral realm!");

            brewModule.AddOption("Where can I find shadowberries?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateShadowberryLocationModule())); });

            return brewModule;
        }

        private DialogueModule CreateShadowberryLocationModule()
        {
            DialogueModule locationModule = new DialogueModule("Shadowberries grow near the Abyssal Glade, but beware! The area is guarded by spectral beasts that may not take kindly to intruders.");

            locationModule.AddOption("I’ll be cautious while searching.",
                player => true,
                player => { player.SendMessage("You resolve to be careful on your journey for shadowberries!"); });

            return locationModule;
        }

        private DialogueModule CreateDessertsModule()
        {
            DialogueModule dessertsModule = new DialogueModule("We have exquisite desserts like *Phantom Pies* and *Wraith Whip*. They are crafted to delight both the eyes and the senses.");

            dessertsModule.AddOption("What’s in a Phantom Pie?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreatePhantomPieModule())); });

            return dessertsModule;
        }

        private DialogueModule CreatePhantomPieModule()
        {
            DialogueModule pieModule = new DialogueModule("Phantom Pies are made with spectral fruits and a hint of ethereal cream, baked until golden. They are said to vanish as you eat them!");

            pieModule.AddOption("That sounds amazing!",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDessertsModule())); });

            return pieModule;
        }

        private DialogueModule CreateGeneralKnowledgeModule()
        {
            DialogueModule knowledgeModule = new DialogueModule("I possess knowledge of the darkest arts and the secrets of life and death. What would you like to know?");

            knowledgeModule.AddOption("What are the dark arts?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateDarkArtsModule())); });

            knowledgeModule.AddOption("Tell me about the undead.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateUndeadModule())); });

            return knowledgeModule;
        }

        private DialogueModule CreateDarkArtsModule()
        {
            DialogueModule darkArtsModule = new DialogueModule("The dark arts involve manipulating the forces of life and death. It requires dedication and an understanding of the balance between realms.");

            darkArtsModule.AddOption("How can I learn them?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateLearningDarkArtsModule())); });

            return darkArtsModule;
        }

        private DialogueModule CreateLearningDarkArtsModule()
        {
            DialogueModule learningModule = new DialogueModule("To learn the dark arts, one must find a mentor and study ancient tomes hidden in forgotten places. It's a treacherous path, but the rewards can be great.");

            learningModule.AddOption("I’ll seek a mentor!",
                player => true,
                player => { player.SendMessage("You commit to finding a mentor in the dark arts!"); });

            return learningModule;
        }

        private DialogueModule CreateUndeadModule()
        {
            DialogueModule undeadModule = new DialogueModule("The undead are the spirits of those who have not moved on. They serve me loyally, bound to this realm by their unfulfilled desires.");

            undeadModule.AddOption("Can they be freed?",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateUndeadFreedomModule())); });

            return undeadModule;
        }

        private DialogueModule CreateUndeadFreedomModule()
        {
            DialogueModule freedomModule = new DialogueModule("To free the undead, one must fulfill their last wishes or find a powerful artifact that can sever their bonds. It's a task that requires great courage and insight.");

            freedomModule.AddOption("That sounds like a noble quest.",
                player => true,
                player => { player.SendGump(new DialogueGump(player, CreateGeneralKnowledgeModule())); });

            return freedomModule;
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
