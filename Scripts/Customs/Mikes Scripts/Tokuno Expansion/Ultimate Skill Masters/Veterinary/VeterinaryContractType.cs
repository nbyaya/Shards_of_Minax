using System;
using Server.Mobiles;

namespace Server.Items
{
    public class VeterinaryContractType
    {
        public static VeterinaryContractType[] Get = new VeterinaryContractType[]
        {
            new VeterinaryContractType(typeof(Alligator), "Alligator"),
            new VeterinaryContractType(typeof(BakeKitsune), "BakeKitsune"),
            new VeterinaryContractType(typeof(Beetle), "Beetle"),
            new VeterinaryContractType(typeof(Bird), "Bird"),
            new VeterinaryContractType(typeof(BlackBear), "BlackBear"),
            new VeterinaryContractType(typeof(Boar), "Boar"),
            new VeterinaryContractType(typeof(Bull), "Bull"),
            new VeterinaryContractType(typeof(BullFrog), "BullFrog"),
            new VeterinaryContractType(typeof(Cat), "Cat"),
            new VeterinaryContractType(typeof(Chicken), "Chicken"),
            new VeterinaryContractType(typeof(CoconutCrab), "CoconutCrab"),
            new VeterinaryContractType(typeof(Cougar), "Cougar"),
            new VeterinaryContractType(typeof(Cow), "Cow"),
            new VeterinaryContractType(typeof(Crane), "Crane"),
            new VeterinaryContractType(typeof(CrimsonDrake), "CrimsonDrake"),
            new VeterinaryContractType(typeof(CuSidhe), "CuSidhe"),
            new VeterinaryContractType(typeof(DesertOstard), "DesertOstard"),
            new VeterinaryContractType(typeof(DireWolf), "DireWolf"),
            new VeterinaryContractType(typeof(Dog), "Dog"),
            new VeterinaryContractType(typeof(Dragon), "Dragon"),
            new VeterinaryContractType(typeof(Drake), "Drake"),
            new VeterinaryContractType(typeof(DreadSpider), "DreadSpider"),
            new VeterinaryContractType(typeof(Eagle), "Eagle"),
            new VeterinaryContractType(typeof(Ferret), "Ferret"),
            new VeterinaryContractType(typeof(FireBeetle), "FireBeetle"),
            new VeterinaryContractType(typeof(FireSteed), "FireSteed"),
            new VeterinaryContractType(typeof(FrostDragon), "FrostDragon"),
            new VeterinaryContractType(typeof(FrostDrake), "FrostDrake"),
            new VeterinaryContractType(typeof(FrostSpider), "FrostSpider"),
            new VeterinaryContractType(typeof(Gaman), "Gaman"),
            new VeterinaryContractType(typeof(GiantRat), "GiantRat"),
            new VeterinaryContractType(typeof(GiantSerpent), "GiantSerpent"),
            new VeterinaryContractType(typeof(GiantSpider), "GiantSpider"),
            new VeterinaryContractType(typeof(GiantToad), "GiantToad"),
            new VeterinaryContractType(typeof(GiantTurkey), "GiantTurkey"),
            new VeterinaryContractType(typeof(Goat), "Goat"),
            new VeterinaryContractType(typeof(Gorilla), "Gorilla"),
            new VeterinaryContractType(typeof(GreatHart), "GreatHart"),
            new VeterinaryContractType(typeof(GreaterDragon), "GreaterDragon"),
            new VeterinaryContractType(typeof(GreyWolf), "GreyWolf"),
            new VeterinaryContractType(typeof(GrizzlyBear), "GrizzlyBear"),
            new VeterinaryContractType(typeof(HellHound), "HellHound"),
            new VeterinaryContractType(typeof(Hind), "Hind"),
            new VeterinaryContractType(typeof(Hiryu), "Hiryu"),
            new VeterinaryContractType(typeof(Horse), "Horse"),
            new VeterinaryContractType(typeof(IceSerpent), "IceSerpent"),
            new VeterinaryContractType(typeof(IceSnake), "IceSnake"),
            new VeterinaryContractType(typeof(LavaLizard), "LavaLizard"),
            new VeterinaryContractType(typeof(LesserHiryu), "LesserHiryu"),
            new VeterinaryContractType(typeof(Lion), "Lion"),
            new VeterinaryContractType(typeof(Llama), "Llama"),
            new VeterinaryContractType(typeof(Mongbat), "Mongbat"),
            new VeterinaryContractType(typeof(MountainGoat), "MountainGoat"),
            new VeterinaryContractType(typeof(Nightmare), "Nightmare"),
            new VeterinaryContractType(typeof(Panther), "Panther"),
            new VeterinaryContractType(typeof(Phoenix), "Phoenix"),
            new VeterinaryContractType(typeof(Pig), "Pig"),
            new VeterinaryContractType(typeof(PolarBear), "PolarBear"),
            new VeterinaryContractType(typeof(Rabbit), "Rabbit"),
            new VeterinaryContractType(typeof(Raptor), "Raptor"),
            new VeterinaryContractType(typeof(Rat), "Rat"),
            new VeterinaryContractType(typeof(RidableLlama), "RidableLlama"),
            new VeterinaryContractType(typeof(Ridgeback), "Ridgeback"),
            new VeterinaryContractType(typeof(Scorpion), "Scorpion"),
            new VeterinaryContractType(typeof(Sheep), "Sheep"),
            new VeterinaryContractType(typeof(Squirrel), "Squirrel"),
            new VeterinaryContractType(typeof(SwampDragon), "SwampDragon"),
            new VeterinaryContractType(typeof(TimberWolf), "TimberWolf"),
            new VeterinaryContractType(typeof(Turkey), "Turkey"),
            new VeterinaryContractType(typeof(Unicorn), "Unicorn"),
            new VeterinaryContractType(typeof(VampireBat), "VampireBat"),
            new VeterinaryContractType(typeof(Walrus), "Walrus"),
            new VeterinaryContractType(typeof(WhiteWolf), "WhiteWolf"),
            new VeterinaryContractType(typeof(WhiteWyrm), "WhiteWyrm"),
            new VeterinaryContractType(typeof(WildTiger), "WildTiger"),
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

        public VeterinaryContractType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
