using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalistsGauntlets : LeatherGloves
{
    [Constructable]
    public ElementalistsGauntlets()
    {
        Name = "Elementalist's Gauntlets";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 15;
        Attributes.CastSpeed = 3;
        Attributes.RegenMana = 5;
        Attributes.LowerRegCost = 20;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 50.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 40.0);
        PhysicalBonus = 10;
        EnergyBonus = 20;
        FireBonus = 20;
        ColdBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalistsGauntlets(Serial serial) : base(serial)
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
