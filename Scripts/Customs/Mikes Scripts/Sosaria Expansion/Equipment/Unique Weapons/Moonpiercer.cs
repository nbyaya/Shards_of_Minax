using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Moonpiercer : Pike
{
    [Constructable]
    public Moonpiercer()
    {
        Name = "Moonpiercer";
        Hue = Utility.Random(1250, 1290);  // A soft, ethereal blue with a touch of silver, like moonlight
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.WeaponSpeed = 10;  // Aiming for quicker strikes that mirror the ethereal, quick nature of the moon's influence
        Attributes.Luck = 15;  // The weapon brings good fortune to those seeking knowledge of the unknown
        Attributes.DefendChance = 10;  // Subtle defense, much like the moon's quiet watch over Sosaria
        
        // Slayer effect – Particularly effective against the undead, a weapon of light and purity
        Slayer = SlayerName.Exorcism;
        
        // Weapon attributes – Adding leech effects that channel the power of the moon to restore health and mana
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechMana = 15;
        
        // Skill bonuses focused on dexterity, tactics, and spiritual awareness, tying into the graceful and elusive nature of the moon
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);  // Strategic use in combat, with a focus on precision
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);  // Mastery with polearms, the weapon’s innate elegance
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0);  // Spiritual link, as the moon is a magical force in Sosaria

        // Thematic bonus to enhance magical knowledge or spiritual insight, as the moon pierces the veils of the unknown
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 10.0);  // Drawing upon the spirits under the moon’s influence

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Moonpiercer(Serial serial) : base(serial)
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
