using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonysLeggings : ChainLegs
{
    [Constructable]
    public HarmonysLeggings()
    {
        Name = "Harmony's Leggings";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 60);
        ArmorAttributes.LowerStatReq = 40;
        Attributes.Luck = 200;
        Attributes.BonusStam = 20;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 50.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 40.0);
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 30.0);
        PhysicalBonus = 12;
        ColdBonus = 18;
        FireBonus = 12;
        EnergyBonus = 12;
        PoisonBonus = 12;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonysLeggings(Serial serial) : base(serial)
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
