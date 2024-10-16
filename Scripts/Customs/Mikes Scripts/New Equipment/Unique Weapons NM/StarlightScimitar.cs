using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarlightScimitar : Scimitar
{
    [Constructable]
    public StarlightScimitar()
    {
        Name = "Starlight Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(55, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 15;
        Slayer = SlayerName.Fey;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitMagicArrow = 60;
        WeaponAttributes.HitDispel = 40;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Swords, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarlightScimitar(Serial serial) : base(serial)
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
