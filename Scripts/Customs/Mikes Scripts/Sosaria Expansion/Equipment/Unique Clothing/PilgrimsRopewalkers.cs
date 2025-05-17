using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PilgrimsRopewalkers : Waraji
{
    [Constructable]
    public PilgrimsRopewalkers()
    {
        Name = "Pilgrim's Ropewalkers";
        Hue = 1150; // A natural, earth-toned hue
        Weight = 1.0;

        // Set attributes and bonuses
        Attributes.BonusDex = 5;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 3;
        Attributes.Luck = 50;
        Attributes.DefendChance = 10;

        // Skill Bonuses (thematically related to travel, survival, and agility)
        SkillBonuses.SetValues(0, SkillName.Camping, 15.0);  // Pilgrims often need to camp in the wild
        SkillBonuses.SetValues(1, SkillName.Tracking, 10.0); // Knowledge of paths, forests, and travel routes
        SkillBonuses.SetValues(2, SkillName.AnimalLore, 5.0); // Understanding of wild animals, helpful for pilgrims in nature
        SkillBonuses.SetValues(3, SkillName.Stealth, 10.0); // Stealth to avoid dangers during pilgrimages through hostile lands
        SkillBonuses.SetValues(4, SkillName.Healing, 5.0); // Pilgrims often need basic survival and first aid skills

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PilgrimsRopewalkers(Serial serial) : base(serial)
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
