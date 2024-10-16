using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Grimmblade : Longsword
{
    [Constructable]
    public Grimmblade()
    {
        Name = "Grimmblade";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(75, 105);
        Attributes.BonusStr = 20;
        Attributes.NightSight = 1;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitHarm = 70;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Grimmblade(Serial serial) : base(serial)
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
