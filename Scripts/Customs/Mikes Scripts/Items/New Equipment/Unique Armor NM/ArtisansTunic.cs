using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArtisansTunic : LeatherChest
{
    [Constructable]
    public ArtisansTunic()
    {
        Name = "Artisan's Tunic";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(35, 55);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusDex = 25;
        Attributes.BonusHits = 20;
        Attributes.ReflectPhysical = 15;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 30.0);
        PhysicalBonus = 18;
        FireBonus = 15;
        ColdBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArtisansTunic(Serial serial) : base(serial)
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
