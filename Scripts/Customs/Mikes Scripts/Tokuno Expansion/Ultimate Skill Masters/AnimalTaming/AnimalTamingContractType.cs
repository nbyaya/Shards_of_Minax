using System;
using Server.Mobiles;

namespace Server.Items
{
    public class AnimalTamingContractType
    {
        public static AnimalTamingContractType[] Get = new AnimalTamingContractType[]
        {
			new AnimalTamingContractType(typeof(Alligator), "Alligator"),
			new AnimalTamingContractType(typeof(BakeKitsune), "BakeKitsune"),
			new AnimalTamingContractType(typeof(Beetle), "Beetle"),
			new AnimalTamingContractType(typeof(Bird), "Bird"),
			new AnimalTamingContractType(typeof(BlackBear), "BlackBear"),
			new AnimalTamingContractType(typeof(Boar), "Boar"),
			new AnimalTamingContractType(typeof(Bull), "Bull"),
			new AnimalTamingContractType(typeof(BullFrog), "BullFrog"),
			new AnimalTamingContractType(typeof(Cat), "Cat"),
			new AnimalTamingContractType(typeof(Chicken), "Chicken"),
			new AnimalTamingContractType(typeof(CoconutCrab), "CoconutCrab"),
			new AnimalTamingContractType(typeof(Cougar), "Cougar"),
			new AnimalTamingContractType(typeof(Cow), "Cow"),
			new AnimalTamingContractType(typeof(Crane), "Crane"),
			new AnimalTamingContractType(typeof(CrimsonDrake), "CrimsonDrake"),
			new AnimalTamingContractType(typeof(CuSidhe), "CuSidhe"),
			new AnimalTamingContractType(typeof(DesertOstard), "DesertOstard"),
			new AnimalTamingContractType(typeof(DireWolf), "DireWolf"),
			new AnimalTamingContractType(typeof(Dog), "Dog"),
			new AnimalTamingContractType(typeof(Dragon), "Dragon"),
			new AnimalTamingContractType(typeof(Drake), "Drake"),
			new AnimalTamingContractType(typeof(DreadSpider), "DreadSpider"),
			new AnimalTamingContractType(typeof(Eagle), "Eagle"),
			new AnimalTamingContractType(typeof(Ferret), "Ferret"),
			new AnimalTamingContractType(typeof(FireBeetle), "FireBeetle"),
			new AnimalTamingContractType(typeof(FireSteed), "FireSteed"),
			new AnimalTamingContractType(typeof(FrostDragon), "FrostDragon"),
			new AnimalTamingContractType(typeof(FrostDrake), "FrostDrake"),
			new AnimalTamingContractType(typeof(FrostSpider), "FrostSpider"),
			new AnimalTamingContractType(typeof(Gaman), "Gaman"),
			new AnimalTamingContractType(typeof(GiantRat), "GiantRat"),
			new AnimalTamingContractType(typeof(GiantSerpent), "GiantSerpent"),
			new AnimalTamingContractType(typeof(GiantSpider), "GiantSpider"),
			new AnimalTamingContractType(typeof(GiantToad), "GiantToad"),
			new AnimalTamingContractType(typeof(GiantTurkey), "GiantTurkey"),
			new AnimalTamingContractType(typeof(Goat), "Goat"),
			new AnimalTamingContractType(typeof(Gorilla), "Gorilla"),
			new AnimalTamingContractType(typeof(GreatHart), "GreatHart"),
			new AnimalTamingContractType(typeof(GreaterDragon), "GreaterDragon"),
			new AnimalTamingContractType(typeof(GreyWolf), "GreyWolf"),
			new AnimalTamingContractType(typeof(GrizzlyBear), "GrizzlyBear"),
			new AnimalTamingContractType(typeof(HellHound), "HellHound"),
			new AnimalTamingContractType(typeof(Hind), "Hind"),
			new AnimalTamingContractType(typeof(Hiryu), "Hiryu"),
			new AnimalTamingContractType(typeof(Horse), "Horse"),
			new AnimalTamingContractType(typeof(IceSerpent), "IceSerpent"),
			new AnimalTamingContractType(typeof(IceSnake), "IceSnake"),
			new AnimalTamingContractType(typeof(LavaLizard), "LavaLizard"),
			new AnimalTamingContractType(typeof(LesserHiryu), "LesserHiryu"),
			new AnimalTamingContractType(typeof(Lion), "Lion"),
			new AnimalTamingContractType(typeof(Llama), "Llama"),
			new AnimalTamingContractType(typeof(Mongbat), "Mongbat"),
			new AnimalTamingContractType(typeof(MountainGoat), "MountainGoat"),
			new AnimalTamingContractType(typeof(Nightmare), "Nightmare"),
			new AnimalTamingContractType(typeof(Panther), "Panther"),
			new AnimalTamingContractType(typeof(Phoenix), "Phoenix"),
			new AnimalTamingContractType(typeof(Pig), "Pig"),
			new AnimalTamingContractType(typeof(PolarBear), "PolarBear"),
			new AnimalTamingContractType(typeof(Rabbit), "Rabbit"),
			new AnimalTamingContractType(typeof(Raptor), "Raptor"),
			new AnimalTamingContractType(typeof(Rat), "Rat"),
			new AnimalTamingContractType(typeof(RidableLlama), "RidableLlama"),
			new AnimalTamingContractType(typeof(Ridgeback), "Ridgeback"),
			new AnimalTamingContractType(typeof(Scorpion), "Scorpion"),
			new AnimalTamingContractType(typeof(Sheep), "Sheep"),
			new AnimalTamingContractType(typeof(Squirrel), "Squirrel"),
			new AnimalTamingContractType(typeof(SwampDragon), "SwampDragon"),
			new AnimalTamingContractType(typeof(TimberWolf), "TimberWolf"),
			new AnimalTamingContractType(typeof(Turkey), "Turkey"),
			new AnimalTamingContractType(typeof(Unicorn), "Unicorn"),
			new AnimalTamingContractType(typeof(VampireBat), "VampireBat"),
			new AnimalTamingContractType(typeof(Walrus), "Walrus"),
			new AnimalTamingContractType(typeof(WhiteWolf), "WhiteWolf"),
			new AnimalTamingContractType(typeof(WhiteWyrm), "WhiteWyrm"),
			new AnimalTamingContractType(typeof(WildTiger), "WildTiger"),
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

        public AnimalTamingContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
