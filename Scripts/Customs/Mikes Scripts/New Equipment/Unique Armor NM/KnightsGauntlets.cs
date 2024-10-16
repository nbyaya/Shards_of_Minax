using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KnightsGauntlets : PlateGloves
{
    [Constructable]
    public KnightsGauntlets()
    {
        Name = "Knight's Gauntlets";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStam = 25;
        Attributes.Luck = 100;
        SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 15;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KnightsGauntlets(Serial serial) : base(serial)
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
