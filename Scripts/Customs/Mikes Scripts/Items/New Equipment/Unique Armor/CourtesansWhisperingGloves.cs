using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansWhisperingGloves : LeatherGloves
{
    [Constructable]
    public CourtesansWhisperingGloves()
    {
        Name = "Courtesan's Whispering Gloves";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusDex = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansWhisperingGloves(Serial serial) : base(serial)
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
