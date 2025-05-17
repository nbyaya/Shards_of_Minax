using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GeminiFang : DoubleAxe
{
    [Constructable]
    public GeminiFang()
    {
        Name = "Gemini Fang";
        Hue = Utility.Random(2300, 2400);  // A deep, celestial blue with silver streaks, representing the dual nature of Gemini
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(60, 90);
        
        Attributes.WeaponSpeed = 5;
        Attributes.BonusStr = 10;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.Luck = 25;

        // Slayer effect – Gemini Fang is especially effective against Reptilian creatures, symbolic of dualities between the hunter and the prey
        Slayer = SlayerName.ReptilianDeath;

        // Weapon attributes - A double-axe suited for those who specialize in precise, twin-blade combat
        WeaponAttributes.HitLeechHits = 25;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.BattleLust = 20;
        WeaponAttributes.HitColdArea = 15;  // Gemini symbolizes balance, one side cold, the other fiery
        WeaponAttributes.HitFireArea = 15;

        // Skill bonuses – Fitting the dual nature of the weapon, bonuses are provided to **Tactics** (for precision) and **Throwing** (for ranged control)
        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0);
        SkillBonuses.SetValues(2, SkillName.Imbuing, 10.0); // Dual-wielding style

        // Additional thematic bonuses – A nod to Gemini’s duality
        SkillBonuses.SetValues(3, SkillName.Swords, 10.0);  // Two blades, mastering the art of swordsmanship

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GeminiFang(Serial serial) : base(serial)
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
