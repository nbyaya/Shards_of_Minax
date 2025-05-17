using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GaitOfTheHollowStag : WoodlandLegs
{
    [Constructable]
    public GaitOfTheHollowStag()
    {
        Name = "Gait of the Hollow Stag";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(15, 40); // A moderate AR, befitting the light nature of woodland armor.

        // Unique Attributes
        Attributes.BonusDex = 10;  // The agility and stealth of the wearer.
        Attributes.DefendChance = 10;  // Increased chance to dodge or evade attacks.
        Attributes.ReflectPhysical = 5;  // A little reflection of physical damage, representing the 'wild' nature of the armor.
        
        // Skill Bonuses (thematically connected)
        SkillBonuses.SetValues(0, SkillName.Tracking, 15.0);  // Knowledge of the wild and ability to track creatures.
        SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);  // Connection with animals, perhaps enhancing healing or riding abilities.
        SkillBonuses.SetValues(2, SkillName.Bushido, 5.0);  // A nod to the stealth and honor associated with woodland creatures.

        // Elemental Resistances
        ColdBonus = 15;  // Protection from the cold wilderness.
        PhysicalBonus = 10;  // A small bonus for physical protection, fitting for light armor.
        
        // Attachment of the XML-level item functionality for unique progression
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GaitOfTheHollowStag(Serial serial) : base(serial)
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
