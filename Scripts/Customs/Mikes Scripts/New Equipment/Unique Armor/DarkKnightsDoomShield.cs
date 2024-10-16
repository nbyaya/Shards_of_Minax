using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkKnightsDoomShield : HeaterShield
{
    [Constructable]
    public DarkKnightsDoomShield()
    {
        Name = "Dark Knight's Doom Shield";
        Hue = Utility.Random(10, 20);
        BaseArmorRating = Utility.RandomMinMax(68, 88);
        AbsorptionAttributes.ResonanceKinetic = 20;
        ArmorAttributes.ReactiveParalyze = 2;
        Attributes.DefendChance = 30;
        Attributes.WeaponSpeed = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 30.0);
        ColdBonus = 30;
        EnergyBonus = 25;
        FireBonus = 30;
        PhysicalBonus = 30;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkKnightsDoomShield(Serial serial) : base(serial)
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
