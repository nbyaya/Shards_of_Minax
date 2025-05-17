using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GhostOfTheFirstReaper : Scythe
{
    [Constructable]
    public GhostOfTheFirstReaper()
    {
        Name = "Ghost of the First Reaper";
        Hue = Utility.Random(1150, 1180);  // A spectral, eerie greenish glow, signifying its connection to the undead
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 10;

        // Slayer effect – this scythe is especially effective against the undead, making it a potent tool of death
        Slayer = SlayerName.Exorcism;
        
        // Weapon attributes – granting the wielder strength against the forces of the undead
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.HitDispel = 50;  // Adds a strong chance to dispel summoned undead and spectral entities
        
        // Skill bonuses related to necromancy, spirit magic, and tactical combat with undead forces
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Increasing effectiveness against foes
        SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0);  // Enhancing necromantic spellcasting
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 15.0);  // Boosting the spirit-wielder's ability to communicate with the dead
        
        // Additional thematic bonus to amplify the weapon's spectral nature
        Attributes.RegenMana = 5;  // Helps the wielder tap into dark magical energies more efficiently

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GhostOfTheFirstReaper(Serial serial) : base(serial)
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
