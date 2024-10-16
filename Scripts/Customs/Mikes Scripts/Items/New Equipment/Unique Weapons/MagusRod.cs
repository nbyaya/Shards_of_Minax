using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MagusRod : BlackStaff
{
    [Constructable]
    public MagusRod()
    {
        Name = "Magus Rod";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(15, 45);
        MaxDamage = Utility.RandomMinMax(45, 75);
        Attributes.BonusInt = 15;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.MageWeapon = 2;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MagusRod(Serial serial) : base(serial)
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
