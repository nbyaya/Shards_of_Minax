using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RasSearingDagger : Dagger
{
    [Constructable]
    public RasSearingDagger()
    {
        Name = "Ra's Searing Dagger";
        Hue = Utility.Random(600, 2800);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 70);
        Attributes.SpellDamage = 5;
        Attributes.NightSight = 1;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.HitFireball = 50;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RasSearingDagger(Serial serial) : base(serial)
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
