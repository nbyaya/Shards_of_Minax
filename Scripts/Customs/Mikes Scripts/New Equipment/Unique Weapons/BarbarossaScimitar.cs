using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BarbarossaScimitar : Scimitar
{
    [Constructable]
    public BarbarossaScimitar()
    {
        Name = "Barbarossa Scimitar";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusDex = 15;
        Attributes.Luck = 100;
        Slayer = SlayerName.ElementalHealth;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BarbarossaScimitar(Serial serial) : base(serial)
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
