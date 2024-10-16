using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestsEdge : Cutlass
{
    [Constructable]
    public TempestsEdge()
    {
        Name = "Tempest's Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 90);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.AttackChance = 20;
        Attributes.WeaponDamage = 35;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.GargoylesFoe;
        WeaponAttributes.HitLightning = 55;
        WeaponAttributes.HitLeechStam = 25;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestsEdge(Serial serial) : base(serial)
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
