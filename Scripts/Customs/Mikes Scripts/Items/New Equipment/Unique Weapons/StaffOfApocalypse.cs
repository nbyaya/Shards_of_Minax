using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StaffOfApocalypse : MysticStaff
{
    [Constructable]
    public StaffOfApocalypse()
    {
        Name = "Staff of Apocalypse";
        Hue = Utility.Random(900, 2900);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 60);
        Attributes.SpellDamage = 20;
        Attributes.LowerRegCost = 15;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.HitManaDrain = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StaffOfApocalypse(Serial serial) : base(serial)
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
