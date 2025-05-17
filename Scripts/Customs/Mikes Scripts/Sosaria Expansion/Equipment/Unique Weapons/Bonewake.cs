using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Bonewake : NecromancersStaff
{
    [Constructable]
    public Bonewake()
    {
        Name = "Bonewake";
        Hue = Utility.Random(1100, 1150); // A bone-white, shadowy hue, indicative of necrotic power
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        
        // Attributes tied to the mastery of necromantic arts
        Attributes.BonusInt = 15;
        Attributes.BonusMana = 20;
        Attributes.SpellDamage = 10;
        Attributes.LowerManaCost = 5;
        
        // Thematic Slayer against those who would oppose necromancy, like reptilian or undead foes
        Slayer = SlayerName.ReptilianDeath;
        
        // Weapon attributes that synergize with necromantic powers
        WeaponAttributes.HitLeechHits = 15;  // Leeching life force from foes, as necromancers often do
        WeaponAttributes.HitLeechMana = 20;  // Restores mana by stealing the energy of the living
        WeaponAttributes.HitLeechStam = 10;  // Absorbing stamina, weakening enemies in battle

        // Skill bonuses appropriate for a necromancer
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);  // Mastery of death magic
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);  // Increased spiritual communication
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Understanding of body and soul, perfect for a necromancer

        // Additional thematic bonus: Enhancing the ability to control the dead or weaken the living
        SkillBonuses.SetValues(3, SkillName.MagicResist, 10.0);

        // Adding unique functionality: the staff gives off an eerie aura in dark areas, enhancing spellcasting
        Attributes.NightSight = 1;  // The staff's eerie glow provides vision in the dark
        
        // Xml attachment for future customization or upgrades
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Bonewake(Serial serial) : base(serial)
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
