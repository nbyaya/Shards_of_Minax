using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LilyveilKimono : FemaleKimono
{
    [Constructable]
    public LilyveilKimono()
    {
        Name = "Lilyveil Kimono";
        Hue = 0xB96; // Soft, elegant color palette inspired by lilies

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 30;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 10;
        Resistances.Energy = 5;

        // Skill Bonuses (thematically fitting the elegant and mystical qualities)
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0); // Connection with nature
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0); // Grace and nurturing
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Inner peace and balance

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LilyveilKimono(Serial serial) : base(serial)
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
