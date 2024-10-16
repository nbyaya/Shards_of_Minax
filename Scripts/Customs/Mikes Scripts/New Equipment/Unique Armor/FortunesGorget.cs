using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortunesGorget : PlateGorget
{
    [Constructable]
    public FortunesGorget()
    {
        Name = "Fortune's Gorget";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.LowerStatReq = 15;
        Attributes.Luck = 100;
        SkillBonuses.SetValues(0, SkillName.TasteID, 10.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortunesGorget(Serial serial) : base(serial)
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
