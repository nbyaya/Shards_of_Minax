using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheAscendantGale : WingedHelm
{
    [Constructable]
    public TheAscendantGale()
    {
        Name = "The Ascendant Gale";
        Hue = Utility.Random(1150, 2500); // A sky-blue hue, reminiscent of stormy skies
        BaseArmorRating = Utility.RandomMinMax(15, 50); // Reasonable base armor, balancing its powers with defense

        // Attributes that represent wind and agility
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 10;
        Attributes.CastSpeed = 1;
        Attributes.Luck = 75;

        // Wind and agility-based skill bonuses
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0); // To help with focus and calm
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0); // Fitting for an agile warrior who harnesses the wind
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0); // Enhance strategic advantage in battle

        // Bonus resistances to the elements, especially those related to air and storms
        EnergyBonus = 20;
        FireBonus = 10;

        // XmlLevelItem integration for level-based effects
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheAscendantGale(Serial serial) : base(serial)
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
