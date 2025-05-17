using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HungerEnd : CooksCleaver
{
    [Constructable]
    public HungerEnd()
    {
        Name = "Hunger's End";
        Hue = Utility.Random(1150, 1250); // Deep, earthy brown to match the cleaver's association with food and harvest
        MinDamage = Utility.RandomMinMax(15, 30);  // A solid, reliable cleaver for cutting through tough meat
        MaxDamage = Utility.RandomMinMax(35, 55);  // Damage meant to be decisive, especially when cutting through large pieces

        // Weapon attributes - reflecting the cleaver’s ability to end hunger and deal with large challenges
        Attributes.WeaponSpeed = 10;  // Speeds up the attack for quick cooking strikes
        Attributes.Luck = 10;         // A touch of fortune in gathering fresh, perfect ingredients

        // Slayer effect - Hunger's End is effective against creatures of hunger, such as undead and other fiends
        Slayer = SlayerName.BloodDrinking; // It's especially effective against undead, creatures that feed off the living

        // Weapon attributes for combat efficiency
        WeaponAttributes.HitLeechHits = 25; // The cleaver leeches health from creatures it strikes, useful in dark corners of the world
        WeaponAttributes.HitLeechStam = 10; // Adds a stamina leech effect, fitting for a weapon used to slay the hungry and undead

        // Skill bonuses - enhancing the player’s ability to deal with both combat and culinary challenges
        SkillBonuses.SetValues(0, SkillName.Cooking, 15.0); // Increases cooking skill, allowing for better dishes
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);  // Adds a tactical advantage when using the cleaver in combat
        SkillBonuses.SetValues(2, SkillName.Healing, 10.0);  // Increases healing skill, as the cleaver can also be used for surgery

        // Additional thematic bonus: enhancing survival instincts in dire times
        SkillBonuses.SetValues(3, SkillName.Tracking, 5.0); // Helps with foraging and preparing in the wilderness

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HungerEnd(Serial serial) : base(serial)
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
