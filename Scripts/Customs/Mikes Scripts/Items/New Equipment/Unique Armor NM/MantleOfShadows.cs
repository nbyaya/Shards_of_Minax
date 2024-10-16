using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MantleOfShadows : LeatherNinjaHood
{
    [Constructable]
    public MantleOfShadows()
    {
        Name = "Mantle of Shadows";
        Hue = Utility.Random(1100, 1199);
        BaseArmorRating = Utility.RandomMinMax(60, 80);
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.SelfRepair = 10;
        Attributes.NightSight = 1;
        Attributes.BonusDex = 25;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Hiding, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 50.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 50.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MantleOfShadows(Serial serial) : base(serial)
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
