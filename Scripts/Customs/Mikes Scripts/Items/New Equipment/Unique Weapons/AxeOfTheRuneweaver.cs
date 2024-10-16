using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AxeOfTheRuneweaver : WarAxe
{
    [Constructable]
    public AxeOfTheRuneweaver()
    {
        Name = "Axe of the Runeweaver";
        Hue = Utility.Random(890, 2910);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(80, 110);
        Attributes.SpellDamage = 10;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitMagicArrow = 25;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AxeOfTheRuneweaver(Serial serial) : base(serial)
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
