using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RandomMagicArmor : BaseArmor
{
    public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Chainmail; } }
    private static string[] prefixes = { "Mighty", "Powerful", "Mystic", "Enchanted", "Arcane", "Enchanted", "Mystical", "Elemental", "Eternal", "Infernal", "Celestial", "Eldritch", "Spectral", "Tempest", "Frozen", "Blazing", "Thunder", "Shadow", "Radiant", "Dark", "Light", "Phantom", "Void", "Ethereal", "Necrotic", "Divine", "Astral", "Prismatic", "Runic", "Venomous", "Frost", "Storm", "Invisible", "Invincible", "Majestic", "Cursed", "Blessed", "Soulbound", "Vortex", "Twilight", "Dawn", "Dusk", "Starforged", "Moonlit", "Sunflare", "Comet", "Eclipse", "Galactic", "Cosmic", "Dimensional", "Temporal", "Spatial", "Quantum", "Mythic", "Legendary", "Ancient", "Primordial", "Forgotten", "Unseen", "Chaos", "Harmony", "Balance", "Rage", "Serenity", "Oblivion", "Creation", "Destruction", "Rebirth", "Fate", "Dream", "Nightmare", "Illusion", "Reality", "Vision", "Ghostly", "Glorious", "Sacred", "Unholy", "Vigilant", "Warrior's", "Sorcerer's", "Seer's", "Dragon's", "Titan's", "Phoenix", "Demonic", "Angelic", "Heavenly", "Abyssal", "Solar", "Lunar", "Stellar", "Voidwalker's", "Battleworn", "Savage", "Berserker's", "Monarch's", "Guardian's", "Pirate's", "Royal", "Revenant", "Warden's", "Spectral", "Stormbringer's", "Windwalker's", "Flamebearer's", "Icewrought", "Thunderous", "Stoneskin", "Nature's", "Beastmaster's", "Shamanic", "Witch's", "Siren's", "Mercurial", "Adamant", "Sylvan", "Arcanist's", "Noble", "Explorer's", "Sentry's", "Ranger's", "Corsair's", "Assassin's", "Necromancer's", "Paladin's", "Rogue's", "Cleric's", "Elementalist's", "Chronomancer's", "Geomancer's", "Pyromancer's", "Hydromancer's", "Aeromancer's", "Biomancer's", "Cybernetic", "Technomancer's", "Alchemist's", "Summoner's", "Psionic", "Sage's", "Prophet's", "Martyr's", "Zealot's", "Reclaimer's", "Pioneer's", "Innovator's", "Vindicator's", "Arbiter's", "Sentinel's", "Defender's", "Avenger's", "Champion's", "Conqueror's", "Master's", "Primeval", "Arcadian", "Myrmidon's", "Valkyrie's", "Bard's", "Jester's", "Gladiator's", "Knight's", "Samurai's", "Ninja's", "Viking's", "Pilgrim's", "Hermit's", "Sculptor's", "Painter's", "Poet's", "Minstrel's", "Troubadour's", "Wanderer's", "Explorer's", "Adventurer's", "Seeker's", "Scholar's", "Philosopher's", "Oracle's", "Muse's", "Mystic's", "Seer's", "Soothsayer's", "Prognosticator's", "Diviner's", "Augur's", "Sibyl's", "Clairvoyant's", "Telepath's", "Empath's", "Psychic's", "Medium's", "Spiritualist's", "Channeler's", "Shapeshifter's", "Transformer's", "Metamorph's", "Changeling's", "Morpher's", "Transmuter's", "Alchemist's", "Chemist's", "Potioneer's", "Apothecary's", "Herbalist's", "Botanist's", "Horticulturist's", "Agronomist's", "Cultivator's", "Farmer's", "Gardener's", "Landscaper's", "Arborist's", "Forester's", "Logger's", "Woodcutter's", "Carpenter's", "Joiner's", "Cabinetmaker's", "Woodworker's", "Craftsman's", "Artisan's", "Maker's", "Creator's", "Inventor's", "Designer's", "Architect's", "Engineer's", "Builder's", "Constructor's", "Fabricator's", "Manufacturer's", "Producer's", "Director's", "Manager's", "Supervisor's", "Coordinator's", "Organizer's", "Planner's", "Strategist's", "Analyst's", "Consultant's", "Advisor's", "Counselor's", "Mentor's", "Tutor's", "Teacher's", "Instructor's", "Educator's", "Professor's", "Lecturer's", "Trainer's", "Coach's", "Drillmaster's", "Taskmaster's", "Mastermind's", "Genius's", "Savant's", "Expert's", "Specialist's", "Professional's", "Technician's", "Mechanic's", "Operator's", "Worker's", "Laborer's", "Handyman's", "Repairman's", "Serviceman's", "Maintenance's", "Custodian's", "Janitor's", "Cleaner's", "Sanitation's", "Hygienist's", "Health's", "Medical's", "Nurse's", "Doctor's", "Physician's", "Surgeon's", "Dentist's", "Pharmacist's", "Therapist's", "Psychologist's", "Psychiatrist's", "Counselor's", "Social Worker's", "Case Manager's", "Advocate's", "Mediator's", "Negotiator's", "Arbitrator's", "Judge's", "Magistrate's", "Jurist's", "Lawyer's", "Attorney's", "Counsel's", "Solicitor's", "Barrister's", "Advocate's", "Prosecutor's", "Defender's", "Litigator's", "Trial Lawyer's", "Appellate Lawyer's", "Legal Advisor's", "Legal Consultant's", "Legal Analyst's", "Paralegal's", "Legal Assistant's", "Clerk's", "Secretary's", "Assistant's", "Aide's", "Helper's", "Support's", "Backer's", "Sponsor's", "Patron's", "Benefactor's", "Donor's", "Contributor's", "Investor's", "Shareholder's", "Stakeholder's", "Partner's", "Co-owner's", "Joint Venture's", "Syndicate's", "Consortium's", "Alliance's", "Coalition's", "Federation's", "Union's", "Association's", "Society's", "Club's", "Group's", "Team's", "Crew's", "Gang's", "Band's", "Troupe's", "Company's", "Corporation's", "Enterprise's", "Firm's", "Business's", "Agency's", "Bureau's", "Office's", "Department's", "Division's", "Section's", "Unit's", "Branch's", "Subsidiary's", "Affiliate's", "Franchise's", "Chain's", "Outlet's", "Store's", "Shop's", "Boutique's", "Emporium's", "Marketplace's", "Mart's", "Mall's", "Plaza's", "Center's", "Complex's", "Hub's", "Terminal's", "Station's", "Port's", "Harbor's", "Marina's", "Dock's", "Wharf's", "Quay's", "Pier's", "Jetty's", "Breakwater's", "Seawall's", "Bulwark's", "Rampart's", "Bastion's", "Fortress's", "Castle's", "Palace's", "Manor's", "Mansion's", "Estate's", "Villa's", "Chateau's", "Lodge's", "Cabin's", "Cottage's", "Bungalow's", "Hut's", "Shack's", "Shed's", "Barn's", "Stable's", "Kennel's", "Cattery's", "Aviary's", "Aquarium's", "Zoo's", "Safari Park's", "Wildlife Reserve's", "Nature Preserve's", "National Park's", "State Park's", "Provincial Park's", "Regional Park's", "City Park's", "Public Garden's", "Botanical Garden's", "Arboretum's", "Greenhouse's", "Nursery's", "Farm's", "Ranch's", "Plantation's", "Orchard's", "Vineyard's", "Winery's", "Brewery's", "Distillery's", "Factory's", "Mill's", "Plant's", "Workshop's", "Studio's", "Gallery's", "Museum's", "Library's", "Archive's", "Repository's", "Depot's", "Warehouse's", "Storage's", "Silo's", "Tank's", "Reservoir's", "Container's", "Vessel's", "Ship's", "Boat's", "Vessel's", "Craft's", "Yacht's", "Sailboat's", "Motorboat's", "Speedboat's", "Ferry's", "Cruise Ship's", "Liner's", "Tanker's", "Freighter's", "Cargo Ship's", "Container Ship's", "Battleship's", "Destroyer's", "Frigate's", "Submarine's", "Aircraft Carrier's", "Warship's", "Naval Ship's", "Military Vessel's", "Patrol Boat's", "Coast Guard Cutter's", "Icebreaker's", "Research Vessel's", "Exploration Ship's", "Adventure's", "Expedition's", "Voyage's", "Journey's", "Trip's", "Tour's", "Excursion's", "Outing's", "Safari's", "Trek's", "Hike's", "Walk's", "Stroll's", "Ramble's", "Wander's", "Roam's", "Travel's", "Venture's", "Quest's", "Mission's", "Campaign's", "Crusade's", "Drive's", "Push's", "Effort's", "Attempt's", "Trial's", "Test's", "Experiment's", "Study's", "Investigation's", "Inquiry's", "Research's", "Exploration's", "Discovery's", "Find's", "Revelation's", "Uncovering's", "Exposure's", "Reveal's", "Show's", "Presentation's", "Display's", "Exhibition's", "Demonstration's", "Performance's", "Act's", "Scene's", "Episode's", "Chapter's", "Volume's", "Book's", "Publication's", "Release's", "Launch's", "Debut's", "Premiere's", "Opening's", "Introduction's", "Inauguration's", "Commencement's", "Start's", "Beginning's", "Origin's", "Genesis's", "Creation's", "Formation's", "Development's", "Evolution's", "Progress's", "Advancement's", "Improvement's", "Enhancement's", "Upgrade's", "Update's", "Revision's", "Modification's", "Change's", "Alteration's", "Transformation's", "Conversion's", "Switch's", "Substitution's", "Replacement's", "Exchange's", "Trade's", "Swap's", "Shift's", "Transfer's", "Movement's", "Motion's", "Action's", "Activity's", "Operation's", "Function's", "Process's", "Procedure's", "Method's", "Technique's", "Strategy's", "Tactic's", "Plan's", "Scheme's", "Design's", "Blueprint's", "Outline's", "Sketch's", "Draft's", "Diagram's", "Chart's", "Map's", "Plan's", "Layout's", "Arrangement's", "Organization's", "Structure's", "Framework's", "System's", "Network's", "Grid's", "Matrix's", "Web's", "Complex's", "Compound's", "Aggregate's", "Mixture's", "Blend's", "Combination's", "Amalgamation's", "Integration's", "Union's", "Fusion's", "Merger's", "Consolidation's", "Unification's", "Synthesis's", "Harmonization's", "Coordination's", "Alignment's", "Congruence's", "Correspondence's", "Match's", "Pairing's", "Coupling's", "Linkage's", "Connection's", "Bond's", "Tie's", "Relationship's", "Association's", "Affiliation's", "Partnership's", "Collaboration's", "Cooperation's", "Teamwork's", "Synergy's", "Interplay's", "Interaction's", "Interrelation's", "Interdependence's", "Mutuality's", "Reciprocity's", "Exchange's", "Dialogue's", "Conversation's", "Discussion's", "Debate's", "Argument's", "Dispute's", "Controversy's", "Conflict's", "Struggle's", "Fight's", "Battle's", "War's", "Combat's", "Engagement's", "Encounter's", "Skirmish's", "Clash's", "Confrontation's", "Showdown's", "Face-off's", "Duel's", "Matchup's", "Competition's", "Contest's", "Tournament's", "Championship's", "Race's", "Game's", "Sport's", "Event's", "Occasion's", "Celebration's", "Festival's", "Fair's", "Carnival's", "Party's", "Gathering's", "Meeting's", "Assembly's", "Convention's", "Conference's", "Symposium's", "Seminar's", "Workshop's", "Course's", "Class's" };
    private static string[] suffixes =
    {
        "Guardian", "Protector", "Defender", "Shield", "Ward", "Barrier", "Safeguard",
		"Bulwark", "Fortress", "Palisade", "Rampart", "Bastion", "Parapet", "Redoubt",
		"Stronghold", "Keep", "Tower", "Castle", "Citadel", "Armory", "Sanctuary",
		"Refuge", "Haven", "Asylum", "Retreat", "Hideaway", "Sanctum", "Cloak",
		"Aegis", "Mail", "Plate", "Scale", "Shell", "Cover", "Veil", "Pavise",
		"Carapace", "Plating", "Coating", "Padding", "Armor", "Screen", "Mantle",
		"Pelt", "Brace", "Mask", "Helm", "Visor", "Crest", "Signet", "Aegis",
		"Buckler", "Targe", "Vanguard", "Champion", "Sentinel", "Watchman", 
		"Sentry", "Guard", "Keeper", "Savior", "Rescuer", "Warrior", "Knight",
		"Paladin", "Crusader", "Templar", "Mercenary", "Gladiator", "Berserker",
		"Shieldbearer", "Protectorate", "Defiance", "Resistance", "Garrison",
		"Barricade", "Enclosure", "Fortification", "Battlement", "Cordon", 
		"Emblem", "Insignia", "Totem", "Ensign", "Banner", "Standard", "Pennant",
		"Cuirass", "Hauberk", "Lorica", "Brigandine", "Greave", "Gauntlet", "Sabaton",
		"Vambrace", "Rerebrace", "Pauldron", "Spaulder", "Gorget", "Bevor", 
		"Fauld", "Tasset", "Cuisse", "Poleyn", "Greave", "Sabaton", "Armet",
		"Sallet", "Barbute", "Bascinet", "Morion", "Cabasset", "Burgonet",
		"Cassolette", "Aventail", "Camail", "Bevor", "Gorget", "Rondel",
		"Besague", "Couter", "Vamplate", "Gauntlet", "Mitten", "Bracer",
		"Armlet", "Band", "Circlet", "Diadem", "Coronet", "Tiara", "Crown",
		"Orb", "Scepter", "Anklet", "Talisman", "Amulet", "Charm", "Figurine",
		"Pendant", "Locket", "Medallion", "Brooch", "Badge", "Patch", "Seal",
		"Token", "Symbol", "Glyph", "Rune", "Sigil", "Mark", "Emboss", "Inlay",
		"Overlay", "Underlay", "Filigree", "Engraving", "Carving", "Chiseling",
		"Etching", "Inscription", "Script", "Scroll", "Manuscript", "Codex",
		"Tome", "Volume", "Book", "Ledger", "Register", "Record", "Chronicle",
		"Annals", "Archive", "Dossier", "Portfolio", "Case", "Box", "Chest",
		"Casket", "Coffer", "Reliquary", "Vault", "Cache", "Repository", "Store",
		"Stockpile", "Reserve", "Depot", "Arsenal", "Magazine", "Armamentarium",
		"Quiver", "Sheath", "Scabbard", "Holster", "Case", "Carrier", "Rack",
		"Stand", "Bracket", "Holder", "Hook", "Hanger", "Rest", "Cradle", "Support",
		"Frame", "Structure", "Framework", "Scaffold", "Lattice", "Grid", "Matrix",
		"Fabric", "Texture", "Weave", "Mesh", "Net", "Web", "Plexus", "Network",
		"System", "Complex", "Composite", "Compound", "Mixture", "Blend", "Concoction",
		"Brew", "Infusion", "Tincture", "Potion", "Draught", "Elixir", "Panacea",
		"Remedy", "Cure", "Solution", "Antidote", "Nostrum", "Balm", "Salve",
		"Ointment", "Lotion", "Cream", "Poultice", "Plaster", "Dressing", "Pack",
		"Wrap", "Bandage", "Binding", "Tie", "Ligature", "Strap", "Belt", "Girdle",
		"Harness", "Yoke", "Collar", "Leash", "Chain", "Shackle", "Manacle", "Fetter",
		"Bond", "Constraint", "Brace", "Support", "Stay", "Buttress", "Prop", "Shore",
		"Anchor", "Mooring", "Berth", "Dock", "Quay", "Wharf", "Pier", "Jetty", 
		"Breakwater", "Seawall", "Dyke", "Levee", "Barrage", "Dam", "Weir", "Lock",
		"Gate", "Barrier", "Fence", "Wall", "Hedge", "Ditch", "Moat", "Trench",
		"Channel", "Conduit", "Aqueduct", "Pipe", "Tube", "Duct", "Tunnel", "Passage",
		"Passageway", "Corridor", "Hallway", "Aisle", "Path", "Pathway", "Route",
		"Trail", "Track", "Road", "Street", "Highway", "Expressway", "Freeway",
		"Turnpike", "Bridge", "Overpass", "Underpass", "Crossing", "Intersection",
		"Junction", "Exchange", "Roundabout", "Circle", "Loop", "Bend", "Curve",
		"Arc", "Arch", "Bow", "Crescent", "Ring", "Circle", "Disk", "Sphere", 
		"Globe", "Orb", "Ball", "Bubble", "Globule", "Droplet", "Bead", "Pearl",
		"Gem", "Jewel", "Crystal", "Diamond", "Sapphire", "Ruby", "Emerald",
		"Topaz", "Amethyst", "Opal", "Jade", "Pearl", "Coral", "Amber", "Obsidian"
    };

    private static Random rand = new Random();

	private static Type[] armorTypes = new Type[]
	{
		typeof(Bascinet), typeof(CloseHelm), typeof(NorseHelm), typeof(OrcHelm), typeof(Helmet),
		typeof(LeatherGorget), typeof(PlateGorget), typeof(StuddedGorget), typeof(BoneHelm), // Added StuddedGorget and BoneHelm
		typeof(LeatherGloves), typeof(PlateGloves), typeof(StuddedGloves), typeof(BoneGloves), // Added StuddedGloves and BoneGloves
		typeof(LeatherArms), typeof(PlateArms), typeof(StuddedArms), typeof(BoneArms), // Added StuddedArms and BoneArms
		typeof(RingmailChest), typeof(ChainChest), typeof(PlateChest), typeof(LeatherChest),
		typeof(FemaleLeatherChest), typeof(FemalePlateChest), typeof(StuddedChest), typeof(BoneChest), // Added StuddedChest and BoneChest
		typeof(PlateLegs), typeof(LeatherLegs), typeof(ChainLegs), typeof(StuddedLegs), typeof(BoneLegs), // Added StuddedLegs and BoneLegs
	};

    private static SkillName[] skillNames = (SkillName[])Enum.GetValues(typeof(SkillName));

    private static Action<BaseArmor>[] armorAttributes = new Action<BaseArmor>[]
    {
        (armor) => armor.Attributes.ReflectPhysical = rand.Next(1, 30),
        (armor) => armor.Attributes.DefendChance = rand.Next(1, 30),
        (armor) => armor.Attributes.CastSpeed = rand.Next(0, 2),
        (armor) => armor.Attributes.CastRecovery = rand.Next(0, 3),
        (armor) => armor.Attributes.RegenHits = rand.Next(0, 50),
        (armor) => armor.Attributes.RegenMana = rand.Next(0, 50),
        (armor) => armor.Attributes.RegenStam = rand.Next(0, 50),
        (armor) => armor.Attributes.SpellDamage = rand.Next(0, 50),
        (armor) => armor.Attributes.WeaponDamage = rand.Next(0, 50),
        (armor) => armor.Attributes.LowerManaCost = rand.Next(0, 50),
        (armor) => armor.Attributes.LowerRegCost = rand.Next(0, 50),
        (armor) => armor.Attributes.BonusStr = rand.Next(0, 30),
        (armor) => armor.Attributes.BonusDex = rand.Next(0, 30),
        (armor) => armor.Attributes.BonusInt = rand.Next(0, 30),
        (armor) => armor.Attributes.BonusHits = rand.Next(0, 50),
        (armor) => armor.Attributes.BonusStam = rand.Next(0, 50),
        (armor) => armor.Attributes.BonusMana = rand.Next(0, 50),
        (armor) => armor.PhysicalBonus = rand.Next(1, 50),
        (armor) => armor.FireBonus = rand.Next(1, 50),
        (armor) => armor.ColdBonus = rand.Next(1, 50),
        (armor) => armor.PoisonBonus = rand.Next(1, 50),
        (armor) => armor.EnergyBonus = rand.Next(1, 50)
    };

    [Constructable]
    public RandomMagicArmor() : base(GetRandomItemID())
    {
        Type selectedType = armorTypes[rand.Next(armorTypes.Length)];
        BaseArmor tempArmor = (BaseArmor)Activator.CreateInstance(selectedType);

        string name = prefixes[rand.Next(prefixes.Length)] + " " + suffixes[rand.Next(suffixes.Length)];
        this.Name = name;

        ApplyRandomArmorAttributes(tempArmor);
        ApplySkillBonuses();
        
        this.Hue = rand.Next(1, 3001); // Generates a random hue for the armor

        int numberOfSockets = rand.Next(0, 7); // Random sockets
        XmlAttach.AttachTo(this, new XmlSockets(numberOfSockets));

        tempArmor.Delete();
    }

    private void ApplyRandomArmorAttributes(BaseArmor tempArmor)
    {
        List<Action<BaseArmor>> selectedAttributes = new List<Action<BaseArmor>>(armorAttributes);
        int numberOfAttributes = rand.Next(5, 11); // Select 5-10 attributes randomly

        for (int i = 0; i < numberOfAttributes; i++)
        {
            int index = rand.Next(selectedAttributes.Count);
            selectedAttributes[index](this);
            selectedAttributes.RemoveAt(index); // Remove to avoid duplicate attributes
        }
    }

	private void ApplySkillBonuses()
	{
		// Weighted probability distribution
		int[] probabilities = new int[] { 10, 20, 30, 40 }; // Adjust these values as needed to make higher bonuses rarer
		int totalWeight = 100; // Sum of probabilities
		int randomValue = rand.Next(totalWeight);

		int numberOfSkills = 0;
		int cumulativeWeight = 0;

		for (int i = 0; i < probabilities.Length; i++)
		{
			cumulativeWeight += probabilities[i];
			if (randomValue < cumulativeWeight)
			{
				numberOfSkills = i + 1;
				break;
			}
		}

		for (int i = 0; i < numberOfSkills; i++)
		{
			SkillName skill = skillNames[rand.Next(skillNames.Length)];
			this.SkillBonuses.SetValues(i, skill, rand.Next(1, 21)); // Bonus between 1 and 20
		}
	}


    private static int GetRandomItemID()
    {
        Type selectedType = armorTypes[rand.Next(armorTypes.Length)];
        BaseArmor tempArmor = (BaseArmor)Activator.CreateInstance(selectedType);
        int itemID = tempArmor.ItemID;
        tempArmor.Delete();
        return itemID;
    }

    public RandomMagicArmor(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
