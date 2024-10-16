using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaleCleaver : WarAxe
{
    [Constructable]
    public GaleCleaver()
    {
        Name = "Gale Cleaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 30;
        Attributes.WeaponSpeed = 35;
        Slayer = SlayerName.ElementalBan;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitColdArea = 40;
        WeaponAttributes.HitEnergyArea = 30;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 15.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaleCleaver(Serial serial) : base(serial)
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
