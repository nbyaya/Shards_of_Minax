using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Bonehew : Maul
{
    [Constructable]
    public Bonehew()
    {
        Name = "Bonehew";
        Hue = Utility.Random(400, 2900);
        MinDamage = Utility.RandomMinMax(35, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.DefendChance = 5;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitHarm = 25;
        WeaponAttributes.HitPoisonArea = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Bonehew(Serial serial) : base(serial)
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
