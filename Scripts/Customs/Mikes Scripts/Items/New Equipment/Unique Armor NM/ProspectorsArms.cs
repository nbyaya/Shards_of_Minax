using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ProspectorsArms : RingmailArms
{
    [Constructable]
    public ProspectorsArms()
    {
        Name = "Prospector's Arms";
        Hue = Utility.Random(600, 700);
        BaseArmorRating = Utility.RandomMinMax(40, 60);
        ArmorAttributes.SelfRepair = 10;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStr = 15;
        Attributes.RegenHits = 3;
        Attributes.AttackChance = 20;
        SkillBonuses.SetValues(0, SkillName.Mining, 45.0);
        SkillBonuses.SetValues(1, SkillName.Blacksmith, 30.0);
        PhysicalBonus = 12;
        ColdBonus = 8;
        EnergyBonus = 15;
        FireBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ProspectorsArms(Serial serial) : base(serial)
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
