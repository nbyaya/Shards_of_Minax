using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowleafWrap : ClothNinjaJacket
{
    [Constructable]
    public ShadowleafWrap()
    {
        Name = "Shadowleaf Wrap";
        Hue = 1150; // A deep shade of green to represent the "Shadowleaf" theme
        
        // Set attributes and bonuses
        Attributes.BonusDex = 15; // Boosts Dexterity for agility and stealth
        Attributes.BonusStam = 10; // Increases stamina for swift movement
        Attributes.WeaponDamage = 10; // Boosts weapon damage, fitting for a ninja
        Attributes.DefendChance = 10; // Increases defense chance, helps with dodging attacks
        Attributes.LowerManaCost = 10; // Reduces mana cost, great for stealth-based abilities

        // Resistances
        Resistances.Physical = 15; // Moderate physical resistance
        Resistances.Poison = 20; // Poison resistance, a common threat for ninjas
        Resistances.Energy = 10; // Energy resistance for magical defense

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0); // Enhances the ninja’s stealth capabilities
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 15.0); // Boosts the Ninjitsu skill for enhanced abilities
        SkillBonuses.SetValues(2, SkillName.Hiding, 15.0); // Increases Hiding, crucial for stealthy actions
        SkillBonuses.SetValues(3, SkillName.Swords, 10.0); // Enhances sword fighting for combat-ready ninjas

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowleafWrap(Serial serial) : base(serial)
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
