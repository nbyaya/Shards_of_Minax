using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasamunesGrace : Katana
{
    [Constructable]
    public MasamunesGrace()
    {
        Name = "Masamune's Grace";
        Hue = Utility.Random(200, 2250);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.BonusDex = 15;
        Attributes.SpellChanneling = 1;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasamunesGrace(Serial serial) : base(serial)
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
