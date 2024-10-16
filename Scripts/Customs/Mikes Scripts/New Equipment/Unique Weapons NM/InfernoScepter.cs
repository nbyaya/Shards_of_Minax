using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernoScepter : QuarterStaff
{
    [Constructable]
    public InfernoScepter()
    {
        Name = "Inferno Scepter";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 10;
        Attributes.WeaponDamage = 25;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireball = 55;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernoScepter(Serial serial) : base(serial)
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
