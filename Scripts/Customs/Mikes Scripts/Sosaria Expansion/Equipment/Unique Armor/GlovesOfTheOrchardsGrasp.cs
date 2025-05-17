using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlovesOfTheOrchardsGrasp : LeatherGloves
{
    [Constructable]
    public GlovesOfTheOrchardsGrasp()
    {
        Name = "Gloves of the Orchard’s Grasp";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(15, 50);
        
        // Attributes for enhancing nature-based gameplay
        Attributes.BonusDex = 15;  // Dexterity, perfect for farming or nature-related tasks
        Attributes.BonusStr = 5;   // Strength boost to assist with light combat and labor
        Attributes.BonusInt = 10;  // Intelligence boost for learning lore or magical studies
        
        // Skill bonuses related to nature, animals, and crafting
        SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0);  // Healing and tending to animals in the orchards
        SkillBonuses.SetValues(1, SkillName.Camping, 15.0);      // Creating campfires or shelters in the wild
        SkillBonuses.SetValues(2, SkillName.Herding, 10.0);      // Herding animals in the orchards or fields

        // Environmental bonuses reflecting the enchanted nature of the gloves
        ColdBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        
        // Adding the XmlLevelItem attribute for rare drop or quest reward
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlovesOfTheOrchardsGrasp(Serial serial) : base(serial)
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
