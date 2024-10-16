using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkKnightsObsidianHelm : PlateHelm
{
    [Constructable]
    public DarkKnightsObsidianHelm()
    {
        Name = "Dark Knight's Obsidian Helm";
        Hue = Utility.Random(10, 20);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusHits = 60;
        Attributes.ReflectPhysical = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 30;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkKnightsObsidianHelm(Serial serial) : base(serial)
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
