using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LunchadorsMask : LeatherNinjaHood
{
    [Constructable]
    public LunchadorsMask()
    {
        Name = "Luchador's Mask";
        Hue = Utility.Random(200, 850);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusDex = 30;
        Attributes.ReflectPhysical = 20;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        PhysicalBonus = 20;
        ColdBonus = 15;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LunchadorsMask(Serial serial) : base(serial)
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
