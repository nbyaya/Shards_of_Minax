using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DisarmingLeatherArms : LeatherArms
{
    [Constructable]
    public DisarmingLeatherArms()
    {
        Name = "Disarming Leather Arms";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(40, 60);
        ArmorAttributes.LowerStatReq = 30;
        ArmorAttributes.DurabilityBonus = 80;
        Attributes.DefendChance = 20;
        Attributes.RegenMana = 10;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 30.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 50.0);
        SkillBonuses.SetValues(2, SkillName.Lockpicking, 40.0);
        PhysicalBonus = 15;
        FireBonus = 15;
        ColdBonus = 25;
        EnergyBonus = 5;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DisarmingLeatherArms(Serial serial) : base(serial)
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
