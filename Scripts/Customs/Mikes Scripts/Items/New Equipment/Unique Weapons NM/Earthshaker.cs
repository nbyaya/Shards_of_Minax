using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Earthshaker : WarMace
{
    [Constructable]
    public Earthshaker()
    {
        Name = "Earthshaker";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(75, 125);
        Attributes.BonusStr = 20;
        Attributes.WeaponDamage = 40;
        Slayer = SlayerName.EarthShatter;
        Slayer2 = SlayerName.ReptilianDeath;
        WeaponAttributes.HitFireArea = 65;
        WeaponAttributes.HitHarm = 25;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Chivalry, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Earthshaker(Serial serial) : base(serial)
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
