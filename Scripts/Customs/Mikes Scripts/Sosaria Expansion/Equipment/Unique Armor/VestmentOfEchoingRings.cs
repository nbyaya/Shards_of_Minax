using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VestmentOfEchoingRings : RingmailChest
{
    [Constructable]
    public VestmentOfEchoingRings()
    {
        Name = "Vestment of Echoing Rings";
        Hue = Utility.Random(1000, 2000);
        BaseArmorRating = Utility.RandomMinMax(35, 55);

        Attributes.BonusDex = 15; // Enhancing agility
        Attributes.DefendChance = 10; // Improved defensive capabilities
        Attributes.LowerManaCost = 5; // Reduced mana cost for casting

        SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);  // Enhances combat strategies
        SkillBonuses.SetValues(1, SkillName.Parry, 15.0);  // Bolsters defensive skills with parrying
        SkillBonuses.SetValues(2, SkillName.Carpentry, 10.0);  // Allows for breaking through enemy defenses

        ColdBonus = 10; // Provides resistance against cold-based attacks
        FireBonus = 5; // Moderate fire resistance
        PoisonBonus = 10; // Increased resistance against poison-based attacks

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VestmentOfEchoingRings(Serial serial) : base(serial)
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
