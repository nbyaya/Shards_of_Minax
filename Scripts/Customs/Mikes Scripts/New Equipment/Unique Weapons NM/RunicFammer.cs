using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RunicFammer : WarMace
{
    [Constructable]
    public RunicFammer()
    {
        Name = "Runic Slammer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellChanneling = 1;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.Terathan;
        WeaponAttributes.HitMagicArrow = 55;
        WeaponAttributes.MageWeapon = -10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RunicFammer(Serial serial) : base(serial)
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
