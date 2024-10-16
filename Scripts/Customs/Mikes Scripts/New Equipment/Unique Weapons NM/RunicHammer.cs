using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RunicHummer : Maul
{
    [Constructable]
    public RunicHummer()
    {
        Name = "Runic Hammer";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(45, 95);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.Luck = 100;
        Attributes.NightSight = 1;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitFireball = 40;
        WeaponAttributes.DurabilityBonus = 50;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 30.0);
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RunicHummer(Serial serial) : base(serial)
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
