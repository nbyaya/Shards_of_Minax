using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheFrozenFang : Spear
{
    [Constructable]
    public TheFrozenFang()
    {
        Name = "The Frozen Fang";
        Hue = 1153;  // Icy, cold hue to represent the frost element
        MinDamage = Utility.RandomMinMax(25, 45);  // Frost effects help balance the power
        MaxDamage = Utility.RandomMinMax(50, 80); 
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;
        
        // Special Slayer effect: Frost creatures or anything connected to ice or cold
        Slayer = SlayerName.ElementalBan;  // Since it's associated with ice, elemental forces (frost) are its foes
        
        // Weapon attributes - enhancing cold damage effects
        WeaponAttributes.HitColdArea = 40;  // This gives it a chance to deal cold area damage to enemies
        WeaponAttributes.HitDispel = 25;    // Helps dispel magical barriers, fitting the cold, unyielding theme
        
        // Skill bonuses focused on cold and nature-oriented abilities, enhancing its ability to control and manipulate elemental power
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Increases the strategic use in battle
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);   // Boosts combat proficiency
        SkillBonuses.SetValues(2, SkillName.Anatomy, 5.0);   // The cold strikes are accurate and piercing
        
        // Thematically, the weapon ties into nature's cold powers, so skill bonuses for nature-related combat are emphasized
        SkillBonuses.SetValues(3, SkillName.Mysticism, 5.0);  // Mystical energies of ice flow through its strikes
           
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheFrozenFang(Serial serial) : base(serial)
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
