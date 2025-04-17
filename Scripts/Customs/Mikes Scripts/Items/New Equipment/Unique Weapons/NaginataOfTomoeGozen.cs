using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaginataOfTomoeGozen : ShadowSai
{
    [Constructable]
    public NaginataOfTomoeGozen()
    {
        Name = "Naginata of Tomoe Gozen";
        Hue = Utility.Random(250, 2300);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(75, 110);
        Attributes.BonusDex = 15;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLightning = 20;
        WeaponAttributes.BattleLust = 10;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaginataOfTomoeGozen(Serial serial) : base(serial)
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
