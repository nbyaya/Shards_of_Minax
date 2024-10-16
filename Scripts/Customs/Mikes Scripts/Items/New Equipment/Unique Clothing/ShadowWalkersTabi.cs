using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowWalkersTabi : NinjaTabi
{
    [Constructable]
    public ShadowWalkersTabi()
    {
        Name = "Shadow Walker's Tabi";
        Hue = Utility.Random(500, 2500);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
        Resistances.Energy = 10;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowWalkersTabi(Serial serial) : base(serial)
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
