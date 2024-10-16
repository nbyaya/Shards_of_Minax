using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrimsonRipper : Dagger
{
    [Constructable]
    public CrimsonRipper()
    {
        Name = "Crimson Ripper";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 90);
        MaxDamage = Utility.RandomMinMax(150, 250);
        Attributes.BonusStr = 20;
        Attributes.Luck = 100;
        Slayer = SlayerName.BloodDrinking;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitHarm = 35;
        WeaponAttributes.BloodDrinker = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrimsonRipper(Serial serial) : base(serial)
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
