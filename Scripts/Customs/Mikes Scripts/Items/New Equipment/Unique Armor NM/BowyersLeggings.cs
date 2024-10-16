using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowyersLeggings : LeatherLegs
{
    [Constructable]
    public BowyersLeggings()
    {
        Name = "Bowyer's Leggings";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 50);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusStr = 15;
        Attributes.BonusStam = 25;
        SkillBonuses.SetValues(0, SkillName.Fletching, 50.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 40.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 30.0);
        PhysicalBonus = 18;
        FireBonus = 10;
        ColdBonus = 15;
        EnergyBonus = 15;
        PoisonBonus = 18;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowyersLeggings(Serial serial) : base(serial)
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
