using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerenadesEmbrace : ChainChest
{
    [Constructable]
    public SerenadesEmbrace()
    {
        Name = "Serenade's Embrace";
        Hue = Utility.Random(250, 750);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.ResonanceKinetic = 10;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusStr = 15;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 15.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerenadesEmbrace(Serial serial) : base(serial)
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
