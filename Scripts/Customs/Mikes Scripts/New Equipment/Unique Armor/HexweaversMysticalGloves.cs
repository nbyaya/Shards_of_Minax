using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HexweaversMysticalGloves : LeatherGloves
{
    [Constructable]
    public HexweaversMysticalGloves()
    {
        Name = "Hexweaver's Mystical Gloves";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(15, 55);
        AbsorptionAttributes.EaterCold = 20;
        Attributes.CastSpeed = 1;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HexweaversMysticalGloves(Serial serial) : base(serial)
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
