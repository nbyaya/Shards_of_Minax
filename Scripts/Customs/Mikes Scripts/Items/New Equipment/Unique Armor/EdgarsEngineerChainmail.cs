using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EdgarsEngineerChainmail : ChainChest
{
    [Constructable]
    public EdgarsEngineerChainmail()
    {
        Name = "Edgar's Engineer Chainmail";
        Hue = Utility.Random(250, 550);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.ResonanceKinetic = 15;
        Attributes.Luck = 20;
        Attributes.BonusStr = 10;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
        SkillBonuses.SetValues(1, SkillName.Blacksmith, 20.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EdgarsEngineerChainmail(Serial serial) : base(serial)
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
