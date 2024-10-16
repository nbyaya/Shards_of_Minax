using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtierSilkenRobe : LeatherChest
{
    [Constructable]
    public CourtierSilkenRobe()
    {
        Name = "Courtier's Silken Robe";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(40, 65);
        AbsorptionAttributes.ResonanceEnergy = 10;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusMana = 20;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtierSilkenRobe(Serial serial) : base(serial)
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
