using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThievesGuildPants : LeatherLegs
{
    [Constructable]
    public ThievesGuildPants()
    {
        Name = "Thieves' Guild Pants";
        Hue = Utility.Random(150, 500);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 20;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Lockpicking, 15.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);
        PhysicalBonus = 10;
        EnergyBonus = 10;
        ColdBonus = 5;
        PoisonBonus = 5;
        FireBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThievesGuildPants(Serial serial) : base(serial)
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
