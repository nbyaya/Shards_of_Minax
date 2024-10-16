using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InquisitorsGuard : CloseHelm
{
    [Constructable]
    public InquisitorsGuard()
    {
        Name = "Inquisitor's Guard";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 85);
        AbsorptionAttributes.ResonanceEnergy = 25;
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 15;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
        ColdBonus = 10;
        EnergyBonus = 25;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InquisitorsGuard(Serial serial) : base(serial)
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
