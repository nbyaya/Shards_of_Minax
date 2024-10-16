using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladeOfTheStars : Dagger
{
    [Constructable]
    public BladeOfTheStars()
    {
        Name = "Blade of the Stars";
        Hue = Utility.Random(100, 2900);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.LowerManaCost = 10;
        Attributes.NightSight = 1;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitMagicArrow = 30;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladeOfTheStars(Serial serial) : base(serial)
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
