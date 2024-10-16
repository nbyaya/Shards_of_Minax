using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThunderclapForger : HammerPick
{
    [Constructable]
    public ThunderclapForger()
    {
        Name = "Thunderclap Forger";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 10;
        Attributes.BonusStr = 20;
        Attributes.Luck = 100;
        Slayer = SlayerName.GargoylesFoe;
        WeaponAttributes.HitEnergyArea = 40;
        WeaponAttributes.HitDispel = 20;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThunderclapForger(Serial serial) : base(serial)
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
