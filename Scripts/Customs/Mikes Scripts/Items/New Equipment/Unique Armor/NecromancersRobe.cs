using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecromancersRobe : BoneChest
{
    [Constructable]
    public NecromancersRobe()
    {
        Name = "Necromancer's Robe";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusMana = 40;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecromancersRobe(Serial serial) : base(serial)
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
