using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArtisansCraftedGauntlets : LeatherGloves
{
    [Constructable]
    public ArtisansCraftedGauntlets()
    {
        Name = "Artisan's Crafted Gauntlets";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 70);
        ArmorAttributes.LowerStatReq = 15;
        Attributes.LowerRegCost = 20;
        Attributes.Luck = 50;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        SkillBonuses.SetValues(1, SkillName.Carpentry, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tailoring, 10.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArtisansCraftedGauntlets(Serial serial) : base(serial)
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
