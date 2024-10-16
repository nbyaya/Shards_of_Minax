using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EarthboundStave : QuarterStaff
{
    [Constructable]
    public EarthboundStave()
    {
        Name = "Earthbound Stave";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitPhysicalArea = 35;
        WeaponAttributes.ResistPoisonBonus = 25;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
        SkillBonuses.SetValues(1, SkillName.Mining, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EarthboundStave(Serial serial) : base(serial)
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
