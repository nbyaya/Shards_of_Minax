using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StridersEmberweave : TattsukeHakama
{
    [Constructable]
    public StridersEmberweave()
    {
        Name = "Strider's Emberweave";
        Hue = 1152; // Fiery orange-red hue

        // Set attributes and bonuses
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 10;


        // Resistances
        Resistances.Physical = 5;
        Resistances.Fire = 15;
        Resistances.Cold = -5;

        // Skill Bonuses (Thematically tied to travel, stealth, and fire)
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        SkillBonuses.SetValues(2, SkillName.Archery, 10.0); // For ranged combat and mobility
        SkillBonuses.SetValues(3, SkillName.Tactics, 10.0); // For strategic maneuvers in combat

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StridersEmberweave(Serial serial) : base(serial)
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
