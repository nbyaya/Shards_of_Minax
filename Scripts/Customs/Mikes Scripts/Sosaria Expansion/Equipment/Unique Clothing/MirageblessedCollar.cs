using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MirageblessedCollar : FancyShirt
{
    [Constructable]
    public MirageblessedCollar()
    {
        Name = "Mirageblessed Collar";
        Hue = Utility.Random(1100, 1150); // A mystical, shifting color

        // Set attributes and bonuses
        Attributes.RegenStam = 3;
        Attributes.RegenMana = 3;
        Attributes.Luck = 50;
        Attributes.ReflectPhysical = 5;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 5;
        Resistances.Poison = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0);
        SkillBonuses.SetValues(2, SkillName.EvalInt, 5.0);
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 5.0);

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MirageblessedCollar(Serial serial) : base(serial)
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
