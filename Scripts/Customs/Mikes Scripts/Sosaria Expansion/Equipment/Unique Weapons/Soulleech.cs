using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Soulleech : Kryss
{
    [Constructable]
    public Soulleech()
    {
        Name = "Soulleech";
        Hue = Utility.Random(1250, 1290); // A dark and eerie purple hue, symbolizing necrotic energy
        MinDamage = Utility.RandomMinMax(25, 50);
        MaxDamage = Utility.RandomMinMax(55, 85);

        // Weapon attributes related to life leeching and damage absorption
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 10;

        // Thematic Slayer - particularly effective against souls and undead creatures
        Slayer = SlayerName.ArachnidDoom; // Fits the soul-stealing theme as it's good against the undead or those "cursed"
        
        // Life-leeching effects to match its name
        WeaponAttributes.HitLeechHits = 40;  // Leech 40% of damage dealt as health
        WeaponAttributes.HitLeechMana = 30;  // Leech 30% of damage dealt as mana
        WeaponAttributes.HitLeechStam = 20;  // Leech 20% of damage dealt as stamina

        // Skill bonuses with a focus on tactics and necromancy
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // For better combat efficiency
        SkillBonuses.SetValues(1, SkillName.Fencing, 20.0);   // Increases sword skill
        SkillBonuses.SetValues(2, SkillName.Necromancy, 10.0);  // Bonus to necromancy, amplifying the soul-leeching theme

        // Additional thematic bonus for soul-related powers
        SkillBonuses.SetValues(3, SkillName.SpiritSpeak, 15.0);  // Enhance ability to communicate with spirits

        // Attach to XML level item for additional functionality in-game
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Soulleech(Serial serial) : base(serial)
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
