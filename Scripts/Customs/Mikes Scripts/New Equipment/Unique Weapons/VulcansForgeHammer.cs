using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VulcansForgeHammer : WarHammer
{
    [Constructable]
    public VulcansForgeHammer()
    {
        Name = "Vulcan's Forge Hammer";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(35, 90);
        MaxDamage = Utility.RandomMinMax(90, 130);
        Attributes.DefendChance = 10;
        Attributes.RegenStam = 5;
        Slayer = SlayerName.FlameDousing;
        Slayer2 = SlayerName.BloodDrinking;
        WeaponAttributes.HitFireball = 80;
        WeaponAttributes.SelfRepair = 5;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 30.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VulcansForgeHammer(Serial serial) : base(serial)
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
