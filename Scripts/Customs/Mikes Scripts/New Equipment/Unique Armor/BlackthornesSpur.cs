using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlackthornesSpur : PlateLegs
{
    [Constructable]
    public BlackthornesSpur()
    {
        Name = "Blackthorne's Spur";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterPoison = 25;
        Attributes.RegenStam = 15;
        Attributes.Luck = 40;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 30;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlackthornesSpur(Serial serial) : base(serial)
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
