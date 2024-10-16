using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoguesShadowCloak : Cloak
{
    [Constructable]
    public RoguesShadowCloak()
    {
        Name = "Rogue's Shadow Cloak";
        Hue = Utility.Random(1100, 2800);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoguesShadowCloak(Serial serial) : base(serial)
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
