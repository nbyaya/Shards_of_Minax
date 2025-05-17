using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkullOfTheSixthMoon : BoneHelm
{
    [Constructable]
    public SkullOfTheSixthMoon()
    {
        Name = "Skull of the Sixth Moon";
        Hue = 1150; // Bone-themed color, slightly eerie
        BaseArmorRating = Utility.RandomMinMax(20, 70);

        // Attributes tied to necromantic and eerie power
        Attributes.BonusInt = 15;
        Attributes.BonusHits = 25;
        Attributes.DefendChance = 10;
        Attributes.SpellDamage = 10;
        Attributes.NightSight = 1;  // A nod to the sixth moon's dim nature

        // Skill bonuses that suit the theme of necromancy and undead-related knowledge
        SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
        SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Knowledge of the body, necromancy tied to anatomy

        // Elemental resistances with a mysterious, sinister touch
        ColdBonus = 10;
        PoisonBonus = 10;

        // Add extra functionality for an enhanced roleplay experience
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkullOfTheSixthMoon(Serial serial) : base(serial)
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
