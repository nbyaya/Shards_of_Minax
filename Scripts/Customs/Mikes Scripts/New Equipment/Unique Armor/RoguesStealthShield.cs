using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoguesStealthShield : Buckler
{
    [Constructable]
    public RoguesStealthShield()
    {
        Name = "Rogue's Stealth Shield";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.ResonanceKinetic = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 20;
        Attributes.NightSight = 1;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoguesStealthShield(Serial serial) : base(serial)
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
