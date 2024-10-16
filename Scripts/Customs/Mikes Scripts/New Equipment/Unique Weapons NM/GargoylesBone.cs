using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesBone : Club
{
    [Constructable]
    public GargoylesBone()
    {
        Name = "Gargoyle's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 35;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.HitLightning = 55;
        WeaponAttributes.HitLowerAttack = 20;
        SkillBonuses.SetValues(0, SkillName.ArmsLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.Blacksmith, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesBone(Serial serial) : base(serial)
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
