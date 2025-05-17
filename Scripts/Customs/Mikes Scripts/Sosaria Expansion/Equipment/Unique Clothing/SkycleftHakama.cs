using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkycleftHakama : Hakama
{
    [Constructable]
    public SkycleftHakama()
    {
        Name = "Skycleft Hakama";
        Hue = 1157;  // Celestial blue-green hue for a mystical look

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.Luck = 50;
        Attributes.NightSight = 1; // Celestial connection, the stars guide you
        Attributes.RegenStam = 3; // Boosts stamina regeneration, keeping you nimble in battle

        // Resistances
        Resistances.Physical = 5;
        Resistances.Cold = 8;
        Resistances.Energy = 5;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0); // Helps in remaining unseen, fitting the celestial nature
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 10.0); // Mystical and elusive arts tie in with celestial themes
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Connection to the stars, inner peace
        SkillBonuses.SetValues(3, SkillName.Focus, 10.0); // Focused power from the skies above, harnessing energy
        SkillBonuses.SetValues(4, SkillName.AnimalLore, 5.0); // A link to nature, possibly tied to celestial creatures

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkycleftHakama(Serial serial) : base(serial)
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
