using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MoonmarkAegis : BashingShield
{
    [Constructable]
    public MoonmarkAegis()
    {
        Name = "Moonmark Aegis";
        Hue = 1150; // Light silver, reminiscent of moonlight
        MinDamage = Utility.RandomMinMax(10, 20);  // Low damage for a shield, as it’s meant for defense
        MaxDamage = Utility.RandomMinMax(15, 30);  // Provides a moderate bash effect, but not an offensive weapon

        // Shield Attributes for defense and protection
        Attributes.DefendChance = 20;  // The shield excels in defense, offering high block chances
        Attributes.Luck = 30;  // Fosters an aura of good fortune for the wielder
        Attributes.RegenHits = 5;  // The moon's energy restores some health over time
        Attributes.RegenMana = 5;  // Provides a slight mana regeneration as well

        // Slayer effect - effective against undead creatures, invoking the moon's light to purify
        Slayer = SlayerName.Exorcism;

        // WeaponAttributes for magical properties
        WeaponAttributes.HitDispel = 50;  // The shield can dispel evil magic in the vicinity, helping protect against dark forces
        WeaponAttributes.HitEnergyArea = 30;  // Releases a burst of energy when the shield is struck, dealing damage to nearby enemies

        // Skill bonuses that reflect the shield’s defensive and mystical properties
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);  // Enhances the wielder's tactics, improving their defensive abilities
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);  // Boosts the Parry skill, making the wielder more adept at blocking attacks
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0);  // Connects the shield with mystical energy, providing a deeper bond with the celestial realm

        // Additional thematic bonus - connects with the moon’s influence
        Attributes.NightSight = 1;  // Grants the ability to see in the dark, invoking the moon’s light

        // Attach XmlLevelItem for level-based progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MoonmarkAegis(Serial serial) : base(serial)
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
