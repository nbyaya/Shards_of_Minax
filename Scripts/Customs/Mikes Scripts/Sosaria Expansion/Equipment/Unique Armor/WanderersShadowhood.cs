using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WanderersShadowhood : LeatherCap
{
    [Constructable]
    public WanderersShadowhood()
    {
        Name = "Wanderer's Shadowhood";
        Hue = Utility.Random(2001, 2200); // Dark and shadowy hues
        BaseArmorRating = Utility.RandomMinMax(5, 20); // Light armor, to suit stealthy types

        Attributes.BonusDex = 15;
        Attributes.BonusInt = 5;
        Attributes.BonusMana = 10;
        Attributes.Luck = 50; // Favorable for those who slip by unnoticed
        Attributes.NightSight = 1; // Helps with vision in dark places
        Attributes.ReflectPhysical = 5; // Reflects some physical damage

        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0); // Key skill for stealth
        SkillBonuses.SetValues(1, SkillName.Hiding, 10.0); // For sneaky maneuvers
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0); // Tracking as part of wandering
        SkillBonuses.SetValues(3, SkillName.Snooping, 10.0); // A little extra edge in detecting hidden things

        ColdBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 5;

        // Adding the XmlLevelItem for compatibility
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WanderersShadowhood(Serial serial) : base(serial)
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
