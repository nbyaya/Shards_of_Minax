using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChakramBlade : Cutlass
{
    [Constructable]
    public ChakramBlade()
    {
        Name = "Chakram Blade";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 85);
        Attributes.LowerRegCost = 10;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.HitPhysicalArea = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChakramBlade(Serial serial) : base(serial)
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
