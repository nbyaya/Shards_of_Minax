using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Clockfang : RepeatingCrossbow
{
    [Constructable]
    public Clockfang()
    {
        Name = "Clockfang";
        Hue = Utility.Random(1150, 1250);  // A mix of golden and bronze hues, reminiscent of clockwork mechanisms
        MinDamage = Utility.RandomMinMax(20, 35);
        MaxDamage = Utility.RandomMinMax(40, 55); 
        Attributes.WeaponSpeed = 15; // Faster firing rate to match the repeating nature of the crossbow
        Attributes.Luck = 15;
        
        // Slayer effect - Clockfang is especially effective against Time Spirits and creatures related to temporal magic
        Slayer = SlayerName.ElementalHealth;
        
        // Weapon attributes - enhancing the crossbow's precision and rapid firing
        WeaponAttributes.HitLeechHits = 10;
        WeaponAttributes.HitLeechMana = 5;
        WeaponAttributes.BattleLust = 15;
        WeaponAttributes.HitDispel = 25;  // Dispel magic for time-related enemies
        
        // Skill bonuses for accuracy and precision in ranged combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);  // Improving tactical advantages
        SkillBonuses.SetValues(1, SkillName.Archery, 15.0);  // Enhancing precision with the crossbow
        SkillBonuses.SetValues(2, SkillName.Fletching, 10.0);  // Increased skill for crafting crossbow bolts
        
        // Additional thematic bonus for manipulating time or speeding up actions
        SkillBonuses.SetValues(3, SkillName.Meditation, 10.0);  // Represents calm precision and control under pressure
        
        // Thematic bonus related to its mechanical nature (like gears and clockwork)
        Attributes.RegenMana = 2;  // Recharge for sustaining the weapon's rapid fire
        
        // Attach the item to the XmlLevelItem to make it function as a unique item with special characteristics
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Clockfang(Serial serial) : base(serial)
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
