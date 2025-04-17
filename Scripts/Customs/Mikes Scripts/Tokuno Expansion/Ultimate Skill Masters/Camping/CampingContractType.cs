using System;
using Server.Mobiles;

namespace Server.Items
{
    public class CampingContractType
    {
        public static CampingContractType[] Get = new CampingContractType[]
        {
			new CampingContractType(typeof(Alligator), "Alligator"),
			new CampingContractType(typeof(BakeKitsune), "BakeKitsune"),
			new CampingContractType(typeof(Beetle), "Beetle"),
			new CampingContractType(typeof(Bird), "Bird"),
			new CampingContractType(typeof(BlackBear), "BlackBear"),
			new CampingContractType(typeof(Boar), "Boar"),
			new CampingContractType(typeof(Bull), "Bull"),
			new CampingContractType(typeof(BullFrog), "BullFrog"),
			new CampingContractType(typeof(Cat), "Cat"),
			new CampingContractType(typeof(Chicken), "Chicken"),
			new CampingContractType(typeof(CoconutCrab), "CoconutCrab"),
			new CampingContractType(typeof(Cougar), "Cougar"),
			new CampingContractType(typeof(Cow), "Cow"),
			new CampingContractType(typeof(Crane), "Crane"),
			new CampingContractType(typeof(CrimsonDrake), "CrimsonDrake"),
			new CampingContractType(typeof(CuSidhe), "CuSidhe"),
			new CampingContractType(typeof(DesertOstard), "DesertOstard"),
			new CampingContractType(typeof(DireWolf), "DireWolf"),
			new CampingContractType(typeof(Dog), "Dog"),
			new CampingContractType(typeof(Dragon), "Dragon"),
			new CampingContractType(typeof(Drake), "Drake"),
			new CampingContractType(typeof(DreadSpider), "DreadSpider"),
			new CampingContractType(typeof(Eagle), "Eagle"),
			new CampingContractType(typeof(Ferret), "Ferret"),
			new CampingContractType(typeof(FireBeetle), "FireBeetle"),
			new CampingContractType(typeof(FireSteed), "FireSteed"),
			new CampingContractType(typeof(FrostDragon), "FrostDragon"),
			new CampingContractType(typeof(FrostDrake), "FrostDrake"),
			new CampingContractType(typeof(FrostSpider), "FrostSpider"),
			new CampingContractType(typeof(Gaman), "Gaman"),
			new CampingContractType(typeof(GiantRat), "GiantRat"),
			new CampingContractType(typeof(GiantSerpent), "GiantSerpent"),
			new CampingContractType(typeof(GiantSpider), "GiantSpider"),
			new CampingContractType(typeof(GiantToad), "GiantToad"),
			new CampingContractType(typeof(GiantTurkey), "GiantTurkey"),
			new CampingContractType(typeof(Goat), "Goat"),
			new CampingContractType(typeof(Gorilla), "Gorilla"),
			new CampingContractType(typeof(GreatHart), "GreatHart"),
			new CampingContractType(typeof(GreaterDragon), "GreaterDragon"),
			new CampingContractType(typeof(GreyWolf), "GreyWolf"),
			new CampingContractType(typeof(GrizzlyBear), "GrizzlyBear"),
			new CampingContractType(typeof(HellHound), "HellHound"),
			new CampingContractType(typeof(Hind), "Hind"),
			new CampingContractType(typeof(Hiryu), "Hiryu"),
			new CampingContractType(typeof(Horse), "Horse"),
			new CampingContractType(typeof(IceSerpent), "IceSerpent"),
			new CampingContractType(typeof(IceSnake), "IceSnake"),
			new CampingContractType(typeof(LavaLizard), "LavaLizard"),
			new CampingContractType(typeof(LesserHiryu), "LesserHiryu"),
			new CampingContractType(typeof(Lion), "Lion"),
			new CampingContractType(typeof(Llama), "Llama"),
			new CampingContractType(typeof(Mongbat), "Mongbat"),
			new CampingContractType(typeof(MountainGoat), "MountainGoat"),
			new CampingContractType(typeof(Nightmare), "Nightmare"),
			new CampingContractType(typeof(Panther), "Panther"),
			new CampingContractType(typeof(Phoenix), "Phoenix"),
			new CampingContractType(typeof(Pig), "Pig"),
			new CampingContractType(typeof(PolarBear), "PolarBear"),
			new CampingContractType(typeof(Rabbit), "Rabbit"),
			new CampingContractType(typeof(Raptor), "Raptor"),
			new CampingContractType(typeof(Rat), "Rat"),
			new CampingContractType(typeof(RidableLlama), "RidableLlama"),
			new CampingContractType(typeof(Ridgeback), "Ridgeback"),
			new CampingContractType(typeof(Scorpion), "Scorpion"),
			new CampingContractType(typeof(Sheep), "Sheep"),
			new CampingContractType(typeof(Squirrel), "Squirrel"),
			new CampingContractType(typeof(SwampDragon), "SwampDragon"),
			new CampingContractType(typeof(TimberWolf), "TimberWolf"),
			new CampingContractType(typeof(Turkey), "Turkey"),
			new CampingContractType(typeof(Unicorn), "Unicorn"),
			new CampingContractType(typeof(VampireBat), "VampireBat"),
			new CampingContractType(typeof(Walrus), "Walrus"),
			new CampingContractType(typeof(WhiteWolf), "WhiteWolf"),
			new CampingContractType(typeof(WhiteWyrm), "WhiteWyrm"),
			new CampingContractType(typeof(WildTiger), "WildTiger"),
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

        public CampingContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
