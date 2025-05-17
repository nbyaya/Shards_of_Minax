using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheMoonherder : ShepherdsCrook
{
    [Constructable]
    public TheMoonherder()
    {
        Name = "The Moonherder";
        Hue = Utility.Random(1150, 1250); // A soft silver hue with a faint moonlit glow.
        MinDamage = Utility.RandomMinMax(15, 30);
        MaxDamage = Utility.RandomMinMax(35, 50);
        Attributes.WeaponSpeed = 10;  // Fast to wield and guide the herd swiftly.
        Attributes.Luck = 15;         // Blessed by the moons, lucky in guiding paths.

        // Bonus to skills related to animals, as the crook helps in herding mystical creatures.
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
        SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);

        // Thematically connected to the moon, helping to control and lead animals under its phases.
        Slayer = SlayerName.EodonTribe;  // The Eodon Tribe have deep connections with the moon and the animals of the wild.

        // Weapon attributes for controlling and aiding animals, and infusing lunar magic.
        WeaponAttributes.HitLeechStam = 15;  // Helps in sustaining the shepherd and the herd.
        WeaponAttributes.HitLeechHits = 10;  // Aids in healing when guiding the flock.
        
        // Magic for moonlight's guidance and protection
        Attributes.CastSpeed = 1;           // Speeds up the casting of spells during moon phases.
        Attributes.CastRecovery = 2;        // Fast recovery, helping the shepherd in combat.
        
        // Lunar affinity, invoking healing and growth among the animals
        Attributes.RegenHits = 3;           // Regenerates hits slowly but steadily when in control of the herd.
        Attributes.RegenStam = 3;           // Regenerates stamina in the quiet peace of the night.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheMoonherder(Serial serial) : base(serial)
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
