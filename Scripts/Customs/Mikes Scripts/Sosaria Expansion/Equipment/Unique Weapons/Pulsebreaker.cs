using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Pulsebreaker : WarHammer
{
    [Constructable]
    public Pulsebreaker()
    {
        Name = "Pulsebreaker";
        Hue = Utility.Random(1150, 1250);  // A deep red hue, representing the force and power of the weapon
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 110);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;

        // Slayer effect – Pulsebreaker is devastating to Elementals, absorbing their elemental energy
        Slayer = SlayerName.ElementalBan;

        // Weapon attributes - Pulsing force, stunning enemies in its wake
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechStam = 20;
        WeaponAttributes.HitDispel = 30;  // Reflects the weapon's ability to disrupt magic
        WeaponAttributes.HitEnergyArea = 50;  // Pulsebreaker sends out waves of energy

        // Skill bonuses, supporting strength and tactics to match Pulsebreaker's devastating attacks
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 25.0);  // Increases damage and handling for maces


        // Additional thematic bonus
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);  // Increased effectiveness when striking vulnerable targets

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Pulsebreaker(Serial serial) : base(serial)
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
