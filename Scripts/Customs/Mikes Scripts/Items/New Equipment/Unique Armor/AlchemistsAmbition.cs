using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsAmbition : LeatherCap
{
    [Constructable]
    public AlchemistsAmbition()
    {
        Name = "Alchemist's Ambition";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(35, 82);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.SelfRepair = 15;
        Attributes.EnhancePotions = 30;
        Attributes.RegenHits = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 30.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsAmbition(Serial serial) : base(serial)
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
