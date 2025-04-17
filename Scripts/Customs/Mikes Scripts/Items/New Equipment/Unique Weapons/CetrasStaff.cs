using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CetrasStaff : BeggersStick
{
    [Constructable]
    public CetrasStaff()
    {
        Name = "Cetra's Staff";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.LowerRegCost = 10;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CetrasStaff(Serial serial) : base(serial)
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
