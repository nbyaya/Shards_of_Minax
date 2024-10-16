using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarmonyGauntlets : PlateGloves
{
    [Constructable]
    public HarmonyGauntlets()
    {
        Name = "Harmony Gauntlets";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.ResonanceEnergy = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenMana = 8;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarmonyGauntlets(Serial serial) : base(serial)
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
