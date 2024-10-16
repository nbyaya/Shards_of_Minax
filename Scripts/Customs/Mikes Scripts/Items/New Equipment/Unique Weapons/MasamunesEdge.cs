using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasamunesEdge : Katana
{
    [Constructable]
    public MasamunesEdge()
    {
        Name = "Masamune's Edge";
        Hue = Utility.Random(910, 2930);
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(85, 105);
        Attributes.BonusDex = 10;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasamunesEdge(Serial serial) : base(serial)
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
