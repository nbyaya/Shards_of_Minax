using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersShadowBoots : BoneArms
{
    [Constructable]
    public NecromancersShadowBoots()
    {
        Name = "Necromancer's Shadow Boots";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(25, 65);
        AbsorptionAttributes.EaterKinetic = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.NightSight = 1;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersShadowBoots(Serial serial) : base(serial)
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
