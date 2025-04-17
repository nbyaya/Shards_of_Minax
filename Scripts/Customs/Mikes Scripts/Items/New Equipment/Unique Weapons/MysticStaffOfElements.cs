using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticStaffOfElements : MysticStaff
{
    [Constructable]
    public MysticStaffOfElements()
    {
        Name = "Mystic Staff of Elements";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(35, 65);
        MaxDamage = Utility.RandomMinMax(75, 115);
        Attributes.SpellChanneling = 1;
        Attributes.SpellDamage = 20;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.EarthShatter;
        WeaponAttributes.HitEnergyArea = 40;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticStaffOfElements(Serial serial) : base(serial)
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
