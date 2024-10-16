using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReapersJodgment : Halberd
{
    [Constructable]
    public ReapersJodgment()
    {
        Name = "Reaper's Judgment";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(60, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 40;
        Attributes.BonusStam = 20;
        Slayer = SlayerName.Repond;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitHarm = 45;
        WeaponAttributes.BattleLust = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReapersJodgment(Serial serial) : base(serial)
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
