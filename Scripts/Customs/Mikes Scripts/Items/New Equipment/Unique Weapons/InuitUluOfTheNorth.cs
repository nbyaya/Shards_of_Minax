using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InuitUluOfTheNorth : ButcherKnife
{
    [Constructable]
    public InuitUluOfTheNorth()
    {
        Name = "Inuit Ulu of the North";
        Hue = Utility.Random(100, 2300);
        MinDamage = Utility.RandomMinMax(15, 50);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.BonusInt = 5;
        Attributes.LowerManaCost = 10;
        Slayer = SlayerName.SnakesBane;
        WeaponAttributes.HitColdArea = 25;
        WeaponAttributes.SelfRepair = 3;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0);
        SkillBonuses.SetValues(1, SkillName.Cooking, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InuitUluOfTheNorth(Serial serial) : base(serial)
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
