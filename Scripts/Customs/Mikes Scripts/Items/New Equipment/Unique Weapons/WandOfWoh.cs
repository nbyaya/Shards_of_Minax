using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WandOfWoh : BlackStaff
{
    [Constructable]
    public WandOfWoh()
    {
        Name = "Wand of Woh";
        Hue = Utility.Random(200, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.SpellChanneling = 1;
        Attributes.BonusMana = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 90;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WandOfWoh(Serial serial) : base(serial)
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
