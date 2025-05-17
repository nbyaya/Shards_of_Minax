using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WailOfTheForgotten : SpiritScepter
{
    [Constructable]
    public WailOfTheForgotten()
    {
        Name = "Wail of the Forgotten";
        Hue = Utility.Random(1150, 1250);  // Dark ethereal purple, representing loss and sorrow
        MinDamage = Utility.RandomMinMax(20, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);

        // Attributes to enhance the scepter's connection to the spirits and necromancy
        Attributes.BonusInt = 15; // Increased intelligence to tap into ethereal knowledge
        Attributes.BonusMana = 30; // More mana to use spirit-based abilities
        Attributes.CastSpeed = 1; // Increases the casting speed, allowing for quicker spirit calls
        Attributes.Luck = 15; // A small amount of luck to reflect its otherworldly origins
        
        // Slayer effect – The scepter is powerful against the undead and other spirits
        Slayer = SlayerName.Exorcism;

        // Weapon attributes – Enhances spirit-based effects such as leeching and damage over time
        WeaponAttributes.HitLeechMana = 20; // Leech mana from foes in battle
        WeaponAttributes.HitLeechHits = 10; // Leech health from enemies

        // Skill bonuses for spirit-based magic and summoning
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0); // Increases SpiritSpeak for communicating with the dead
        SkillBonuses.SetValues(1, SkillName.Necromancy, 25.0); // Boosts Necromancy for powerful undead magic
        SkillBonuses.SetValues(2, SkillName.Mysticism, 15.0); // Enhances Mysticism for arcane rituals

        // Thematic bonus: enhances interactions with the dead and communicates with lost souls
        SkillBonuses.SetValues(3, SkillName.Meditation, 10.0); // Helps focus and attune to the spirit world

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WailOfTheForgotten(Serial serial) : base(serial)
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
