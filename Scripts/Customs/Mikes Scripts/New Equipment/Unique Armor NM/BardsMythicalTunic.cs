using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BardsMythicalTunic : LeatherChest
{
    [Constructable]
    public BardsMythicalTunic()
    {
        Name = "Bard's Mythical Tunic";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.ResonanceEnergy = 20;
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusDex = 25;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 50.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 40.0);
        PhysicalBonus = 12;
        EnergyBonus = 18;
        FireBonus = 12;
        ColdBonus = 12;
        PoisonBonus = 12;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BardsMythicalTunic(Serial serial) : base(serial)
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
