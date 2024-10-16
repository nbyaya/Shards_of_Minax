using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StaffOfEldritchPower : GnarledStaff
{
    [Constructable]
    public StaffOfEldritchPower()
    {
        Name = "Staff of Eldritch Power";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(25, 75);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 25;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitEnergyArea = 45;
        WeaponAttributes.MageWeapon = -10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StaffOfEldritchPower(Serial serial) : base(serial)
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
