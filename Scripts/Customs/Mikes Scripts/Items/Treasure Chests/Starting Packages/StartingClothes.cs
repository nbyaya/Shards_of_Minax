using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    public class StartingClothes : LockableContainer
    {
        [Constructable]
        public StartingClothes() : base(0xA4D) // Armoire item ID
        {
            Name = "Personal Armoire";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add random clothing and equipment
            AddItemWithProbability(Loot.RandomClothing(), 0.25);
            AddItemWithProbability(Loot.RandomArmor(), 0.15);
            AddItemWithProbability(Loot.RandomWeapon(), 0.10);
			AddItemWithProbability(Loot.RandomClothing(), 0.25);
            AddItemWithProbability(Loot.RandomArmor(), 0.15);
			AddItemWithProbability(Loot.RandomClothing(), 0.25);
			AddItemWithProbability(Loot.RandomArmor(), 0.15);
			AddItemWithProbability(Loot.RandomWeapon(), 0.10);
			AddItemWithProbability(Loot.RandomClothing(), 0.25);
			AddItemWithProbability(Loot.RandomArmor(), 0.15);
			AddItemWithProbability(Loot.RandomWeapon(), 0.10);
			AddItemWithProbability(Loot.RandomClothing(), 0.25);
			AddItemWithProbability(Loot.RandomArmor(), 0.15);
			AddItemWithProbability(Loot.RandomWeapon(), 0.10);
			AddItemWithProbability(Loot.RandomClothing(), 0.25);
			AddItemWithProbability(Loot.RandomArmor(), 0.15);
			AddItemWithProbability(Loot.RandomWeapon(), 0.10);
			AddItemWithProbability(CreateRandomClothing(), 0.25);
            AddItemWithProbability(CreateRandomArmor(), 0.15);
            AddItemWithProbability(CreateRandomWeapon(), 0.10);
            AddItemWithProbability(CreateRandomHat(), 0.10);
            AddItemWithProbability(CreateRandomShield(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomInstrument(), 0.10);
            AddItemWithProbability(CreateRandomGem(), 0.10);
			AddItemWithProbability(CreateRandomClothing(), 0.25);
            AddItemWithProbability(CreateRandomArmor(), 0.15);
            AddItemWithProbability(CreateRandomWeapon(), 0.10);
            AddItemWithProbability(CreateRandomHat(), 0.10);
            AddItemWithProbability(CreateRandomShield(), 0.10);
            AddItemWithProbability(CreateRandomJewelry(), 0.10);
            AddItemWithProbability(CreateRandomInstrument(), 0.10);
            AddItemWithProbability(CreateRandomGem(), 0.10);
            AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(new MaxxiaScroll(), 0.20);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(1, 10000)), 0.20);
			AddItemWithProbability(new BoltOfCloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 3000) }, 0.15);
			AddItemWithProbability(new Cloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 3000) }, 0.15);
			AddItemWithProbability(new UncutCloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 3000) }, 0.15);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			
			
            AddItemWithProbability(Loot.RandomWeapon(), 0.10);// Add personal notes
            AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(CreatePersonalNote(), 0.20);
			AddItemWithProbability(CreatePersonalNote(), 0.20);
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateRandomClothing()
        {
            var item = Loot.RandomClothing();
            item.Name = "Unique Clothing";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomArmor()
        {
            var item = Loot.RandomArmor();
            item.Name = "Exquisite Armor";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomWeapon()
        {
            var item = Loot.RandomWeapon();
            item.Name = "Rare Weapon";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomHat()
        {
            var item = Loot.RandomHat();
            item.Name = "Stylish Hat";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomShield()
        {
            var item = Loot.RandomShield();
            item.Name = "Family Shield";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomJewelry()
        {
            var item = Loot.RandomJewelry();
            item.Name = "Elegant Jewelry";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomInstrument()
        {
            var item = Loot.RandomInstrument();
            item.Name = "Family Instrument";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreateRandomGem()
        {
            var item = Loot.RandomGem();
            item.Name = "Precious Gem";
            item.Hue = Utility.RandomMinMax(1, 2000);
            return item;
        }

        private Item CreatePersonalNote()
        {
            string[] notes = new string[]
            {
                "I must remember to visit the tailor tomorrow.",
                "Don't forget to feed the cat!",
                "Meeting with the mayor at noon.",
                "Hidden the key in the garden.",
                "Look for the hidden treasure behind the old oak tree.",
                "The blacksmith's work is exceptional.",
                "Our anniversary is coming up; must plan something special.",
                "The children love the new nanny.",
                "Paid the gardener his due; the lawn looks magnificent.",
                "Preparing for the big feast next week.",
				"Must not forget to polish my collection of antique spoons every Friday.",
				"Remember to turn the doorknob three times before leaving the house.",
				"The cat prefers to be fed exactly at 7:00 PM.",
				"Only wear mismatched socks on Tuesdays.",
				"The garden gnome must face east at all times.",
				"Do not whistle after sunset; it attracts bad luck.",
				"Rotate the house plants clockwise once a week.",
				"Always take the third step on the staircase with the left foot first.",
				"The clock must be wound counterclockwise every night.",
				"Tea must be brewed for exactly 4 minutes and 37 seconds.",
				"The mirror in the hallway needs to be cleaned with vinegar daily.",
				"Keep a bowl of salt by the windowsill to ward off evil spirits.",
				"Write a letter to myself every full moon.",
				"The rug in the living room must be straightened hourly.",
				"Sing a lullaby to the potted fern every morning.",
				"The silverware drawer should only be opened with my right hand.",
				"The front gate needs to be opened and closed three times before leaving.",
				"Never eat the last piece of bread in the loaf.",
				"Talk to the portrait of Aunt Gertrude before going to bed.",
				"Whisper 'goodnight' to each window before drawing the curtains.",
				"The curtains must be drawn in alphabetical order by color.",
				"Arrange the bookshelf by the number of pages in each book.",
				"Walk backward through doorways to avoid bad luck.",
				"Sprinkle sugar on the doorstep to keep away unwanted guests.",
				"Stir my coffee five times counterclockwise before drinking.",
				"Turn off the lights in a specific sequence every night.",
				"Must touch every doorknob in the house before going to bed.",
				"The pantry should be organized according to the Fibonacci sequence.",
				"Recite the alphabet backwards while brushing my teeth.",
				"Open and close the wardrobe door exactly seven times each morning.",
				"Arrange the pillows on the couch in even numbers only.",
				"Wear gloves while reading books to keep the pages clean.",
				"Keep a feather under my pillow for pleasant dreams.",
				"Always leave one candle burning in the kitchen at night.",
				"Knock on the door frame twice before entering any room.",
				"Rotate the dining chairs 90 degrees every Sunday.",
				"The curtains must be tied in a double knot during thunderstorms.",
				"The left shoe always goes on before the right.",
				"Hang garlic by the bedroom window to ward off bad dreams.",
				"Count all the windows in the house before leaving for work.",
				"Clap my hands three times before turning on any light.",
				"Always tie a yellow ribbon around the front gate on Wednesdays.",
				"Mutter a secret password before unlocking the front door.",
				"Must tap the top of the doorframe before walking through.",
				"The carpet fringe must be perfectly aligned at all times.",
				"Arrange the vegetables in the fridge alphabetically.",
				"Place a coin under the doormat for good fortune.",
				"The clock chimes must be counted out loud each hour.",
				"Spin around twice before answering the telephone.",
				"The soap must be replaced every three days, no exceptions.",
				"Keep a pebble in my left pocket for luck.",
				"Recite a rhyme before starting any house chores.",
				"Wear a hat indoors on rainy days.",
				"Whistle a specific tune while washing dishes.",
				"The broom must be stored bristle-side up to avoid bad luck.",
				"Never leave a book open when not reading it.",
				"The curtains must be ironed every Thursday.",
				"Always wash the windows during a full moon.",
				"The bed must be made with hospital corners every day.",
				"Leave a note for the house spirits every night.",
				"Cross the threshold with my right foot first in the morning.",
				"The moonstone I found glows brighter each night. What secrets does it hold?",
				"Experimenting with the new alchemical mixture. Hope it doesn't explode this time.",
				"Must remember to consult the grimoire before attempting another summoning ritual.",
				"The ancient runes in the cave are fascinating. I need to decipher them.",
				"Found a strange, glowing fungus in the forest. Could it be the key to my potion?",
				"The enchanted mirror whispered my name today. I need to investigate further.",
				"Practicing the new invisibility spell. The results are... unpredictable.",
				"The black cat keeps following me. Is it a familiar or something more sinister?",
				"Need to gather more nightshade for the hex I'm working on.",
				"The crystal ball showed a dark figure in my future. Must prepare for what’s coming.",
				"Tried to summon a minor spirit, ended up with a banshee. Quite the surprise!",
				"Discovered an old spellbook in the attic. The spells are unlike anything I've seen.",
				"The local herbalist mentioned a rare herb that grows under the full moon. Must find it.",
				"The old tower ruins might be a perfect place for my next ritual.",
				"I've been hearing whispers from the old well. Perhaps there's something trapped inside?",
				"The enchanted candle burns with a blue flame. What magic fuels it?",
				"Visited the witch's hut in the swamp. She offered me a deal... Should I accept?",
				"Found a feather from a griffin. Need to find out if it's magical.",
				"The runestones I buried in the garden have started to hum. Something is happening.",
				"Must stay up tonight to watch for the ghost in the graveyard. It appeared again in my dreams.",
				"A local bard sang of a hidden portal in the woods. I should investigate.",
				"Discovered a secret compartment in my spellbook. What's inside?",
				"The gargoyle statues in town are moving. Or am I imagining it?",
				"The potion turned purple this time. Must remember this combination.",
				"The moon reflects strangely in the enchanted pond. What does it mean?",
				"Encountered a will-o'-the-wisp near the old oak tree. Need to catch one for study.",
				"The ley lines converge near the standing stones. Perfect spot for a powerful spell.",
				"Met a wandering mage who spoke of a hidden library of forbidden knowledge.",
				"The crystal amulet glows when I wear it. What magic does it contain?",
				"The new incantation caused a minor earthquake. Note to self: practice in the open fields.",
				"The haunted mansion on the hill might hold the key to my research.",
				"Discovered a spell to speak with animals. The birds have so much to say!",
				"The nightshade blooms at midnight. Must gather it quickly before it withers.",
				"The old wizard's staff I found has a hidden compartment. What's inside?",
				"The enchanted quill writes on its own. What stories does it want to tell?",
				"The ghost in the library spoke to me last night. I need to learn its language.",
				"Found a map to a dragon's lair. Should I seek its treasure or its wisdom?",
				"The spectral horse appeared again. It seems to want to lead me somewhere.",
				"The alchemist mentioned a potion that grants visions. Need to find the ingredients.",
				"The ancient oak in the forest has a face. It whispered a warning to me.",
				"The silver mirror shows glimpses of other realms. I need to study it more.",
				"The fairy ring in the meadow glows under the moonlight. Must explore its magic.",
				"The blacksmith's forge emits a strange energy at night. Could it be enchanted?",
				"Found a cursed locket. Should I try to break the curse or learn its origin?",
				"The oracle in the mountains spoke of a prophecy involving me. Need to learn more.",
				"The old tome mentions a ritual to contact the stars. Must gather the required items.",
				"The phantom ship appeared on the lake again. What spirits does it carry?",
				"The enchanted rose blooms only in moonlight. I need to discover its secrets.",
				"The ghostly figure in the mirror seemed familiar. Who could it be?",
				"The magic circle in the basement started glowing on its own. What triggered it?",
				"Met a druid who spoke of ancient forest magic. Need to learn his secrets.",
				"The old lantern I found shines with an ethereal light. What powers it?",
				"The ruins by the cliff are said to be haunted. Must explore with caution.",
				"The whispers in the wind seem to carry a message. Need to decipher it.",
				"The new spellbook contains spells for controlling the elements. Exciting!",
				"The enchanted ring I found has strange inscriptions. Must translate them.",
				"The herbalist's new potion made me see otherworldly creatures. Need more!",
				"The ancient statue in town glows under the full moon. What magic does it hold?",
				"The spectral wolf followed me home. Is it a guardian or a threat?",
				"Found a portal in the forest. Should I step through?",
				"The witch in the swamp offered me a charm. Should I trust her?",
				"I must remember to visit the tailor tomorrow.",
				"Don't forget to feed the cat!",
				"Meeting with the mayor at noon.",
				"Hidden the key in the garden.",
				"Look for the hidden treasure behind the old oak tree.",
				"The blacksmith's work is exceptional.",
				"Our anniversary is coming up; must plan something special.",
				"The children love the new nanny.",
				"Paid the gardener his due; the lawn looks magnificent.",
				"Preparing for the big feast next week.",
				"The milk has soured again. We need a new cow.",
				"The new bread recipe is a success. Must bake more tomorrow.",
				"Gertrude borrowed the spinning wheel; must get it back.",
				"Sent the boys to gather firewood; hope they return before dark.",
				"The roof is leaking again; need to find a carpenter.",
				"Thank the herbalist for the remedy; it worked wonders.",
				"Must sew a new cloak for the winter.",
				"Invite the neighbors for the harvest celebration.",
				"The chickens are not laying eggs; need to find out why.",
				"The well water is low; ask the men to dig deeper.",
				"Harvest the herbs before the frost sets in.",
				"The merchant's caravan arrives next week; prepare the list of goods.",
				"Repair the fence to keep the wolves out.",
				"Teach the children their letters by candlelight.",
				"The cellar door creaks loudly; apply some oil.",
				"Check the snares for rabbits before dusk.",
				"The new cobbler has excellent shoes; order a pair.",
				"The lord's tax collector is due soon; gather the coins.",
				"Set aside some grain for the coming winter.",
				"Mend the fishing nets for the next trip to the river.",
				"Remind the children to keep away from the deep woods.",
				"The new batch of ale is ready; fetch it from the brewery.",
				"The tapestry is almost complete; finish it by next week.",
				"Ask the healer for a potion to ease the baby's colic.",
				"The baker's bread is much better than last month's.",
				"The new goose feathers make for fine quills.",
				"Store the dried meats properly to prevent spoiling.",
				"Remind John to sharpen the plow before the planting season.",
				"Collect the apples from the orchard before they rot.",
				"The new lord's taxes are higher than last year's.",
				"Must trade for some new cloth at the market.",
				"The weather has been too dry; the crops need watering.",
				"The old hen has stopped laying; consider replacing her.",
				"Gather herbs from the forest for the winter medicines.",
				"The candle supply is running low; make more soon.",
				"Ask the blacksmith to repair the broken gate hinge.",
				"The wheat harvest looks promising this year.",
				"Ensure the children are well-behaved for the feast.",
				"The moon will be full tomorrow; avoid the woods at night.",
				"Invite the healer for dinner; she has been a great help.",
				"The old cart needs a new wheel; ask the carpenter.",
				"The goat has gone missing again; search the fields.",
				"Thank the midwife for her assistance during the birth.",
				"Must repair the old quilt before the cold sets in.",
				"The new lord is rumored to be fair and just.",
				"Gather the fallen leaves for composting.",
				"The barn roof needs patching before the rain comes.",
				"Teach the girls to spin wool in the evenings.",
				"The cat caught another mouse; praise her well.",
				"Check on the bee hives for fresh honey.",
				"The new blacksmith's tools are highly praised.",
				"Finish the new curtains before the guests arrive.",
				"The village fair is next week; prepare the goods for trade.",
				"Collect the herbs for the healer's potion.",
				"The river is high; warn the children to stay away.",
				"The lord's steward visits tomorrow; prepare the house.",
				"The old mare is not well; call for the vet.",
				"The roof thatch needs replacing before the storms.",
				"The sheep have wandered again; send the boys to fetch them.",
				"The new brew tastes excellent; make a larger batch.",
				"Ask the weaver for more cloth for the winter garments.",
				"The miller has raised his prices; consider alternatives.",
				"The herb garden is thriving; harvest what you can.",
				"The full moon brings out the bandits; be cautious.",
				"Prepare a warm meal for the traveling monks.",
				"Repair the worn out shoes before winter.",
				"The lanterns need oiling; do it before nightfall.",
				"The new neighbor seems friendly; invite them over.",
				"The frost has come early; cover the remaining crops.",
				"Ask the hunter for more game meat for the feast.",
				"The fireplace needs cleaning; ask the boys to help.",
				"The candles are running low; make more.",
				"The roof is leaking; ask the carpenter to fix it.",
				"Store the dried herbs properly to preserve them.",
				"The old horse is lame; consider getting a new one.",
				"Check the snares for rabbits tomorrow morning.",
				"The new ale is ready; fetch it from the brewery.",
				"Prepare the wool for spinning during the winter months.",
				"The children need new shoes before winter.",
				"Gather kindling for the hearth; it's getting colder.",
				"The bread has been baked; set it out to cool.",
				"Ask the midwife to check on the newborn.",
				"The fence needs mending; the sheep keep escaping.",
				"The market is bustling today; trade for fresh produce.",
				"Remember to thank the blacksmith for his good work.",
				"The healer's remedies are working well; stock up on supplies."
            };

            return new SimpleNote
            {
                NoteString = notes[Utility.Random(notes.Length)],
                TitleString = "Personal Note"
            };
        }

        public StartingClothes(Serial serial) : base(serial)
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
