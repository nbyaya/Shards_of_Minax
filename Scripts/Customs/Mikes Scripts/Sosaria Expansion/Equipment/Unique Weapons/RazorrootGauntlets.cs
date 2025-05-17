using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RazorrootGauntlets : AnimalClaws
{
    [Constructable]
    public RazorrootGauntlets()
    {
        Name = "Razorroot Gauntlets";
        Hue = Utility.Random(1300, 1320); // Earthy tones, representing the strength of nature
        Weight = 1.0;

        // Damage and functionality are tied to animal-related abilities and survival
        Attributes.WeaponSpeed = 10;  // Increased speed for more strikes with wild ferocity
        Attributes.BonusStr = 10; // Increases strength for physical combat

        // Special bonuses related to animal and nature
        Attributes.BonusHits = 20;  // Enhances survivability, making the wearer more resilient
        Attributes.BonusStam = 15;  // Increases stamina for more sustained combat

        // Skills to align with the primal and beast-taming nature of the weapon
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);  // Better understanding and communication with animals
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0);  // Enhances the healing of animals, linking to the primal nature of the gauntlets
        SkillBonuses.SetValues(2, SkillName.Wrestling, 10.0);  // Helps in close combat, with more natural and animalistic grappling skills

        // Slayer effect – These gauntlets are designed to deal with nature's fiercest creatures
        Slayer = SlayerName.ReptilianDeath;  // Special bonus against reptiles, showcasing the wild nature of these gauntlets

        // Special Weapon Attributes for thematic abilities
        WeaponAttributes.HitLeechHits = 25; // Leech health from foes when striking with the claws
        WeaponAttributes.HitLeechStam = 15; // Leech stamina for quick strikes and exhaustion tactics
        WeaponAttributes.HitPoisonArea = 10; // Adds a chance for poison damage, representing the wild unpredictability of nature

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaches custom XML level data for further gameplay interaction
    }

    public RazorrootGauntlets(Serial serial) : base(serial)
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
