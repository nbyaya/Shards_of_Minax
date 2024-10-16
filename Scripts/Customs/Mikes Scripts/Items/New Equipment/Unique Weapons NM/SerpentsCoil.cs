using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsCoil : QuarterStaff
{
    [Constructable]
    public SerpentsCoil()
    {
        Name = "Serpent's Coil";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.Luck = 200;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPoisonArea = 45;
        WeaponAttributes.HitMagicArrow = 30;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
        SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsCoil(Serial serial) : base(serial)
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
