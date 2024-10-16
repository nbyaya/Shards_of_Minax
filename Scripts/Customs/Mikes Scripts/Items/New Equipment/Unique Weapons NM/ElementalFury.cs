using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalFury : Club
{
    [Constructable]
    public ElementalFury()
    {
        Name = "Elemental Fury";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStam = 30;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitEnergyArea = 30;
        WeaponAttributes.HitFireArea = 30;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalFury(Serial serial) : base(serial)
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
