using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AxeOfTheWhiteDragon : BattleAxe
{
    [Constructable]
    public AxeOfTheWhiteDragon()
    {
        Name = "Axe of the White Dragon";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 45;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AxeOfTheWhiteDragon(Serial serial) : base(serial)
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
