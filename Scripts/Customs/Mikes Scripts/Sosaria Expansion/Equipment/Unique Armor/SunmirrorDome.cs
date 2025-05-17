using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SunmirrorDome : LightPlateJingasa
{
    [Constructable]
    public SunmirrorDome()
    {
        Name = "Sunmirror Dome";
        Hue = Utility.Random(1150, 1350);  // A warm, radiant hue for a sun-themed item
        BaseArmorRating = Utility.RandomMinMax(25, 50);  // Balanced armor rating

        // Special Attributes for the item
        ArmorAttributes.SelfRepair = 10;  // Durability with time
        Attributes.BonusStr = 5;  // Enhances physical strength for the wearer
        Attributes.BonusDex = 10;  // Adds dexterity, fitting for a swift warrior
        Attributes.DefendChance = 5;  // Provides a small defense boost, symbolizing the shield of sunlight
        Attributes.Luck = 25;  // Boosts luck, representing good fortune under the sunlight
        Attributes.NightSight = 1;  // Helps the wearer see in the dark, as sunlight often symbolizes clarity

        // Skill bonuses that enhance specific capabilities
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);  // Enhances combat strategy, focusing on tactical advantage
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);  // Increases the ability to parry attacks, fitting for a defensive item
        SkillBonuses.SetValues(2, SkillName.Swords, 5.0);  // Slight enhancement to swordsmanship, symbolic of a warrior wielding the sun’s strength

        // Elemental resistances related to the theme of light and protection
        FireBonus = 15;  // Fire damage reduction, as sunlight represents fire's purest form
        EnergyBonus = 10;  // Energy resistance, symbolizing resilience against elemental forces

        // Attach to the XML level item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SunmirrorDome(Serial serial) : base(serial)
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
