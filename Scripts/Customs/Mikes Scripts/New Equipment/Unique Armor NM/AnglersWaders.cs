using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AnglersWaders : LeatherLegs
{
    [Constructable]
    public AnglersWaders()
    {
        Name = "Angler's Waders";
        Hue = Utility.Random(100, 200);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusDex = 20;
        Attributes.EnhancePotions = 50;
        Attributes.Luck = 300;
        SkillBonuses.SetValues(0, SkillName.Fishing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 10;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AnglersWaders(Serial serial) : base(serial)
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
