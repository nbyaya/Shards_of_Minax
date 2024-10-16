using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArtificersLeggings : PlateLegs
{
    [Constructable]
    public ArtificersLeggings()
    {
        Name = "Artificer's Leggings";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.SelfRepair = 8;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStam = 15;
        Attributes.BonusHits = 10;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 50.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 30.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 30.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArtificersLeggings(Serial serial) : base(serial)
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
