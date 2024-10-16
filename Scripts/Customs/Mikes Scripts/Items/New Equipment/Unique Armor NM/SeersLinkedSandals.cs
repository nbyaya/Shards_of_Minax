using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeersLinkedSandals : ChainLegs
{
    [Constructable]
    public SeersLinkedSandals()
    {
        Name = "Seer's Linked Sandals";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(45, 60);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusInt = 35;
        Attributes.LowerManaCost = 15;
        Attributes.EnhancePotions = 25;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 30.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 50.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 40.0);
        PhysicalBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        ColdBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeersLinkedSandals(Serial serial) : base(serial)
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
