using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SpellweaversEnchantedShoes : Shoes
{
    [Constructable]
    public SpellweaversEnchantedShoes()
    {
        Name = "Spellweaver's Enchanted Shoes";
        Hue = Utility.Random(100, 2000);
        Attributes.LowerManaCost = 10;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 20.0);
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SpellweaversEnchantedShoes(Serial serial) : base(serial)
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
