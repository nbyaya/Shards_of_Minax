using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WraithsBane : PlateChest
{
    [Constructable]
    public WraithsBane()
    {
        Name = "Wraith's Bane";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.DurabilityBonus = -15;
        Attributes.IncreasedKarmaLoss = 20;
        Attributes.Luck = -50;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        ColdBonus = 20;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WraithsBane(Serial serial) : base(serial)
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
