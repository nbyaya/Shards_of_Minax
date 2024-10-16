using System;
using Server.Mobiles;

namespace Server.Items
{
    public class AnimalContractType
    {
        public static AnimalContractType[] Get = new AnimalContractType[]
        {
			new AnimalContractType(typeof(Alligator), "Alligator"),
			new AnimalContractType(typeof(BakeKitsune), "BakeKitsune"),
			new AnimalContractType(typeof(Beetle), "Beetle"),
			new AnimalContractType(typeof(Bird), "Bird"),
			new AnimalContractType(typeof(BlackBear), "BlackBear"),
			new AnimalContractType(typeof(Boar), "Boar"),
			new AnimalContractType(typeof(Bull), "Bull"),
			new AnimalContractType(typeof(BullFrog), "BullFrog"),
			new AnimalContractType(typeof(Cat), "Cat"),
			new AnimalContractType(typeof(Chicken), "Chicken"),
			new AnimalContractType(typeof(CoconutCrab), "CoconutCrab"),
			new AnimalContractType(typeof(Cougar), "Cougar"),
			new AnimalContractType(typeof(Cow), "Cow"),
			new AnimalContractType(typeof(Crane), "Crane"),
			new AnimalContractType(typeof(CrimsonDrake), "CrimsonDrake"),
			new AnimalContractType(typeof(CuSidhe), "CuSidhe"),
			new AnimalContractType(typeof(DesertOstard), "DesertOstard"),
			new AnimalContractType(typeof(DireWolf), "DireWolf"),
			new AnimalContractType(typeof(Dog), "Dog"),
			new AnimalContractType(typeof(Dragon), "Dragon"),
			new AnimalContractType(typeof(Drake), "Drake"),
			new AnimalContractType(typeof(DreadSpider), "DreadSpider"),
			new AnimalContractType(typeof(Eagle), "Eagle"),
			new AnimalContractType(typeof(Ferret), "Ferret"),
			new AnimalContractType(typeof(FireBeetle), "FireBeetle"),
			new AnimalContractType(typeof(FireSteed), "FireSteed"),
			new AnimalContractType(typeof(FrostDragon), "FrostDragon"),
			new AnimalContractType(typeof(FrostDrake), "FrostDrake"),
			new AnimalContractType(typeof(FrostSpider), "FrostSpider"),
			new AnimalContractType(typeof(Gaman), "Gaman"),
			new AnimalContractType(typeof(GiantRat), "GiantRat"),
			new AnimalContractType(typeof(GiantSerpent), "GiantSerpent"),
			new AnimalContractType(typeof(GiantSpider), "GiantSpider"),
			new AnimalContractType(typeof(GiantToad), "GiantToad"),
			new AnimalContractType(typeof(GiantTurkey), "GiantTurkey"),
			new AnimalContractType(typeof(Goat), "Goat"),
			new AnimalContractType(typeof(Gorilla), "Gorilla"),
			new AnimalContractType(typeof(GreatHart), "GreatHart"),
			new AnimalContractType(typeof(GreaterDragon), "GreaterDragon"),
			new AnimalContractType(typeof(GreyWolf), "GreyWolf"),
			new AnimalContractType(typeof(GrizzlyBear), "GrizzlyBear"),
			new AnimalContractType(typeof(HellHound), "HellHound"),
			new AnimalContractType(typeof(Hind), "Hind"),
			new AnimalContractType(typeof(Hiryu), "Hiryu"),
			new AnimalContractType(typeof(Horse), "Horse"),
			new AnimalContractType(typeof(IceSerpent), "IceSerpent"),
			new AnimalContractType(typeof(IceSnake), "IceSnake"),
			new AnimalContractType(typeof(LavaLizard), "LavaLizard"),
			new AnimalContractType(typeof(LesserHiryu), "LesserHiryu"),
			new AnimalContractType(typeof(Lion), "Lion"),
			new AnimalContractType(typeof(Llama), "Llama"),
			new AnimalContractType(typeof(Mongbat), "Mongbat"),
			new AnimalContractType(typeof(MountainGoat), "MountainGoat"),
			new AnimalContractType(typeof(Nightmare), "Nightmare"),
			new AnimalContractType(typeof(Panther), "Panther"),
			new AnimalContractType(typeof(Phoenix), "Phoenix"),
			new AnimalContractType(typeof(Pig), "Pig"),
			new AnimalContractType(typeof(PolarBear), "PolarBear"),
			new AnimalContractType(typeof(Rabbit), "Rabbit"),
			new AnimalContractType(typeof(Raptor), "Raptor"),
			new AnimalContractType(typeof(Rat), "Rat"),
			new AnimalContractType(typeof(RidableLlama), "RidableLlama"),
			new AnimalContractType(typeof(Ridgeback), "Ridgeback"),
			new AnimalContractType(typeof(Scorpion), "Scorpion"),
			new AnimalContractType(typeof(Sheep), "Sheep"),
			new AnimalContractType(typeof(Squirrel), "Squirrel"),
			new AnimalContractType(typeof(SwampDragon), "SwampDragon"),
			new AnimalContractType(typeof(TimberWolf), "TimberWolf"),
			new AnimalContractType(typeof(Turkey), "Turkey"),
			new AnimalContractType(typeof(Unicorn), "Unicorn"),
			new AnimalContractType(typeof(VampireBat), "VampireBat"),
			new AnimalContractType(typeof(Walrus), "Walrus"),
			new AnimalContractType(typeof(WhiteWolf), "WhiteWolf"),
			new AnimalContractType(typeof(WhiteWyrm), "WhiteWyrm"),
			new AnimalContractType(typeof(WildTiger), "WildTiger"),
			new AnimalContractType(typeof(Wyvern), "Wyvern"),
            // Add other types as needed
        };

        private Type m_Type;
        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public AnimalContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
