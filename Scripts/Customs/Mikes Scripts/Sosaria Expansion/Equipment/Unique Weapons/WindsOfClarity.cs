using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WindsOfClarity : MeditationFans
{
    [Constructable]
    public WindsOfClarity()
    {
        Name = "Winds of Clarity";
        Hue = Utility.Random(1200, 1250);  // A soft, calming shade of green and blue
        Weight = 1.0;

        // Thematic attributes
        Attributes.BonusInt = 15;  // Increases the user's intelligence for enhanced focus and clarity
        Attributes.BonusMana = 20;  // Grants additional mana for meditation and spiritual connection
        Attributes.RegenMana = 5;  // Increases mana regeneration during meditation or calm moments

        // Skill bonuses aligned with meditation and strategic calm
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);  // Boosts meditation for faster mana regeneration
        SkillBonuses.SetValues(1, SkillName.Focus, 10.0);  // Enhances focus for better concentration
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 10.0);  // Provides additional mastery in weaving spells



        // Slayer effect - This fan offers protection from enemies that disrupt the peace, such as those who cause stress or chaos
        Slayer = SlayerName.ArachnidDoom;

        // Attach the XMLLevelItem for future expansion
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WindsOfClarity(Serial serial) : base(serial)
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
