using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MendicantsMysticRobe : LeatherChest
{
    [Constructable]
    public MendicantsMysticRobe()
    {
        Name = "Mendicant's Mystic Robe";
        Hue = Utility.Random(800, 950);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        AbsorptionAttributes.EaterEnergy = 40;
        ArmorAttributes.SelfRepair = 20;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.LowerManaCost = 20;
        Attributes.LowerRegCost = 20;
        SkillBonuses.SetValues(0, SkillName.Begging, 50.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 30.0);
        PhysicalBonus = 15;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MendicantsMysticRobe(Serial serial) : base(serial)
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
