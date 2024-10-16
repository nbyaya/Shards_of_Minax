using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThorsHammer : WarHammer
{
    [Constructable]
    public ThorsHammer()
    {
        Name = "Thor's Hammer";
        Hue = Utility.Random(550, 2900);
        MinDamage = Utility.RandomMinMax(35, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.RegenMana = 5;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitEnergyArea = 30;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThorsHammer(Serial serial) : base(serial)
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
