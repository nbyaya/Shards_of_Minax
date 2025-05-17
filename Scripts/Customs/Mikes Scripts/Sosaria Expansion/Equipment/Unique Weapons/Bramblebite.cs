using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Bramblebite : Bow
{
    [Constructable]
    public Bramblebite()
    {
        Name = "Bramblebite";
        Hue = Utility.Random(1250, 1290); // A natural greenish hue to symbolize nature and the forest
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10; // Quick and deadly, allowing swift shots
        Attributes.Luck = 20; // Luck attribute, for favorable hits in the wild
        Attributes.DefendChance = 10; // Enhances defensive capability, reflecting agility and precision
        
        // Slayer effect – Bramblebite is especially effective against reptilian foes, symbolizing its connection to nature's defense
        Slayer = SlayerName.ReptilianDeath;

        // Weapon attributes – Bramblebite causes additional effects to reflect its wild nature and thorn-like traits
        WeaponAttributes.HitLeechHits = 15; // Leeching life from foes as if the bow is drawing energy from the forest itself
        WeaponAttributes.HitLeechStam = 10; // Drains stamina from the target, mirroring the thorny nature of the weapon
        
        // Skill bonuses related to survival and nature
        SkillBonuses.SetValues(0, SkillName.Archery, 15.0); // Boosts Archery, enhancing ranged attack precision
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);  // Gives a tactical advantage for better combat awareness
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0);  // Enhances the ability to track and hunt targets, aligning with the bow's forest origins

        // Additional thematic bonus for nature-themed combat
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 10.0); // This ties in with the bow's connection to nature and creatures
        
        // Attach XmlLevelItem to enhance the item and provide advanced interaction
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Bramblebite(Serial serial) : base(serial)
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
