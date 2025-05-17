using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Cogfang : GearLauncher
{
    [Constructable]
    public Cogfang()
    {
        Name = "Cogfang";
        Hue = Utility.Random(1250, 1290);  // A mechanical, steely gray with hints of blue and green
        ItemID = 0x2A96;  // GearLauncher graphic or equivalent
        Weight = 15.0;  // Heavier item due to the mechanical nature

        // Unique properties for the GearLauncher
        Attributes.WeaponDamage = 20;
        Attributes.BonusStr = 15;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        Attributes.BonusMana = 15;
        Attributes.BonusHits = 20;
        
        // Slayer effect: Cogfang is specifically effective against mechanical and construct-type enemies
        Slayer = SlayerName.ElementalHealth;

        // Weapon attributes, reflecting the technical nature of the weapon
        WeaponAttributes.HitEnergyArea = 30;  // Releases a burst of energy upon impact
        WeaponAttributes.HitFireball = 25;    // Fires small explosive projectiles on hit
        WeaponAttributes.HitLightning = 15;   // Deals electric damage upon striking
        WeaponAttributes.HitLeechMana = 10;   // Drains mana on successful hit
        WeaponAttributes.BattleLust = 15;     // Instills an increase in battle focus after strikes

        // Skill bonuses: This weapon enhances technical and engineering skills, reinforcing the mechanical theme
        SkillBonuses.SetValues(0, SkillName.Tinkering, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);
        
        // Additional thematic bonus, reflecting the intellectual and inventive nature of the weapon
        SkillBonuses.SetValues(3, SkillName.Alchemy, 20.0);

        // Attach custom metadata or level attributes
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Cogfang(Serial serial) : base(serial)
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
