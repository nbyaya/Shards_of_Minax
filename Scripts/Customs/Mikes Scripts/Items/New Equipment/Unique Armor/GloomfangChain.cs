using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GloomfangChain : StuddedArms
{
    [Constructable]
    public GloomfangChain()
    {
        Name = "Gloomfang Chain";
        Hue = Utility.Random(250, 750);
        BaseArmorRating = Utility.RandomMinMax(30, 75);
        AbsorptionAttributes.EaterPoison = 35;
        Attributes.BonusHits = -30;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 25.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GloomfangChain(Serial serial) : base(serial)
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
