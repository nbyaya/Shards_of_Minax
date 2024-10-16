using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SummonersEmbrace : LeatherChest
{
    [Constructable]
    public SummonersEmbrace()
    {
        Name = "Summoner's Embrace";
        Hue = Utility.Random(250, 500);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 25;
        Attributes.CastRecovery = 2;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SummonersEmbrace(Serial serial) : base(serial)
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
