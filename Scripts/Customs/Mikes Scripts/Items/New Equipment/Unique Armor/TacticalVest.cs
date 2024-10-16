using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TacticalVest : LeatherChest
{
    [Constructable]
    public TacticalVest()
    {
        Name = "Tactical Vest";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 75);
        AbsorptionAttributes.EaterKinetic = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 10;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TacticalVest(Serial serial) : base(serial)
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
