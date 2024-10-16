using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MinstrelsMelody : PlateGorget
{
    [Constructable]
    public MinstrelsMelody()
    {
        Name = "Minstrel's Melody";
        Hue = Utility.Random(400, 800);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.ResonanceEnergy = 10;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusMana = 25;
        Attributes.EnhancePotions = 10;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MinstrelsMelody(Serial serial) : base(serial)
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
