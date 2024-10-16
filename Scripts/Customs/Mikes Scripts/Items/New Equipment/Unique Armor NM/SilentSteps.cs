using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentSteps : PlateLegs
{
    [Constructable]
    public SilentSteps()
    {
        Name = "Silent Steps";
        Hue = Utility.Random(1401, 1500);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusDex = 35;
        Attributes.Luck = 100;
        SkillBonuses.SetValues(0, SkillName.Hiding, 30.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 50.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 30.0);
        PhysicalBonus = 17;
        ColdBonus = 10;
        FireBonus = 18;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentSteps(Serial serial) : base(serial)
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
