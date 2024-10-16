using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FiendsFork : Pitchfork
{
    [Constructable]
    public FiendsFork()
    {
        Name = "Fiend's Fork";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusInt = 10;
        Attributes.BonusMana = 20;
        Attributes.CastSpeed = 1;
        Slayer = SlayerName.DaemonDismissal;
        Slayer2 = SlayerName.BloodDrinking;
        WeaponAttributes.HitFireArea = 50;
        WeaponAttributes.HitCurse = 20;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FiendsFork(Serial serial) : base(serial)
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
