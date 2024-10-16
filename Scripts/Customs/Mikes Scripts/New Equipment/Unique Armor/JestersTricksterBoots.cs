using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JestersTricksterBoots : LeatherLegs
{
    [Constructable]
    public JestersTricksterBoots()
    {
        Name = "Jester's Trickster Boots";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusDex = 35;
        Attributes.DefendChance = 20;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JestersTricksterBoots(Serial serial) : base(serial)
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
