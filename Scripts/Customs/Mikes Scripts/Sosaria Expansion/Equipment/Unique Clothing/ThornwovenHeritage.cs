using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ThornwovenHeritage : MaleElvenRobe
{
    [Constructable]
    public ThornwovenHeritage()
    {
        Name = "Thornwoven Heritage";
        Hue = 0x4F1; // A deep green hue, reminiscent of Elven forests.

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 25;
        Attributes.BonusHits = 20;
        Attributes.LowerRegCost = 15;
        Attributes.EnhancePotions = 20;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 8;
        Resistances.Fire = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 12;
        Resistances.Energy = 10;

        // Skill Bonuses - Elven magic and nature skills
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0); // Elven mastery of magic
        SkillBonuses.SetValues(1, SkillName.Mysticism, 10.0); // Tied to nature and spiritual energy
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0); // Connection with nature and animals
        SkillBonuses.SetValues(3, SkillName.Healing, 5.0); // Elves are known for their knowledge of healing herbs
        SkillBonuses.SetValues(4, SkillName.Tracking, 10.0); // Elves are master trackers in their forested environments

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ThornwovenHeritage(Serial serial) : base(serial)
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
