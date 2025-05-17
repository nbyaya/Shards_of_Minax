using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChesswardensResolve : CheckeredKilt
{
    [Constructable]
    public ChesswardensResolve()
    {
        Name = "Chesswarden's Resolve";
        Hue = 1161; // Dark grey with subtle accents, matching a chessboard theme

        // Set attributes and bonuses
        Attributes.BonusDex = 15;
        Attributes.BonusHits = 30;
        Attributes.WeaponDamage = 10;
        Attributes.DefendChance = 12;
        Attributes.Luck = 25;

        // Resistances
        Resistances.Physical = 12;
        Resistances.Fire = 5;
        Resistances.Cold = 10;
        Resistances.Poison = 7;
        Resistances.Energy = 5;

        // Skill Bonuses (tactical, stealth, and combat-focused)
        SkillBonuses.SetValues(0, SkillName.Swords, 10.0);      // Strengthens sword fighting, reflecting the warden's combat prowess
        SkillBonuses.SetValues(1, SkillName.Tactics, 12.0);     // Enhances tactical awareness, key for positioning on the battlefield
        SkillBonuses.SetValues(2, SkillName.Stealth, 5.0);      // Subtlety in approach, for sneak attacks or ambushes
        SkillBonuses.SetValues(3, SkillName.Parry, 8.0);        // Improves defense, necessary for blocking attacks in battle
        SkillBonuses.SetValues(4, SkillName.DetectHidden, 7.0); // Helps spot hidden enemies, crucial for tactical planning

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChesswardensResolve(Serial serial) : base(serial)
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
