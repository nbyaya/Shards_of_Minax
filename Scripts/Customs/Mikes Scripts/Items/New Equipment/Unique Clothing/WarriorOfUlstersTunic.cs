using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarriorOfUlstersTunic : Tunic
{
    [Constructable]
    public WarriorOfUlstersTunic()
    {
        Name = "Warrior of Ulster's Tunic";
        Hue = Utility.Random(450, 2480);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusStr = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        Resistances.Physical = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarriorOfUlstersTunic(Serial serial) : base(serial)
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
