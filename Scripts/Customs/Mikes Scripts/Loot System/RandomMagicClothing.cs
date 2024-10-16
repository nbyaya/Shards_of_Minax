using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RandomMagicClothing : BaseClothing
{
    private static string[] prefixes = { "Mighty", "Powerful", "Mystic", "Enchanted", "Arcane", "Enchanted", "Mystical", "Elemental", "Eternal", "Infernal", "Celestial", "Eldritch", "Spectral", "Tempest", "Frozen", "Blazing", "Thunder", "Shadow", "Radiant", "Dark", "Light", "Phantom", "Void", "Ethereal", "Necrotic", "Divine", "Astral", "Prismatic", "Runic", "Venomous", "Frost", "Storm", "Invisible", "Invincible", "Majestic", "Cursed", "Blessed", "Soulbound", "Vortex", "Twilight", "Dawn", "Dusk", "Starforged", "Moonlit", "Sunflare", "Comet", "Eclipse", "Galactic", "Cosmic", "Dimensional", "Temporal", "Spatial", "Quantum", "Mythic", "Legendary", "Ancient", "Primordial", "Forgotten", "Unseen", "Chaos", "Harmony", "Balance", "Rage", "Serenity", "Oblivion", "Creation", "Destruction", "Rebirth", "Fate", "Dream", "Nightmare", "Illusion", "Reality", "Vision", "Ghostly", "Glorious", "Sacred", "Unholy", "Vigilant", "Warrior's", "Sorcerer's", "Seer's", "Dragon's", "Titan's", "Phoenix", "Demonic", "Angelic", "Heavenly", "Abyssal", "Solar", "Lunar", "Stellar", "Voidwalker's", "Battleworn", "Savage", "Berserker's", "Monarch's", "Guardian's", "Pirate's", "Royal", "Revenant", "Warden's", "Spectral", "Stormbringer's", "Windwalker's", "Flamebearer's", "Icewrought", "Thunderous", "Stoneskin", "Nature's", "Beastmaster's", "Shamanic", "Witch's", "Siren's", "Mercurial", "Adamant", "Sylvan", "Arcanist's", "Noble", "Explorer's", "Sentry's", "Ranger's", "Corsair's", "Assassin's", "Necromancer's", "Paladin's", "Rogue's", "Cleric's", "Elementalist's", "Chronomancer's", "Geomancer's", "Pyromancer's", "Hydromancer's", "Aeromancer's", "Biomancer's", "Cybernetic", "Technomancer's", "Alchemist's", "Summoner's", "Psionic", "Sage's", "Prophet's", "Martyr's", "Zealot's", "Reclaimer's", "Pioneer's", "Innovator's", "Vindicator's", "Arbiter's", "Sentinel's", "Defender's", "Avenger's", "Champion's", "Conqueror's", "Master's", "Primeval", "Arcadian", "Myrmidon's", "Valkyrie's", "Bard's", "Jester's", "Gladiator's", "Knight's", "Samurai's", "Ninja's", "Viking's", "Pilgrim's", "Hermit's", "Sculptor's", "Painter's", "Poet's", "Minstrel's", "Troubadour's", "Wanderer's", "Explorer's", "Adventurer's", "Seeker's", "Scholar's", "Philosopher's", "Oracle's", "Muse's", "Mystic's", "Seer's", "Soothsayer's", "Prognosticator's", "Diviner's", "Augur's", "Sibyl's", "Clairvoyant's", "Telepath's", "Empath's", "Psychic's", "Medium's", "Spiritualist's", "Channeler's", "Shapeshifter's", "Transformer's", "Metamorph's", "Changeling's", "Morpher's", "Transmuter's", "Alchemist's", "Chemist's", "Potioneer's", "Apothecary's", "Herbalist's", "Botanist's", "Horticulturist's", "Agronomist's", "Cultivator's", "Farmer's", "Gardener's", "Landscaper's", "Arborist's", "Forester's", "Logger's", "Woodcutter's", "Carpenter's", "Joiner's", "Cabinetmaker's", "Woodworker's", "Craftsman's", "Artisan's", "Maker's", "Creator's", "Inventor's", "Designer's", "Architect's", "Engineer's", "Builder's", "Constructor's", "Fabricator's", "Manufacturer's", "Producer's", "Director's", "Manager's", "Supervisor's", "Coordinator's", "Organizer's", "Planner's", "Strategist's", "Analyst's", "Consultant's", "Advisor's", "Counselor's", "Mentor's", "Tutor's", "Teacher's", "Instructor's", "Educator's", "Professor's", "Lecturer's", "Trainer's", "Coach's", "Drillmaster's", "Taskmaster's", "Mastermind's", "Genius's", "Savant's", "Expert's", "Specialist's", "Professional's", "Technician's", "Mechanic's", "Operator's", "Worker's", "Laborer's", "Handyman's", "Repairman's", "Serviceman's", "Maintenance's", "Custodian's", "Janitor's", "Cleaner's", "Sanitation's", "Hygienist's", "Health's", "Medical's", "Nurse's", "Doctor's", "Physician's", "Surgeon's", "Dentist's", "Pharmacist's", "Therapist's", "Psychologist's", "Psychiatrist's", "Counselor's", "Social Worker's", "Case Manager's", "Advocate's", "Mediator's", "Negotiator's", "Arbitrator's", "Judge's", "Magistrate's", "Jurist's", "Lawyer's", "Attorney's", "Counsel's", "Solicitor's", "Barrister's", "Advocate's", "Prosecutor's", "Defender's", "Litigator's", "Trial Lawyer's", "Appellate Lawyer's", "Legal Advisor's", "Legal Consultant's", "Legal Analyst's", "Paralegal's", "Legal Assistant's", "Clerk's", "Secretary's", "Assistant's", "Aide's", "Helper's", "Support's", "Backer's", "Sponsor's", "Patron's", "Benefactor's", "Donor's", "Contributor's", "Investor's", "Shareholder's", "Stakeholder's", "Partner's", "Co-owner's", "Joint Venture's", "Syndicate's", "Consortium's", "Alliance's", "Coalition's", "Federation's", "Union's", "Association's", "Society's", "Club's", "Group's", "Team's", "Crew's", "Gang's", "Band's", "Troupe's", "Company's", "Corporation's", "Enterprise's", "Firm's", "Business's", "Agency's", "Bureau's", "Office's", "Department's", "Division's", "Section's", "Unit's", "Branch's", "Subsidiary's", "Affiliate's", "Franchise's", "Chain's", "Outlet's", "Store's", "Shop's", "Boutique's", "Emporium's", "Marketplace's", "Mart's", "Mall's", "Plaza's", "Center's", "Complex's", "Hub's", "Terminal's", "Station's", "Port's", "Harbor's", "Marina's", "Dock's", "Wharf's", "Quay's", "Pier's", "Jetty's", "Breakwater's", "Seawall's", "Bulwark's", "Rampart's", "Bastion's", "Fortress's", "Castle's", "Palace's", "Manor's", "Mansion's", "Estate's", "Villa's", "Chateau's", "Lodge's", "Cabin's", "Cottage's", "Bungalow's", "Hut's", "Shack's", "Shed's", "Barn's", "Stable's", "Kennel's", "Cattery's", "Aviary's", "Aquarium's", "Zoo's", "Safari Park's", "Wildlife Reserve's", "Nature Preserve's", "National Park's", "State Park's", "Provincial Park's", "Regional Park's", "City Park's", "Public Garden's", "Botanical Garden's", "Arboretum's", "Greenhouse's", "Nursery's", "Farm's", "Ranch's", "Plantation's", "Orchard's", "Vineyard's", "Winery's", "Brewery's", "Distillery's", "Factory's", "Mill's", "Plant's", "Workshop's", "Studio's", "Gallery's", "Museum's", "Library's", "Archive's", "Repository's", "Depot's", "Warehouse's", "Storage's", "Silo's", "Tank's", "Reservoir's", "Container's", "Vessel's", "Ship's", "Boat's", "Vessel's", "Craft's", "Yacht's", "Sailboat's", "Motorboat's", "Speedboat's", "Ferry's", "Cruise Ship's", "Liner's", "Tanker's", "Freighter's", "Cargo Ship's", "Container Ship's", "Battleship's", "Destroyer's", "Frigate's", "Submarine's", "Aircraft Carrier's", "Warship's", "Naval Ship's", "Military Vessel's", "Patrol Boat's", "Coast Guard Cutter's", "Icebreaker's", "Research Vessel's", "Exploration Ship's", "Adventure's", "Expedition's", "Voyage's", "Journey's", "Trip's", "Tour's", "Excursion's", "Outing's", "Safari's", "Trek's", "Hike's", "Walk's", "Stroll's", "Ramble's", "Wander's", "Roam's", "Travel's", "Venture's", "Quest's", "Mission's", "Campaign's", "Crusade's", "Drive's", "Push's", "Effort's", "Attempt's", "Trial's", "Test's", "Experiment's", "Study's", "Investigation's", "Inquiry's", "Research's", "Exploration's", "Discovery's", "Find's", "Revelation's", "Uncovering's", "Exposure's", "Reveal's", "Show's", "Presentation's", "Display's", "Exhibition's", "Demonstration's", "Performance's", "Act's", "Scene's", "Episode's", "Chapter's", "Volume's", "Book's", "Publication's", "Release's", "Launch's", "Debut's", "Premiere's", "Opening's", "Introduction's", "Inauguration's", "Commencement's", "Start's", "Beginning's", "Origin's", "Genesis's", "Creation's", "Formation's", "Development's", "Evolution's", "Progress's", "Advancement's", "Improvement's", "Enhancement's", "Upgrade's", "Update's", "Revision's", "Modification's", "Change's", "Alteration's", "Transformation's", "Conversion's", "Switch's", "Substitution's", "Replacement's", "Exchange's", "Trade's", "Swap's", "Shift's", "Transfer's", "Movement's", "Motion's", "Action's", "Activity's", "Operation's", "Function's", "Process's", "Procedure's", "Method's", "Technique's", "Strategy's", "Tactic's", "Plan's", "Scheme's", "Design's", "Blueprint's", "Outline's", "Sketch's", "Draft's", "Diagram's", "Chart's", "Map's", "Plan's", "Layout's", "Arrangement's", "Organization's", "Structure's", "Framework's", "System's", "Network's", "Grid's", "Matrix's", "Web's", "Complex's", "Compound's", "Aggregate's", "Mixture's", "Blend's", "Combination's", "Amalgamation's", "Integration's", "Union's", "Fusion's", "Merger's", "Consolidation's", "Unification's", "Synthesis's", "Harmonization's", "Coordination's", "Alignment's", "Congruence's", "Correspondence's", "Match's", "Pairing's", "Coupling's", "Linkage's", "Connection's", "Bond's", "Tie's", "Relationship's", "Association's", "Affiliation's", "Partnership's", "Collaboration's", "Cooperation's", "Teamwork's", "Synergy's", "Interplay's", "Interaction's", "Interrelation's", "Interdependence's", "Mutuality's", "Reciprocity's", "Exchange's", "Dialogue's", "Conversation's", "Discussion's", "Debate's", "Argument's", "Dispute's", "Controversy's", "Conflict's", "Struggle's", "Fight's", "Battle's", "War's", "Combat's", "Engagement's", "Encounter's", "Skirmish's", "Clash's", "Confrontation's", "Showdown's", "Face-off's", "Duel's", "Matchup's", "Competition's", "Contest's", "Tournament's", "Championship's", "Race's", "Game's", "Sport's", "Event's", "Occasion's", "Celebration's", "Festival's", "Fair's", "Carnival's", "Party's", "Gathering's", "Meeting's", "Assembly's", "Convention's", "Conference's", "Symposium's", "Seminar's", "Workshop's", "Course's", "Class's" };
    private static string[] suffixes = { "Majesty", "Splendor", "Virtue", "Valor", "Honor", "Prestige", "Glory",
		"Brilliance", "Radiance", "Gleam", "Shine", "Luster", "Glow", "Sparkle", "Dazzle", "Twinkle",
		"Whisper", "Silence", "Shadow", "Veil", "Mantle", "Cloak", "Shroud", "Mask", "Enigma",
		"Promise", "Hope", "Miracle", "Wonder", "Bliss", "Delight", "Pleasure", "Joy", "Rapture",
		"Harmony", "Peace", "Serenity", "Tranquility", "Calm", "Rest", "Quiet", "Stillness", "Silence",
		"Mystery", "Secret", "Obscurity", "Enigma", "Cipher", "Riddle", "Puzzle", "Conundrum", "Anomaly",
		"Vision", "Dream", "Illusion", "Fantasy", "Fable", "Legend", "Myth", "Epic", "Saga",
		"Aura", "Aegis", "Shield", "Guardian", "Protector", "Defender", "Keeper", "Warden", "Champion",
		"Flame", "Fire", "Fervor", "Fury", "Wrath", "Rage", "Tempest", "Storm", "Thunder",
		"Wave", "River", "Stream", "Flow", "Tide", "Surge", "Flood", "Deluge", "Torrent",
		"Wind", "Breeze", "Gust", "Blast", "Whirlwind", "Cyclone", "Hurricane", "Tornado", "Typhoon",
		"Stone", "Rock", "Boulder", "Mound", "Hill", "Mountain", "Peak", "Summit", "Pinnacle",
		"Star", "Comet", "Meteor", "Orbit", "Galaxy", "Universe", "Cosmos", "Space", "Void",
		"Light", "Ray", "Beam", "Gleam", "Flash", "Burst", "Blaze", "Flare", "Glimmer",
		"Darkness", "Night", "Gloom", "Shadow", "Eclipse", "Obsidian", "Onyx", "Void", "Abyss",
		"Frost", "Ice", "Snow", "Chill", "Cold", "Freeze", "Glacier", "Winter", "Arctic",
		"Thread", "Weave", "Tapestry", "Embroidery", "Fabric", "Texture", "Pattern", "Motif", "Design",
		"Silk", "Velvet", "Satin", "Linen", "Cotton", "Wool", "Leather", "Lace", "Brocade" };

    private static Type[] clothingTypes = new Type[]
    {
        typeof(Shirt),
        typeof(ShortPants),
        typeof(LongPants),
        typeof(Skirt),
        typeof(Kilt),
        typeof(HalfApron),
        typeof(FullApron),
        typeof(Robe),
        typeof(Cloak),
        typeof(Cap),
        typeof(WideBrimHat),
        typeof(StrawHat),
        typeof(TallStrawHat),
        typeof(WizardsHat),
        typeof(Bonnet),
        typeof(FeatheredHat),
        typeof(TricorneHat),
        typeof(JesterHat),
        typeof(FloppyHat),
        typeof(Bandana),
        typeof(SkullCap),
        typeof(ThighBoots),
        typeof(Boots),
        typeof(Sandals),
        typeof(Shoes),
        typeof(BodySash),
        typeof(Doublet),
        typeof(Tunic),
        typeof(Surcoat),
        typeof(PlainDress),
        typeof(FancyDress),
        typeof(Cloak),
        typeof(Robe),
        typeof(JesterSuit),
        typeof(HoodedShroudOfShadows),
        typeof(ElvenShirt),
        typeof(ElvenPants),
        typeof(ElvenBoots),
    };

    private static SkillName[] allSkills = new SkillName[]
    {
        SkillName.Alchemy,
        SkillName.Anatomy,
        SkillName.AnimalLore,
        SkillName.ItemID,
        SkillName.ArmsLore,
        SkillName.Parry,
        SkillName.Begging,
        SkillName.Blacksmith,
        SkillName.Fletching,
        SkillName.Peacemaking,
        SkillName.Camping,
        SkillName.Carpentry,
        SkillName.Cartography,
        SkillName.Cooking,
        SkillName.DetectHidden,
        SkillName.Discordance,
        SkillName.EvalInt,
        SkillName.Healing,
        SkillName.Fishing,
        SkillName.Forensics,
        SkillName.Herding,
        SkillName.Hiding,
        SkillName.Provocation,
        SkillName.Inscribe,
        SkillName.Lockpicking,
        SkillName.Magery,
        SkillName.MagicResist,
        SkillName.Tactics,
        SkillName.Snooping,
        SkillName.Musicianship,
        SkillName.Poisoning,
        SkillName.Archery,
        SkillName.SpiritSpeak,
        SkillName.Stealing,
        SkillName.Tailoring,
        SkillName.AnimalTaming,
        SkillName.TasteID,
        SkillName.Tinkering,
        SkillName.Tracking,
        SkillName.Veterinary,
        SkillName.Swords,
        SkillName.Macing,
        SkillName.Fencing,
        SkillName.Wrestling,
        SkillName.Lumberjacking,
        SkillName.Mining,
        SkillName.Meditation,
        SkillName.Stealth,
        SkillName.RemoveTrap,
        SkillName.Necromancy,
        SkillName.Focus,
        SkillName.Chivalry,
        SkillName.Bushido,
        SkillName.Ninjitsu,
        SkillName.Spellweaving,
        SkillName.Mysticism,
        SkillName.Imbuing,
        SkillName.Throwing
    };

    private static Random rand = new Random();

    // A list of all possible effects, customize this list according to your game's requirements
    private static Action<BaseClothing>[] allEffects = new Action<BaseClothing>[]
    {
        (clothing) => clothing.Attributes.BonusStr = rand.Next(1, 20),
        (clothing) => clothing.Attributes.BonusDex = rand.Next(1, 20),
        (clothing) => clothing.Attributes.BonusInt = rand.Next(1, 20),
        (clothing) => clothing.Attributes.BonusHits = rand.Next(1, 30),
        (clothing) => clothing.Attributes.BonusStam = rand.Next(1, 30),
        (clothing) => clothing.Attributes.BonusMana = rand.Next(1, 30),
        (clothing) => clothing.Attributes.RegenHits = rand.Next(1, 20),
        (clothing) => clothing.Attributes.RegenStam = rand.Next(1, 20),
        (clothing) => clothing.Attributes.RegenMana = rand.Next(1, 20),
        (clothing) => clothing.Attributes.DefendChance = rand.Next(5, 16),
        (clothing) => clothing.Attributes.AttackChance = rand.Next(5, 16),
        (clothing) => clothing.Attributes.Luck = rand.Next(10, 500),
        (clothing) => clothing.Attributes.WeaponDamage = rand.Next(1, 25),
        (clothing) => clothing.Attributes.WeaponSpeed = rand.Next(5, 16),
        (clothing) => clothing.Attributes.SpellDamage = rand.Next(1, 50),
        (clothing) => clothing.Attributes.CastSpeed = rand.Next(1, 3),
        (clothing) => clothing.Attributes.CastRecovery = rand.Next(1, 3),
        (clothing) => clothing.Attributes.LowerManaCost = rand.Next(5, 50),
        (clothing) => clothing.Attributes.LowerRegCost = rand.Next(5, 50),
        (clothing) => clothing.Attributes.ReflectPhysical = rand.Next(1, 50),
        (clothing) => clothing.Attributes.EnhancePotions = rand.Next(5, 50),
        (clothing) => clothing.Attributes.SpellChanneling = 1,
        (clothing) => clothing.Attributes.NightSight = 1,
        (clothing) => clothing.Resistances.Physical = rand.Next(1, 50),
        (clothing) => clothing.Resistances.Fire = rand.Next(1, 50),
        (clothing) => clothing.Resistances.Cold = rand.Next(1, 50),
        (clothing) => clothing.Resistances.Poison = rand.Next(1, 50),
        (clothing) => clothing.Resistances.Energy = rand.Next(1, 50),
    };

    [Constructable]
    public RandomMagicClothing() : base(0, Layer.Invalid) // Initialize with invalid layer
    {
        Type selectedType = clothingTypes[rand.Next(clothingTypes.Length)];
        BaseClothing tempClothing = (BaseClothing)Activator.CreateInstance(selectedType);

        // Set the correct properties based on the selected clothing type
        this.ItemID = tempClothing.ItemID;
        this.Layer = tempClothing.Layer;

        this.Name = prefixes[rand.Next(prefixes.Length)] + " " + suffixes[rand.Next(suffixes.Length)];
        this.Hue = rand.Next(1, 3001);

        // Add the socket attachment code here
        int numberOfSockets = rand.Next(0, 7); // Adjust based on your game's design
        XmlAttach.AttachTo(this, new XmlSockets(numberOfSockets));

        InitializeClothingAttributes();

        tempClothing.Delete();
    }

    private void InitializeClothingAttributes()
    {
        double tierChance = rand.NextDouble();

        int effectCount;
        if (tierChance < 0.05) // Very rare
        {
            effectCount = 5;
        }
        else if (tierChance < 0.2) // Rare
        {
            effectCount = 4;
        }
        else if (tierChance < 0.5) // Uncommon
        {
            effectCount = 3;
        }
        else // Common
        {
            effectCount = 2;
        }

        ApplyRandomEffects(this, effectCount);
        ApplyRandomSkillBonuses(this);
    }

    private void ApplyRandomEffects(BaseClothing clothing, int numberOfEffects)
    {
       		
		for (int i = 0; i < numberOfEffects; i++)
        {
            int effectIndex = rand.Next(allEffects.Length);
            allEffects[effectIndex](clothing);
        }
    }

	private void ApplyRandomSkillBonuses(BaseClothing clothing)
	{
		int skillBonusCount;
		double tierChance = rand.NextDouble();

		// Define rarity tiers for skill bonuses
		if (tierChance < 0.1) // Very rare
		{
			skillBonusCount = 5;
		}
		else if (tierChance < 0.3) // Rare
		{
			skillBonusCount = 4;
		}
		else if (tierChance < 0.6) // Uncommon
		{
			skillBonusCount = 3;
		}
		else // Common
		{
			skillBonusCount = 2;
		}

		// Apply the chosen number of skill bonuses
		for (int i = 0; i < skillBonusCount; i++)
		{
			SkillName randomSkill = allSkills[rand.Next(allSkills.Length)];
			int randomValue = rand.Next(1, 15); // Skill bonus between 1 and 15
			clothing.SkillBonuses.SetValues(i, randomSkill, randomValue);
		}
	}


    private static int GetRandomItemID()
    {
        Type selectedType = clothingTypes[rand.Next(clothingTypes.Length)];
        BaseClothing tempClothing = (BaseClothing)Activator.CreateInstance(selectedType);
        return tempClothing.ItemID;
    }

    public RandomMagicClothing(Serial serial) : base(serial) { }

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
