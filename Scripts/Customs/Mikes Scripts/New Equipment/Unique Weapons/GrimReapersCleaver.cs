using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrimReapersCleaver : Cleaver
{
    [Constructable]
    public GrimReapersCleaver()
    {
        Name = "Grim Reaper's Cleaver";
        Hue = Utility.Random(800, 2850);
        MinDamage = Utility.RandomMinMax(55, 95);
        MaxDamage = Utility.RandomMinMax(95, 155);
        Attributes.LowerManaCost = 20;
        Attributes.ReflectPhysical = 10;
        Slayer = SlayerName.BloodDrinking;
        Slayer2 = SlayerName.Exorcism;
        WeaponAttributes.HitHarm = 50;
        WeaponAttributes.HitPoisonArea = 40;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 30.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrimReapersCleaver(Serial serial) : base(serial)
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
