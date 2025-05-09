using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JestersPlayfulTunic : StuddedChest
{
    [Constructable]
    public JestersPlayfulTunic()
    {
        Name = "Jester's Playful Tunic";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(20, 60);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusStam = 40;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Provocation, 25.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JestersPlayfulTunic(Serial serial) : base(serial)
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
