using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarpeBlade : Scimitar
{
    [Constructable]
    public HarpeBlade()
    {
        Name = "Harpē Blade";
        Hue = Utility.Random(250, 2450);
        MinDamage = Utility.RandomMinMax(25, 70);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.Luck = 100;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.Ophidian;
        WeaponAttributes.HitDispel = 25;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarpeBlade(Serial serial) : base(serial)
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
