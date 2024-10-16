using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ClericsSacredSash : BodySash
{
    [Constructable]
    public ClericsSacredSash()
    {
        Name = "Cleric's Sacred Sash";
        Hue = Utility.Random(220, 2220);
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Healing, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        Resistances.Energy = 15;
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ClericsSacredSash(Serial serial) : base(serial)
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
