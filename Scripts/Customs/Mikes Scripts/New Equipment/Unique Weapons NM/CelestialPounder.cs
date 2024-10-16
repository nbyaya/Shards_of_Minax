using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialPounder : Mace
{
    [Constructable]
    public CelestialPounder()
    {
        Name = "Celestial Pounder";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.LowerManaCost = 10;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitMagicArrow = 35;
        WeaponAttributes.ResistFireBonus = 15;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialPounder(Serial serial) : base(serial)
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
