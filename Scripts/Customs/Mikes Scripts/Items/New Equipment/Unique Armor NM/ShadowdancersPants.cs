using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowdancersPants : LeatherNinjaPants
{
    [Constructable]
    public ShadowdancersPants()
    {
        Name = "Shadowdancer's Pants";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(15, 60);
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 25;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0);
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);
        PhysicalBonus = 10;
        ColdBonus = 5;
        FireBonus = 5;
        EnergyBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowdancersPants(Serial serial) : base(serial)
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
