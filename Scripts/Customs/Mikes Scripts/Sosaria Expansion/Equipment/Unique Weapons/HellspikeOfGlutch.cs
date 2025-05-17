using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HellspikeOfGlutch : WarFork
{
    [Constructable]
    public HellspikeOfGlutch()
    {
        Name = "Hellspike of Glutch";
        Hue = Utility.Random(1250, 1290);  // A deep, fiery red, symbolizing the dark power of Glutch
        MinDamage = Utility.RandomMinMax(40, 60);
        MaxDamage = Utility.RandomMinMax(70, 100);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect - effective against those who would trespass in the cursed land of Glutch
        Slayer = SlayerName.Exorcism;

        // Weapon attributes - enhancing its fiery, dangerous nature in combat
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.HitFireball = 25;
        WeaponAttributes.BattleLust = 15;

        // Skill bonuses to represent the deadly precision and mastery over dark forces
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 20.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 15.0);
        
        // Additional thematic bonus - to represent the hellish nature of the weapon
        SkillBonuses.SetValues(3, SkillName.Necromancy, 10.0);

        // Attach the XmlLevelItem attribute for further customization or loot-level scaling
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HellspikeOfGlutch(Serial serial) : base(serial)
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
