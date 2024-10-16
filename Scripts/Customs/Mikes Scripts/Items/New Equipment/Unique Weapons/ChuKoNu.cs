using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChuKoNu : HeavyCrossbow
{
    [Constructable]
    public ChuKoNu()
    {
        Name = "Chu Ko Nu";
        Hue = Utility.Random(400, 2600);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(50, 90);
        Attributes.LowerRegCost = 20;
        Attributes.AttackChance = 5;
        Slayer = SlayerName.ReptilianDeath;
        WeaponAttributes.HitPhysicalArea = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChuKoNu(Serial serial) : base(serial)
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
