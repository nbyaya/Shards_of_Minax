using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesCursedRobe : LeatherChest
{
    [Constructable]
    public WitchesCursedRobe()
    {
        Name = "Witch's Cursed Robe";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(30, 60);
        AbsorptionAttributes.EaterPoison = 20;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.LowerManaCost = 10;
        Attributes.BonusMana = 20;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesCursedRobe(Serial serial) : base(serial)
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