using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoiceOfTheStarborn : Scepter
{
    [Constructable]
    public VoiceOfTheStarborn()
    {
        Name = "Voice of the Starborn";
        Hue = Utility.Random(1150, 1190); // A celestial, starry glow
        MinDamage = Utility.RandomMinMax(20, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);
        
        // Attributes that enhance magical capabilities and intelligence
        Attributes.BonusInt = 15;
        Attributes.CastSpeed = 1;  // Faster casting speed, befitting of a celestial artifact
        Attributes.LowerManaCost = 10;  // Lower mana cost, allowing for more spellcasting
        Attributes.SpellDamage = 10;  // Boost to spell damage, representing the celestial influence
        Attributes.Luck = 20;  // A slight bonus to luck, enhancing the chances of beneficial outcomes

        // Slayer effect - specifically crafted for beings associated with the stars, such as stargazers, night entities, or starfallen creatures
        Slayer = SlayerName.Fey;
        
        // Weapon Attributes that align with a cosmic entity
        WeaponAttributes.HitLeechMana = 15;  // Leeching mana, perhaps drawing energy from the stars themselves
        WeaponAttributes.HitLeechHits = 10;  // A small amount of leeching health, aligning with the healing nature of celestial forces
        
        // Skill bonuses that fit the theme of cosmic knowledge, prophecy, and star-based magic
        SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);  // Mysticism, a key skill in the study of celestial and arcane powers
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 15.0);  // Spellweaving, supporting celestial magic and arcane weaves
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);  // EvalInt, enhancing intelligence-based magic and insight

        // A thematic bonus for those who seek to learn or understand the cosmos
        SkillBonuses.SetValues(3, SkillName.Inscribe, 5.0);  // Inscribe, enhancing the crafting of magical texts and scrolls

        // Attach the unique item functionality to this object
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VoiceOfTheStarborn(Serial serial) : base(serial)
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
