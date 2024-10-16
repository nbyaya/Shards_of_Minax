using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Doombringer : PlateLegs
{
    [Constructable]
    public Doombringer()
    {
        Name = "Doombringer";
        Hue = Utility.Random(10, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 70);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.IncreasedKarmaLoss = 15;
        Attributes.Luck = -40;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 15.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Doombringer(Serial serial) : base(serial)
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
