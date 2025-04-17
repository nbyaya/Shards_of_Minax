using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MageMasher : WitchBurningTorch
{
    [Constructable]
    public MageMasher()
    {
        Name = "MageMasher";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.BonusInt = -10;
        Attributes.SpellDamage = -5;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitDispel = 40;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MageMasher(Serial serial) : base(serial)
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
