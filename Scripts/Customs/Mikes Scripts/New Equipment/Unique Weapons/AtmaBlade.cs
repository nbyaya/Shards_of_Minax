using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AtmaBlade : Longsword
{
    [Constructable]
    public AtmaBlade()
    {
        Name = "Atma Blade";
        Hue = Utility.Random(900, 2000);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusHits = 20;
        Attributes.AttackChance = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.SelfRepair = 5;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AtmaBlade(Serial serial) : base(serial)
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
