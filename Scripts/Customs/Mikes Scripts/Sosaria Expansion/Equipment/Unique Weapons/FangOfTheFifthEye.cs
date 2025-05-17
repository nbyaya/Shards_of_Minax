using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FangOfTheFifthEye : Dagger
{
    [Constructable]
    public FangOfTheFifthEye()
    {
        Name = "Fang of the Fifth Eye";
        Hue = Utility.Random(1300, 1400);  // A deep, eerie green hue, reminiscent of venom and poison
        MinDamage = Utility.RandomMinMax(25, 50);
        MaxDamage = Utility.RandomMinMax(55, 85);

        // Unique attributes for a weapon with mystical ties to forbidden knowledge and the occult
        Attributes.BonusInt = 10;
        Attributes.BonusDex = 5;
        Attributes.Luck = 50;

        // Weapon Attributes - The dagger is imbued with poison magic and is effective against serpentine creatures
        WeaponAttributes.HitPoisonArea = 50;
        WeaponAttributes.HitLeechMana = 20;

        // Slayer effect - The Fang is particularly lethal against Ophidians, echoing its serpent-like power
        Slayer = SlayerName.Ophidian;

        // Skill bonuses - The dagger boosts the skills of those who dabble in the arcane or secretive arts
        SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);  // Ties into the forbidden knowledge aspect
        SkillBonuses.SetValues(1, SkillName.Mysticism, 10.0);   // A mystical aura that aids in understanding ancient rituals
        SkillBonuses.SetValues(2, SkillName.Fencing, 5.0);       // Enhances combat with a blade

        // Additional thematic bonus - The weapon empowers its wielder’s spiritual senses
        Attributes.NightSight = 1;  // Illuminates the darkness, revealing hidden things in the night

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FangOfTheFifthEye(Serial serial) : base(serial)
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
