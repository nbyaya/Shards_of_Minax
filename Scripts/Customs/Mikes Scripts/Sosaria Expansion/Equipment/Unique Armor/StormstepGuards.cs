using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormstepGuards : LeatherSuneate
{
    [Constructable]
    public StormstepGuards()
    {
        Name = "Stormstep Guards";
        Hue = Utility.Random(1150, 1200);  // A hue suggesting a stormy sky or lightning.
        BaseArmorRating = Utility.RandomMinMax(25, 60);  // Balanced for light protection.

        // Adding attributes related to speed, defense, and elemental connection.
        Attributes.BonusDex = 15;  // Agility bonus to enhance speed and evasion.
        Attributes.DefendChance = 10;  // Chance to defend against attacks, tying to evasion theme.
        Attributes.RegenStam = 3;  // Stamina regeneration to support quick movements.

        // Skill bonuses connected to agility, stealth, and nature.
        SkillBonuses.SetValues(0, SkillName.Swords, 15.0);  // For those who strike swiftly in combat.
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);  // Enhancing parry for defensive agility.
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0);  // Adding stealth for swift, quiet movements.

        // Elemental resistances to reflect the storm theme.
        ColdBonus = 10;  // Resist cold, a common feature in stormy weather.
        EnergyBonus = 10;  // Resist energy-based attacks, which can represent lightning or arcane energy.
        PhysicalBonus = 5;  // Resist physical attacks, as stormy conditions may increase combat agility.
        
        // Attaching the item with XmlLevelItem for scaling attributes or use in quests.
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormstepGuards(Serial serial) : base(serial)
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
