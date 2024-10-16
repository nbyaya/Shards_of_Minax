using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GarbOfTheGrandCouturier : BoneChest
{
    [Constructable]
    public GarbOfTheGrandCouturier()
    {
        Name = "Garb of the Grand Couturier";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(60, 70);
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusInt = 20;
        Attributes.Luck = 250;
        Attributes.LowerManaCost = 25;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 50.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 30.0);
        PhysicalBonus = 25;
        ColdBonus = 20;
        FireBonus = 25;
        EnergyBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GarbOfTheGrandCouturier(Serial serial) : base(serial)
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
