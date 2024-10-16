using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Duskfall : ExecutionersAxe
{
    [Constructable]
    public Duskfall()
    {
        Name = "Duskfall";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.Luck = 100;
        Slayer = SlayerName.Vacuum;
        Slayer2 = SlayerName.ScorpionsBane;
        WeaponAttributes.HitFatigue = 50;
        WeaponAttributes.HitManaDrain = 25;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Duskfall(Serial serial) : base(serial)
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
