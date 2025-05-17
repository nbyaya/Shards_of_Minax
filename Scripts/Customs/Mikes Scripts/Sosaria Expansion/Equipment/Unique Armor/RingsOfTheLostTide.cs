using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RingsOfTheLostTide : ChainHatsuburi
{
    [Constructable]
    public RingsOfTheLostTide()
    {
        Name = "Rings of the Lost Tide";
        Hue = Utility.Random(2100, 2200);  // A deep oceanic hue
        BaseArmorRating = Utility.RandomMinMax(30, 60);

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 10;
        Attributes.RegenMana = 2;
        Attributes.DefendChance = 10;
        Attributes.CastRecovery = 1;
        Attributes.LowerManaCost = 5;

        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);  // The sea-calling skill bonus
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);  // Tracking oceanic shifts and creatures
        SkillBonuses.SetValues(2, SkillName.Mysticism, 20.0);  // Tapping into forgotten sea magics

        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 5;

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RingsOfTheLostTide(Serial serial) : base(serial)
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
