using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheUmbralFlame : BlackStaff
{
    [Constructable]
    public TheUmbralFlame()
    {
        Name = "The Umbral Flame";
        Hue = 1175;  // A dark shade of purple, representing the deep shadows and flame
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(45, 75);

        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 15;
        Attributes.SpellDamage = 15;
        Attributes.CastSpeed = 1;

        // Skill bonuses related to dark magic and arcane mastery
        SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 15.0);
        SkillBonuses.SetValues(2, SkillName.Magery, 10.0);

        // Unique Slayer: Fey, reflecting the staff's affinity to ancient shadow magic tied to mystical realms
        Slayer = SlayerName.Fey;

        // Adding weapon attributes that reflect its magical potency
        WeaponAttributes.HitFireball = 50;  // Unleashes bursts of fire
        WeaponAttributes.HitEnergyArea = 40;  // Radiates energy damage in the surrounding area
        WeaponAttributes.HitLeechMana = 20;  // Leeches mana with each hit

        // Thematic magical bonuses
        Attributes.NightSight = 1;  // Provides sight in the darkest environments
        Attributes.RegenMana = 2;  // Slowly regenerates mana

        // Adding thematic effects
        WeaponAttributes.HitDispel = 25;  // Dispel magic when striking

        // Additional flavor
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheUmbralFlame(Serial serial) : base(serial)
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
