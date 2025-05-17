using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Arcspike : BoltRod
{
    [Constructable]
    public Arcspike()
    {
        Name = "Arcspike";
        Hue = 1157;  // A bluish hue representing electrical energy
        MinDamage = Utility.RandomMinMax(20, 45);
        MaxDamage = Utility.RandomMinMax(50, 80);

        // Weapon attributes to emphasize its magical potency
        Attributes.WeaponSpeed = 10;  // Faster casting and quicker reaction
        Attributes.SpellDamage = 15;  // Increase in spell effectiveness
        Attributes.CastSpeed = 1;     // Faster casting
        Attributes.Luck = 10;         // Boosts chances of finding valuable items

        // Slayer type – effective against Elementals, creatures with electrical vulnerabilities
        Slayer = SlayerName.ElementalBan;

        // Weapon-specific effects
        WeaponAttributes.HitEnergyArea = 50;  // Electrical burst that damages all nearby enemies
        WeaponAttributes.HitLightning = 25;    // Strikes with a lightning effect

        // Skill bonuses related to mystical combat and elemental mastery
        SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);  // Boosts mystical powers
        SkillBonuses.SetValues(1, SkillName.Magery, 20.0);     // Strengthens spellcasting abilities
        SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);     // Improves intelligence evaluation for spellcasting

        // Additional thematic effect for combat and magical utility
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Arcspike(Serial serial) : base(serial)
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
