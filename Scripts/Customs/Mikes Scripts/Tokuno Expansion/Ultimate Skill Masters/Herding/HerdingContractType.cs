using System;
using Server.Mobiles;

namespace Server.Items
{
    public class HerdingContractType
    {
        public static HerdingContractType[] Get = new HerdingContractType[]
        {
            new HerdingContractType(typeof(Alligator), "Alligator"),
            new HerdingContractType(typeof(BakeKitsune), "BakeKitsune"),
            new HerdingContractType(typeof(Beetle), "Beetle"),
            new HerdingContractType(typeof(Bird), "Bird"),
            new HerdingContractType(typeof(BlackBear), "BlackBear"),
            new HerdingContractType(typeof(Boar), "Boar"),
            new HerdingContractType(typeof(Bull), "Bull"),
            new HerdingContractType(typeof(BullFrog), "BullFrog"),
            new HerdingContractType(typeof(Cat), "Cat"),
            new HerdingContractType(typeof(Chicken), "Chicken"),
            new HerdingContractType(typeof(CoconutCrab), "CoconutCrab"),
            new HerdingContractType(typeof(Cougar), "Cougar"),
            new HerdingContractType(typeof(Cow), "Cow"),
            new HerdingContractType(typeof(Crane), "Crane"),
            new HerdingContractType(typeof(CrimsonDrake), "CrimsonDrake"),
            new HerdingContractType(typeof(CuSidhe), "CuSidhe"),
            new HerdingContractType(typeof(DesertOstard), "DesertOstard"),
            new HerdingContractType(typeof(DireWolf), "DireWolf"),
            new HerdingContractType(typeof(Dog), "Dog"),
            new HerdingContractType(typeof(Dragon), "Dragon"),
            new HerdingContractType(typeof(Drake), "Drake"),
            new HerdingContractType(typeof(DreadSpider), "DreadSpider"),
            new HerdingContractType(typeof(Eagle), "Eagle"),
            new HerdingContractType(typeof(Ferret), "Ferret"),
            new HerdingContractType(typeof(FireBeetle), "FireBeetle"),
            new HerdingContractType(typeof(FireSteed), "FireSteed"),
            new HerdingContractType(typeof(FrostDragon), "FrostDragon"),
            new HerdingContractType(typeof(FrostDrake), "FrostDrake"),
            new HerdingContractType(typeof(FrostSpider), "FrostSpider"),
            new HerdingContractType(typeof(Gaman), "Gaman"),
            new HerdingContractType(typeof(GiantRat), "GiantRat"),
            new HerdingContractType(typeof(GiantSerpent), "GiantSerpent"),
            new HerdingContractType(typeof(GiantSpider), "GiantSpider"),
            new HerdingContractType(typeof(GiantToad), "GiantToad"),
            new HerdingContractType(typeof(GiantTurkey), "GiantTurkey"),
            new HerdingContractType(typeof(Goat), "Goat"),
            new HerdingContractType(typeof(Gorilla), "Gorilla"),
            new HerdingContractType(typeof(GreatHart), "GreatHart"),
            new HerdingContractType(typeof(GreaterDragon), "GreaterDragon"),
            new HerdingContractType(typeof(GreyWolf), "GreyWolf"),
            new HerdingContractType(typeof(GrizzlyBear), "GrizzlyBear"),
            new HerdingContractType(typeof(HellHound), "HellHound"),
            new HerdingContractType(typeof(Hind), "Hind"),
            new HerdingContractType(typeof(Hiryu), "Hiryu"),
            new HerdingContractType(typeof(Horse), "Horse"),
            new HerdingContractType(typeof(IceSerpent), "IceSerpent"),
            new HerdingContractType(typeof(IceSnake), "IceSnake"),
            new HerdingContractType(typeof(LavaLizard), "LavaLizard"),
            new HerdingContractType(typeof(LesserHiryu), "LesserHiryu"),
            new HerdingContractType(typeof(Lion), "Lion"),
            new HerdingContractType(typeof(Llama), "Llama"),
            new HerdingContractType(typeof(Mongbat), "Mongbat"),
            new HerdingContractType(typeof(MountainGoat), "MountainGoat"),
            new HerdingContractType(typeof(Nightmare), "Nightmare"),
            new HerdingContractType(typeof(Panther), "Panther"),
            new HerdingContractType(typeof(Phoenix), "Phoenix"),
            new HerdingContractType(typeof(Pig), "Pig"),
            new HerdingContractType(typeof(PolarBear), "PolarBear"),
            new HerdingContractType(typeof(Rabbit), "Rabbit"),
            new HerdingContractType(typeof(Raptor), "Raptor"),
            new HerdingContractType(typeof(Rat), "Rat"),
            new HerdingContractType(typeof(RidableLlama), "RidableLlama"),
            new HerdingContractType(typeof(Ridgeback), "Ridgeback"),
            new HerdingContractType(typeof(Scorpion), "Scorpion"),
            new HerdingContractType(typeof(Sheep), "Sheep"),
            new HerdingContractType(typeof(Squirrel), "Squirrel"),
            new HerdingContractType(typeof(SwampDragon), "SwampDragon"),
            new HerdingContractType(typeof(TimberWolf), "TimberWolf"),
            new HerdingContractType(typeof(Turkey), "Turkey"),
            new HerdingContractType(typeof(Unicorn), "Unicorn"),
            new HerdingContractType(typeof(VampireBat), "VampireBat"),
            new HerdingContractType(typeof(Walrus), "Walrus"),
            new HerdingContractType(typeof(WhiteWolf), "WhiteWolf"),
            new HerdingContractType(typeof(WhiteWyrm), "WhiteWyrm"),
            new HerdingContractType(typeof(WildTiger), "WildTiger"),
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

        public HerdingContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
