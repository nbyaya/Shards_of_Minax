using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ImmortalKingsIronCrown : PlateHelm
{
    [Constructable]
    public ImmortalKingsIronCrown()
    {
        Name = "Immortal King's Iron Crown";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusStr = 40;
        Attributes.BonusStam = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        FireBonus = 20;
        PhysicalBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ImmortalKingsIronCrown(Serial serial) : base(serial)
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
