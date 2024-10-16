using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChefsAegisApron : PlateChest
{
    [Constructable]
    public ChefsAegisApron()
    {
        Name = "Chef's Aegis Apron";
        Hue = 488;
        BaseArmorRating = Utility.RandomMinMax(65, 75);
        AbsorptionAttributes.EaterFire = 40;
        ArmorAttributes.DurabilityBonus = 100;
        ArmorAttributes.LowerStatReq = 50;
        Attributes.BonusStr = 20;
        Attributes.Luck = 200;
        Attributes.EnhancePotions = 50;
        SkillBonuses.SetValues(0, SkillName.Cooking, 50.0);
        SkillBonuses.SetValues(1, SkillName.TasteID, 40.0);
        PhysicalBonus = 20;
        FireBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 25;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChefsAegisApron(Serial serial) : base(serial)
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
