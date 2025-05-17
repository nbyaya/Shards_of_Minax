using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoiceOfRuin : Mace
{
    [Constructable]
    public VoiceOfRuin()
    {
        Name = "Voice of Ruin";
        Hue = Utility.Random(1150, 1180);  // A dull grey, representing decay and the passage of time
        MinDamage = Utility.RandomMinMax(35, 50);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect – particularly deadly against undead foes
        Slayer = SlayerName.Exorcism;

        // Weapon attributes - enhancing the mace's ability to drain strength and inflict ruin
        WeaponAttributes.HitLeechHits = 20;
        WeaponAttributes.HitLeechMana = 10;
        WeaponAttributes.HitLeechStam = 15;
        WeaponAttributes.HitDispel = 30;  // The mace's strikes have a chance to dispel magical effects

        // Skill bonuses related to battle tactics and defensive prowess
        SkillBonuses.SetValues(0, SkillName.Macing, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);

        // Additional thematic bonus to align with the idea of ruin and destruction
        SkillBonuses.SetValues(3, SkillName.Necromancy, 5.0);  // Represents a connection to dark powers

        // Attach to the item for XML level interaction
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VoiceOfRuin(Serial serial) : base(serial)
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
