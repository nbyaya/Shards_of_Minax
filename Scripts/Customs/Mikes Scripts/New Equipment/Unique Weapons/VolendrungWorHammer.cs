using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VolendrungWorHammer : WarHammer
{
    [Constructable]
    public VolendrungWorHammer()
    {
        Name = "Volendrung WarHammer";
        Hue = Utility.Random(400, 2550);
        MinDamage = Utility.RandomMinMax(35, 80);
        MaxDamage = Utility.RandomMinMax(80, 125);
        Attributes.BonusHits = 20;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFatigue = 40;
        WeaponAttributes.HitPhysicalArea = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VolendrungWorHammer(Serial serial) : base(serial)
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
