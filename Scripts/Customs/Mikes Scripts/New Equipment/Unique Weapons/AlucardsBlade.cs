using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlucardsBlade : Longsword
{
    [Constructable]
    public AlucardsBlade()
    {
        Name = "Alucard's Blade";
        Hue = Utility.Random(900, 2950);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusInt = 15;
        Attributes.LowerManaCost = 5;
        Slayer = SlayerName.Ophidian;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitMagicArrow = 30;
        WeaponAttributes.MageWeapon = 1;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlucardsBlade(Serial serial) : base(serial)
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
