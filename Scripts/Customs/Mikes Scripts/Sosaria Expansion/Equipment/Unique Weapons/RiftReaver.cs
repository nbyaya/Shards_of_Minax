using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RiftReaver : LargeBattleAxe
{
    [Constructable]
    public RiftReaver()
    {
        Name = "The Rift Reaver";
        Hue = Utility.Random(1150, 1200);  // A dark, swirling hue of purple and black, symbolizing the Rift's power
        MinDamage = Utility.RandomMinMax(40, 60);  // High damage to reflect the power of the Rift
        MaxDamage = Utility.RandomMinMax(70, 100); 
        Attributes.WeaponSpeed = 5;  // Slightly faster to emphasize the power of its swing
        Attributes.Luck = 15;  // Luck to aid in the discovery of secrets of the Rift
        Attributes.DefendChance = 15;  // Improved defense, as the weapon channels protective magic from the rift

        // Slayer effect – The Rift Reaver is especially deadly against Elementals, drawn from its origin tied to the Rift
        Slayer = SlayerName.ElementalBan;
        
        // Weapon attributes – high chances for elemental damage effects to enhance the weapon's power
        WeaponAttributes.HitLightning = 25;
        WeaponAttributes.HitFireball = 15;
        WeaponAttributes.HitColdArea = 20;

        // Skill bonuses – To support both destructive and defensive combat, the Rift Reaver enhances combat with elemental powers
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 20.0);
        SkillBonuses.SetValues(2, SkillName.Hiding, 10.0);  // Tied to its crushing power and destructive nature
        SkillBonuses.SetValues(3, SkillName.MagicResist, 10.0);  // Resistance to magic, reflecting its origins from the Rift's wild energies

        // Additional thematic bonus - enhancing the wielder's resilience against elemental forces
        Attributes.ReflectPhysical = 10;

        // Attach the XmlLevelItem for further integration with other systems
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RiftReaver(Serial serial) : base(serial)
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
