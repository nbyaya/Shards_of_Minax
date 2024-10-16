using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PlasmaInfusedWarHammer : WarHammer
{
    [Constructable]
    public PlasmaInfusedWarHammer()
    {
        Name = "Plasma Infused WarHammer";
        Hue = Utility.Random(150, 2350);
        MinDamage = Utility.RandomMinMax(30, 85);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.SpellChanneling = 1;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.ElementalHealth;
        Slayer2 = SlayerName.ElementalBan;
        WeaponAttributes.HitEnergyArea = 30;
        WeaponAttributes.HitDispel = 15;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PlasmaInfusedWarHammer(Serial serial) : base(serial)
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
