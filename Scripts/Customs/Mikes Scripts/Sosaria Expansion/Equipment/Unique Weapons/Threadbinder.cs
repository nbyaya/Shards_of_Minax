using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Threadbinder : SpellWeaversWand
{
    [Constructable]
    public Threadbinder()
    {
        Name = "Threadbinder";
        Hue = Utility.Random(1150, 1200); // A deep purple hue, symbolizing arcane mastery and the magic of threads.
        MinDamage = Utility.RandomMinMax(10, 20);
        MaxDamage = Utility.RandomMinMax(30, 50); 

        // Magical attributes enhancing spellcasting
        Attributes.CastSpeed = 1;  // Faster casting time
        Attributes.CastRecovery = 2; // Reduced recovery time
        Attributes.SpellDamage = 15; // Boosts spell damage

        // Slayer – The wand is especially effective against magical foes, as it binds their threads of existence.
        Slayer = SlayerName.Fey;

        // Weapon attributes – enhancing the user's mastery over magic
        WeaponAttributes.HitLeechMana = 25;  // Drains mana from enemies to fuel the caster's power
        WeaponAttributes.HitDispel = 50;  // Greatly enhances the ability to dispel magical effects

        // Skill bonuses – enhancing magical prowess and spellweaving ability
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 30.0);  // Increases Spellweaving skill
        SkillBonuses.SetValues(1, SkillName.Mysticism, 20.0);  // Enhances Mysticism for further spellcasting
        SkillBonuses.SetValues(2, SkillName.EvalInt, 15.0);  // Boosts intelligence evaluation for improved magical insight
        
        // Additional thematic bonus
        Attributes.LowerManaCost = 15;  // Lower the cost of casting spells

        // Attach XmlLevelItem to handle any special behavior in-game
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Threadbinder(Serial serial) : base(serial)
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
