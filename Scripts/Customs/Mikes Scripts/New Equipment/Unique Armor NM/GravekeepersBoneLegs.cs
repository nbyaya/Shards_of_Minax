using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GravekeepersBoneLegs : BoneLegs
{
    [Constructable]
    public GravekeepersBoneLegs()
    {
        Name = "Gravekeeper's BoneLegs";
        Hue = Utility.Random(333, 666);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        AbsorptionAttributes.EaterPoison = 40;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStr = 25;
        Attributes.BonusDex = 15;
        Attributes.Luck = 200;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 45.0);
        SkillBonuses.SetValues(1, SkillName.Necromancy, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 25;
        FireBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 30;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GravekeepersBoneLegs(Serial serial) : base(serial)
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
