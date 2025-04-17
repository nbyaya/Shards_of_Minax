using System;
using Server.Mobiles;

namespace Server.Items
{
    public class TrackingContractType
    {
        public static TrackingContractType[] Get = new TrackingContractType[]
        {
            new TrackingContractType(typeof(Alligator), "Alligator"),
            new TrackingContractType(typeof(BakeKitsune), "BakeKitsune"),
            new TrackingContractType(typeof(Beetle), "Beetle"),
            new TrackingContractType(typeof(Bird), "Bird"),
            new TrackingContractType(typeof(BlackBear), "BlackBear"),
            new TrackingContractType(typeof(Boar), "Boar"),
            new TrackingContractType(typeof(Bull), "Bull"),
            new TrackingContractType(typeof(BullFrog), "BullFrog"),
            new TrackingContractType(typeof(Cat), "Cat"),
            new TrackingContractType(typeof(Chicken), "Chicken"),
            new TrackingContractType(typeof(CoconutCrab), "CoconutCrab"),
            new TrackingContractType(typeof(Cougar), "Cougar"),
            new TrackingContractType(typeof(Cow), "Cow"),
            new TrackingContractType(typeof(Crane), "Crane"),
            new TrackingContractType(typeof(CrimsonDrake), "CrimsonDrake"),
            new TrackingContractType(typeof(CuSidhe), "CuSidhe"),
            new TrackingContractType(typeof(DesertOstard), "DesertOstard"),
            new TrackingContractType(typeof(DireWolf), "DireWolf"),
            new TrackingContractType(typeof(Dog), "Dog"),
            new TrackingContractType(typeof(Dragon), "Dragon"),
            new TrackingContractType(typeof(Drake), "Drake"),
            new TrackingContractType(typeof(DreadSpider), "DreadSpider"),
            new TrackingContractType(typeof(Eagle), "Eagle"),
            new TrackingContractType(typeof(Ferret), "Ferret"),
            new TrackingContractType(typeof(FireBeetle), "FireBeetle"),
            new TrackingContractType(typeof(FireSteed), "FireSteed"),
            new TrackingContractType(typeof(FrostDragon), "FrostDragon"),
            new TrackingContractType(typeof(FrostDrake), "FrostDrake"),
            new TrackingContractType(typeof(FrostSpider), "FrostSpider"),
            new TrackingContractType(typeof(Gaman), "Gaman"),
            new TrackingContractType(typeof(GiantRat), "GiantRat"),
            new TrackingContractType(typeof(GiantSerpent), "GiantSerpent"),
            new TrackingContractType(typeof(GiantSpider), "GiantSpider"),
            new TrackingContractType(typeof(GiantToad), "GiantToad"),
            new TrackingContractType(typeof(GiantTurkey), "GiantTurkey"),
            new TrackingContractType(typeof(Goat), "Goat"),
            new TrackingContractType(typeof(Gorilla), "Gorilla"),
            new TrackingContractType(typeof(GreatHart), "GreatHart"),
            new TrackingContractType(typeof(GreaterDragon), "GreaterDragon"),
            new TrackingContractType(typeof(GreyWolf), "GreyWolf"),
            new TrackingContractType(typeof(GrizzlyBear), "GrizzlyBear"),
            new TrackingContractType(typeof(HellHound), "HellHound"),
            new TrackingContractType(typeof(Hind), "Hind"),
            new TrackingContractType(typeof(Hiryu), "Hiryu"),
            new TrackingContractType(typeof(Horse), "Horse"),
            new TrackingContractType(typeof(IceSerpent), "IceSerpent"),
            new TrackingContractType(typeof(IceSnake), "IceSnake"),
            new TrackingContractType(typeof(LavaLizard), "LavaLizard"),
            new TrackingContractType(typeof(LesserHiryu), "LesserHiryu"),
            new TrackingContractType(typeof(Lion), "Lion"),
            new TrackingContractType(typeof(Llama), "Llama"),
            new TrackingContractType(typeof(Mongbat), "Mongbat"),
            new TrackingContractType(typeof(MountainGoat), "MountainGoat"),
            new TrackingContractType(typeof(Nightmare), "Nightmare"),
            new TrackingContractType(typeof(Panther), "Panther"),
            new TrackingContractType(typeof(Phoenix), "Phoenix"),
            new TrackingContractType(typeof(Pig), "Pig"),
            new TrackingContractType(typeof(PolarBear), "PolarBear"),
            new TrackingContractType(typeof(Rabbit), "Rabbit"),
            new TrackingContractType(typeof(Raptor), "Raptor"),
            new TrackingContractType(typeof(Rat), "Rat"),
            new TrackingContractType(typeof(RidableLlama), "RidableLlama"),
            new TrackingContractType(typeof(Ridgeback), "Ridgeback"),
            new TrackingContractType(typeof(Scorpion), "Scorpion"),
            new TrackingContractType(typeof(Sheep), "Sheep"),
            new TrackingContractType(typeof(Squirrel), "Squirrel"),
            new TrackingContractType(typeof(SwampDragon), "SwampDragon"),
            new TrackingContractType(typeof(TimberWolf), "TimberWolf"),
            new TrackingContractType(typeof(Turkey), "Turkey"),
            new TrackingContractType(typeof(Unicorn), "Unicorn"),
            new TrackingContractType(typeof(VampireBat), "VampireBat"),
            new TrackingContractType(typeof(Walrus), "Walrus"),
            new TrackingContractType(typeof(WhiteWolf), "WhiteWolf"),
            new TrackingContractType(typeof(WhiteWyrm), "WhiteWyrm"),
            new TrackingContractType(typeof(WildTiger), "WildTiger"),
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

        public TrackingContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
