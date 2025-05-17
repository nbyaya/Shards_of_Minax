using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PatchbornPandemonium : JesterSuit
{
    [Constructable]
    public PatchbornPandemonium()
    {
        Name = "Patchborn Pandemonium";
        Hue = Utility.Random(1000, 1500);  // Bright, chaotic hues that change occasionally, embodying the patchwork theme

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 10;
        Attributes.BonusMana = 10;
        Attributes.Luck = 50;  // A jester's luck is unpredictable
        Attributes.RegenMana = 2;  // Mana regeneration to support magic performance

        // Resistances
        Resistances.Physical = 10;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 5;
        Resistances.Energy = 5;

        // Skill Bonuses (Thematically fitting the role of a mischievous jester)
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);  // Jesters are great at provoking and manipulating
        SkillBonuses.SetValues(1, SkillName.Musicianship, 20.0);  // Music and performance are key to a jester's art
        SkillBonuses.SetValues(2, SkillName.Discordance, 15.0);  // Discordance to cause chaos in battle and in the mind
        SkillBonuses.SetValues(3, SkillName.Snooping, 10.0);  // Jesters always need to be aware of secrets around them

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PatchbornPandemonium(Serial serial) : base(serial)
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
