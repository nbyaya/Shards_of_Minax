using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DragonsClaw : SkinningKnife
{
    [Constructable]
    public DragonsClaw()
    {
        Name = "Dragon's Claw";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireArea = 50;
        WeaponAttributes.HitMagicArrow = 25;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Anatomy, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DragonsClaw(Serial serial) : base(serial)
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
