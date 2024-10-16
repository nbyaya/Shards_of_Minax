using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VyrsGraspingGauntlets : LeatherGloves
{
    [Constructable]
    public VyrsGraspingGauntlets()
    {
        Name = "Vyr's Grasping Gauntlets";
        Hue = Utility.Random(150, 450);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 50;
        Attributes.SpellDamage = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VyrsGraspingGauntlets(Serial serial) : base(serial)
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
