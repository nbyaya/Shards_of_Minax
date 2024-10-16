using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FangOfStorms : Dagger
{
    [Constructable]
    public FangOfStorms()
    {
        Name = "Fang of Storms";
        Hue = Utility.Random(100, 2250);
        MinDamage = Utility.RandomMinMax(20, 45);
        MaxDamage = Utility.RandomMinMax(45, 85);
        Attributes.BonusInt = 10;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitLightning = 30;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FangOfStorms(Serial serial) : base(serial)
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
