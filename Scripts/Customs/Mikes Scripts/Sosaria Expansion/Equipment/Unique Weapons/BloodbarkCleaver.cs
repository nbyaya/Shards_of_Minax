using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BloodbarkCleaver : Axe
{
    [Constructable]
    public BloodbarkCleaver()
    {
        Name = "Bloodbark Cleaver";
        Hue = 1356;  // A deep red with a dark green undertone, reflecting its connection to blood and the forest
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(75, 110); 
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 20;
        
        // Slayer effect – The axe excels against reptiles, particularly those dwelling in the wild or cursed forests
        Slayer = SlayerName.ReptilianDeath;
        
        // Weapon attributes - This axe is infused with nature's power, enhancing its combat capabilities
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 15;
        
        // Skill bonuses, enhancing combat prowess in the wilderness
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // For strategic combat in the wild
        SkillBonuses.SetValues(1, SkillName.Healing, 10.0);  // For grappling and overpowering opponents
        SkillBonuses.SetValues(2, SkillName.Lumberjacking, 10.0);  // For powerful strikes that shatter bones
        
        // Additional thematic bonuses for survival in the forest and combat situations
        SkillBonuses.SetValues(3, SkillName.Lumberjacking, 20.0);  // A nod to its wood-cutting heritage
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BloodbarkCleaver(Serial serial) : base(serial)
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
