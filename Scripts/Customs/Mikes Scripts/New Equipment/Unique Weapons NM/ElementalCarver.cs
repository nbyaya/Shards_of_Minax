using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalCarver : ButcherKnife
{
    [Constructable]
    public ElementalCarver()
    {
        Name = "Elemental Carver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.HitColdArea = 30;
        SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalCarver(Serial serial) : base(serial)
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
