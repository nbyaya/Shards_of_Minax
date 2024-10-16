using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TempestBolt : HeavyCrossbow
{
    [Constructable]
    public TempestBolt()
    {
        Name = "Tempest Bolt";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 20;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.HitLowerAttack = 40;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TempestBolt(Serial serial) : base(serial)
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
