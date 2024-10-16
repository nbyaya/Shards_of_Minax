using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TimberlordsHelm : PlateHelm
{
    [Constructable]
    public TimberlordsHelm()
    {
        Name = "Timberlord's Helm";
        Hue = Utility.Random(200, 300);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusStr = 25;
        Attributes.BonusStam = 15;
        Attributes.Luck = 200;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TimberlordsHelm(Serial serial) : base(serial)
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
