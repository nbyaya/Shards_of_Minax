using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernoBlade : Longsword
{
    [Constructable]
    public InfernoBlade()
    {
        Name = "Inferno Blade";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.AttackChance = 15;
        Slayer = SlayerName.FlameDousing;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.HitFireArea = 35;
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernoBlade(Serial serial) : base(serial)
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
