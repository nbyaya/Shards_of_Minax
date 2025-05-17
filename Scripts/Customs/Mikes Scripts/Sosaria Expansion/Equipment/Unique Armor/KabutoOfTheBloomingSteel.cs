using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KabutoOfTheBloomingSteel : DecorativePlateKabuto
{
    [Constructable]
    public KabutoOfTheBloomingSteel()
    {
        Name = "Kabuto of the Blooming Steel";
        Hue = Utility.Random(0, 1000);  // Random hues to reflect steel's polished or aged look.
        BaseArmorRating = Utility.RandomMinMax(30, 75);  // A strong armor rating, befitting a rare item.

        ArmorAttributes.SelfRepair = 15;  // The kabuto is crafted for durability.
        Attributes.BonusStr = 10;  // A slight boost to strength, as it's an item for warriors.
        Attributes.BonusDex = 10;  // Enhances agility, representing the flowing grace of steel.
        Attributes.BonusInt = 10;  // Adds intelligence, representing the sharpness of the mind needed for mastery.
        
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);  // The kabuto aids in tactical awareness, guiding its wearer.
        SkillBonuses.SetValues(1, SkillName.Fencing, 15.0);  // Enhances fencing skills, the weapon associated with this kabuto.
        SkillBonuses.SetValues(2, SkillName.Parry, 10.0);  // Grants the wearer better defensive capabilities.

        ColdBonus = 15;  // Protection against cold, reflecting the steel’s resilience in harsh conditions.
        FireBonus = 10;  // Fire resistance, perhaps due to the forging process of the kabuto.
        PhysicalBonus = 20;  // Physical damage reduction, due to the sturdy nature of the bloomsteel.

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KabutoOfTheBloomingSteel(Serial serial) : base(serial)
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
