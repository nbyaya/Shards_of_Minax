using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarforgedVow : Longsword
{
    [Constructable]
    public StarforgedVow()
    {
        Name = "Starforged Vow";
        Hue = Utility.Random(1200, 1350);  // A glowing celestial hue, shifting from silver to a deep blue
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(65, 95);
        
        Attributes.WeaponSpeed = 5;  // This weapon is forged for swift strikes
        Attributes.Luck = 15;  // The blade brings the fortune of the stars

        // The Starforged Vow is a weapon of celestial origin, tied to the power of fate
        Slayer = SlayerName.DragonSlaying;  // Effective against evil, tied to the cosmic balance

        // Weapon attributes - enhancing the wielder’s abilities
        WeaponAttributes.HitLeechHits = 30;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 20;

        // Skill bonuses in alignment with cosmic willpower and celestial knowledge
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Combat finesse is vital for this weapon
        SkillBonuses.SetValues(1, SkillName.Swords, 25.0);  // Mastery over swordplay is key
        SkillBonuses.SetValues(2, SkillName.Chivalry, 10.0);  // Celestial weapons often require a righteous cause

        // Additional bonus reflecting its divine origins
        SkillBonuses.SetValues(3, SkillName.Meditation, 15.0);  // Connection to the cosmos, serenity and focus

        // Adding XMLLevelItem attribute for item progression and special effects
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarforgedVow(Serial serial) : base(serial)
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
