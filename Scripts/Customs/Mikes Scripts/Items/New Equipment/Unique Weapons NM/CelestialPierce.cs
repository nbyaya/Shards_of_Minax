using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialPierce : Halberd
{
    [Constructable]
    public CelestialPierce()
    {
        Name = "Celestial Pierce";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.Exorcism;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.MageWeapon = -10;
        WeaponAttributes.HitMagicArrow = 35;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialPierce(Serial serial) : base(serial)
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
