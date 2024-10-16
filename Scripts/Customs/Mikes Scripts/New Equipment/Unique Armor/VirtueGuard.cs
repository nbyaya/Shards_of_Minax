using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VirtueGuard : Buckler
{
    [Constructable]
    public VirtueGuard()
    {
        Name = "Virtue Guard";
        Hue = Utility.Random(444, 555);
        BaseArmorRating = Utility.RandomMinMax(35, 65);
        AbsorptionAttributes.ResonanceKinetic = 15;
        ArmorAttributes.SelfRepair = 10;
        Attributes.IncreasedKarmaLoss = -10;
        Attributes.LowerRegCost = 20;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VirtueGuard(Serial serial) : base(serial)
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
