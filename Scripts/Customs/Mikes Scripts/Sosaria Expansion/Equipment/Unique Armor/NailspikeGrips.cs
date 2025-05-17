using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NailspikeGrips : StuddedGloves
{
    [Constructable]
    public NailspikeGrips()
    {
        Name = "Nailspike Grips";
        Hue = Utility.Random(1000, 2000); // Dark, rusty red hue reminiscent of bloodstains or rust.
        BaseArmorRating = Utility.RandomMinMax(15, 45); // Mid-range armor protection.
        
        // Attributes to enhance the gloves' combat potential:
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 10;
        Attributes.BonusStam = 5;
        Attributes.DefendChance = 10;  // Increased defense chance
        Attributes.LowerManaCost = 10; // Lowers mana cost, fitting for a fighter.
        Attributes.RegenStam = 3; // Stamina regeneration for aggressive fighting.

        // Skill bonuses that reflect the gloves’ use in combat and stealthy actions:
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Tactics to increase combat effectiveness.
        SkillBonuses.SetValues(1, SkillName.Fencing, 15.0); // Tied to the use of piercing weapons, in sync with the "spikes" on the gloves.
        SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Subtlety in movement, fitting for a stealthy warrior.

        // Additional resistances for durability:
        PhysicalBonus = 10; // Resistance to physical damage.
        FireBonus = 5;      // Fire resistance in case of heated combat.
        ColdBonus = 5;      // Resistance to cold, useful for battling in harsh environments.
        
        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach XmlLevelItem for special properties.

        // Unique flavor to make it stand out:
        ItemID = 0x13F7; // Set an appropriate graphic for the studded gloves.
    }

    public NailspikeGrips(Serial serial) : base(serial)
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
