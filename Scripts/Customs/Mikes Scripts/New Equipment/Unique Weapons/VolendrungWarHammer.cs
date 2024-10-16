using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VolendrungWarHammer : WarHammer
{
    [Constructable]
    public VolendrungWarHammer()
    {
        Name = "Volendrung WarHammer";
        Hue = Utility.Random(150, 2200);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.BonusHits = 20;
        Attributes.AttackChance = 10;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.HitHarm = 30;
        SkillBonuses.SetValues(0, SkillName.Macing, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VolendrungWarHammer(Serial serial) : base(serial)
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
