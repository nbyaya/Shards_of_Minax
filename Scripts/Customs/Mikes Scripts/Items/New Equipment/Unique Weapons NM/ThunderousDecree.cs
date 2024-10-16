using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThunderousDecree : Halberd
{
    [Constructable]
    public ThunderousDecree()
    {
        Name = "Thunderous Decree";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusInt = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 60;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 15.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThunderousDecree(Serial serial) : base(serial)
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
