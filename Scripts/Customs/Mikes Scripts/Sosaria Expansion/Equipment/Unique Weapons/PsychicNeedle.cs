using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PsychicNeedle : FocusKryss
{
    [Constructable]
    public PsychicNeedle()
    {
        Name = "Psychic Needle";
        Hue = Utility.Random(1150, 1250);  // A muted shade of purple, symbolizing psychic energy and focus.
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);
        Attributes.WeaponSpeed = 15;  // High weapon speed, reflecting the precision of a psychic strike.
        
        // Attributes to enhance mental and physical mastery
        Attributes.BonusInt = 10;
        Attributes.BonusMana = 20;
        Attributes.RegenMana = 3;

        // Slayer effect – especially effective against those with strong will or mental fortitude
        Slayer = SlayerName.ElementalBan;

        // Weapon attributes - exploiting mental vulnerabilities of foes
        WeaponAttributes.HitLeechMana = 25;  // Leech mana from enemies with every strike
        WeaponAttributes.HitHarm = 15;  // Inflicts instant harm, bypassing physical armor
        
        // Skill bonuses focused on mental control and focus
        SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 10.0);
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);
        SkillBonuses.SetValues(3, SkillName.Spellweaving, 10.0);
        
        // Additional thematic bonus for psychic concentration and precision
        SkillBonuses.SetValues(4, SkillName.Tactics, 10.0);

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PsychicNeedle(Serial serial) : base(serial)
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
