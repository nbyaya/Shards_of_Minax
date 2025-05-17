using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DancersOath : FancyKilt
{
    [Constructable]
    public DancersOath()
    {
        Name = "Dancer's Oath";
        Hue = 1150; // A vibrant, dance-inspired color
        Weight = 5.0;

        // Set attributes and bonuses
        Attributes.BonusStr = 5; // Slight strength boost for agility
        Attributes.BonusDex = 15; // Major bonus for agility and quickness
        Attributes.BonusHits = 10; // Slight health boost for endurance

        Attributes.RegenHits = 2; // Health regeneration, essential for dancers staying agile
        Attributes.RegenStam = 5; // Stam regen, as dancing requires stamina
        Attributes.WeaponSpeed = 5; // Faster weapon speed, related to fast movements

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Discordance, 25.0); // Major boost for dancing, in line with the kilt's theme
        SkillBonuses.SetValues(1, SkillName.Musicianship, 15.0); // Musicianship to enhance performance skills
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Stealth to help with fluid, quiet movement on stage
        SkillBonuses.SetValues(3, SkillName.Begging, 10.0); // Additional performance skill related to dexterity

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DancersOath(Serial serial) : base(serial)
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
