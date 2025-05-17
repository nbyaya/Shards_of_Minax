using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Pathcleaver : ExplorersMachete
{
    [Constructable]
    public Pathcleaver()
    {
        Name = "Pathcleaver";
        Hue = Utility.Random(2200, 2300); // A natural earthy green, reflecting its connection to the wild
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 55); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 10;
        
        // Slayer effect – The Pathcleaver is particularly effective against certain creatures of the wild
        Slayer = SlayerName.ReptilianDeath;  // Common theme with exploring dangerous jungles or wild places
        
        // Weapon attributes - aiding in survival and exploration
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 5;
        WeaponAttributes.BattleLust = 15;

        // Skill bonuses – focused on survival and exploration-related skills
        SkillBonuses.SetValues(0, SkillName.Anatomy, 10.0);  // Knowledge of animal and creature anatomy for better combat
        SkillBonuses.SetValues(1, SkillName.Tracking, 20.0);     // Expertise in tracking elusive creatures or uncovering hidden paths
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);    // Enhanced ability to hide in the wilderness while exploring
        SkillBonuses.SetValues(3, SkillName.Camping, 15.0);   // Enhances survival skills, especially with resting and building camps

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Attach custom behavior
    }

    public Pathcleaver(Serial serial) : base(serial)
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
