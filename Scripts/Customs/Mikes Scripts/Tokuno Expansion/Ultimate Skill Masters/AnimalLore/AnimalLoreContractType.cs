using System;
using Server.Mobiles;

namespace Server.Items
{
    public class AnimalLoreContractType
    {
        public static AnimalLoreContractType[] Get = new AnimalLoreContractType[]
        {
			new AnimalLoreContractType(typeof(Alligator), "Alligator"),
			new AnimalLoreContractType(typeof(BakeKitsune), "BakeKitsune"),
			new AnimalLoreContractType(typeof(Beetle), "Beetle"),
			new AnimalLoreContractType(typeof(Bird), "Bird"),
			new AnimalLoreContractType(typeof(BlackBear), "BlackBear"),
			new AnimalLoreContractType(typeof(Boar), "Boar"),
			new AnimalLoreContractType(typeof(Bull), "Bull"),
			new AnimalLoreContractType(typeof(BullFrog), "BullFrog"),
			new AnimalLoreContractType(typeof(Cat), "Cat"),
			new AnimalLoreContractType(typeof(Chicken), "Chicken"),
			new AnimalLoreContractType(typeof(CoconutCrab), "CoconutCrab"),
			new AnimalLoreContractType(typeof(Cougar), "Cougar"),
			new AnimalLoreContractType(typeof(Cow), "Cow"),
			new AnimalLoreContractType(typeof(Crane), "Crane"),
			new AnimalLoreContractType(typeof(CrimsonDrake), "CrimsonDrake"),
			new AnimalLoreContractType(typeof(CuSidhe), "CuSidhe"),
			new AnimalLoreContractType(typeof(DesertOstard), "DesertOstard"),
			new AnimalLoreContractType(typeof(DireWolf), "DireWolf"),
			new AnimalLoreContractType(typeof(Dog), "Dog"),
			new AnimalLoreContractType(typeof(Dragon), "Dragon"),
			new AnimalLoreContractType(typeof(Drake), "Drake"),
			new AnimalLoreContractType(typeof(DreadSpider), "DreadSpider"),
			new AnimalLoreContractType(typeof(Eagle), "Eagle"),
			new AnimalLoreContractType(typeof(Ferret), "Ferret"),
			new AnimalLoreContractType(typeof(FireBeetle), "FireBeetle"),
			new AnimalLoreContractType(typeof(FireSteed), "FireSteed"),
			new AnimalLoreContractType(typeof(FrostDragon), "FrostDragon"),
			new AnimalLoreContractType(typeof(FrostDrake), "FrostDrake"),
			new AnimalLoreContractType(typeof(FrostSpider), "FrostSpider"),
			new AnimalLoreContractType(typeof(Gaman), "Gaman"),
			new AnimalLoreContractType(typeof(GiantRat), "GiantRat"),
			new AnimalLoreContractType(typeof(GiantSerpent), "GiantSerpent"),
			new AnimalLoreContractType(typeof(GiantSpider), "GiantSpider"),
			new AnimalLoreContractType(typeof(GiantToad), "GiantToad"),
			new AnimalLoreContractType(typeof(GiantTurkey), "GiantTurkey"),
			new AnimalLoreContractType(typeof(Goat), "Goat"),
			new AnimalLoreContractType(typeof(Gorilla), "Gorilla"),
			new AnimalLoreContractType(typeof(GreatHart), "GreatHart"),
			new AnimalLoreContractType(typeof(GreaterDragon), "GreaterDragon"),
			new AnimalLoreContractType(typeof(GreyWolf), "GreyWolf"),
			new AnimalLoreContractType(typeof(GrizzlyBear), "GrizzlyBear"),
			new AnimalLoreContractType(typeof(HellHound), "HellHound"),
			new AnimalLoreContractType(typeof(Hind), "Hind"),
			new AnimalLoreContractType(typeof(Hiryu), "Hiryu"),
			new AnimalLoreContractType(typeof(Horse), "Horse"),
			new AnimalLoreContractType(typeof(IceSerpent), "IceSerpent"),
			new AnimalLoreContractType(typeof(IceSnake), "IceSnake"),
			new AnimalLoreContractType(typeof(LavaLizard), "LavaLizard"),
			new AnimalLoreContractType(typeof(LesserHiryu), "LesserHiryu"),
			new AnimalLoreContractType(typeof(Lion), "Lion"),
			new AnimalLoreContractType(typeof(Llama), "Llama"),
			new AnimalLoreContractType(typeof(Mongbat), "Mongbat"),
			new AnimalLoreContractType(typeof(MountainGoat), "MountainGoat"),
			new AnimalLoreContractType(typeof(Nightmare), "Nightmare"),
			new AnimalLoreContractType(typeof(Panther), "Panther"),
			new AnimalLoreContractType(typeof(Phoenix), "Phoenix"),
			new AnimalLoreContractType(typeof(Pig), "Pig"),
			new AnimalLoreContractType(typeof(PolarBear), "PolarBear"),
			new AnimalLoreContractType(typeof(Rabbit), "Rabbit"),
			new AnimalLoreContractType(typeof(Raptor), "Raptor"),
			new AnimalLoreContractType(typeof(Rat), "Rat"),
			new AnimalLoreContractType(typeof(RidableLlama), "RidableLlama"),
			new AnimalLoreContractType(typeof(Ridgeback), "Ridgeback"),
			new AnimalLoreContractType(typeof(Scorpion), "Scorpion"),
			new AnimalLoreContractType(typeof(Sheep), "Sheep"),
			new AnimalLoreContractType(typeof(Squirrel), "Squirrel"),
			new AnimalLoreContractType(typeof(SwampDragon), "SwampDragon"),
			new AnimalLoreContractType(typeof(TimberWolf), "TimberWolf"),
			new AnimalLoreContractType(typeof(Turkey), "Turkey"),
			new AnimalLoreContractType(typeof(Unicorn), "Unicorn"),
			new AnimalLoreContractType(typeof(VampireBat), "VampireBat"),
			new AnimalLoreContractType(typeof(Walrus), "Walrus"),
			new AnimalLoreContractType(typeof(WhiteWolf), "WhiteWolf"),
			new AnimalLoreContractType(typeof(WhiteWyrm), "WhiteWyrm"),
			new AnimalLoreContractType(typeof(WildTiger), "WildTiger"),
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

        public AnimalLoreContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
