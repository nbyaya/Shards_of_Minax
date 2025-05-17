using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CrownOfTheJungleKing : TigerPeltHelm
{
    [Constructable]
    public CrownOfTheJungleKing()
    {
        Name = "Crown of the Jungle King";
        Hue = 1157; // A rich, earthy color reminiscent of jungle foliage
        BaseArmorRating = Utility.RandomMinMax(25, 60); // Adequate protection with some room for flexibility in stats

        Attributes.BonusStr = 15; // Strength of a jungle predator
        Attributes.BonusDex = 10; // Quick reflexes for swift movements
        Attributes.BonusHits = 30; // Enhanced endurance, like the king of the jungle withstanding trials
        Attributes.DefendChance = 10; // Increased defense to mimic the predator's evasiveness

        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0); // Understanding the ways of beasts
        SkillBonuses.SetValues(1, SkillName.Veterinary, 15.0); // Healing beasts in the wild
        SkillBonuses.SetValues(2, SkillName.Tracking, 20.0); // The ability to track your prey through the jungle

        ColdBonus = 5;  // Slightly resilient to cold, a predator’s endurance
        PhysicalBonus = 10;  // The armor's primal nature increases resistance to physical damage
        PoisonBonus = 10; // Natural resistance to poisons found in the jungle

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CrownOfTheJungleKing(Serial serial) : base(serial)
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
