using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CircuitOfTheEchoingWill : ArtificerWand
{
    [Constructable]
    public CircuitOfTheEchoingWill()
    {
        Name = "Circuit of the Echoing Will";
        Hue = 1152;  // A faint, electric blue glow representing arcane energy and artificer’s magic
        MinDamage = Utility.RandomMinMax(25, 40);
        MaxDamage = Utility.RandomMinMax(50, 70); 
        Attributes.CastSpeed = 1;  // Speed to enhance casting ability
        Attributes.BonusInt = 10;  // Increases intelligence, central to any artificer’s craft
        Attributes.SpellDamage = 15;  // Enhances the power of spells
        Attributes.LowerManaCost = 15;  // Reduces the mana cost of spells

        // Slayer effect – Affects Mechanical Constructs (e.g. Golems, Automata) and machines
        Slayer = SlayerName.ElementalHealth;  // In line with the artificer theme, as it relates to the vitality of constructs

        // Weapon attributes - increasing the wand's versatility with magical effects
        WeaponAttributes.HitEnergyArea = 30;  // Energy-based attacks are thematic with the "Circuit" and the artificer's arcane expertise
        WeaponAttributes.HitLeechMana = 25;  // Leech mana to fuel the artificer’s creations

        // Skill bonuses related to arcane knowledge, mechanics, and the understanding of magical constructs
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Inscribe, 15.0);
        SkillBonuses.SetValues(2, SkillName.Tinkering, 10.0);  // The artificer's love for mechanics and invention
        SkillBonuses.SetValues(3, SkillName.Mysticism, 10.0);  // Adds to the magical essence of the wand’s creation

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Ensures the item gets attached with level information
    }

    public CircuitOfTheEchoingWill(Serial serial) : base(serial)
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
