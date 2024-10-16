using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GargoylesBane : Bardiche
{
    [Constructable]
    public GargoylesBane()
    {
        Name = "Gargoyle's Bane";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(220, 250);
        Attributes.BonusInt = 25;
        Attributes.DefendChance = 20;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.HitDispel = 45;
        WeaponAttributes.HitFireball = 30;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(2, SkillName.Wrestling, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GargoylesBane(Serial serial) : base(serial)
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
