using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CelestialArbalest : HeavyCrossbow
{
    [Constructable]
    public CelestialArbalest()
    {
        Name = "Celestial Arbalest";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 20;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.MageWeapon = -10;
        WeaponAttributes.HitFireball = 35;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CelestialArbalest(Serial serial) : base(serial)
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
