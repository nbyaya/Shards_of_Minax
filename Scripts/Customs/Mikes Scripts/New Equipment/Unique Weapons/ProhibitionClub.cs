using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ProhibitionClub : Club
{
    [Constructable]
    public ProhibitionClub()
    {
        Name = "Prohibition Club";
        Hue = Utility.Random(50, 2250);
        MinDamage = Utility.RandomMinMax(10, 40);
        MaxDamage = Utility.RandomMinMax(40, 70);
        Attributes.BonusDex = 5;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.LizardmanSlaughter;
        WeaponAttributes.HitLowerAttack = 20;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 15.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ProhibitionClub(Serial serial) : base(serial)
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
