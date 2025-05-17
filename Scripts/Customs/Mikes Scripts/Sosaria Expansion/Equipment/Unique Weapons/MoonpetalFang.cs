using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MoonpetalFang : Katana
{
    [Constructable]
    public MoonpetalFang()
    {
        Name = "Moonpetal Fang";
        Hue = 1153;  // A soft, ethereal glow, reflecting the moon's energy
        MinDamage = Utility.RandomMinMax(30, 45);
        MaxDamage = Utility.RandomMinMax(55, 75); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 50;  // Luck that stems from the moon’s favor
        Attributes.DefendChance = 10;  // The sword’s defensive potential is heightened
        Attributes.RegenMana = 5;  // Restores mana over time, linked to moon’s energy
        
        // Slayer effect – Moonpetal Fang deals additional damage to creatures of darkness
        Slayer = SlayerName.Fey;
        
        // Weapon attributes – Effective for swift, high-speed strikes
        WeaponAttributes.HitLeechHits = 20;  // Drains life from enemies with every cut
        WeaponAttributes.HitLeechMana = 15;  // Leech mana from the enemy, empowering the wielder
        WeaponAttributes.BattleLust = 25;  // The more the wielder strikes, the stronger they become
        
        // Skill bonuses for combat and mystical prowess
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Enhances combat awareness and skill
        SkillBonuses.SetValues(1, SkillName.Swords, 25.0);   // Boosts swordsmanship with the katana
        SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);  // Channeling the peaceful power of the moon
        
        // Additional thematic bonus, tied to moon magic
        SkillBonuses.SetValues(3, SkillName.Spellweaving, 5.0);  // Small bonus to spellcasting tied to moon magic
        
        // Attach custom XML properties related to item level scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MoonpetalFang(Serial serial) : base(serial)
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
