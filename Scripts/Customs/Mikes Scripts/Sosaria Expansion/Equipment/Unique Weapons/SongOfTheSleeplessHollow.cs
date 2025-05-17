using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SongOfTheSleeplessHollow : ResonantHarp
{
    [Constructable]
    public SongOfTheSleeplessHollow()
    {
        Name = "Song of the Sleepless Hollow";
        Hue = Utility.Random(1150, 1200); // A deep, ethereal hue representing a spectral aura
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 70);		
        Attributes.Luck = 15;  // Lucky charm to guide its user through spectral realms
        Attributes.WeaponSpeed = 5;  // Enhances speed for those who use the harp in combat

        // Slayer effect – the harp resonates with energies that disturb the dead
        Slayer = SlayerName.ArachnidDoom; // A connection to the restless spirits of the Hollow
        Slayer2 = SlayerName.SpidersDeath; // Strengthens against spiritual and otherworldly threats

        // Weapon attributes - the harp strikes enemies with spiritual energy
        WeaponAttributes.HitLeechMana = 25; // The spectral energy drains mana
        WeaponAttributes.HitLeechStam = 15; // Steals stamina as the haunting music lingers

        // Skill bonuses - This item encourages mastery of music, spirit, and combat tactics
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0); // Strengthens musical prowess
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0); // Enhances communication with spirits
        SkillBonuses.SetValues(2, SkillName.Mysticism, 10.0); // Affinity for mystical forces
        SkillBonuses.SetValues(3, SkillName.Necromancy, 10.0); // A subtle connection to necrotic magic

        // Additional thematic bonus - harnesses the power of ghosts and spirits
        Attributes.CastSpeed = 1;  // Quickens casting, useful for summoning or channeling spirits
        Attributes.CastRecovery = 2;  // Quick recovery between casting powerful spirit-related spells

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Ensures that this item is recognized as a special drop
    }

    public SongOfTheSleeplessHollow(Serial serial) : base(serial)
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
