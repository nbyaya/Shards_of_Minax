using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Custom
{
    public class AbandonedRefuseChest : BaseContainer
    {
        private bool _initialized;

        [Constructable]
        public AbandonedRefuseChest() : base(0xE77) // Trash barrel item ID
        {
            Name = "Abandoned Refuse";
            Hue = Utility.RandomMinMax(1, 1600);
            _initialized = false; // Indicates whether items have been added
        }

        private void InitializeItems()
        {
            if (_initialized) return;

            // Add items with probabilities
            AddItemWithProbability(new MaxxiaScroll(), 0.05);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(1, 500)), 0.07);
			AddItemWithProbability(new BoltOfCloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Cloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new UncutCloth(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new Cotton(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new Wool(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new Flax(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new SpoolOfThread(), 0.07);
			AddItemWithProbability(new OakLog(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new AshLog(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new YewLog(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new SackFlour(), 0.07);
			AddItemWithProbability(new Board(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new BreadLoaf(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new ApplePie() { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Cake() { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Muffins() { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new CheeseWheel(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new CookedBird(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bottle(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Bacon(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Ham(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new Sausage(Utility.RandomMinMax(1, 5)) { Hue = Utility.RandomMinMax(1, 1600) }, 0.07);
			AddItemWithProbability(new IronIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new DullCopperIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new ShadowIronIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new CopperIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new BronzeIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new GoldIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new AgapiteIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new VeriteIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new ValoriteIngot(Utility.RandomMinMax(1, 5)), 0.07);
			AddItemWithProbability(new RefreshPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new AgilityPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new NightSightPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new LesserHealPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new StrengthPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new LesserPoisonPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
			AddItemWithProbability(new LesserExplosionPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.07);
            AddItemWithProbability(new Item(0xEED) { Name = "Rusted Coin" }, 0.05); // Gold item ID
            AddItemWithProbability(new Item(0x99B) { Name = "Cracked Potion Bottle", Hue = 1149 }, 0.05); // Empty bottle item ID
			AddItemWithProbability(new WoodDebris() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedArmoire() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedBookcase() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedBooks() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedChair() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedFallenChairA() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedFallenChairB() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
			AddItemWithProbability(new RuinedPainting() { Name = "Pile of Discarded Junk", Hue = Utility.RandomMinMax(600, 1600), Movable = true }, 0.05);
            AddItemWithProbability(new GoldBracelet() { Name = "Twisted Bracelet", Hue = Utility.RandomMinMax(600, 1600) }, 0.05); // Bracelet item ID
            AddItemWithProbability(new SimpleNote { NoteString = "Abandoned and forgotten, here lies what was once treasured", TitleString = "Lost Testament" }, 0.05);
            AddItemWithProbability(new Sapphire(1) { Name = "Faded Sapphire Shard", Hue = 1102 }, 0.05); // Sapphire item ID
            AddItemWithProbability(new Item(0x99B) { Name = "Shattered Wine Bottle", Hue = Utility.RandomMinMax(600, 1600) }, 0.05); // Empty bottle item ID
            AddItemWithProbability(new Item(0xEED) { Name = "Tarnished Coin", Hue = Utility.RandomMinMax(600, 1600) }, 0.05); // Gold item ID
            AddItemWithProbability(new Sandals() { Name = "Tattered Sandals of the Wanderer", Hue = 1135 }, 0.05);
            AddItemWithProbability(new GoldRing() { Name = "Corroded Ring of Decay", Hue = Utility.RandomMinMax(600, 1600) }, 0.05);
            AddItemWithProbability(new SimpleMap { Name = "Map to the Forgotten", Bounds = new Rectangle2D(1000, 1000, 400, 400), NewPin = new Point2D(1200, 1200), Protected = true }, 0.05);
            AddItemWithProbability(new Spyglass() { Name = "Foggy Spyglass", Hue = Utility.RandomMinMax(600, 1600) }, 0.05); // Spyglass item ID
            AddItemWithProbability(new Cloth() { Name = "Banner of the Abandoned", Hue = Utility.RandomMinMax(600, 1600) }, 0.05); // Banner item ID
            AddItemWithProbability(new GreaterHealPotion() { Name = "Weak Healing Elixir", Hue = Utility.RandomMinMax(600, 1600) }, 0.05);
            AddItemWithProbability(CreateRandomWeapon(0, 2, "Shattered Weapon"), 0.05);
            AddItemWithProbability(CreateRandomArmor(0, 2, "Dented Armor"), 0.05);
            AddItemWithProbability(new Boots() { Name = "Old Guard's Worn Boots", Hue = Utility.RandomMinMax(600, 1600) }, 0.05);
            AddItemWithProbability(new PlateGloves() { Name = "Gloves of the Forgotten", Hue = Utility.RandomMinMax(1, 1000) }, 0.05);
            AddItemWithProbability(new Longsword() { Name = "Broken Blade", Hue = Utility.RandomMinMax(50, 250), MinDamage = Utility.RandomMinMax(5, 15), MaxDamage = Utility.RandomMinMax(15, 30) }, 0.05);

            // Add special loot keyword items with probabilities
            AddItemWithProbability(CreateRandomArmor(0, 5, "Soiled Armor"), 0.05);
            AddItemWithProbability(CreateRandomWeapon(0, 5, "Dull Weapon"), 0.05);
            AddItemWithProbability(CreateRandomJewelry(0, 5, "Broken Jewelry"), 0.05);
            AddItemWithProbability(CreateRandomWeapon(0, 5, "Smelly Weapon"), 0.05);
            AddItemWithProbability(CreateRandomArmor(0, 5, "Battered Shield"), 0.05);
            AddItemWithProbability(new GreaterHealPotion() { Name = "Mystery Potion", Hue = Utility.RandomMinMax(1, 2000) }, 0.05);
            AddItemWithProbability(CreateRandomShield(0, 5, "Ancient Shield"), 0.05);
            AddItemWithProbability(CreateRandomArmorOrShield(0, 5, "Ancient Armor"), 0.05);
			AddItemWithProbability(new SimpleNote { NoteString = "Beware of the creatures lurking in the dark.", TitleString = "Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The market has fresh produce today.", TitleString = "Market Notice" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Remember to pay your taxes by the end of the month.", TitleString = "Tax Reminder" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Meet me by the old oak tree at midnight.", TitleString = "Secret Meeting" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Wanted: Brave adventurers for a dangerous quest.", TitleString = "Quest Posting" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Don't forget to feed the chickens.", TitleString = "Farm Reminder" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The blacksmith's shop will be closed tomorrow.", TitleString = "Shop Notice" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Looking for a tutor to teach my children.", TitleString = "Tutor Needed" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "There's a sale on wool at the tailor's shop.", TitleString = "Sale Announcement" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The town council meets every Wednesday.", TitleString = "Council Meeting" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Found: A stray dog near the river.", TitleString = "Found Notice" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Lost: One brown leather glove.", TitleString = "Lost Notice" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Join us for a feast at the inn this Saturday.", TitleString = "Feast Invitation" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Seeking a skilled healer for a sick child.", TitleString = "Healer Needed" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The baker's bread is fresh and warm.", TitleString = "Bakery Ad" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Remember to lock your doors at night.", TitleString = "Safety Tip" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "A storm is expected to hit the town soon.", TitleString = "Weather Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Looking for a carpenter to repair my roof.", TitleString = "Carpenter Needed" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The herbalist has new potions in stock.", TitleString = "Herbalist Ad" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Thank you for your kindness.", TitleString = "Thank You Note" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The shadows whisper secrets best left unheard.", TitleString = "Shadow Whisper" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Heed the warnings of the old crone, lest you fall victim to the curse.", TitleString = "Old Crone's Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The well water has been tainted. Drink at your own risk.", TitleString = "Tainted Water" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Beware the full moon; it brings out the darkest creatures.", TitleString = "Full Moon Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do not trust the man in the red cloak.", TitleString = "Mysterious Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The old mill is haunted. Stay away after dark.", TitleString = "Haunted Mill" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "There is a price to pay for prying into forbidden knowledge.", TitleString = "Forbidden Knowledge" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The silence of the forest is a trap. Do not enter alone.", TitleString = "Forest Trap" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The catacombs hold secrets that should remain buried.", TitleString = "Catacombs Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Those who venture into the crypts seldom return.", TitleString = "Crypts Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The fog hides many dangers. Proceed with caution.", TitleString = "Fog Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "They are watching us, always waiting.", TitleString = "Constant Watch" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do not speak of the pact made in the dead of night.", TitleString = "Secret Pact" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The whispers in the wind are not your imagination.", TitleString = "Whispers Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "An ancient evil stirs beneath the ground.", TitleString = "Ancient Evil" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The watchtower is abandoned for a reason.", TitleString = "Watchtower Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do not trust the smiles of the villagers; they hide dark secrets.", TitleString = "Village Secrets" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The bells toll for those marked by the curse.", TitleString = "Cursed Bells" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The night brings horrors that the day cannot chase away.", TitleString = "Night Horrors" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "In the dark corners of the world, monsters dwell.", TitleString = "Dark Corners" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "They are watching you from the shadows.", TitleString = "Eerie Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The whispers in the dark are not your imagination.", TitleString = "Dark Whispers" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do not trust the man with the crooked smile.", TitleString = "Cryptic Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The cellar door creaks open at midnight.", TitleString = "Midnight Horror" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "She never truly left the old house.", TitleString = "Ghostly Presence" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The eyes in the portrait follow you.", TitleString = "Haunting Gaze" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Beware the sound of footsteps behind you.", TitleString = "Stalker" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The air grows colder with each passing night.", TitleString = "Chilling Air" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "A child's laughter echoes in the empty halls.", TitleString = "Phantom Laughter" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Leave this place before it's too late.", TitleString = "Dire Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The old well hides many secrets.", TitleString = "Dark Secrets" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "You cannot hide from the inevitable.", TitleString = "Ominous Message" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The trees whisper your name at night.", TitleString = "Whispering Woods" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The past will come back to haunt you.", TitleString = "Haunting Past" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do you hear the cries of the damned?", TitleString = "Cursed Voices" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The ground beneath you is not solid.", TitleString = "Unsteady Ground" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The candles flicker without wind.", TitleString = "Supernatural Flicker" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The dolls move when you're not looking.", TitleString = "Creepy Dolls" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "She waits for you in the mirror.", TitleString = "Mirror Specter" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Do not open the door after dark.", TitleString = "Nighttime Warning" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Schedule a meeting with the council regarding the new trade routes.", TitleString = "Council Meeting" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Inspect the barracks to ensure the soldiers are well equipped.", TitleString = "Barracks Inspection" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Authorize the funds for the orphanage renovation project.", TitleString = "Orphanage Renovation" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Prepare a speech for the annual harvest festival.", TitleString = "Harvest Festival" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Review the reports from the royal tax collectors.", TitleString = "Tax Reports" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Ensure the castle defenses are bolstered before winter.", TitleString = "Castle Defenses" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Arrange a diplomatic visit to the neighboring kingdom.", TitleString = "Diplomatic Visit" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Approve the new training regimen for the royal guards.", TitleString = "Training Approval" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Send a thank you note to the Duke of Trinsic for his assistance.", TitleString = "Thank You Note" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Oversee the construction of the new library in Britain.", TitleString = "Library Construction" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Organize a hunting trip with the visiting dignitaries.", TitleString = "Hunting Trip" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Review the progress of the naval fleet expansion.", TitleString = "Fleet Expansion" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Discuss the new agricultural policies with the farmers.", TitleString = "Agricultural Policies" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Prepare a banquet for the return of the knights from the crusade.", TitleString = "Banquet Preparation" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Inspect the royal mint for any discrepancies in coin production.", TitleString = "Royal Mint Inspection" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Issue a royal decree to reduce tariffs on imported goods.", TitleString = "Royal Decree" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Attend the ceremony for the opening of the new marketplace.", TitleString = "Marketplace Ceremony" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Approve the request for additional funding for the mage's guild.", TitleString = "Guild Funding" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Arrange a private audience with the ambassador from Moonglow.", TitleString = "Private Audience" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Ensure the royal stables are prepared for the new horses.", TitleString = "Stables Preparation" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Beneath the old willow, secrets lie hidden.", TitleString = "Cryptic Clue" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The moon's shadow reveals the path.", TitleString = "Moonlit Path" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Three stones mark the entrance to fortune.", TitleString = "Stone Markers" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "When the crows gather, look to the west.", TitleString = "Crow's Gathering" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Whisper your wish to the ancient oak.", TitleString = "Ancient Oak" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The key lies where water meets the sky.", TitleString = "Water and Sky" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Seek the tallest tree at dawn's light.", TitleString = "Dawn's Light" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "A broken sword points the way.", TitleString = "Broken Sword" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Follow the river until it sings.", TitleString = "Singing River" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Beneath the warrior's gaze lies your prize.", TitleString = "Warrior's Gaze" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The owl hoots twice before the hidden door opens.", TitleString = "Owl's Hoot" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The crimson rose guards the secret.", TitleString = "Crimson Rose" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Between the two peaks, treasures sleep.", TitleString = "Sleeping Peaks" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The seventh stone in the circle holds the answer.", TitleString = "Seventh Stone" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Under the watchful eye of the eagle.", TitleString = "Eagle's Eye" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The old mill's shadow points to gold.", TitleString = "Mill's Shadow" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Within the dragon's footprint, riches await.", TitleString = "Dragon's Footprint" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The silver lily blooms where treasure lies.", TitleString = "Silver Lily" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "The wind's whisper will guide you.", TitleString = "Wind's Whisper" }, 0.01);
			AddItemWithProbability(new SimpleNote { NoteString = "Beneath the midnight sun, secrets are unveiled.", TitleString = "Midnight Sun" }, 0.01);

            _initialized = true; // Mark as initialized
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateRandomWeapon(int min, int max, string name)
        {
            BaseWeapon weapon = Loot.RandomWeapon();
            weapon.Name = name;
            weapon.Hue = Utility.RandomMinMax(1, 2000);
            weapon.Attributes.WeaponDamage = Utility.RandomMinMax(min, max) * 5;
            return weapon;
        }

        private Item CreateRandomArmor(int min, int max, string name)
        {
            BaseArmor armor = Loot.RandomArmor();
            armor.Name = name;
            armor.Hue = Utility.RandomMinMax(1, 2000);
            armor.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(min, max);
            return armor;
        }

        private Item CreateRandomJewelry(int min, int max, string name)
        {
            BaseJewel jewelry = Loot.RandomJewelry();
            jewelry.Name = name;
            jewelry.Hue = Utility.RandomMinMax(1, 2000);
            jewelry.Attributes.Luck = Utility.RandomMinMax(min, max) * 10;
            return jewelry;
        }

        private Item CreateRandomShield(int min, int max, string name)
        {
            BaseShield shield = Loot.RandomShield();
            shield.Name = name;
            shield.Hue = Utility.RandomMinMax(1, 2000);
            shield.ProtectionLevel = (ArmorProtectionLevel)Utility.RandomMinMax(min, max);
            return shield;
        }

        private Item CreateRandomArmorOrShield(int min, int max, string name)
        {
            if (Utility.RandomBool())
                return CreateRandomArmor(min, max, name);
            else
                return CreateRandomShield(min, max, name);
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            InitializeItems(); // Initialize items when opened for the first time
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            base.OnItemLifted(from, item);

            if (from != null && from is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)from;

                // Amount of fame and karma to lose
                int fameLoss = 10; // Adjust this value as needed
                int karmaLoss = 10; // Adjust this value as needed

                // Decrease fame and karma
                player.Fame = Math.Max(player.Fame - fameLoss, 0);
                player.Karma = Math.Max(player.Karma - karmaLoss, -10000); // Minimum karma is -10000

                // Send message to player
                player.SendMessage(38, "You are noticed picking garbage and lose some fame and karma.");
            }
        }

        public AbandonedRefuseChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(_initialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _initialized = reader.ReadBool();
        }
    }
}
