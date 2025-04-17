using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoltaxicRiftLance : VeterinaryLance
{
    [Constructable]
    public VoltaxicRiftLance()
    {
        Name = "Voltaxic Rift Lance";
        Hue = Utility.Random(800, 2900);
        MinDamage = Utility.RandomMinMax(25, 65);
        MaxDamage = Utility.RandomMinMax(65, 95);
        Attributes.LowerManaCost = 10;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitEnergyArea = 25;
        WeaponAttributes.ResistPoisonBonus = 15;
        SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VoltaxicRiftLance(Serial serial) : base(serial)
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
