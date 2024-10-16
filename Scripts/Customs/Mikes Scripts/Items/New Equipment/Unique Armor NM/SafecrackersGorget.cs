using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SafecrackersGorget : PlateGorget
{
    [Constructable]
    public SafecrackersGorget()
    {
        Name = "Safecracker's Gorget";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.LowerStatReq = 40;
        Attributes.EnhancePotions = 25;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 40.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 50.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 40.0);
        PhysicalBonus = 10;
        ColdBonus = 20;
        FireBonus = 20;
        EnergyBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SafecrackersGorget(Serial serial) : base(serial)
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
