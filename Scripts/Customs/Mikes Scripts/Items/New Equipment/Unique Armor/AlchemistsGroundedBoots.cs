using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsGroundedBoots : LeatherArms
{
    [Constructable]
    public AlchemistsGroundedBoots()
    {
        Name = "Alchemist's Grounded Boots";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStam = 10;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Camping, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsGroundedBoots(Serial serial) : base(serial)
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
