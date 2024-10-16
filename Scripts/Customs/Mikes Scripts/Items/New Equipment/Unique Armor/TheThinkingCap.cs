using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheThinkingCap : Helmet
{
    [Constructable]
    public TheThinkingCap()
    {
        Name = "The Thinking Cap";
        Hue = Utility.Random(150, 650);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 40;
        Attributes.CastRecovery = 2;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheThinkingCap(Serial serial) : base(serial)
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
