using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowOfAuriel : Bow
{
    [Constructable]
    public BowOfAuriel()
    {
        Name = "Bow of Auriel";
        Hue = Utility.Random(100, 2250);
        MinDamage = Utility.RandomMinMax(20, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.LowerRegCost = 15;
        Attributes.Luck = 150;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 25;
        WeaponAttributes.HitManaDrain = 10;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowOfAuriel(Serial serial) : base(serial)
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
