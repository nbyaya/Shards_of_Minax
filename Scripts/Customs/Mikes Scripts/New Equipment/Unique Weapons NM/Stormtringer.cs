using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormtringer : Longsword
{
    [Constructable]
    public Stormtringer()
    {
        Name = "Stormtringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormtringer(Serial serial) : base(serial)
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
