using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardianOfTheAbyss : ChaosShield
{
    [Constructable]
    public GuardianOfTheAbyss()
    {
        Name = "Guardian of the Abyss";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterPoison = 30;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardianOfTheAbyss(Serial serial) : base(serial)
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
