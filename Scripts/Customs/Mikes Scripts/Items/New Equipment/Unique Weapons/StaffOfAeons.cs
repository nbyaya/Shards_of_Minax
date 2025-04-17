using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StaffOfAeons : MysticStaff
{
    [Constructable]
    public StaffOfAeons()
    {
        Name = "Staff of Aeons";
        Hue = Utility.Random(520, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.CastRecovery = 3;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.HitManaDrain = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StaffOfAeons(Serial serial) : base(serial)
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
