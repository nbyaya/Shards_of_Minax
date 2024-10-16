using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoguesShadowBoots : LeatherLegs
{
    [Constructable]
    public RoguesShadowBoots()
    {
        Name = "Rogue's Shadow Boots";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 60);
        AbsorptionAttributes.EaterPoison = 25;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusDex = 40;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoguesShadowBoots(Serial serial) : base(serial)
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
