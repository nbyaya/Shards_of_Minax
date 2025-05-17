using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhispersOfChaos : Nunchucks
{
    [Constructable]
    public WhispersOfChaos()
    {
        Name = "Whispers of Chaos";
        Hue = Utility.Random(2400, 2500);  // A chaotic mix of swirling colors, representing the unpredictable nature of the weapon
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 15;
        Attributes.Luck = 15;
        Attributes.BonusDex = 10;
        
        // Slayer effect – Whispers of Chaos is particularly effective against those who thrive in order and structure, adding to the chaos it invokes
        Slayer = SlayerName.Fey;

        // Weapon attributes - Chaos infuses the weapon with instability, allowing it to occasionally confuse enemies
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.HitLowerDefend = 30;

        // Skill bonuses focused on agility and surprise, reflecting the chaotic and unpredictable nature of nunchucks
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Ninjitsu, 20.0);
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);
        
        // Additional thematic bonus reflecting the weapon's chaotic nature
        Attributes.ReflectPhysical = 10; // Reflects the chaos back at the attacker
        
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhispersOfChaos(Serial serial) : base(serial)
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
