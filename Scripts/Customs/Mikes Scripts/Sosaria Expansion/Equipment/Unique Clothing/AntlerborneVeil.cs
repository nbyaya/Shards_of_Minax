using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AntlerborneVeil : DeerMask
{
    [Constructable]
    public AntlerborneVeil()
    {
        Name = "Antlerborne Veil";
        Hue = 1150; // Natural, earthy tones of browns and greens

        // Set attributes and bonuses
        Attributes.BonusHits = 15;
        Attributes.RegenHits = 2;
        Attributes.RegenStam = 2;
        Attributes.RegenMana = 2;

        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 5;
        Resistances.Energy = 5;

        // Skill Bonuses (thematically chosen)
        SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0); // Affinity with animals
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 12.0); // Deep knowledge of forest creatures
        SkillBonuses.SetValues(2, SkillName.Healing, 5.0); // The mask connects the wearer to the spirit of life and restoration
        SkillBonuses.SetValues(3, SkillName.Tracking, 8.0); // Keen senses of the wilderness

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AntlerborneVeil(Serial serial) : base(serial)
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
