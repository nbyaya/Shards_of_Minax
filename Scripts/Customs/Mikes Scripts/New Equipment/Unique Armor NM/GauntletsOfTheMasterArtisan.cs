using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfTheMasterArtisan : PlateGloves
{
    [Constructable]
    public GauntletsOfTheMasterArtisan()
    {
        Name = "Gauntlets of the Master Artisan";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 70);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusDex = 20;
        Attributes.BonusInt = 20;
        Attributes.EnhancePotions = 25;
        Attributes.CastSpeed = 1;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tailoring, 30.0);
        SkillBonuses.SetValues(2, SkillName.Tinkering, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 15;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GauntletsOfTheMasterArtisan(Serial serial) : base(serial)
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
