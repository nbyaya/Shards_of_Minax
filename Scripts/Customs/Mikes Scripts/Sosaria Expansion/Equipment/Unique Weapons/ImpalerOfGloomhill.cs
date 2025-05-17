using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ImpalerOfGloomhill : Halberd
{
    [Constructable]
    public ImpalerOfGloomhill()
    {
        Name = "The Impaler of Gloomhill";
        Hue = Utility.Random(1300, 1600);  // A dark purple-black hue, representing the oppressive and eerie aura of Gloomhill.
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        
        // Thematic Attributes – Reflecting its cursed origins and its effectiveness against spirits
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 30;
        
        // Slayer effect – especially effective against Undead and Ghostly beings, fitting its lore
        Slayer = SlayerName.Exorcism;
        
        // Weapon attributes – granting extra power for a more decisive strike against cursed enemies
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.BattleLust = 10;
        WeaponAttributes.HitDispel = 50;  // A powerful dispelling effect to eradicate supernatural forces

        // Skill bonuses focused on combat against the supernatural
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // Enhances tactical combat skills
        SkillBonuses.SetValues(1, SkillName.Swords, 10.0);  // Boosts halberd-related combat expertise
        SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0);  // Helps with communication or defense against the spirit realm
        
        // Additional thematic bonus – The weapon feels attuned to the night and the supernatural
        Attributes.NightSight = 1;  // A dark aura that gives its wielder the ability to see in the darkest of places

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ImpalerOfGloomhill(Serial serial) : base(serial)
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
