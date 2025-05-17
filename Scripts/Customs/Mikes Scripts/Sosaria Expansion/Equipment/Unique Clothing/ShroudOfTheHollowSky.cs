using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShroudOfTheHollowSky : Cloak
{
    [Constructable]
    public ShroudOfTheHollowSky()
    {
        Name = "Shroud of the Hollow Sky";
        Hue = Utility.Random(1150, 1350); // Dark, ethereal blue with hints of silver

        // Set attributes and bonuses

        Attributes.BonusInt = 10;
        Attributes.BonusHits = 25;
        Attributes.DefendChance = 10;
        Attributes.LowerManaCost = 10;
        Attributes.NightSight = 1; // Symbolic of the ability to see in darkness
        Attributes.Luck = 50; // The cloak draws luck from the hidden sky

        // Resistances
        Resistances.Physical = 8;
        Resistances.Cold = 12;
        Resistances.Poison = 10;
        Resistances.Energy = 15;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Ties into the cloak’s shadowy nature
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0); // Increases stealth and hiding capabilities
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 5.0); // Reflects connection to the shadow realm and spirits
        SkillBonuses.SetValues(3, SkillName.Magery, 5.0); // Adds a hint of arcane power, relevant to the shroud's magical nature

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShroudOfTheHollowSky(Serial serial) : base(serial)
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
