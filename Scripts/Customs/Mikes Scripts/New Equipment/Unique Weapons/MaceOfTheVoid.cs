using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MaceOfTheVoid : WarMace
{
    [Constructable]
    public MaceOfTheVoid()
    {
        Name = "Mace of the Void";
        Hue = Utility.Random(750, 2900);
        MinDamage = Utility.RandomMinMax(45, 85);
        MaxDamage = Utility.RandomMinMax(95, 135);
        Attributes.LowerManaCost = 20;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Vacuum;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitManaDrain = 40;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 35.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MaceOfTheVoid(Serial serial) : base(serial)
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
