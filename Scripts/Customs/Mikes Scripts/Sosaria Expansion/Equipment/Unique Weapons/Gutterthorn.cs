using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Gutterthorn : ShortSpear
{
    [Constructable]
    public Gutterthorn()
    {
        Name = "Gutterthorn";
        Hue = Utility.Random(1100, 1200);  // A deep, shadowy green to represent the weapon’s connection to the underground and decay
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 60); 
        
        Attributes.WeaponSpeed = 10;  // This weapon should strike quickly in the hands of its wielder
        Attributes.Luck = 20;  // Boosts the wielder’s fortune in finding rare items and avoiding danger
        
        // Slayer effect – Effective against those dwelling in filth and shadows, such as sewer beasts or corrupt creatures
        Slayer = SlayerName.TrollSlaughter;

        // Weapon attributes - Gutterthorn deals with leeching and debilitating effects, making it a perfect weapon for draining enemies
        WeaponAttributes.HitLeechStam = 30;  // The spear drains stamina with each strike, draining the energy of its victim
        WeaponAttributes.HitLeechMana = 15;  // Also leeches a small amount of mana, siphoning magical energies
        
        // Skill bonuses related to tactics and stealth, suitable for a weapon used by a rogue or assassin
        SkillBonuses.SetValues(0, SkillName.Fishing, 10.0);  // Boosts swordsmanship to emphasize its ability to work in the hands of a skilled combatant
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);  // Gutterthorn is a weapon suited for those who hide in the shadows
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);  // Enhances the wielder’s ability to strike tactically in a fight

        // Thematically appropriate bonus to enhance the dark, underground nature of the weapon
        SkillBonuses.SetValues(3, SkillName.Tracking, 5.0);  // Helps track enemies through dark, hidden places such as dungeons or sewers
        
        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attaches level-specific information to the item
    }

    public Gutterthorn(Serial serial) : base(serial)
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
