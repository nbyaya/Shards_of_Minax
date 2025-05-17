using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrownOfTheForgottenOath : Helmet
{
    [Constructable]
    public CrownOfTheForgottenOath()
    {
        Name = "Crown of the Forgotten Oath";
        Hue = Utility.Random(1100, 2300);  // A mysterious hue, possibly deep blues or purples to hint at old, forgotten power.
        BaseArmorRating = Utility.RandomMinMax(25, 70); // Strong defense for a helmet, a symbol of forgotten power.

        // Attributes related to strength, defense, and combat prowess.
        Attributes.BonusStr = 10;  // Strength boost for better combat performance
        Attributes.BonusDex = 5;   // Dexterity to improve agility in battle
        Attributes.BonusHits = 20; // Increased health, representing the longevity of the oath

        // Combat-related bonuses
        Attributes.DefendChance = 10;   // Increased defense, as if the helmet provides mystical protection.
        Attributes.ReflectPhysical = 5; // A minor physical reflect to hint at the armor’s historical resilience.
        
        // Skill bonuses - Thematically related to knights and combat strategy
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0); // Boost to tactics, for strategic combat thinking
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);  // Boost to swords, as the wearer of this helm is likely to be a master swordsman
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Boost to anatomy, representing knowledge of weak points in foes

        // Elemental resistances, tying into the enduring legacy of a knight's oath.
        FireBonus = 5;  // Fire resistance, representing the burning passion of the knights who once wore this helm
        PhysicalBonus = 5; // Physical resistance, symbolizing the strong shield of the forgotten order

        // Attach the XmlLevelItem to make this a special, level-based item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrownOfTheForgottenOath(Serial serial) : base(serial)
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
