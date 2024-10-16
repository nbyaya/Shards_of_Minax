using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReapersGrasp : Pitchfork
{
    [Constructable]
    public ReapersGrasp()
    {
        Name = "Reaper's Grasp";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(60, 100);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusHits = 30;
        Attributes.WeaponSpeed = 25;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.Repond;
        Slayer2 = SlayerName.OrcSlaying;
        WeaponAttributes.HitHarm = 60;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReapersGrasp(Serial serial) : base(serial)
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
