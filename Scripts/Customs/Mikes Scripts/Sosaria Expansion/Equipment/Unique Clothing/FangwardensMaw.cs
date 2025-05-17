using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FangwardensMaw : BearMask
{
    [Constructable]
    public FangwardensMaw()
    {
        Name = "Fangwarden's Maw";
        Hue = Utility.Random(3000, 2300);  // Custom hue for a bear-themed item

        // Set attributes and bonuses
        Attributes.BonusStr = 15;
        Attributes.BonusHits = 20;
        Attributes.BonusStam = 10;
        Attributes.DefendChance = 10;
        Attributes.Luck = 50;

        // Resistances
        Resistances.Physical = 15;
        Resistances.Poison = 10;

        // Skill Bonuses
        SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0); // Enhances animal handling and healing
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);  // Helps with knowledge of animals
        SkillBonuses.SetValues(2, SkillName.AnimalTaming, 10.0); // Helps in taming animals

        // Make it able to gain levels
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FangwardensMaw(Serial serial) : base(serial)
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
