using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalAdge : DoubleAxe
{
    [Constructable]
    public ElementalAdge()
    {
        Name = "Elemental Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitColdArea = 50;
        WeaponAttributes.HitEnergyArea = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalAdge(Serial serial) : base(serial)
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
