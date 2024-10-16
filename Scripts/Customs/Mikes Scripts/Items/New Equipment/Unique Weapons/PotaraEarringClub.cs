using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PotaraEarringClub : Club
{
    [Constructable]
    public PotaraEarringClub()
    {
        Name = "Potara Earring Club";
        Hue = Utility.Random(300, 2350);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.BonusInt = 15;
        Attributes.RegenHits = 3;
        Slayer = SlayerName.DaemonDismissal;
        WeaponAttributes.MageWeapon = 1;
        WeaponAttributes.HitMagicArrow = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PotaraEarringClub(Serial serial) : base(serial)
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
