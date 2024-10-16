using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestHammer : WarHammer
{
    [Constructable]
    public TempestHammer()
    {
        Name = "Tempest Hammer";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(45, 75);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.LowerManaCost = 15;
        Attributes.Luck = 150;
        Slayer = SlayerName.Vacuum;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitLightning = 45;
        WeaponAttributes.HitLeechMana = 35;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestHammer(Serial serial) : base(serial)
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
