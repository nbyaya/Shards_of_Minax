using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class QamarDagger : Dagger
{
    [Constructable]
    public QamarDagger()
    {
        Name = "Qamar Dagger";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.NightSight = 1;
        Attributes.BonusDex = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitHarm = 25;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public QamarDagger(Serial serial) : base(serial)
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
