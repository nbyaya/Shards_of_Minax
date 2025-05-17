using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentFlight : NinjaBow
{
    [Constructable]
    public SilentFlight()
    {
        Name = "Silent Flight";
        Hue = Utility.Random(1250, 1290);  // A dark, muted hue symbolizing shadows and stealth
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75); 
        Attributes.WeaponSpeed = 10;  // Boosting the bow's speed for quick, silent shots
        Attributes.Luck = 20;  // Luck to evade detection while in the shadows

        // Slayer effect - especially effective against Reptiles, a nod to the deadly precision of ninjas against snakes and lizards
        Slayer = SlayerName.ReptilianDeath;

        // Weapon attributes - emphasizing quick strikes and stealthy maneuvers
        WeaponAttributes.HitLeechHits = 15;  // Ensuring that hits restore health, keeping the assassin in fighting shape
        WeaponAttributes.HitLeechMana = 10;  // Leeching mana as well, to sustain the ninja’s secret techniques
        WeaponAttributes.BattleLust = 10;  // Encouraging aggression when sneaking through dangerous situations

        // Skill bonuses focused on stealth, archery, and precision
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 10.0);  // Enhancing the ninja’s hidden combat techniques
        SkillBonuses.SetValues(3, SkillName.Tactics, 10.0);  // Strategic advantage in combat situations

        // A thematic bonus linked to quick escapes and the ninja’s ability to vanish without a trace
        Attributes.DefendChance = 10;  // Increasing the chances of avoiding attacks while using stealth

        // Attach the item to XML level for persistent in-game tracking
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentFlight(Serial serial) : base(serial)
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
