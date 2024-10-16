using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersHood : BoneHelm
{
    [Constructable]
    public NecromancersHood()
    {
        Name = "Necromancer's Hood";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusInt = 30;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersHood(Serial serial) : base(serial)
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
