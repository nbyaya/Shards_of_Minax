using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TwoShotCrossbow : Crossbow
{
    [Constructable]
    public TwoShotCrossbow()
    {
        Name = "Two-shot Crossbow";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 95);
        Attributes.BonusHits = 20;
        Attributes.Luck = 100;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLightning = 70;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TwoShotCrossbow(Serial serial) : base(serial)
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
