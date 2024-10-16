using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlamoDefendersAxe : BattleAxe
{
    [Constructable]
    public AlamoDefendersAxe()
    {
        Name = "Alamo Defender's Axe";
        Hue = Utility.Random(200, 2400);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusHits = 10;
        Attributes.RegenStam = 15;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlamoDefendersAxe(Serial serial) : base(serial)
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
