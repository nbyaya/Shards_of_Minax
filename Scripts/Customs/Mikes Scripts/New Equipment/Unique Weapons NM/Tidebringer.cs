using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Tidebringer : QuarterStaff
{
    [Constructable]
    public Tidebringer()
    {
        Name = "Tidebringer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusMana = 15;
        Attributes.CastRecovery = 1;
        Slayer = SlayerName.WaterDissipation;
        Slayer2 = SlayerName.Vacuum;
        WeaponAttributes.HitColdArea = 40;
        WeaponAttributes.HitManaDrain = 30;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Tidebringer(Serial serial) : base(serial)
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
