using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlameOfFinalJudgement : WitchBurningTorch
{
    [Constructable]
    public FlameOfFinalJudgement()
    {
        Name = "Flame of Final Judgement";
        Hue = 1150;  // A fiery orange with a subtle red glow, symbolizing the judgmental fire
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75); 
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect – Especially effective against Necromancy and dark magic users, representing the torch's role in purging evil
        Slayer = SlayerName.Exorcism;
        
        // Weapon attributes to enhance the torch's ability to "burn" enemies and deal spiritual damage
        WeaponAttributes.HitFireball = 50;
        WeaponAttributes.HitDispel = 40;
        WeaponAttributes.HitLeechMana = 20;
        
        // Skill bonuses for magical and combat efficiency, specifically geared towards those purging the undead or witches
        SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);  // Weakens necromantic magic
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);  // Enhances resistance to magic, ideal for warding off curses
        SkillBonuses.SetValues(2, SkillName.Spellweaving, 10.0); // Enhances weaving of powerful magical protections

        // Additional thematic bonus to emphasize the purification role of the weapon
        SkillBonuses.SetValues(3, SkillName.Healing, 5.0); // Adds a touch of healing magic to purify enemies or heal allies in battle

        // Attach the weapon as a level item
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlameOfFinalJudgement(Serial serial) : base(serial)
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
