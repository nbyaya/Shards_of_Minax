using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElderbloomBonnet : Bonnet
{
    [Constructable]
    public ElderbloomBonnet()
    {
        Name = "Elderbloom Bonnet";
        Hue = 0x2D1;  // Soft floral color (tweaks can be made as per design)

        // Set attributes and bonuses
        Attributes.BonusInt = 5;
        Attributes.BonusMana = 10;
        Attributes.BonusHits = 5;
        Attributes.RegenMana = 2;
        Attributes.Luck = 25;

        // Resistances (fits the theme of nature and balance)
        Resistances.Physical = 5;
        Resistances.Fire = 5;
        Resistances.Cold = 15;
        Resistances.Poison = 10;
        Resistances.Energy = 5;

        // Skill Bonuses (fitting the natural and spiritual connection)
        SkillBonuses.SetValues(0, SkillName.Healing, 10.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
        SkillBonuses.SetValues(2, SkillName.Herding, 10.0);
        SkillBonuses.SetValues(3, SkillName.Veterinary, 10.0); // The themes of nature, animals, and healing

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElderbloomBonnet(Serial serial) : base(serial)
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
