using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlovesOfTheSilentAssassin : LeatherGloves
{
    [Constructable]
    public GlovesOfTheSilentAssassin()
    {
        Name = "Gloves of the Silent Assassin";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.RegenStam = 5;
        Attributes.WeaponSpeed = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlovesOfTheSilentAssassin(Serial serial) : base(serial)
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
