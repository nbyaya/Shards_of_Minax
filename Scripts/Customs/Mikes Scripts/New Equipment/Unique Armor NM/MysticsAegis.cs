using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticsAegis : BronzeShield
{
    [Constructable]
    public MysticsAegis()
    {
        Name = "Mystic's Aegis";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(35, 55);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.SpellChanneling = 1;
        Attributes.ReflectPhysical = 15;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 30.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 50.0);
        PhysicalBonus = 20;
        ColdBonus = 10;
        FireBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticsAegis(Serial serial) : base(serial)
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
