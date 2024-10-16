using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BulKathosTribalGuardian : WarAxe
{
    [Constructable]
    public BulKathosTribalGuardian()
    {
        Name = "Bul-Kathos' Tribal Guardian";
        Hue = Utility.Random(100, 2900);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.BonusStr = 10;
        Attributes.RegenStam = 3;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitFireArea = 35;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BulKathosTribalGuardian(Serial serial) : base(serial)
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
