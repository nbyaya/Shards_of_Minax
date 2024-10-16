using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlameDancerScimitar : Scimitar
{
    [Constructable]
    public FlameDancerScimitar()
    {
        Name = "Flame Dancer Scimitar";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.ResistFireBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlameDancerScimitar(Serial serial) : base(serial)
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
