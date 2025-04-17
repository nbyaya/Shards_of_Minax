using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NeptunesTrident : FishermansTrident
{
    [Constructable]
    public NeptunesTrident()
    {
        Name = "Neptune's Trident";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(75, 115);
        Attributes.BonusMana = 20;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.WaterDissipation;
        Slayer2 = SlayerName.Vacuum;
        WeaponAttributes.HitColdArea = 30;
        WeaponAttributes.HitEnergyArea = 20;
        SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NeptunesTrident(Serial serial) : base(serial)
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
