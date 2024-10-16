using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ErdricksBlade : Longsword
{
    [Constructable]
    public ErdricksBlade()
    {
        Name = "Erdrick's Blade";
        Hue = Utility.Random(50, 2250);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusStr = 10;
        Attributes.SpellDamage = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 70;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ErdricksBlade(Serial serial) : base(serial)
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
