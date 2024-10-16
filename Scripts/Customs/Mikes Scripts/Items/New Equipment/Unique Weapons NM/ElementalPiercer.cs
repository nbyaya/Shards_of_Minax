using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalPiercer : WarFork
{
    [Constructable]
    public ElementalPiercer()
    {
        Name = "Elemental Piercer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 10;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitFireArea = 40;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalPiercer(Serial serial) : base(serial)
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
