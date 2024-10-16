using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BismarckianWarAxe : WarAxe
{
    [Constructable]
    public BismarckianWarAxe()
    {
        Name = "Bismarckian WarAxe";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(25, 85);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.LowerManaCost = 10;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitCurse = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 60.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BismarckianWarAxe(Serial serial) : base(serial)
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
