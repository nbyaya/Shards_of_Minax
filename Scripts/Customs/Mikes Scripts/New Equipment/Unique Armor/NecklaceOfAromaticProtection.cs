using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NecklaceOfAromaticProtection : LeatherGorget
{
    [Constructable]
    public NecklaceOfAromaticProtection()
    {
        Name = "Necklace of Aromatic Protection";
        Hue = Utility.Random(250, 750);
        BaseArmorRating = Utility.RandomMinMax(22, 52);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 5;
        SkillBonuses.SetValues(0, SkillName.Cooking, 10.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NecklaceOfAromaticProtection(Serial serial) : base(serial)
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
