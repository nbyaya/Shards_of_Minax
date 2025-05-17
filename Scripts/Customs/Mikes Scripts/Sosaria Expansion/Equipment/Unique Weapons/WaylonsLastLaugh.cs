using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WaylonsLastLaugh : Cutlass
{
    [Constructable]
    public WaylonsLastLaugh()
    {
        Name = "Waylon’s Last Laugh";
        Hue = Utility.Random(2100, 2150); // A ghostly greenish hue, resembling the eerie light of cursed treasure
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        
        // The weapon’s infamous ability to strike fear into enemies
        WeaponAttributes.HitHarm = 25;
        WeaponAttributes.HitLowerDefend = 20;
        WeaponAttributes.HitLowerAttack = 15;

        // Special effect tied to pirate legends, Waylon’s weapon brings both fortune and misfortune
        Slayer = SlayerName.Repond; // Custom Slayer against other pirates, symbolic of Waylon’s rivalry with his kind
        
        // Skill bonuses to support swift, tactical combat, especially in chaotic, close-quarters pirate fights
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(2, SkillName.Begging, 25.0); // Custom skill for pirate-related abilities (such as swashbuckling and intimidation)
        
        // Additional thematic bonus to enhance the unpredictable, sea-faring nature of the weapon
        SkillBonuses.SetValues(3, SkillName.Stealth, 10.0); // As a pirate, Waylon could strike from the shadows

        // Attach a special tag for the weapon that connects it to the system of leveled items
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WaylonsLastLaugh(Serial serial) : base(serial)
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
