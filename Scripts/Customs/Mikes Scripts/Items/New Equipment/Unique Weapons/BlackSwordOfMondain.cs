using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlackSwordOfMondain : Longsword
{
    [Constructable]
    public BlackSwordOfMondain()
    {
        Name = "Black Sword of Mondain";
        Hue = Utility.Random(800, 2900);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.SpellChanneling = 1;
        Attributes.BonusStr = 20;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.HitMagicArrow = 40;
        WeaponAttributes.HitManaDrain = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlackSwordOfMondain(Serial serial) : base(serial)
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
