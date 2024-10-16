using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HanseaticCrossbow : Crossbow
{
    [Constructable]
    public HanseaticCrossbow()
    {
        Name = "Hanseatic Crossbow";
        Hue = Utility.Random(150, 2350);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 100);
        Attributes.AttackChance = 10;
        Attributes.LowerRegCost = 20;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitLightning = 30;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HanseaticCrossbow(Serial serial) : base(serial)
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
