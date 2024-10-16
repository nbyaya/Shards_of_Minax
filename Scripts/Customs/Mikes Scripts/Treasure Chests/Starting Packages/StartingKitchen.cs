using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    public class StartingKitchen : LockableContainer
    {
        [Constructable]
        public StartingKitchen() : base(0x9AB) // Cabinet item ID
        {
            Name = "Kitchen Cabinet";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add random kitchen items
            AddItemWithProbability(new CheeseWheel(), 0.20);
            AddItemWithProbability(new CookedBird(), 0.15);
            AddItemWithProbability(new Sausage(), 0.15);
            AddItemWithProbability(new SackFlour(), 0.10);
            AddItemWithProbability(new ApplePie(), 0.10);
            AddItemWithProbability(new Cake(), 0.10);
            AddItemWithProbability(new Muffins(), 0.10);
            AddItemWithProbability(new Pitcher(), 0.10);
            AddItemWithProbability(new Fork(), 0.10);
            AddItemWithProbability(new Spoon(), 0.10);
            AddItemWithProbability(new Knife(), 0.10);
            AddItemWithProbability(new RollingPin(), 0.05);
            AddItemWithProbability(new Skillet(), 0.05);
            AddItemWithProbability(new Goblet(), 0.05);
            AddItemWithProbability(new BottleOfWine(), 0.05);
            AddItemWithProbability(new Apple(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Banana(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Carrot(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Onion(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Pumpkin(), 0.07);
            AddItemWithProbability(new WoodenBowlOfPeas(), 0.05);
            AddItemWithProbability(new WoodenBowlOfCarrots(), 0.05);
            AddItemWithProbability(new WoodenBowlOfCorn(), 0.05);
            AddItemWithProbability(new WoodenBowlOfLettuce(), 0.05);
            AddItemWithProbability(new SackOfSugar(), 0.05);
            AddItemWithProbability(new SackFlour(), 0.05);
            AddItemWithProbability(new Watermelon(), 0.05);
            AddItemWithProbability(new Grapes(Utility.RandomMinMax(1, 5)), 0.05);
            AddItemWithProbability(new BreadLoaf(), 0.25);
            AddItemWithProbability(new CheeseWheel(), 0.20);
            AddItemWithProbability(new CookedBird(), 0.15);
            AddItemWithProbability(new Sausage(), 0.15);
            AddItemWithProbability(new SackFlour(), 0.10);
            AddItemWithProbability(new ApplePie(), 0.10);
            AddItemWithProbability(new Cake(), 0.10);
            AddItemWithProbability(new Muffins(), 0.10);
            AddItemWithProbability(new Pitcher(), 0.10);
            AddItemWithProbability(new Fork(), 0.10);
            AddItemWithProbability(new Spoon(), 0.10);
            AddItemWithProbability(new Knife(), 0.10);
            AddItemWithProbability(new RollingPin(), 0.05);
            AddItemWithProbability(new Skillet(), 0.05);
            AddItemWithProbability(new Goblet(), 0.05);
            AddItemWithProbability(new BottleOfWine(), 0.05);
            AddItemWithProbability(new Apple(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Banana(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Carrot(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Onion(Utility.RandomMinMax(1, 5)), 0.07);
            AddItemWithProbability(new Pumpkin(), 0.07);
            AddItemWithProbability(new WoodenBowlOfPeas(), 0.05);
            AddItemWithProbability(new WoodenBowlOfCarrots(), 0.05);
            AddItemWithProbability(new WoodenBowlOfCorn(), 0.05);
            AddItemWithProbability(new WoodenBowlOfLettuce(), 0.05);
            AddItemWithProbability(new SackOfSugar(), 0.05);
            AddItemWithProbability(new SackFlour(), 0.05);
            AddItemWithProbability(new Watermelon(), 0.05);
            AddItemWithProbability(new Grapes(Utility.RandomMinMax(1, 5)), 0.05);
			// Unique named kitchen items with random hues
			AddItemWithProbability(new BreadLoaf() { Name = "Artisan Bread", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new ApplePie() { Name = "Grandma's Apple Pie", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Cake() { Name = "Celebration Cake", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Muffins() { Name = "Blueberry Muffins", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new CheeseWheel() { Name = "Aged Cheese Wheel", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new CookedBird() { Name = "Roasted Pheasant", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bottle() { Name = "Vintage Wine Bottle", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bacon() { Name = "Crispy Bacon Strips", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Ham() { Name = "Honey-Glazed Ham", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Sausage() { Name = "Spicy Sausage", Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Pitcher() { Name = "Pitcher of Ale", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Pitcher() { Name = "Ornate Bowl", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Fork() { Name = "Silver Fork", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Spoon() { Name = "Golden Spoon", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Knife() { Name = "Carving Knife", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new RollingPin() { Name = "Wooden Rolling Pin", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Skillet() { Name = "Cast Iron Skillet", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Skillet() { Name = "Copper Frypan", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Pitcher() { Name = "Large Cooking Pot", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Goblet() { Name = "Jeweled Goblet", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new BottleOfWine() { Name = "Rare Wine Bottle", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Pitcher() { Name = "Ceremonial Cooking Pot", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Apple(Utility.RandomMinMax(1, 5)) { Name = "Golden Apple", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Banana(Utility.RandomMinMax(1, 5)) { Name = "Tropical Banana", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Carrot(Utility.RandomMinMax(1, 5)) { Name = "Fresh Carrot", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Onion(Utility.RandomMinMax(1, 5)) { Name = "Sweet Onion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Pumpkin() { Name = "Giant Pumpkin", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new WoodenBowlOfPeas() { Name = "Bowl of Sweet Peas", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new WoodenBowlOfCarrots() { Name = "Bowl of Diced Carrots", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new WoodenBowlOfCorn() { Name = "Bowl of Golden Corn", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new WoodenBowlOfLettuce() { Name = "Bowl of Crisp Lettuce", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new SackOfSugar() { Name = "Bag of Fine Sugar", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new SackFlour() { Name = "Sack of Premium Flour", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Pitcher() { Name = "Jar of Wild Honey", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Watermelon() { Name = "Sweet Watermelon", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new Grapes(Utility.RandomMinMax(1, 5)) { Name = "Bunch of Grapes", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
			AddItemWithProbability(new RandomFancyBakedGoods(), 0.05);
			AddItemWithProbability(new RandomFancyCheese(), 0.05);
			AddItemWithProbability(new RandomFancyDinner(), 0.10);
			AddItemWithProbability(new RandomFancyFish(), 0.05);
			AddItemWithProbability(new RandomFancyBakedGoods(), 0.05);
			AddItemWithProbability(new RandomFancyCheese(), 0.05);
			AddItemWithProbability(new RandomFancyDinner(), 0.10);
			AddItemWithProbability(new RandomFancyFish(), 0.05);

			
			            // Add personal notes
            AddItemWithProbability(CreateRecipeNote(), 0.20);
			AddItemWithProbability(CreateRecipeNote(), 0.20);
			AddItemWithProbability(CreateRecipeNote(), 0.20);
			AddItemWithProbability(CreateRecipeNote(), 0.20);
			AddItemWithProbability(CreateRecipeNote(), 0.20);

        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }
		
        private Item CreateRecipeNote()
        {
            string[] notes = new string[]
            {
                "To make the perfect loaf of bread, knead the dough for at least 10 minutes and let it rise until doubled in size.",
                "For a delicious apple pie, use a mix of tart and sweet apples and add a dash of cinnamon and nutmeg.",
                "When making soup, always start with a base of onions, carrots, and celery for the best flavor.",
                "To get crispy roast potatoes, parboil them first and then toss in hot oil before roasting.",
                "For a rich and creamy sauce, use a mixture of heavy cream and butter, and simmer until thickened.",
                "Always taste your food as you cook and adjust the seasoning with salt and pepper as needed.",
                "To make fluffy pancakes, don't overmix the batter; a few lumps are perfectly fine.",
                "For tender and juicy roasted chicken, baste it regularly with its own juices while it cooks.",
                "A pinch of sugar can balance out the acidity in tomato-based sauces.",
                "To keep herbs fresh for longer, store them in a glass of water in the refrigerator.",
                "For a perfectly cooked steak, sear it on high heat and then finish in the oven.",
                "When baking, make sure all ingredients are at room temperature before mixing.",
                "To caramelize onions, cook them slowly over low heat with a bit of salt and sugar.",
                "For the best mashed potatoes, use Yukon Gold potatoes and plenty of butter and cream.",
                "To prevent garlic from burning, add it to the pan after the onions have softened.",
                "For a flaky pie crust, keep the butter and water as cold as possible when mixing the dough.",
                "To make a simple vinaigrette, mix three parts oil to one part vinegar and season with salt and pepper.",
                "For extra crispy bacon, bake it in the oven on a wire rack.",
                "To prevent pasta from sticking, stir it occasionally while it cooks and don't rinse it after draining.",
                "For a rich and flavorful gravy, use the drippings from roasted meat and cook with flour and broth until thickened.",
				"To create enchanted bread that never molds, mix the dough with a pinch of pixie dust and bake under the light of a full moon.",
				"For a rejuvenating apple pie, use apples from the Grove of Renewal and add a drop of unicorn's tears to the filling.",
				"When preparing Dragonfire Stew, include a piece of dragon scale for a spicy kick and an immunity to fire for a short period.",
				"To brew a potion of endless energy, combine coffee beans from the Feywild with a splash of phoenix feather essence.",
				"For a cake that grants temporary flight, blend in ground griffon talons and bake with cloudberries harvested at dawn.",
				"To make invisible soup, stir in a shadow mushroom and serve in a black cauldron under a new moon.",
				"For an elixir of eternal youth, mix dew collected from the petals of the Everlasting Rose with honey from a royal bee.",
				"To craft a pie that induces prophetic dreams, include dreamberries picked under a crescent moon and sprinkle with stardust.",
				"For a dish that enhances strength, use ogre marrow in your meat pie and bake with a sprig of ironroot herb.",
				"To make a potion of underwater breathing, combine kelp from the Siren's Lagoon with a drop of mermaid's song.",
				"For a bread that ensures safe travels, add ground lodestone and bake with a charm blessed by a wandering sage.",
				"To prepare a feast that attracts good luck, include clover honey and a pinch of leprechaun gold in every dish.",
				"For a stew that heals wounds, mix troll fat with a broth made from water of the Healing Spring and simmer with moonwort leaves.",
				"To brew a tea that reveals hidden truths, steep leaves of the Truthseeker's Bush with a single phoenix down.",
				"For a salad that grants night vision, toss moonvine leaves with luminescent mushrooms from the Deep Forest.",
				"To make a potion of speed, blend quicksilver with juice from the Swiftfoot Berry and a dash of air elemental essence.",
				"For a bread that never stales, knead in a pinch of time dust collected from the Hourglass Desert.",
				"To create a roast that fortifies the body, stuff it with ginseng root and bake with a glaze made from dragon's blood.",
				"For a potion that calms the mind, mix lavender petals with essence of tranquility harvested from the Serene Valley.",
				"To make enchanted cookies that grant happiness, blend joyberries with a laugh from a giggling sprite and bake until golden.",
				"For a soup that restores mana, simmer manaweed with a dash of starflower nectar and a sprinkle of powdered moonstone.",
				"To craft a pie that wards off evil, use holy water in the crust and fill with blessed fruit from the Guardian's Orchard.",
				"For a drink that enhances wisdom, blend sage leaves with elderberry juice and a few drops of owl feather extract.",
				"To make candy that grants invisibility, melt sugar with shadow essence and mold under the cover of darkness.",
				"For a dish that boosts agility, marinate rabbit meat in quicksilver and serve with a side of speedroot salad.",
				"To prepare a feast that attracts prosperity, cook with golden apples and season with dust from a leprechaun's pot of gold.",
				"For a potion that enhances charisma, mix charmflower petals with the essence of a silver-tongued fox and a drop of siren's call.",
				"To bake a cake that brings peace, add serenity petals and a whisper of angel's breath to the batter.",
				"For a stew that enhances bravery, include lionheart root and a pinch of courage dust gathered from a hero's grave.",
				"To make bread that strengthens the soul, knead in essence of soulstone and bake with the flames of a guardian spirit.",
				"For a pie that induces joy, use joyberries and bake with a sprinkle of laughter from a mischievous faerie.",
				"To create a drink that inspires creativity, blend muse petals with stardust and a few drops of artist's passion.",
				"For a soup that provides protection, simmer shieldleaf with guardian's essence and a splash of dragon scale powder.",
				"To prepare enchanted pancakes, mix in pixie honey and cook on a griddle made from meteorite metal.",
				"For a potion that ensures restful sleep, combine dreamflower petals with a drop of moonbeam essence and a whisper of lullaby.",
				"To make cookies that boost intelligence, blend in intellect root and bake with a dash of scholar's ink.",
				"For a stew that enhances endurance, use stalwart root and simmer with a broth made from the Water of Life.",
				"To brew a tea that grants clarity, steep clarity leaves with crystal dew collected from the Clarity Falls.",
				"For a salad that enhances sight, toss visionberry leaves with luminescent carrots from the Cave of Light.",
				"To make a potion of levitation, blend floating fern with air elemental essence and a drop of cloud nectar.",
				"For a dish that ensures good health, cook with vitality root and season with a pinch of wellness dust from the Healing Glade.",
				"To create a roast that enhances magical power, stuff with mana root and bake with a glaze made from wizard's fire.",
				"For a potion that grants foresight, mix foresight petals with a single drop of oracle's eye and a whisper of prophecy.",
				"To make bread that brings harmony, knead in harmony dust and bake with the warmth of a friendship flame.",
				"For a pie that induces laughter, use giggleberries and bake with a sprinkle of joy from a laughing spirit.",
				"To craft a drink that enhances endurance, blend endurance leaves with a few drops of marathon runner's sweat and a pinch of grit.",
				"To make orcish stew, use a mixture of wild vegetables and chunks of meat, seasoned with a generous amount of bloodroot.",
				"Orcish bread is best made with coarse flour and a splash of ale for a hearty flavor.",
				"For a robust orcish ale, ferment wild berries and barley for several weeks until it reaches the desired strength.",
				"To prepare orcish roast boar, marinate the meat in a mixture of crushed garlic, wild herbs, and orcish stout before roasting.",
				"An orcish feast isn't complete without a side of spiced grubs, fried in boar fat until crispy.",
				"To make orcish blood pudding, mix fresh blood with oats and spices, then boil until thickened.",
				"For a savory orcish soup, use bone broth as a base and add chunks of meat, root vegetables, and a pinch of troll thyme.",
				"Orcish sausage is made by grinding meat with bloodroot and stuffing it into natural casings, then smoking over a low fire.",
				"To create a traditional orcish fish stew, use river fish, wild greens, and a splash of orcish grog.",
				"Orcish bone broth is best when simmered for several days with marrow bones and a handful of wild herbs.",
				"For a sweet orcish treat, try making honeyed insects by glazing them with honey and roasting until caramelized.",
				"Orcish mead is brewed with wild honey, water, and a handful of orcish hops for a bitter finish.",
				"To make orcish black pudding, mix blood with barley and spices, then fry until crisp on the outside.",
				"For a hearty orcish breakfast, serve a mix of fried boar slices, blood sausage, and spiced potatoes.",
				"Orcish warbread is dense and made with dark grains, perfect for sustaining energy during battle.",
				"To make orcish mushroom stew, gather a variety of wild mushrooms and cook with bone broth and a splash of grog.",
				"Orcish roasted vegetables are best when cooked over an open flame with plenty of salt and boar fat.",
				"For a tangy orcish sauce, ferment wild berries with garlic and bloodroot for several days.",
				"Orcish baked apples are a simple treat, stuffed with nuts and honey, then roasted until soft.",
				"To make orcish jerky, dry strips of meat with a mixture of salt and crushed bloodroot, then smoke until hard.",
				"For an orcish feast, serve whole roasted boar with sides of spiced grubs and wild greens.",
				"Orcish root mash is made by boiling root vegetables until soft, then mashing with boar fat and salt.",
				"To prepare orcish spiced fish, coat river fish with a blend of bloodroot and wild herbs before grilling.",
				"Orcish grog is a strong drink made by fermenting wild fruits and grains for a potent brew.",
				"For a rich orcish porridge, cook coarse grains with bone broth and a handful of wild berries.",
				"To make orcish stuffed mushrooms, fill large mushroom caps with a mixture of minced meat and herbs, then bake.",
				"Orcish berry pie is made with a thick crust of coarse flour and filled with wild berries and honey.",
				"To create orcish spiced nuts, roast a mixture of nuts with bloodroot and salt until crunchy.",
				"Orcish wild salad is a mix of foraged greens, nuts, and berries, tossed with a simple vinegar dressing.",
				"For a hearty orcish stew, use a variety of meats, wild vegetables, and a splash of strong ale.",
				"To make orcish garlic bread, spread roasted garlic and boar fat on thick slices of dark bread and bake.",
				"Orcish meatballs are made by mixing ground meat with bloodroot and herbs, then frying until golden.",
				"To prepare orcish herb paste, grind wild herbs with salt and boar fat for a flavorful spread.",
				"Orcish grilled vegetables are best when skewered and cooked over an open flame with a sprinkle of salt.",
				"For a savory orcish pie, fill a thick crust with a mixture of meat, root vegetables, and wild herbs.",
				"To make orcish fermented fish, pack river fish in salt and leave to ferment for several weeks.",
				"Orcish spiced tea is brewed with a blend of wild herbs and a pinch of bloodroot for a warming drink.",
				"To prepare orcish smoked meat, cure the meat with salt and bloodroot, then smoke over a low fire for several days.",
				"Orcish roasted nuts are a simple snack, seasoned with salt and bloodroot, then roasted until crunchy.",
				"For a sweet orcish beverage, mix wild honey with water and a splash of orcish grog.",
				"To make orcish stew, use a mixture of wild vegetables and chunks of meat, seasoned with a generous amount of bloodroot.",
				"Orcish bread is best made with coarse flour and a splash of ale for a hearty flavor.",
				"For a robust orcish ale, ferment wild berries and barley for several weeks until it reaches the desired strength.",
				"To prepare orcish roast boar, marinate the meat in a mixture of crushed garlic, wild herbs, and orcish stout before roasting.",
				"An orcish feast isn't complete without a side of spiced grubs, fried in boar fat until crispy.",
				"To make orcish blood pudding, mix fresh blood with oats and spices, then boil until thickened.",
				"For a savory orcish soup, use bone broth as a base and add chunks of meat, root vegetables, and a pinch of troll thyme.",
				"Orcish sausage is made by grinding meat with bloodroot and stuffing it into natural casings, then smoking over a low fire.",
				"To create a traditional orcish fish stew, use river fish, wild greens, and a splash of orcish grog.",
				"Orcish bone broth is best when simmered for several days with marrow bones and a handful of wild herbs.",
				"For a sweet orcish treat, try making honeyed insects by glazing them with honey and roasting until caramelized.",
				"Orcish mead is brewed with wild honey, water, and a handful of orcish hops for a bitter finish.",
				"To make orcish black pudding, mix blood with barley and spices, then fry until crisp on the outside.",
				"For a hearty orcish breakfast, serve a mix of fried boar slices, blood sausage, and spiced potatoes.",
				"Orcish warbread is dense and made with dark grains, perfect for sustaining energy during battle.",
				"To make orcish mushroom stew, gather a variety of wild mushrooms and cook with bone broth and a splash of grog.",
				"Orcish roasted vegetables are best when cooked over an open flame with plenty of salt and boar fat.",
				"For a tangy orcish sauce, ferment wild berries with garlic and bloodroot for several days.",
				"Orcish baked apples are a simple treat, stuffed with nuts and honey, then roasted until soft.",
				"To make orcish jerky, dry strips of meat with a mixture of salt and crushed bloodroot, then smoke until hard.",
				"For an orcish feast, serve whole roasted boar with sides of spiced grubs and wild greens.",
				"Orcish root mash is made by boiling root vegetables until soft, then mashing with boar fat and salt.",
				"To prepare orcish spiced fish, coat river fish with a blend of bloodroot and wild herbs before grilling.",
				"Orcish grog is a strong drink made by fermenting wild fruits and grains for a potent brew.",
				"For a rich orcish porridge, cook coarse grains with bone broth and a handful of wild berries.",
				"To make orcish stuffed mushrooms, fill large mushroom caps with a mixture of minced meat and herbs, then bake.",
				"Orcish berry pie is made with a thick crust of coarse flour and filled with wild berries and honey.",
				"To create orcish spiced nuts, roast a mixture of nuts with bloodroot and salt until crunchy.",
				"Orcish wild salad is a mix of foraged greens, nuts, and berries, tossed with a simple vinegar dressing.",
				"For a hearty orcish stew, use a variety of meats, wild vegetables, and a splash of strong ale.",
				"To make orcish garlic bread, spread roasted garlic and boar fat on thick slices of dark bread and bake.",
				"Orcish meatballs are made by mixing ground meat with bloodroot and herbs, then frying until golden.",
				"To prepare orcish herb paste, grind wild herbs with salt and boar fat for a flavorful spread.",
				"Orcish grilled vegetables are best when skewered and cooked over an open flame with a sprinkle of salt.",
				"For a savory orcish pie, fill a thick crust with a mixture of meat, root vegetables, and wild herbs.",
				"To make orcish fermented fish, pack river fish in salt and leave to ferment for several weeks.",
				"Orcish spiced tea is brewed with a blend of wild herbs and a pinch of bloodroot for a warming drink.",
				"To prepare orcish smoked meat, cure the meat with salt and bloodroot, then smoke over a low fire for several days.",
				"Orcish roasted nuts are a simple snack, seasoned with salt and bloodroot, then roasted until crunchy.",
				"For a sweet orcish beverage, mix wild honey with water and a splash of orcish grog.",
				"For a traditional Elvish lembas bread, mix honey and fine wheat flour, and bake until golden.",
				"Elvish berry tarts require the freshest forest berries and a light pastry crust.",
				"To make Elvish herbal tea, steep a blend of dried chamomile, mint, and elderflower in hot water.",
				"Elvish mushroom stew is best made with a variety of wild mushrooms and seasoned with thyme and sage.",
				"For a sweet treat, try Elvish honey cakes made with wildflower honey and almond flour.",
				"Elvish roast venison is marinated in a mixture of red wine, juniper berries, and rosemary.",
				"Elvish nut bread combines finely chopped hazelnuts, walnuts, and a touch of cinnamon.",
				"For a refreshing drink, Elvish fruit punch mixes fresh berry juice with sparkling spring water.",
				"Elvish vegetable soup includes root vegetables, leeks, and a hint of garlic and bay leaf.",
				"To make Elvish spiced cider, heat apple cider with cinnamon sticks, cloves, and a splash of orange juice.",
				"Elvish green salad is a mix of forest greens, edible flowers, and a light lemon vinaigrette.",
				"For a hearty meal, try Elvish barley and mushroom risotto cooked with vegetable broth.",
				"Elvish herbal bread is made with fresh rosemary, thyme, and a sprinkle of sea salt.",
				"Elvish fruit compote combines stewed apples, pears, and dried cranberries with a dash of nutmeg.",
				"For a special occasion, Elvish wine-braised pheasant is seasoned with juniper and lavender.",
				"Elvish spiced nuts are roasted with a blend of cinnamon, nutmeg, and a touch of honey.",
				"Elvish cheese and herb pastries feature a filling of creamy goat cheese and fresh dill.",
				"To make Elvish honey mead, ferment a mixture of honey, water, and a few drops of elvish nectar.",
				"Elvish roasted root vegetables are tossed with olive oil, rosemary, and sea salt before baking.",
				"Elvish wild rice pilaf includes wild rice, dried currants, and toasted pine nuts.",
				"Elvish almond cookies are made with ground almonds, honey, and a hint of lemon zest.",
				"For a refreshing soup, Elvish cucumber and mint soup is served chilled with a dollop of cream.",
				"Elvish berry preserve is made by simmering forest berries with honey and a splash of lemon juice.",
				"Elvish herbal quiche combines fresh spinach, leeks, and a blend of forest herbs in a flaky crust.",
				"Elvish elderberry wine is a favorite, made by fermenting elderberries with a touch of wildflower honey.",
				"Elvish spiced apple rings are dried apple slices seasoned with cinnamon and nutmeg.",
				"For a sweet and savory treat, Elvish cheese and pear tart is baked with a thin crust and a drizzle of honey.",
				"Elvish herbal broth is a clear soup made with fresh herbs, garlic, and a hint of ginger.",
				"Elvish oatcakes are a hearty snack, made with rolled oats, honey, and dried berries.",
				"Elvish lavender lemonade is a refreshing drink made with fresh lemon juice and a hint of lavender syrup.",
				"Elvish acorn squash is roasted and then filled with a mixture of wild rice, cranberries, and nuts.",
				"For a unique flavor, Elvish spiced pumpkin bread is made with fresh pumpkin puree and a blend of spices.",
				"Elvish chestnut soup is creamy and rich, made with roasted chestnuts and a hint of nutmeg.",
				"Elvish herbal jelly combines the flavors of mint, rosemary, and thyme with a touch of sweetness.",
				"Elvish carrot and coriander soup is a vibrant and flavorful starter, garnished with fresh coriander.",
				"Elvish fruit and nut bars are a perfect trail snack, made with dried fruits, nuts, and a touch of honey.",
				"For a festive treat, Elvish berry and cream trifle is layered with fresh berries, whipped cream, and sponge cake.",
				"Elvish apple and walnut salad combines crisp apples, toasted walnuts, and a tangy vinaigrette.",
				"Elvish herbal tea blend includes dried rose hips, chamomile, and a touch of peppermint.",
				"Elvish savory pastries are filled with wild mushrooms, leeks, and a blend of forest herbs.",
				"To make Elvish herbal honey, infuse wildflower honey with lavender, thyme, and a hint of sage.",
				"Elvish berry cobbler is a rustic dessert made with a mix of forest berries and a crumbly topping.",
				"Elvish spiced wine is a warm drink made by simmering red wine with cinnamon, cloves, and orange peel.",
				"For a hearty breakfast, Elvish porridge is cooked with oats, dried fruits, and a drizzle of honey.",
				"Elvish roasted chestnuts are a simple yet delicious snack, seasoned with sea salt.",
				"Elvish apple butter is made by slow-cooking apples with cinnamon and a touch of clove.",
				"Elvish leek and potato soup is a comforting dish, garnished with fresh chives and a dollop of cream.",
				"Elvish herb-crusted fish is baked with a coating of breadcrumbs, parsley, and lemon zest.",
				"For a sweet treat, Elvish honeycomb candy is made by boiling honey and sugar until golden and crispy.",
				"Elvish roasted nuts mix includes a variety of nuts toasted with a blend of forest spices.",
				"Elvish berry and nut bread is a dense loaf made with dried berries, nuts, and a hint of spice.",
				"To make Elvish herbal vinegar, infuse white wine vinegar with rosemary, thyme, and garlic.",
				"Elvish plum pudding is a festive dessert made with dried plums, spices, and a touch of brandy.",
				"Elvish forest tea is a blend of dried leaves, berries, and flowers found in the deepest parts of the forest.",
				"Elvish lavender shortbread is a delicate cookie made with fresh lavender and butter.",
				"Elvish spiced carrot cake is a moist and flavorful dessert made with grated carrots and warm spices.",
				"For a hearty meal, Elvish vegetable casserole includes a mix of seasonal vegetables and a savory sauce.",
				"Elvish berry jam is made by simmering forest berries with honey and a splash of lemon juice.",
				"Elvish spiced nuts are a popular snack, roasted with a blend of cinnamon, nutmeg, and allspice.",
				"Elvish honey-glazed ham is a festive dish, baked with a coating of honey and herbs.",
				"For a refreshing drink, Elvish mint julep is made with fresh mint, honey, and a splash of spring water.",
				"Elvish fruit salad includes a mix of fresh forest fruits and a drizzle of sweet honey dressing.",
				"Elvish vegetable medley is a colorful side dish made with a variety of roasted seasonal vegetables.",
				"Elvish herbal syrup is made by simmering fresh herbs with honey and water until thickened.",
				"Elvish nutty granola is a wholesome breakfast, made with rolled oats, nuts, and dried fruits.",
				"For a light snack, Elvish rice cakes are topped with a mix of fresh herbs and a drizzle of honey.",
				"Elvish berry sorbet is a refreshing dessert made with pureed forest berries and a touch of honey.",
				"Elvish spiced tea is a warm drink made by brewing black tea with cinnamon, cloves, and cardamom.",
				"Elvish vegetable stew is a hearty dish, made with root vegetables, beans, and a savory broth.",
				"Elvish lemon balm cookies are a delicate treat, made with fresh lemon balm and a hint of citrus.",
				"For a festive drink, Elvish mulled wine is made by simmering red wine with spices and orange peel.",
				"Elvish forest berry tart is a dessert made with a buttery crust and a filling of mixed forest berries.",
				"Elvish sweet potato pie is a rich dessert made with sweet potatoes, spices, and a flaky crust.",
				"Elvish cranberry relish is a tangy side dish made with fresh cranberries, honey, and orange zest.",
				"Elvish garlic and herb butter is a flavorful spread made with fresh garlic, herbs, and butter.",
				"For a sweet treat, Elvish almond brittle is made by boiling sugar and almonds until golden and crispy.",
				"Elvish fruit and nut cake is a dense loaf made with dried fruits, nuts, and a hint of spice.",
				"Elvish spiced cider is a warm drink made by simmering apple cider with cinnamon, cloves, and allspice.",
				"Elvish savory scones are made with fresh herbs, cheese, and a touch of buttermilk.",
				"Elvish berry and cream parfait is a layered dessert made with fresh berries, whipped cream, and granola.",
				"Elvish spiced pumpkin soup is a creamy dish made with fresh pumpkin and a blend of warm spices.",
				"Elvish herbal lemonade is a refreshing drink made with fresh lemon juice and a hint of mint syrup.",
				"Elvish nut and seed mix is a wholesome snack made with toasted nuts, seeds, and a touch of honey.",
				"For a light dessert, Elvish lemon sorbet is made with fresh lemon juice and a touch of honey.",
				"Elvish vegetable stir-fry is a quick dish made with seasonal vegetables and a savory sauce.",
				"Elvish honey and oat bars are a hearty snack made with rolled oats, honey, and dried fruits.",
				"Elvish herbal salt is made by mixing sea salt with dried herbs and a touch of garlic.",
				"Elvish roasted garlic is a flavorful addition to any meal, made by slow-roasting garlic bulbs with olive oil.",
				"Elvish spiced pear compote is a sweet side dish made with fresh pears, honey, and a blend of spices.",
				"Elvish ginger cookies are a warm and spicy treat made with fresh ginger and molasses.",
				"Elvish fruit and nut granola is a wholesome breakfast made with rolled oats, dried fruits, and nuts.",
				"Elvish herbal bread is a savory loaf made with fresh rosemary, thyme, and a touch of garlic.",
				"Elvish berry and nut salad is a light and refreshing dish made with mixed greens, fresh berries, and toasted nuts.",
				"For a festive treat, Elvish spiced cranberry cookies are made with fresh cranberries and a blend of warm spices.",
				"Elvish savory pancakes are made with fresh herbs, cheese, and a touch of buttermilk.",
				"Elvish honey-glazed carrots are a sweet and savory side dish made with fresh carrots and a honey glaze.",
				"Elvish lavender and honey cake is a delicate dessert made with fresh lavender and a touch of honey.",
				"Elvish spiced apple cider is a warm drink made by simmering apple cider with cinnamon, cloves, and orange peel.",
				"Elvish vegetable and herb frittata is a light and fluffy dish made with fresh vegetables and herbs.",
				"Elvish berry and almond tart is a sweet dessert made with fresh berries and a crumbly almond crust.",
				"Elvish nut and seed brittle is a crunchy snack made with toasted nuts, seeds, and a touch of honey.",
				"Elvish spiced hot chocolate is a warm drink made by mixing cocoa with cinnamon, cloves, and a touch of chili.",
				"Elvish herbal cream cheese is a flavorful spread made with fresh herbs, garlic, and cream cheese.",
				"Elvish berry and yogurt parfait is a light dessert made with fresh berries, yogurt, and granola.",
				"Elvish roasted vegetable medley is a colorful side dish made with a variety of seasonal vegetables and herbs.",
				"Elvish spiced carrot soup is a warm and hearty dish made with fresh carrots and a blend of spices.",
				"Elvish honey and herb roasted nuts are a savory snack made with a mix of nuts, honey, and fresh herbs.",
				"Elvish lemon and thyme cookies are a delicate treat made with fresh lemon zest and thyme.",
				"For a refreshing drink, Elvish elderflower cordial is made by steeping fresh elderflowers in sugar syrup.",
				"Elvish vegetable and barley soup is a hearty dish made with a mix of vegetables, barley, and a savory broth.",
				"Elvish spiced nut bread is a dense loaf made with nuts, spices, and a touch of honey.",
				"Elvish herbal honey is made by infusing wildflower honey with fresh herbs and a touch of citrus.",
				"Elvish berry and cream tart is a sweet dessert made with fresh berries and a creamy filling.",
				"Elvish roasted root vegetables are a hearty side dish made with a mix of root vegetables and fresh herbs.",
				"Elvish spiced nut mix is a savory snack made with a mix of nuts, seeds, and a blend of spices.",
				"Elvish lemon balm tea is a refreshing drink made by steeping fresh lemon balm leaves in hot water.",
				"Elvish vegetable and herb quiche is a light and savory dish made with fresh vegetables and herbs.",
				"Elvish berry and almond biscotti is a crunchy treat made with dried berries and toasted almonds.",
				"For a festive treat, Elvish spiced pumpkin muffins are made with fresh pumpkin puree and a blend of spices.",
				"Elvish honey and oat cookies are a hearty snack made with rolled oats, honey, and dried fruits.",
				"Elvish roasted garlic and herb dip is a flavorful spread made with roasted garlic, fresh herbs, and cream cheese.",
				"Elvish spiced hot cider is a warm drink made by simmering apple cider with cinnamon, cloves, and a touch of ginger.",
				"To create a gargoyle's strength potion, mix ground dragon scales with a drop of lava from the deepest volcano.",
				"Gargoyle bread is best baked using flour made from enchanted wheat and water from a sacred spring.",
				"For a hearty gargoyle stew, use chunks of manticore meat and a variety of root vegetables.",
				"To prepare gargoyle honey cakes, mix wild honey with ground stoneberries and bake until golden.",
				"A popular gargoyle drink is brewed by fermenting crushed fire blossoms with mountain spring water.",
				"To make gargoyle rock candy, boil sugar from enchanted sugarcane with shards of glowing crystals.",
				"Gargoyle roast is traditionally made with a whole dragonling, seasoned with volcanic salt and herbs.",
				"For a rich gargoyle soup, use a base of molten lava broth and add chunks of rock crab meat.",
				"Gargoyle pies are filled with a mixture of sweet cave fruits and a sprinkle of powdered moonstone.",
				"To make gargoyle ale, ferment a mix of crushed lava fruits and mountain herbs for several weeks.",
				"Gargoyle cheese is crafted from the milk of stone goats and aged in volcanic caves for a unique flavor.",
				"A favorite gargoyle dessert is lava pudding, made from ground lava rocks and enchanted sugar.",
				"For a spicy gargoyle dish, cook dragon peppers with fire lizard meat and serve over steamed fire grains.",
				"Gargoyle stew often includes a mix of lava fish, cave mushrooms, and a splash of enchanted water.",
				"To make gargoyle berry jam, use crushed stoneberries and a hint of fire blossom nectar.",
				"Gargoyle biscuits are best baked using enchanted flour and a pinch of ground dragon scales.",
				"For a refreshing gargoyle drink, mix crushed ice crystals with the juice of cave fruits.",
				"Gargoyle meatloaf is made from ground fire lizard meat and baked with volcanic herbs and spices.",
				"To create a gargoyle's endurance potion, blend ground manticore horn with a drop of sacred spring water.",
				"Gargoyle fish cakes are made by mixing ground lava fish with enchanted flour and frying in volcanic oil.",
				"A popular gargoyle snack is roasted fire nuts, seasoned with a sprinkle of volcanic salt.",
				"Gargoyle pancakes are made from a batter of enchanted flour, ground stoneberries, and wild honey.",
				"For a hearty gargoyle breakfast, serve fire grains cooked in molten lava broth with a side of cave fruit.",
				"Gargoyle roast is best when marinated in a mixture of volcanic herbs and enchanted spring water.",
				"To make gargoyle honey butter, blend wild honey with butter from stone goat milk.",
				"Gargoyle stone soup is made by boiling enchanted stones with a variety of root vegetables.",
				"A favorite gargoyle treat is fire fruit pie, made from sweet fire fruits and a flaky enchanted crust.",
				"To make gargoyle rock cakes, mix ground lava rocks with enchanted flour and bake until firm.",
				"Gargoyle tea is brewed by steeping fire blossom petals in boiling mountain spring water.",
				"For a savory gargoyle dish, cook fire lizard steaks with a side of roasted lava vegetables.",
				"Gargoyle custard is made from enchanted sugar, stone goat milk, and a dash of molten lava.",
				"To create a gargoyle's healing potion, combine ground moonstone with a drop of enchanted spring water.",
				"Gargoyle berry pie is made from a mix of sweet cave berries and a sprinkle of enchanted sugar.",
				"For a rich gargoyle broth, use a base of molten lava and add chunks of dragonling meat.",
				"Gargoyle roast is best served with a side of fire grains and a drizzle of wild honey.",
				"To make gargoyle spiced bread, mix enchanted flour with ground dragon peppers and bake until golden.",
				"Gargoyle milkshakes are made from stone goat milk, crushed ice crystals, and a hint of fire blossom nectar.",
				"For a hearty gargoyle meal, serve lava fish grilled with a side of roasted root vegetables.",
				"Gargoyle cookies are baked using enchanted flour, ground stoneberries, and wild honey.",
				"To make gargoyle berry scones, mix ground stoneberries with enchanted flour and bake until firm.",
				"Gargoyle stew is often made with a variety of lava meats, cave mushrooms, and a splash of molten lava broth.",
				"For a refreshing gargoyle drink, mix crushed cave fruits with mountain spring water and a hint of wild honey.",
				"Gargoyle pancakes are best served with a drizzle of fire blossom nectar and a sprinkle of ground moonstone.",
				"To make gargoyle meatballs, mix ground fire lizard meat with enchanted flour and fry in volcanic oil.",
				"Gargoyle bread is best when baked with a mixture of enchanted flour and volcanic herbs.",
				"For a rich gargoyle dessert, serve molten lava cake made from ground lava rocks and enchanted sugar.",
				"Gargoyle rock candy is made by boiling enchanted sugar with shards of glowing crystals until hard.",
				"To create a gargoyle's strength potion, mix ground dragon scales with a drop of lava from the deepest volcano.",
				"Gargoyle ale is best brewed using a mixture of crushed lava fruits and mountain herbs.",
				"For a hearty gargoyle dish, cook dragonling stew with a variety of root vegetables and molten lava broth.",
				"Gargoyle honey cakes are made from wild honey, ground stoneberries, and enchanted flour.",
				"To make gargoyle cheese, use milk from stone goats and age it in volcanic caves.",
				"Gargoyle soup is often made with a base of molten lava and chunks of rock crab meat.",
				"For a sweet gargoyle treat, serve fire fruit pie made from cave fruits and enchanted sugar."
            };

            return new SimpleNote
            {
                NoteString = notes[Utility.Random(notes.Length)],
                TitleString = "Recipe Note"
            };
        }

        public StartingKitchen(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
