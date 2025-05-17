using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Noiseblight : DistractingHammer
{
    [Constructable]
    public Noiseblight()
    {
        Name = "Noiseblight";
        Hue = Utility.Random(2000, 2100);  // A garish, glowing yellowish hue to signify its disturbing properties
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(60, 95);
        
        Attributes.WeaponSpeed = 5;  // Faster weapon speed for quick strikes in chaotic combat
        Attributes.Luck = 15;
        
        // Slayer effect – this weapon has a disruptive effect on certain creatures' ability to concentrate or react
        Slayer = SlayerName.ElementalBan;

        // Weapon attributes - the weapon causes confusion and a disruptive effect
        WeaponAttributes.HitLeechHits = 10;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLowerAttack = 25;
        WeaponAttributes.HitLowerDefend = 20;
        WeaponAttributes.HitDispel = 30;

        // Skill bonuses for disruptive and tactical combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Strengthens offensive tactics
        SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);   // Increases Macing skill for a more effective strike
        SkillBonuses.SetValues(2, SkillName.Provocation, 10.0); // Helps provoke and confuse enemies in combat

        // Additional thematic bonus: Noiseblight causes unsettling effects on enemies
        SkillBonuses.SetValues(3, SkillName.Mysticism, 10.0); // Enhances mystical resonance causing aural disturbances

        XmlAttach.AttachTo(this, new XmlLevelItem());  // XML level item attachment

    }

    public Noiseblight(Serial serial) : base(serial)
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
