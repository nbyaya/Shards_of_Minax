using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DuelistsLegplates : PlateLegs
{
    [Constructable]
    public DuelistsLegplates()
    {
        Name = "Duelist's Legplates";
        Hue = Utility.Random(100, 800);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusHits = 40;
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 40.0);
        SkillBonuses.SetValues(2, SkillName.Parry, 30.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DuelistsLegplates(Serial serial) : base(serial)
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
