using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MasterFletchersCoif : ChainCoif
{
    [Constructable]
    public MasterFletchersCoif()
    {
        Name = "Master Fletcher's Coif";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 65);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.BonusDex = 20;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Fletching, 50.0);
        SkillBonuses.SetValues(1, SkillName.Archery, 40.0);
        PhysicalBonus = 15;
        FireBonus = 20;
        ColdBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MasterFletchersCoif(Serial serial) : base(serial)
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
