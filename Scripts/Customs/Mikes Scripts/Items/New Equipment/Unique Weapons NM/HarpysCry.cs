using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarpysCry : HeavyCrossbow
{
    [Constructable]
    public HarpysCry()
    {
        Name = "Harpy's Cry";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.Luck = 100;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.Fey;
        WeaponAttributes.HitMagicArrow = 40;
        WeaponAttributes.HitDispel = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 5.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarpysCry(Serial serial) : base(serial)
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
