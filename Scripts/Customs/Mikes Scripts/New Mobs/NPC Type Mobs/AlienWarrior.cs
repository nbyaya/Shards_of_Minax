using System;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class AlienWarrior : BaseCreature
    {
        private static readonly string[] AlienNames = 
		{
			"Zoltron", "Krynn", "Thargoid", "Spathi", "Androsynth", "Quixzal",
			"Klingon", "Vulcan", "Borg", "Romulan", "Ferengi", "Cardassian",
			"Bajoran", "Trill", "Betazoid", "Q Continuum", "Species 8472", "Gorn",
			"Tellarite", "Breen", "Hirogen", "Jem'Hadar", "Xindi", "Tribble",
			"Borg Collective", "Dominion", "Vorta", "Kazon", "Pakled", "Talaxian",
			"Denobulan", "Horta", "Gorn", "Suliban", "Orion", "Ktarian",
			"Nausicaan", "Pakled", "Reman", "Tzenkethi", "Tholian", "Voth",
			"Wadi", "Xindi-Insectoid", "Xindi-Aquatic", "Xindi-Primate", "Xindi-Reptilian",
			"Zalkonian", "Tellarite", "Antican", "Selay", "Vorgon", "Xindi-Arboreal",
			"Aenar", "Ba'ku", "Bynar", "Caitian", "Chalnoth", "Dosi",
			"El-Aurian", "Elasian", "Eminian", "Halkan", "Iotian", "Kelvan",
			"Malcorian", "Mintakan", "Napean", "Pah-wraith", "Paradan", "Preserver",
			"Selay", "Sheliak", "Tak Tak", "Tamarian", "Tarellian", "Thasian",
			"Tosk", "Vidiians", "Vorgon", "Xarantine", "Xindi-Primate", "Zalkonian",
			"Dalek", "Cyberman", "Sontaran", "Silurian", "Ood", "Weeping Angel", 
			"Judoon", "Time Lord", "Silence", "Slitheen", "Zygon", "Ice Warrior", 
			"Draconian", "Adipose", "Auton", "Davros", "Gelth", "Haemovore",
			"Zoltron", "Krynn", "Thargoid", "Spathi", "Androsynth", "Quixzal",
			"Wookiee", "Twilek", "Rodian", "Mon Calamari", "Sullustan", "Bothan",
			"Togruta", "Trandoshan", "Hutt", "Gamorrean", "Jawa", "Ewok",
			"Chiss", "Aqualish", "Nautolan", "Devaronian", "Ithorian", "Kel Dor",
			"Twi'lek", "Zabrak", "Mirialan", "Rattataki", "Cathar", "Nikto",
			"Gran", "Togruta", "Chadra-Fan", "Muun", "Herglic", "Yoda's Species",
			"Gungan", "Dug", "Ewok", "Gotal", "Yuzzum", "Tusken Raider",
			"Squib", "Mon Calamari", "Geonosian", "Sullustan", "Hutt", "Rodian",
			"Trandoshan", "Jawa", "Wookiee", "Kaleesh", "Abyssin", "Jango Fett's Species",
			"Dathomirian", "Selkath", "Kaminoan", "Chadra-Fan", "Amani", "Ssi-ruuk",
			"Talz", "Muun", "Yoda's Species", "Gungan", "Dug", "Gotal", "Yuzzum",
			"Tusken Raider", "Squib", "Geonosian", "Kaleesh", "Abyssin", "Dathomirian",
			"Selkath", "Kaminoan", "Amani", "Ssi-ruuk", "Talz", "Womp Rat",
			"Mon Calamari", "Kaminoan", "Bith", "Neimoidian", "Yarkora", "Klatooinian",
			"Toydarian", "Harch", "Weequay", "Kaleesh", "Abyssin", "Dathomirian",
			"Giff", "Illithid", "Beholder", "Neogi", "Eladrin", "Lizardfolk", "Aarakocra",
			"Githyanki", "Githzerai", "Kenku", "Modron", "Yuan-ti", "Saurial", "Kobold",
			"Tiefling", "Aasimar", "Tabaxi", "Firbolg", "Genasi", "Triton", "Warforged",
			"Changeling", "Kalashtar", "Shifter", "Minotaur", "Centaur", "Gnome", "Dwarf",
			"Halfling", "Half-Orc", "Half-Elf", "Goblin", "Hobgoblin", "Orc", "Bugbear",
			"Ur-Quan", "Chenjesu", "Melnorme", "Syreen", "Shofixti", "Orz", 
			"VUX", "Ilwrath", "Pkunk", "Umgah", "Supox", "Myyhrai'k", 
			"Druuge", "Taalo", "Arilou", "Yehat", "Thraddash", "Slylandro", 
			"Utwig", "Zoq-Fot-Pik", "Chmmr", "Burvixese", "Earthling", "Syntesis", 
			"Saiyan", "Namekian", "Frieza Race", "Majin", "Tuffle", "Yardrat",
			"Kai", "Frost Demon", "Ginyu Force", "Android", "Cell", "Bio-Android",
			"Kais", "Frost Demons", "Androids", "Tuffle Parasites", "Konatsians",
			"Changelings", "Brench-seijin", "Jewel People", "Dr. Wheelo's Bio-Warriors",
			"Appule's race", "Kanassan", "Alien Announcer", "Spectre", "Kettle-seijin",
			"Arlians", "Monsters of Babidi", "Monster Carrot's race", "Shamoians",
			"Pudding People", "Tori-Bot's race", "Akina's race", "Amazonians",
			"Biomen", "Fusarians", "God of Destruction's race", "Gyoshu's race",
			"Iru's race", "Yamcha's race", "Poko Priests", "Preecho's race", "Shark",
			"Shin-jin", "Kaioshin", "Cell Jr.", "Shadow Dragons", "Time Breakers",
			"Frieza Clan (Earthlings)", "Frieza Clan (Namekians)", "Ginyu Clan", "Makyan",
			"Tsufruian", "Unnamed Space-Pirates",
			// Add more names as needed
			// You can continue to add more lesser-known names from the Star Trek universe.
		};

        public override bool ClickTitle { get { return false; } }

        [Constructable]
        public AlienWarrior() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            string alienRaceName = AlienNames[Utility.Random(AlienNames.Length)];

            Title = "";
            Hue = Utility.Random(1, 3000);

            Body = Utility.RandomBool() ? 0x190 : 0x191;
            Name = alienRaceName + " Warrior";

            SetStr(300, 500);
            SetDex(300, 500);
            SetInt(300, 500);
			SetHits(500, 900);

            SetDamage(100, 150);

            SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);

            Fame = 7500;
            Karma = -7500;

            int armorHue = Utility.Random(2000, 1001);

            AddItem(new PlateChest() { Hue = armorHue, Name = alienRaceName + " Space Armor" });
            AddItem(new PlateLegs() { Hue = armorHue, Name = alienRaceName + " Space Armor" });
            AddItem(new PlateArms() { Hue = armorHue, Name = alienRaceName + " Space Armor" });
            AddItem(new PlateGloves() { Hue = armorHue, Name = alienRaceName + " Space Armor" });
            AddItem(new PlateHelm() { Hue = armorHue, Name = alienRaceName + " Space Armor" });

            // Add hair and other attributes as needed
        }

        public override void GenerateLoot()
        {
            // Your loot logic here
			AddLoot(LootPack.UltraRich);  // Even richer loot than before
        }

        public AlienWarrior(Serial serial) : base(serial)
        {
        }

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
}
