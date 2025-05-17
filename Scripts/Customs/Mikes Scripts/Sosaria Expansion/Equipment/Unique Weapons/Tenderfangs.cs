using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Tenderfangs : MeatPicks
{
    [Constructable]
    public Tenderfangs()
    {
        Name = "Tenderfangs";
        Hue = Utility.Random(1250, 1290);  // A dark reddish hue, reflecting the blood and hunting theme.
        MinDamage = Utility.RandomMinMax(15, 25);
        MaxDamage = Utility.RandomMinMax(30, 50);
        Attributes.WeaponSpeed = 10;  // Increases the speed, reflecting how fast the picks can be used for hunting and skinning.
        Attributes.Luck = 15;  // Adds a bit of luck to ensure better harvests.

        // Slayer effect – These picks are especially effective against creatures of the wild
        Slayer = SlayerName.ReptilianDeath;  // The weapon is great for hunting creatures like reptiles and wild beasts.

        // Weapon attributes - To assist in survival and combat
        WeaponAttributes.HitLeechHits = 15;  // Leech health from enemies, important for hunters.
        WeaponAttributes.HitLeechStam = 10;  // Leech stamina to keep the user strong and energized.
        WeaponAttributes.HitLeechMana = 5;  // Provides small mana leech for magical combat when needed.

        // Skill bonuses
        SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0);  // Increases the effectiveness of animal care and healing.
        SkillBonuses.SetValues(1, SkillName.Anatomy, 15.0);  // Increases the damage to living creatures with weak spots.
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);  // Improves the combat effectiveness against wild creatures.

        // Additional thematic bonus
        SkillBonuses.SetValues(3, SkillName.AnimalLore, 10.0);  // An additional bonus to make the weapon even more effective in the wilderness.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Tenderfangs(Serial serial) : base(serial)
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
