using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SlitheringSeal : RingmailGloves
{
    [Constructable]
    public SlitheringSeal()
    {
        Name = "Slithering Seal";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.ResonancePoison = 10;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusMana = 10;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SlitheringSeal(Serial serial) : base(serial)
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
