using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerpentsCry : Spear
{
    [Constructable]
    public SerpentsCry()
    {
        Name = "Serpent's Cry";
        Hue = Utility.Random(2000, 2100);  // A greenish hue with faint golden accents, evoking the colors of a serpent
        MinDamage = Utility.RandomMinMax(30, 55);
        MaxDamage = Utility.RandomMinMax(60, 95);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;
        
        // Slayer effect – Serpent’s Cry is especially effective against snakes, serpents, and reptilian creatures
        Slayer = SlayerName.SnakesBane;

        // Weapon attributes for damage absorption and tactical combat
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 5;
        WeaponAttributes.BattleLust = 25;
        
        // Skill bonuses enhancing combat, agility, and serpent lore knowledge
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Fishing, 15.0);
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);
        SkillBonuses.SetValues(3, SkillName.Tracking, 15.0);

        // Additional thematic bonuses for serpent-based lore
        Attributes.NightSight = 1;  // The weapon grants the wielder the ability to see in the dark, a serpent-like trait

        // Attach the item to the XML level system for integration
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerpentsCry(Serial serial) : base(serial)
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
