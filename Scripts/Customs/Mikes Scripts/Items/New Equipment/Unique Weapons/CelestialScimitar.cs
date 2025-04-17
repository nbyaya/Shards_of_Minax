using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialScimitar : LoreSword
{
    [Constructable]
    public CelestialScimitar()
    {
        Name = "Celestial Scimitar";
        Hue = Utility.Random(50, 2150);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(90, 150);
        Attributes.BonusMana = 20;
        Attributes.CastSpeed = 3;
		Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.ArachnidDoom;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 65.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialScimitar(Serial serial) : base(serial)
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
