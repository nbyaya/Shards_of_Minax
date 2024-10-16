using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersDarkLeggings : BoneLegs
{
    [Constructable]
    public NecromancersDarkLeggings()
    {
        Name = "Necromancer's DarkLeggings";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(35, 75);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStam = 20;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Focus, 20.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersDarkLeggings(Serial serial) : base(serial)
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
