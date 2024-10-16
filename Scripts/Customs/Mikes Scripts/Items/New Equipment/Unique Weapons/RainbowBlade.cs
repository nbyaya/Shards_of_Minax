using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RainbowBlade : Longsword
{
    [Constructable]
    public RainbowBlade()
    {
        Name = "Rainbow Blade";
        Hue = Utility.Random(1, 2000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.Luck = 150;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 25;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RainbowBlade(Serial serial) : base(serial)
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
