using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IstarisTouch : PlateArms
{
    [Constructable]
    public IstarisTouch()
    {
        Name = "Istari's Touch";
        Hue = Utility.Random(400, 600);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.DurabilityBonus = 30;
        Attributes.CastRecovery = 2;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
        ColdBonus = 25;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public IstarisTouch(Serial serial) : base(serial)
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
