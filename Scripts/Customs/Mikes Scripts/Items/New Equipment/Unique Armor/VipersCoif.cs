using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VipersCoif : ChainCoif
{
    [Constructable]
    public VipersCoif()
    {
        Name = "Viper's Coif";
        Hue = Utility.Random(100, 300);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusInt = 15;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VipersCoif(Serial serial) : base(serial)
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
