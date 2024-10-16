using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EchoesOfTheMountain : HammerPick
{
    [Constructable]
    public EchoesOfTheMountain()
    {
        Name = "Echoes of the Mountain";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusHits = 30;
        Attributes.DefendChance = 15;
        Slayer = SlayerName.EarthShatter;
        WeaponAttributes.HitMagicArrow = 20;
        WeaponAttributes.DurabilityBonus = 50;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EchoesOfTheMountain(Serial serial) : base(serial)
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
