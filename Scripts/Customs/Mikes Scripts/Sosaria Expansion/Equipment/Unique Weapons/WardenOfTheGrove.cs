using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WardenOfTheGrove : QuarterStaff
{
    [Constructable]
    public WardenOfTheGrove()
    {
        Name = "Warden of the Grove";
        Hue = Utility.Random(2350, 2380);  // A vibrant green hue, symbolizing the power of nature
        MinDamage = Utility.RandomMinMax(20, 35);
        MaxDamage = Utility.RandomMinMax(40, 60);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;
        
        // Slayer effect – The staff is especially effective against those who desecrate nature
        Slayer = SlayerName.ReptilianDeath;
        
        // Weapon attributes - Empowering the wielder to protect and restore the balance of nature
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.BattleLust = 10;

        // Skill bonuses for nature and guardian-related abilities
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);  // The staff helps communicate with creatures
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);  // For restoring the vitality of the forest and its creatures
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);  // Tactical advantage when fighting for the forest's protection
        SkillBonuses.SetValues(3, SkillName.Macing, 10.0);  // Fencing for precision, as the staff can be wielded like a spear

        // A thematic bonus tied to the guardian nature of the weapon
        Attributes.BonusHits = 15;
        
        // Adding the weapon's unique progression system
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WardenOfTheGrove(Serial serial) : base(serial)
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
