using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TalsRashasRelic : WoodenKiteShield
{
    [Constructable]
    public TalsRashasRelic()
    {
        Name = "Tal Rasha's Relic";
        Hue = Utility.Random(100, 400);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterCold = 15;
        Attributes.CastRecovery = 2;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TalsRashasRelic(Serial serial) : base(serial)
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
