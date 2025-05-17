using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Hopebreaker : BeggersStick
{
    [Constructable]
    public Hopebreaker()
    {
        Name = "Hopebreaker";
        Hue = Utility.Random(1150, 1180);  // A warm, golden hue, symbolizing a glimmer of hope.
        MinDamage = Utility.RandomMinMax(10, 25);
        MaxDamage = Utility.RandomMinMax(30, 50);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;

        // Slayer effect - the weapon does more damage to the hopeless and despairing, metaphorically "breaking" their hope
        Slayer = SlayerName.Vacuum;

        // Weapon attributes - the stick grants enhanced luck and a glimmer of hope during times of great despair.
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.BattleLust = 10;
        WeaponAttributes.HitLowerAttack = 10;

        // Skill bonuses to support the role of the beggar or wanderer, focused on survival and evasion.
        SkillBonuses.SetValues(0, SkillName.Begging, 25.0);
        SkillBonuses.SetValues(1, SkillName.Snooping, 20.0);
        SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);
        
        // Additional thematic bonus to support survival in harsh conditions
        SkillBonuses.SetValues(3, SkillName.Healing, 10.0);

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Hopebreaker(Serial serial) : base(serial)
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
