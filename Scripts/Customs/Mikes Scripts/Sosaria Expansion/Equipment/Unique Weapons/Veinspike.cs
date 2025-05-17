using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Veinspike : BladedStaff
{
    [Constructable]
    public Veinspike()
    {
        Name = "Veinspike";
        Hue = Utility.Random(1250, 1300);  // A dark green hue representing the eerie nature of the weapon
        MinDamage = Utility.RandomMinMax(25, 50);
        MaxDamage = Utility.RandomMinMax(55, 90);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect – Veinspike is especially deadly to those who dwell in dark woods or cursed places
        Slayer = SlayerName.ArachnidDoom;

        // Weapon attributes - enhancing the weapon’s ability to cause bleed damage and weaken foes
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechMana = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.HitPoisonArea = 20;
        
        // Skill bonuses for combat and nature magic, emphasizing strategic use and knowledge of natural lore
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // The weapon's knowledge of anatomy aids in its deadly precision
        SkillBonuses.SetValues(3, SkillName.Poisoning, 15.0); // Enhances the poison effects of the weapon

        // Additional thematic bonus - Veinspike is a tool of those who command the forces of nature and decay
        SkillBonuses.SetValues(4, SkillName.SpiritSpeak, 10.0);

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Veinspike(Serial serial) : base(serial)
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
