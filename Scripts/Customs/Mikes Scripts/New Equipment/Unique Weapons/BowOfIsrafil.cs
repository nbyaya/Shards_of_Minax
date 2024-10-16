using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowOfIsrafil : Bow
{
    [Constructable]
    public BowOfIsrafil()
    {
        Name = "Bow of Israfil";
        Hue = Utility.Random(600, 2800);
        MinDamage = Utility.RandomMinMax(25, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.RegenMana = 5;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitFireArea = 30;
        WeaponAttributes.HitEnergyArea = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowOfIsrafil(Serial serial) : base(serial)
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
