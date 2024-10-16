using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheDecapitator : DoubleAxe
{
    [Constructable]
    public TheDecapitator()
    {
        Name = "The Decapitator";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.OrcSlaying;
        Slayer2 = SlayerName.Repond;
        WeaponAttributes.HitHarm = 40;
        WeaponAttributes.HitLowerAttack = 25;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheDecapitator(Serial serial) : base(serial)
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
