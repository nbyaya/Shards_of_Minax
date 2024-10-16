using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrognaksAxe : TwoHandedAxe
{
    [Constructable]
    public GrognaksAxe()
    {
        Name = "Grognak's Axe";
        Hue = Utility.Random(100, 2300);
        MinDamage = Utility.RandomMinMax(35, 90);
        MaxDamage = Utility.RandomMinMax(90, 130);
        Attributes.BonusStr = 15;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.TrollSlaughter;
        WeaponAttributes.BattleLust = 25;
        WeaponAttributes.HitHarm = 90;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrognaksAxe(Serial serial) : base(serial)
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
