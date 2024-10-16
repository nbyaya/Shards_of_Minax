using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TabulasDagger : Dagger
{
    [Constructable]
    public TabulasDagger()
    {
        Name = "Tabula's Dagger";
        Hue = Utility.Random(900, 2900);
        MinDamage = Utility.RandomMinMax(10, 35);
        MaxDamage = Utility.RandomMinMax(35, 55);
        Attributes.CastSpeed = 1;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.Fey;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TabulasDagger(Serial serial) : base(serial)
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
