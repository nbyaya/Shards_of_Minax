using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SakurawindRobe : MaleKimono
{
    [Constructable]
    public SakurawindRobe()
    {
        Name = "Sakurawind Robe";
        Hue = 1152; // Light pinkish hue, reminiscent of cherry blossoms.

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 20;
        Attributes.BonusHits = 15;
        Attributes.BonusStam = 10;
        Attributes.BonusMana = 30;
        Attributes.EnhancePotions = 15;
        Attributes.Luck = 40;

        // Resistances
        Resistances.Physical = 8;
        Resistances.Fire = 12;
        Resistances.Cold = 5;
        Resistances.Poison = 10;
        Resistances.Energy = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Healing, 10.0); // Ties into nature, life, and balance.
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0); // Inner peace, harmony with nature.
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0); // Understanding of nature and the animal world.
        SkillBonuses.SetValues(3, SkillName.Ninjitsu, 10.0); // Stealth and agility in nature, blending with the wind.
        SkillBonuses.SetValues(4, SkillName.Musicianship, 5.0); // Ties into the serene, cultural aspects of Sosaria.

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SakurawindRobe(Serial serial) : base(serial)
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
