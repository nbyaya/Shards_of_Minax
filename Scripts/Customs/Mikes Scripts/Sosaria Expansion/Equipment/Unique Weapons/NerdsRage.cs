using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NerdsRage : MallKatana
{
    [Constructable]
    public NerdsRage()
    {
        Name = "NerdsRage";
        Hue = 0x8A5;  // A shade of neon red, symbolizing rage and consumer frenzy
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        
        // Weapon attributes - high speed, reflecting the haste and impatience of a shopper in the frenzy
        Attributes.WeaponSpeed = 15;
        Attributes.Luck = 10;

        // Thematic Slayer: Effective against any who hoard or collect items without sharing or regard for others
        Slayer = SlayerName.Repond;  // Repond – to fight against hoarding entities or insidious greed

        // Weapon Special Effects: Damage to those who focus on hoarding and greed
        WeaponAttributes.HitLeechHits = 10;  // Leech health, symbolizing the weapon's ability to drain greed
        WeaponAttributes.HitLeechMana = 10;  // Leech mana, showing its consumption of energy
        WeaponAttributes.HitLeechStam = 5;  // Leech stamina, symbolizing the tired, impatient shopper

        // Skill bonuses: Bonus to tactics and stealing skills, representing the ability to outsmart and overpower foes
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Enhances tactical strikes amidst chaos
        SkillBonuses.SetValues(1, SkillName.Bushido, 10.0);    // Increases sword proficiency, reflecting the sharp edge of the katana
        SkillBonuses.SetValues(2, SkillName.Snooping, 10.0);   // Gives bonus to stealthy loot-seeking (like scavenging through crowds)

        // Additional thematic bonus for “rage” effects
        WeaponAttributes.HitLowerDefend = 15;  // Represents a boost in offensive rage, lowering enemy defense as the wielder gets enraged
        WeaponAttributes.HitLowerAttack = 10;  // The rush of a shopping frenzy also lowers the enemy’s attack speed

        // Attach XML Level Item attribute for dynamic item level scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NerdsRage(Serial serial) : base(serial)
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
