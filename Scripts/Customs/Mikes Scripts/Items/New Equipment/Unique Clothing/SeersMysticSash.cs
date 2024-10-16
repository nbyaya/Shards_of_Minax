using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SeersMysticSash : BodySash
{
    [Constructable]
    public SeersMysticSash()
    {
        Name = "Seer's Mystic Sash";
        Hue = Utility.Random(150, 1800);
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 8;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SeersMysticSash(Serial serial) : base(serial)
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
