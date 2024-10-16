using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WrestlersChestOfPower : PlateChest
{
    [Constructable]
    public WrestlersChestOfPower()
    {
        Name = "Wrestler's Chest of Power";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusStr = 20;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(1, SkillName.Wrestling, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WrestlersChestOfPower(Serial serial) : base(serial)
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
