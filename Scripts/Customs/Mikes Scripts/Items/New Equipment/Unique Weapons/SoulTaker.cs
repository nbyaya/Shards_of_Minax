using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SoulTaker : ResonantHarp
{
    [Constructable]
    public SoulTaker()
    {
        Name = "Soul Taker";
        Hue = Utility.Random(800, 2900);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 115);
        Attributes.LowerManaCost = 20;
        Attributes.WeaponSpeed = 10;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitManaDrain = 30;
        WeaponAttributes.HitLeechHits = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SoulTaker(Serial serial) : base(serial)
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
