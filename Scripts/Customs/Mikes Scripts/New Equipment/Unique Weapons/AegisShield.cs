using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AegisShield : WarHammer
{
    [Constructable]
    public AegisShield()
    {
        Name = "Aegis Shield";
        Hue = Utility.Random(200, 400);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.DefendChance = 15;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.SelfRepair = 3;
        WeaponAttributes.ResistPhysicalBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AegisShield(Serial serial) : base(serial)
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
