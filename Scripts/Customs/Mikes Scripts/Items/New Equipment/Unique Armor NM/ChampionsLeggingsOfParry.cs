using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChampionsLeggingsOfParry : PlateLegs
{
    [Constructable]
    public ChampionsLeggingsOfParry()
    {
        Name = "Champion's Leggings of Parry";
        Hue = Utility.Random(3001, 4000);
        BaseArmorRating = Utility.RandomMinMax(65, 95);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusDex = 30;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 50.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 35.0);
        PhysicalBonus = 20;
        EnergyBonus = 15;
        FireBonus = 10;
        ColdBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChampionsLeggingsOfParry(Serial serial) : base(serial)
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
