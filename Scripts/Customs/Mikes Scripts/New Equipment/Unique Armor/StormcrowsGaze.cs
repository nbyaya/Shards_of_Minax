using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormcrowsGaze : NorseHelm
{
    [Constructable]
    public StormcrowsGaze()
    {
        Name = "Stormcrow's Gaze";
        Hue = Utility.Random(400, 600);
        BaseArmorRating = Utility.RandomMinMax(58, 88);
        AbsorptionAttributes.ResonanceFire = 15;
        AbsorptionAttributes.ResonanceEnergy = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 25.0);
        ColdBonus = 15;
        EnergyBonus = 25;
        FireBonus = 25;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormcrowsGaze(Serial serial) : base(serial)
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
