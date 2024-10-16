using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArtisansHelm : NorseHelm
{
    [Constructable]
    public ArtisansHelm()
    {
        Name = "Artisan's Helm";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.CastingFocus = 10;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusInt = 20;
        Attributes.EnhancePotions = 20;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 15.0);
        SkillBonuses.SetValues(2, SkillName.Blacksmith, 10.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArtisansHelm(Serial serial) : base(serial)
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
