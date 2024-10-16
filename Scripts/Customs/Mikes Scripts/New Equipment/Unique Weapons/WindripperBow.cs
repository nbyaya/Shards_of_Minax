using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WindripperBow : Bow
{
    [Constructable]
    public WindripperBow()
    {
        Name = "Windripper Bow";
        Hue = Utility.Random(650, 2900);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.Luck = 150;
        Attributes.AttackChance = 10;
        Slayer = SlayerName.ElementalBan;
        WeaponAttributes.HitLightning = 20;
        WeaponAttributes.HitColdArea = 20;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WindripperBow(Serial serial) : base(serial)
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
