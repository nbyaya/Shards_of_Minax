using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShellsongVisor : DragonTurtleHideHelm
{
    [Constructable]
    public ShellsongVisor()
    {
        Name = "Shellsong Visor";
        Hue = 1153; // A light green, turtle-like hue.
        BaseArmorRating = Utility.RandomMinMax(30, 50); // Armor rating balanced for this helm.

        // Attributes for the item
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 15;
        Attributes.DefendChance = 10;
        Attributes.LowerManaCost = 5;

        // Skill bonuses with a thematic focus on nature and defense
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 15.0); // Understanding of creatures, especially beasts.
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0); // Tied to healing creatures or caring for them.
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0); // The ability to track creatures in the wild, useful for nature-based skills.

        // Elemental resistances tied to the turtle hide origins
        PhysicalBonus = 12; // Reflects the physical toughness of the turtle shell.
        ColdBonus = 8; // Provides cold resistance, as turtle-like creatures often thrive in cold waters.

        // Attach a level-based item to grant XML functionality
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShellsongVisor(Serial serial) : base(serial)
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
