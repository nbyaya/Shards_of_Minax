using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Regions;
using Server.Multis.Deeds;

namespace Server.Custom
{
    public class TreasureChest : LockableContainer
    {
        private bool _initialized;

        [Constructable]
        public TreasureChest() : base(0xE41) // Treasure Chest item ID
        {
            Name = "Treasure Chest";
            Hue = Utility.RandomMinMax(1, 1600);
            Locked = true;
            LockLevel = Utility.RandomMinMax(50, 100);
            _initialized = false; // Indicates whether items have been added
        }

        private void InitializeItems()
        {
            if (_initialized) return;

            // Add gold
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 1.0);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 0.2);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 0.2);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 0.2);
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 0.2);

            // Add jewels
            AddItemWithProbability(new Amber(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Amethyst(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Citrine(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Diamond(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Emerald(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Ruby(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Sapphire(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new StarSapphire(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Tourmaline(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Amber(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Amethyst(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Citrine(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Diamond(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Emerald(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Ruby(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Sapphire(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new StarSapphire(Utility.RandomMinMax(1, 5)), 0.2);
            AddItemWithProbability(new Tourmaline(Utility.RandomMinMax(1, 5)), 0.2);

            // Add rare artifacts
            AddItemWithProbability(new RandomMagicArmor(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicWeapon(), 0.05);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomMagicClothing(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryA(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryB(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryC(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryD(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryE(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryF(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryG(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryH(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryI(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryJ(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryK(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryL(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryM(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryN(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryO(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryP(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryQ(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryR(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryS(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryT(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryU(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryV(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryW(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryY(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryZ(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAA(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAB(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAC(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAD(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAE(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAF(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAG(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAH(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAI(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAJ(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAK(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAL(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAM(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAN(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAO(), 0.004);
            AddItemWithProbability(new RandomSkillJewelryAP(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);
            AddItemWithProbability(new RandomMagicJewelry(), 0.004);

            // Add valuable documents
            AddItemWithProbability(CreateValuableDocument(), 0.2);
            AddItemWithProbability(CreateValuableDocument(), 0.2);
            AddItemWithProbability(CreateValuableDocument(), 0.2);
            AddItemWithProbability(CreateValuableDocument(), 0.2);
            AddItemWithProbability(CreateValuableDocument(), 0.2);
            AddItemWithProbability(new StonePlasterHouseDeed(), 0.01);
            AddItemWithProbability(new FieldStoneHouseDeed(), 0.01);
            AddItemWithProbability(new SmallBrickHouseDeed(), 0.01);
            AddItemWithProbability(new WoodHouseDeed(), 0.01);
            AddItemWithProbability(new WoodPlasterHouseDeed(), 0.01);
            AddItemWithProbability(new ThatchedRoofCottageDeed(), 0.01);
            AddItemWithProbability(new BrickHouseDeed(), 0.01);
            AddItemWithProbability(new TwoStoryStonePlasterHouseDeed(), 0.01);
            AddItemWithProbability(new TwoStoryWoodPlasterHouseDeed(), 0.01);
            AddItemWithProbability(new TowerDeed(), 0.01);
            AddItemWithProbability(new KeepDeed(), 0.01);
            AddItemWithProbability(new CastleDeed(), 0.01);
            AddItemWithProbability(new LargePatioDeed(), 0.01);
            AddItemWithProbability(new LargeMarbleDeed(), 0.01);
            AddItemWithProbability(new SmallTowerDeed(), 0.01);
            AddItemWithProbability(new LogCabinDeed(), 0.01);
            AddItemWithProbability(new SandstonePatioDeed(), 0.01);
            AddItemWithProbability(new VillaDeed(), 0.01);
            AddItemWithProbability(new StoneWorkshopDeed(), 0.01);
            AddItemWithProbability(new MarbleWorkshopDeed(), 0.01);
            AddItemWithProbability(new SmallBoatDeed(), 0.01);
            AddItemWithProbability(new SmallDragonBoatDeed(), 0.01);
            AddItemWithProbability(new MediumBoatDeed(), 0.01);
            AddItemWithProbability(new MediumDragonBoatDeed(), 0.01);
            AddItemWithProbability(new LargeBoatDeed(), 0.01);
            AddItemWithProbability(new LargeDragonBoatDeed(), 0.01);

            _initialized = true; // Mark as initialized
        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateValuableDocument()
        {
            SimpleNote note = new SimpleNote();

            switch (Utility.Random(95)) // Update the range to the total number of documents
            {
				case 0:
					note.TitleString = "Ancient Map";
					note.NoteString = "This map is tattered and old, showing the location of a long-forgotten treasure buried deep in the mountains.";
					break;
				case 1:
					note.TitleString = "Old Deed";
					note.NoteString = "An old and weathered deed granting ownership of a vast estate to a distant ancestor.";
					break;
				case 2:
					note.TitleString = "Royal Charter";
					note.NoteString = "A beautifully ornate charter bestowing noble titles and privileges upon the bearer by a long-dead king.";
					break;
				case 3:
					note.TitleString = "Pirate's Log";
					note.NoteString = "A weathered logbook detailing the adventures and hidden caches of a notorious pirate captain.";
					break;
				case 4:
					note.TitleString = "Merchant's Ledger";
					note.NoteString = "A detailed ledger showing years of profitable trades, along with some questionable dealings.";
					break;
				case 5:
					note.TitleString = "Scholar's Manuscript";
					note.NoteString = "An ancient manuscript filled with rare and possibly dangerous magical knowledge.";
					break;
				case 6:
					note.TitleString = "Magician's Grimoire";
					note.NoteString = "A grimoire containing powerful spells and incantations used by a legendary magician.";
					break;
				case 7:
					note.TitleString = "King's Proclamation";
					note.NoteString = "A royal proclamation announcing a significant historical event, signed by the king himself.";
					break;
				case 8:
					note.TitleString = "Noble's Will";
					note.NoteString = "The last will and testament of a noble, revealing the secret locations of hidden family wealth.";
					break;
				case 9:
					note.TitleString = "Secret Treaty";
					note.NoteString = "A secret treaty between two kingdoms, outlining a pact that could alter the balance of power.";
					break;
				case 10:
					note.TitleString = "Treaty of Compassion";
					note.NoteString = "An ancient accord signed by Lord British and the leaders of Skara Brae, affirming the principles of compassion and mutual aid.";
					break;
				case 11:
					note.TitleString = "Accords of the Codex";
					note.NoteString = "A detailed manuscript outlining the agreements made between the Britannian government and the Gargoyles regarding the sharing and protection of the Codex of Ultimate Wisdom.";
					break;
				case 12:
					note.TitleString = "Pact of the Fellowship";
					note.NoteString = "A secret document detailing the true intentions and alliances of the Fellowship, including their hidden agendas and connections.";
					break;
				case 13:
					note.TitleString = "Truce of Trinsic";
					note.NoteString = "A historic agreement between the city of Trinsic and the pirates of Buccaneer's Den, promising peace and cooperation in times of crisis.";
					break;
				case 14:
					note.TitleString = "Covenant of the Avatar";
					note.NoteString = "A revered scroll describing the vows taken by the Avatar to uphold the eight Virtues and defend Britannia from all evil.";
					break;
				case 15:
					note.TitleString = "Treaty of Moonglow";
					note.NoteString = "A treaty signed by the mages of Moonglow and the Britannian council, establishing the rights and responsibilities of magical practitioners in the realm.";
					break;
				case 16:
					note.TitleString = "Concord of Yew";
					note.NoteString = "A detailed agreement between the Druids of Yew and the crown of Britannia, concerning the protection of sacred groves and ancient knowledge.";
					break;
				case 17:
					note.TitleString = "Treaty of Jhelom";
					note.NoteString = "A military pact between the warriors of Jhelom and the Britannian army, promising mutual defense and shared training techniques.";
					break;
				case 18:
					note.TitleString = "Alliance of Minoc";
					note.NoteString = "A document highlighting the alliance between the artisans of Minoc and the Britannian economy, ensuring the supply of essential goods and services.";
					break;
				case 19:
					note.TitleString = "Charter of Vesper";
					note.NoteString = "An agreement granting the city of Vesper autonomy in exchange for economic contributions and loyalty to the Britannian crown.";
					break;
				case 20:
					note.TitleString = "Declaration of Cove";
					note.NoteString = "A declaration by the citizens of Cove, pledging their support to Lord British in the face of external threats.";
					break;
				case 21:
					note.TitleString = "Treaty of the Serpent Isle";
					note.NoteString = "A rare document outlining the terms of peace and cooperation between the settlers of Britannia and the native inhabitants of the Serpent Isle.";
					break;
				case 22:
					note.TitleString = "Scroll of the Ophidians";
					note.NoteString = "A sacred scroll detailing the peace agreement between the Britannians and the Ophidians, promoting understanding and cultural exchange.";
					break;
				case 23:
					note.TitleString = "Accords of the Abyss";
					note.NoteString = "An ancient and cryptic document detailing the agreement between Britannian adventurers and the inhabitants of the Stygian Abyss, ensuring safe passage and mutual respect.";
					break;
				case 24:
					note.TitleString = "Pledge of the Rangers";
					note.NoteString = "A solemn pledge made by the rangers of Britannia to protect the wilderness and support the crown in maintaining peace and order.";
					break;
				case 25:
					note.TitleString = "Treaty of Wind";
					note.NoteString = "A treaty ensuring the cooperation of the hidden city of Wind with the Britannian government, focusing on magical research and defense.";
					break;
				case 26:
					note.TitleString = "Treaty of the Underworld";
					note.NoteString = "A secretive and controversial document detailing an agreement between Britannia and the mysterious inhabitants of the Underworld, focusing on mutual non-aggression.";
					break;
				case 27:
					note.TitleString = "Compact of the Time Lord";
					note.NoteString = "A rare and mystical compact outlining the responsibilities and expectations of the Time Lord in relation to the protection of Britannia.";
					break;
				case 28:
					note.TitleString = "Edict of the Virtues";
					note.NoteString = "An edict issued by Lord British, reaffirming the importance of the eight Virtues in governing and daily life, signed by key leaders across Britannia.";
					break;
				case 29:
					note.TitleString = "Scroll of the Silver Serpent";
					note.NoteString = "A revered scroll detailing the agreement between the healers of Britannia and the Order of the Silver Serpent, focusing on medical aid and support.";
					break;
				case 30:
					note.TitleString = "Deed to Castle Britannia";
					note.NoteString = "A highly prized deed granting ownership of Castle Britannia, home to Lord British and the seat of power in the realm.";
					break;
				case 31:
					note.TitleString = "Serpent's Hold Charter";
					note.NoteString = "A charter documenting the founding of Serpent's Hold, a fortress dedicated to training the realm's finest warriors.";
					break;
				case 32:
					note.TitleString = "Deed to the Shrine of Compassion";
					note.NoteString = "An ancient deed detailing the ownership of the Shrine of Compassion, a sacred place in Britannia.";
					break;
				case 33:
					note.TitleString = "Moonglow Mage's License";
					note.NoteString = "A rare document granting the holder permission to practice magic within the city of Moonglow.";
					break;
				case 34:
					note.TitleString = "Order of the Silver Serpent";
					note.NoteString = "A certificate of membership in the Order of the Silver Serpent, an elite group of warriors and knights.";
					break;
				case 35:
					note.TitleString = "Deed to the Lycaeum";
					note.NoteString = "A deed granting control over the Lycaeum, Britannia's greatest center of learning and wisdom.";
					break;
				case 36:
					note.TitleString = "Yew Forest Covenant";
					note.NoteString = "An ancient covenant outlining the protection and preservation of the great Yew forest.";
					break;
				case 37:
					note.TitleString = "Trinsic Guard Commission";
					note.NoteString = "A formal commission appointing the bearer as an officer in the renowned Trinsic Guard.";
					break;
				case 38:
					note.TitleString = "Britain Bank Ledger";
					note.NoteString = "A ledger containing detailed records of transactions and accounts held at the Britain Bank.";
					break;
				case 39:
					note.TitleString = "Skara Brae Rebuilding Plan";
					note.NoteString = "Detailed architectural plans for the rebuilding of Skara Brae after its near destruction by the undead.";
					break;
				case 40:
					note.TitleString = "Magincia Trade Agreement";
					note.NoteString = "An important trade agreement between the merchants of Magincia and other major cities in Britannia.";
					break;
				case 41:
					note.TitleString = "Vesper Shipping Manifest";
					note.NoteString = "A shipping manifest detailing the goods transported by sea to and from the port city of Vesper.";
					break;
				case 42:
					note.TitleString = "Buccaneer's Den Pirate Code";
					note.NoteString = "A rare copy of the Buccaneer's Den Pirate Code, outlining the rules and regulations governing pirate conduct.";
					break;
				case 43:
					note.TitleString = "Nujel'm Diplomatic Letter";
					note.NoteString = "A diplomatic letter detailing the terms of a peace treaty between Nujel'm and the other Britannian cities.";
					break;
				case 44:
					note.TitleString = "Jhelom Warrior's Oath";
					note.NoteString = "An oath sworn by the warriors of Jhelom, pledging their loyalty and bravery in the defense of Britannia.";
					break;
				case 45:
					note.TitleString = "Deed to the Shrine of Honor";
					note.NoteString = "A revered deed granting stewardship over the Shrine of Honor, one of Britannia's eight sacred shrines.";
					break;
				case 46:
					note.TitleString = "Cove Land Grant";
					note.NoteString = "A land grant providing the holder with a significant parcel of land within the town of Cove.";
					break;
				case 47:
					note.TitleString = "Minoc Miner's Contract";
					note.NoteString = "A contract detailing the rights and responsibilities of miners working in the rich mountains surrounding Minoc.";
					break;
				case 48:
					note.TitleString = "Hythloth Exploration Log";
					note.NoteString = "A detailed logbook kept by an explorer who ventured into the depths of the dangerous Hythloth dungeon.";
					break;
				case 49:
					note.TitleString = "Deed to the Shrine of Valor";
					note.NoteString = "A respected deed giving control of the Shrine of Valor, a sacred place dedicated to the virtue of courage.";
					break;
				case 50:
					note.TitleString = "Paws Village Charter";
					note.NoteString = "A charter outlining the founding and governance of the humble village of Paws.";
					break;
				case 51:
					note.TitleString = "Britannian Royal Treasury Inventory";
					note.NoteString = "An inventory list detailing the contents of the Royal Treasury of Britannia.";
					break;
				case 52:
					note.TitleString = "Empath Abbey Records";
					note.NoteString = "Records from Empath Abbey detailing the history and important events of this spiritual center.";
					break;
				case 53:
					note.TitleString = "Shrine of Spirituality Guardian's Journal";
					note.NoteString = "The personal journal of the guardian of the Shrine of Spirituality, detailing daily life and significant events.";
					break;
				case 54:
					note.TitleString = "Deed to the Shrine of Humility";
					note.NoteString = "A deed bestowing responsibility for the Shrine of Humility, a place dedicated to the virtue of humility.";
					break;
				case 55:
					note.TitleString = "Haven Healer's Guide";
					note.NoteString = "A comprehensive guide to the healing practices and potions used by the healers of Haven.";
					break;
				case 56:
					note.TitleString = "Deed to the Shrine of Sacrifice";
					note.NoteString = "A deed granting care over the Shrine of Sacrifice, dedicated to the virtue of selflessness.";
					break;
				case 57:
					note.TitleString = "Delucia Settlement Plan";
					note.NoteString = "A detailed plan for the settlement and development of the frontier town of Delucia.";
					break;
				case 58:
					note.TitleString = "Papua Fishing Logs";
					note.NoteString = "Logs kept by the fishermen of Papua, detailing the types and quantities of fish caught in the surrounding waters.";
					break;
				case 59:
					note.TitleString = "Serpent Isle Navigation Charts";
					note.NoteString = "Charts used by sailors to navigate the treacherous waters around Serpent Isle.";
					break;
				case 60:
					note.TitleString = "Terfin Reconstruction Blueprint";
					note.NoteString = "Blueprints for the reconstruction of Terfin, home to the gargoyles of Britannia.";
					break;
				case 61:
					note.TitleString = "Deed to the Shrine of Justice";
					note.NoteString = "A deed conferring guardianship of the Shrine of Justice, dedicated to the virtue of fairness.";
					break;
				case 62:
					note.TitleString = "Britannian Mining Consortium Agreement";
					note.NoteString = "An agreement between various mining guilds and the Crown to ensure fair practices and distribution of resources.";
					break;
				case 63:
					note.TitleString = "Magincia Agricultural Report";
					note.NoteString = "A report on the agricultural production and prosperity of the island city of Magincia.";
					break;
				case 64:
					note.TitleString = "Yew Council Meeting Minutes";
					note.NoteString = "Minutes from a meeting of the Yew Council, discussing important matters affecting the city and its surroundings.";
					break;
				case 65:
					note.TitleString = "Candle of Love Ritual Scroll";
					note.NoteString = "A scroll detailing the ritual used to reignite the Candle of Love, a powerful artifact in Britannian lore.";
					break;
				case 66:
					note.TitleString = "Proclamation of Unity";
					note.NoteString = "By order of Lord British, all towns and villages shall unite under a single banner to ensure the safety and prosperity of all citizens.";
					break;
				case 67:
					note.TitleString = "Proclamation of Trade";
					note.NoteString = "Let it be known by decree of Lord British, a new era of trade shall commence, opening our borders to friendly nations and fostering economic growth.";
					break;
				case 68:
					note.TitleString = "Proclamation of Justice";
					note.NoteString = "Under the guidance of Lord British, a new court system shall be established to uphold justice and fairness for all inhabitants of the realm.";
					break;
				case 69:
					note.TitleString = "Proclamation of Peace";
					note.NoteString = "In the name of Lord British, a ceasefire is declared, and negotiations for lasting peace with neighboring realms shall begin immediately.";
					break;
				case 70:
					note.TitleString = "Proclamation of Exploration";
					note.NoteString = "By the command of Lord British, brave adventurers are called upon to explore uncharted territories and report back their findings for the glory of the kingdom.";
					break;
				case 71:
					note.TitleString = "Proclamation of Celebration";
					note.NoteString = "Hear ye, hear ye! Lord British declares a week of festivities and celebrations in honor of the kingdom's rich heritage and bright future.";
					break;
				case 72:
					note.TitleString = "Proclamation of Protection";
					note.NoteString = "By decree of Lord British, new defenses shall be constructed along our borders to protect the realm from external threats and ensure the safety of our people.";
					break;
				case 73:
					note.TitleString = "Proclamation of Knowledge";
					note.NoteString = "Lord British proclaims the establishment of a grand library, where scholars from all lands are invited to share their knowledge and advance our understanding of the world.";
					break;
				case 74:
					note.TitleString = "Proclamation of Health";
					note.NoteString = "Let it be known that Lord British has ordered the creation of new hospitals and medical facilities to improve the health and well-being of all citizens.";
					break;
				case 75:
					note.TitleString = "Proclamation of Honor";
					note.NoteString = "In recognition of their bravery and service, Lord British bestows honors and titles upon those who have demonstrated exceptional valor in the defense of the realm.";
					break;
				case 76:
					note.TitleString = "Proclamation of Agriculture";
					note.NoteString = "By the will of Lord British, new farming techniques and resources shall be distributed to ensure a bountiful harvest and the sustenance of our people.";
					break;
				case 77:
					note.TitleString = "Proclamation of Magic";
					note.NoteString = "Lord British announces the formation of a council of mages to regulate the use of magic, ensuring it is used for the benefit of all and not for harm.";
					break;
				case 78:
					note.TitleString = "Proclamation of Arts";
					note.NoteString = "To enrich our culture, Lord British declares the establishment of an academy of the arts, encouraging the creation and appreciation of music, painting, and sculpture.";
					break;
				case 79:
					note.TitleString = "Proclamation of Roads";
					note.NoteString = "Under the guidance of Lord British, new roads and bridges shall be constructed to connect our cities and facilitate travel and commerce.";
					break;
				case 80:
					note.TitleString = "Proclamation of Apprenticeship";
					note.NoteString = "Lord British decrees that a system of apprenticeships shall be established, allowing young citizens to learn trades and skills from masters in various crafts.";
					break;
				case 81:
					note.TitleString = "Proclamation of Law";
					note.NoteString = "By the authority of Lord British, a comprehensive code of laws shall be written and enforced to maintain order and protect the rights of all citizens.";
					break;
				case 82:
					note.TitleString = "Proclamation of Festivals";
					note.NoteString = "Let it be known that Lord British has declared an annual series of festivals to celebrate the changing seasons and bring joy to the people of the realm.";
					break;
				case 83:
					note.TitleString = "Proclamation of Trade Routes";
					note.NoteString = "Lord British orders the establishment of secure trade routes with neighboring kingdoms to promote commerce and mutual prosperity.";
					break;
				case 84:
					note.TitleString = "Proclamation of Education";
					note.NoteString = "By the decree of Lord British, schools and universities shall be founded throughout the land to provide education and enlightenment to all.";
					break;
				case 85:
					note.TitleString = "Proclamation of Innovation";
					note.NoteString = "Lord British encourages the pursuit of new inventions and technologies, offering rewards to those who contribute to the advancement of our society.";
					break;
				case 86:
					note.TitleString = "Proclamation of Sanctuary";
					note.NoteString = "In the name of compassion, Lord British declares certain areas as sanctuaries where the wounded and weary may find refuge and aid.";
					break;
				case 87:
					note.TitleString = "Proclamation of Conservation";
					note.NoteString = "Lord British decrees that efforts shall be made to preserve our natural resources and protect the environment for future generations.";
					break;
				case 88:
					note.TitleString = "Proclamation of Naval Expansion";
					note.NoteString = "By order of Lord British, the kingdom's navy shall be expanded to protect our maritime interests and explore the vast oceans.";
					break;
				case 89:
					note.TitleString = "Proclamation of Heritage";
					note.NoteString = "Lord British proclaims the creation of a museum to preserve and display the artifacts and history of our great kingdom.";
					break;
				case 90:
					note.TitleString = "Proclamation of Loyalty";
					note.NoteString = "A decree from Lord British calls for all citizens to pledge their loyalty to the crown, ensuring unity and strength within the realm.";
					break;
				case 91:
					note.TitleString = "Proclamation of Tolerance";
					note.NoteString = "Let it be known that Lord British promotes tolerance and understanding among all races and cultures within the kingdom, fostering peace and cooperation.";
					break;
				case 92:
					note.TitleString = "Proclamation of Harvest";
					note.NoteString = "By the will of Lord British, a great harvest festival shall be held to give thanks for the bounty of the land and the hard work of our farmers.";
					break;
				case 93:
					note.TitleString = "Proclamation of Valor";
					note.NoteString = "In recognition of their heroism, Lord British awards medals of valor to those who have shown extraordinary courage in the face of danger.";
					break;
				case 94:
					note.TitleString = "Proclamation of Diplomacy";
					note.NoteString = "Lord British announces a new era of diplomacy, inviting ambassadors from foreign lands to discuss treaties and alliances.";
					break;
				case 95:
					note.TitleString = "Proclamation of Vigilance";
					note.NoteString = "To ensure the safety of the realm, Lord British calls upon all citizens to remain vigilant and report any signs of danger or treachery.";
					break;
				case 96:
					note.TitleString = "Proclamation of Craftsmanship";
					note.NoteString = "By decree of Lord British, a guild of craftsmen shall be established to promote excellence in the creation of goods and ensure fair trade practices.";
					break;
				default:
					note.TitleString = "Ancient Document";
					note.NoteString = "This is an ancient document containing historical significance.";
					break;
            }

            return note;
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            InitializeItems(); // Initialize items when opened for the first time
            HandlePlayerInteraction(from);
        }

        public override void OnItemUsed(Mobile from, Item item)
        {
            base.OnItemUsed(from, item);
            HandlePlayerInteraction(from);
        }

        private void HandlePlayerInteraction(Mobile from)
        {
            if (from.Criminal)
            {
                from.SendMessage("You cannot interact with this container because you are flagged as a criminal.");
                return;
            }

            if (from.Hidden)
            {
                double revealChance = (1 - (from.Skills[SkillName.Hiding].Value / 200.0)); // 100 skill = 0.5 chance

                if (Utility.RandomDouble() < revealChance)
                {
                    from.RevealingAction();
                    FlagAsCriminal(from, false); // Do not call CriminalAction; just flag as criminal
                }
                else
                {
                    from.SendMessage("You successfully interact with the container while remaining hidden.");
                }
            }
            else
            {
                FlagAsCriminal(from, true); // Call CriminalAction since the player is not hidden
            }
        }

        private void FlagAsCriminal(Mobile from, bool useCriminalAction)
        {
            if (!from.Criminal)
            {
                if (useCriminalAction)
                {
                    from.CriminalAction(true); // This will flag the player and allow guards to intervene
                }
                else
                {
                    from.Criminal = true; // Only flag as criminal without guard intervention
                }
                from.SendMessage("You feel a sudden sense of guilt as you tamper with the shipping crate.");
            }
        }

        public TreasureChest(Serial serial) : base(serial)
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
