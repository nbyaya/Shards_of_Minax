using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrandmastersParryingChest : PlateChest
{
    [Constructable]
    public GrandmastersParryingChest()
    {
        Name = "Grandmaster's Parrying Chest";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 100);
        AbsorptionAttributes.ResonanceKinetic = 20;
        ArmorAttributes.LowerStatReq = 50;
        ArmorAttributes.DurabilityBonus = 100;
        Attributes.BonusDex = 25;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 50.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 30.0);
        PhysicalBonus = 20;
        ColdBonus = 15;
        FireBonus = 10;
        EnergyBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrandmastersParryingChest(Serial serial) : base(serial)
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
