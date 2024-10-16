using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingWindLeggings : LeatherNinjaPants
{
    [Constructable]
    public WhisperingWindLeggings()
    {
        Name = "Whispering Wind Leggings";
        Hue = Utility.Random(1301, 1400);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusDex = 30;
        Attributes.RegenStam = 7;
        SkillBonuses.SetValues(0, SkillName.Hiding, 45.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 45.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 45.0);
        PhysicalBonus = 20;
        ColdBonus = 12;
        FireBonus = 12;
        EnergyBonus = 22;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingWindLeggings(Serial serial) : base(serial)
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
