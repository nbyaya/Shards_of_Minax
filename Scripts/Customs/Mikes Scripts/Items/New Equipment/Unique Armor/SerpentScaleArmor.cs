using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentScaleArmor : ChainChest
{
    [Constructable]
    public SerpentScaleArmor()
    {
        Name = "Serpent Scale Armor";
        Hue = Utility.Random(100, 350);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        AbsorptionAttributes.ResonancePoison = 15;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusMana = 25;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
        PoisonBonus = 25;
        PhysicalBonus = 20;
        EnergyBonus = 5;
        FireBonus = 10;
        ColdBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentScaleArmor(Serial serial) : base(serial)
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
