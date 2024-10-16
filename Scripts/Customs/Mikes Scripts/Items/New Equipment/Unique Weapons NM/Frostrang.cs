using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Frostrang : WarFork
{
    [Constructable]
    public Frostrang()
    {
        Name = "Frostrang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 55;
        SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Frostrang(Serial serial) : base(serial)
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
