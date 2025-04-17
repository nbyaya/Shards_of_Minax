using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MageMusher : WitchBurningTorch
{
    [Constructable]
    public MageMusher()
    {
        Name = "Mage Masher";
        Hue = Utility.Random(860, 2880);
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(60, 80);
        Attributes.LowerRegCost = 10;
        Attributes.AttackChance = 5;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitManaDrain = 30;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MageMusher(Serial serial) : base(serial)
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
