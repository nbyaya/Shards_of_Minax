using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsEmbrace : PlateChest
{
    [Constructable]
    public SerpentsEmbrace()
    {
        Name = "Serpent's Embrace";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.ResonancePoison = 15;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStam = 20;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 30;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsEmbrace(Serial serial) : base(serial)
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
