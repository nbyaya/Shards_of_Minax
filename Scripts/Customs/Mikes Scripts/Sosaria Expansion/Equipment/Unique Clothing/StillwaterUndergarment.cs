using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StillwaterUndergarment : HakamaShita
{
    [Constructable]
    public StillwaterUndergarment()
    {
        Name = "Stillwater Undergarment";
        Hue = 0x897; // Subtle color tone, appropriate for meditative wear
        
        // Set attributes and bonuses
        Attributes.BonusDex = 5;
        Attributes.BonusInt = 5;
        Attributes.BonusHits = 10;
        Attributes.Luck = 50;
        Attributes.DefendChance = 10;
        
        // Resistances (light focus on physical and magical)
        Resistances.Physical = 5;
        Resistances.Energy = 5;

        // Skill Bonuses related to stealth, focus, and spiritual sensitivity
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0); // Silent movement
        SkillBonuses.SetValues(1, SkillName.Meditation, 10.0); // Focus and inner peace
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 5.0); // Stealth and agility focus
        SkillBonuses.SetValues(3, SkillName.Swords, 5.0); // For those who combine physical combat with discipline
        SkillBonuses.SetValues(4, SkillName.Healing, 5.0); // Some healing knowledge to maintain balance
        
        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StillwaterUndergarment(Serial serial) : base(serial)
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
