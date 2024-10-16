using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DraugrsRemorse : VikingSword
{
    [Constructable]
    public DraugrsRemorse()
    {
        Name = "Draugr's Remorse";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 15;
        Attributes.WeaponDamage = 30;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.Silver;
        Slayer2 = SlayerName.DragonSlaying;
        WeaponAttributes.HitMagicArrow = 50;
        WeaponAttributes.HitLeechStam = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DraugrsRemorse(Serial serial) : base(serial)
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
