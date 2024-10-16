using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ExcaliburLongsword : Longsword
{
    [Constructable]
    public ExcaliburLongsword()
    {
        Name = "Excalibur Longsword";
        Hue = Utility.Random(400, 2990);
        MinDamage = Utility.RandomMinMax(40, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStr = 20;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 40;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ExcaliburLongsword(Serial serial) : base(serial)
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
