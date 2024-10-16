using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PeacekeepersArms : BoneArms
{
    [Constructable]
    public PeacekeepersArms()
    {
        Name = "Peacekeeper's Arms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(35, 55);
        ArmorAttributes.LowerStatReq = 30;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 25;
        Attributes.BonusHits = 15;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 40.0);
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 30.0);
        PhysicalBonus = 10;
        EnergyBonus = 10;
        FireBonus = 15;
        ColdBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PeacekeepersArms(Serial serial) : base(serial)
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
